namespace FlixHub.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services,
                                                     IConfiguration configuration)
    {
        var hangfireOptions = configuration
            .GetSection(HangfireOptions.ConfigurationKey)
            .Get<HangfireOptions>();

        var rabbitMqConfig = configuration
            .GetSection(RabbitMqOptions.ConfigurationKey)
            .Get<RabbitMqOptions>();

        var redisConfig = configuration
            .GetSection(RedisOptions.ConfigurationKey)
            .Get<RedisOptions>();

        var rateLimitOptions = configuration
            .GetSection(RateLimitOptions.ConfigurationKey)
            .Get<RateLimitOptions>();

        services
            .AddHttpClient()
            .AddHttpContextAccessor()
            .AddSingleton<IIdGeneratorService, IdGeneratorService>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<ICorrelationIdService, CorrelationIdService>()
            .AddExceptionHandler<CustomExceptionHandler>()
            .AddScoped<IAzureBlobService, AzureBlobService>()
            .AddMemoryCaching(options =>
            {
                options.CacheProviderType = CacheProviderType.RedisMemory;
                options.RedisUri = redisConfig.Uri;
                options.Password = redisConfig.Password.Decrypt();
                options.IsEnabled = redisConfig.IsEnabled;
                options.Environment = redisConfig.Environment;
            })

            // Configure JSON options for minimal APIs.
            .Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })

            .Configure<RequestLocalizationOptions>(options =>
            {
                var culture = new CultureInfo[]
                {
                    new("en")
                    {
                        DateTimeFormat = new DateTimeFormatInfo
                        {
                            Calendar = new GregorianCalendar(),
                            ShortDatePattern = "yyyy-MM-dd",
                            LongDatePattern = "yyyy-MM-dd hh:mm:ss tt",
                            AMDesignator = "AM",
                            PMDesignator = "PM",
                        },
                        NumberFormat = new NumberFormatInfo
                        {
                            NumberDecimalDigits = 2,
                            NumberDecimalSeparator = ".",
                            NumberGroupSeparator = ",",
                            NumberGroupSizes = [3]
                        }
                    }
                };
                options.DefaultRequestCulture = new RequestCulture(culture.FirstOrDefault());
                options.SupportedCultures = culture;
            })

            .Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            })

            // Register the settings as a singleton service
            .AddSingleton(_ =>
            {
                return ConfigureAppSettings(configuration);
            })

            // Register the settings as a singleton service
            .AddScoped<IManagedCancellationToken>(provider =>
            {
                var timeout = TimeSpan.Parse(configuration["TokenTimeout"]);
                var lifetime = provider.GetRequiredService<IHostApplicationLifetime>();
                var cts = timeout.Ticks == 0 ? null : new CancellationTokenSource(timeout);

                // Configure the managed cancellation token
                return new ManagedCancellationToken
                    (cts is null ? lifetime.ApplicationStopping : cts.Token);
            })

            .AddMessageBus(rabbitMqConfig);

        // HttpClient factory
        services.AddHttpClient<IApiClient, ApiClient>();

        // Add feature management services
        services.AddFeatureManagement(configuration.GetSection("AppFeatures"));

        if (hangfireOptions.IsEnabled)
        {
            services
                .AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseFilter(new AutomaticRetryAttribute
                    {
                        Attempts = 0,
                        OnAttemptsExceeded = AttemptsExceededAction.Fail
                    })
                    .UseSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    })
                    .UsePostgreSqlStorage(
                        opts =>
                        {
                            opts.UseNpgsqlConnection(hangfireOptions.DbConnection.Decrypt());
                        },
                        new PostgreSqlStorageOptions
                        {
                            SchemaName = hangfireOptions.SchemaName,
                            InvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.FromSeconds(5),
                            PrepareSchemaIfNecessary = true
                        }))
                .AddHangfireServer();
        }

        if (rateLimitOptions.IsEnabled)
        {
            services.AddRateLimiter(config =>
            {
                config.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                config.AddFixedWindowLimiter(policyName: rateLimitOptions.TypeFixedPolicy, options =>
                {
                    options.PermitLimit = rateLimitOptions.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(rateLimitOptions.Window);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = rateLimitOptions.QueueLimit;
                    options.AutoReplenishment = rateLimitOptions.AutoReplenishment;
                });
            });
        }

        // When mapping objects with circular references,
        // a stackoverflow exception will result.
        // This is because Mapster will get stuck in a loop
        // trying to recursively map the circular reference.
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        return services;
    }

    private static IServiceCollection AddMessageBus(this IServiceCollection services,
                                                    RabbitMqOptions rabbitMqOptions)
    {
        services
            .AddScoped<IBusService, BusService>()

            .AddDbContext<OutBoxDbContext>((_, options) =>
            {
                options.UseNpgsql(rabbitMqOptions.DbConnection.Decrypt(), npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                });
            });

        if (rabbitMqOptions.IsActive)
        {
            services.AddMassTransit(options =>
            {
                options.SetKebabCaseEndpointNameFormatter();

                // to avoid nullability if no consumer added
                var busConfigurators = BusConfiguration.GetBusConfigurators();
                if (busConfigurators is not null)
                {
                    busConfigurators(options);
                }

                options.AddEntityFrameworkOutbox<OutBoxDbContext>(efConfig =>
                {
                    // configure which database lock provider to use
                    efConfig.UsePostgres();   // 🔄 switched from SQL Server to Postgres

                    // enable the bus outbox based on configuration
                    efConfig.UseBusOutbox(b =>
                    {
                        if (rabbitMqOptions.DisableDeliveryService)
                            b.DisableDeliveryService();
                    });

                    if (rabbitMqOptions.DisableInboxCleanupService)
                        efConfig.DisableInboxCleanupService();
                });

                options.UsingRabbitMq((context, config) =>
                {
                    // applies to all receive endpoints
                    config.UseMessageRetry(r => r.Interval(rabbitMqOptions.RetryCount, rabbitMqOptions.Interval));
                    config.PrefetchCount = rabbitMqOptions.PrefetchCount;
                    config.ConcurrentMessageLimit = rabbitMqOptions.ConcurrentMessageLimit;
                    config.AutoStart = rabbitMqOptions.AutoStart;
                    config.AutoDelete = rabbitMqOptions.AutoDelete;

                    var busFactoryConfigurators = BusConfiguration.GetBusFactoryConfigurators();
                    if (busFactoryConfigurators is not null)
                    {
                        busFactoryConfigurators(config, context);
                    }

                    config.Host(new Uri(rabbitMqOptions.Uri), host =>
                    {
                        host.Username(rabbitMqOptions.UserName.Decrypt());
                        host.Password(rabbitMqOptions.Password.Decrypt());
                    });

                    config.ConfigureEndpoints(context);
                });
            });
        }

        return services;
    }

    public static WebApplicationBuilder ConfigureCoreServices(this WebApplicationBuilder app,
                                                              ConfigurationManager configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var elasticConfig = configuration
            .GetSection(ElasticSearchOptions.ConfigurationKey)
            .Get<ElasticSearchOptions>();

        var elasticApmConfig = configuration
            .GetSection(ElasticApmOptions.ConfigurationKey)
            .Get<ElasticApmOptions>();

        // set api key
        ApiKeyFilter.ApiKey = configuration["ApiKey"].Decrypt();

        app.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithProperty("Project", elasticConfig.Project)
                .Enrich.WithProperty("Application", elasticConfig.Application)
                .Enrich.WithExceptionDetails()
                .Enrich.WithSensitiveDataMasking(options =>
                {
                    // remove the built-in operators (email, IBAN, credit-card) but keep password
                    options.MaskingOperators.Clear();

                    // only mask the Password property
                    options.MaskProperties.Add(MaskProperty.WithDefaults("Password"));
                    options.MaskProperties.Add(MaskProperty.WithDefaults("Pwd"));
                    options.MaskProperties.Add(MaskProperty.WithDefaults("Passphrase"));

                    // optional: customize the mask characters
                    options.MaskValue = new string('*', 10);
                })

                // plain text formatter
                //.WriteTo.File(@$"c:\logs\{elasticConfig.Application}-.log",
                //              rollingInterval: RollingInterval.Day,
                //              rollOnFileSizeLimit: true,
                //              fileSizeLimitBytes: 1073741824, //1GB
                //              retainedFileCountLimit: null)

                // write logs into console
                .WriteTo.Async(a => a.Console())
                // json text formatter
                .WriteTo.Async(a => a.File(path: @$"c:\logs\{elasticConfig.Application}-.json",
                              formatter: new CompactJsonFormatter(),
                              rollingInterval: RollingInterval.Day,
                              rollOnFileSizeLimit: true,
                              retainedFileCountLimit: null,
                              fileSizeLimitBytes: 5242880), blockWhenFull: false)
                .ReadFrom.Configuration(configuration);
        }, preserveStaticLogger: true, writeToProviders: true);
        Log.CloseAndFlush();

        if (elasticApmConfig.IsEnabled)
        {
            app.Services.AddAllElasticApm();
        }

        return app;
    }

    public static IApplicationBuilder UseCoreServices(this IApplicationBuilder app,
                                                      IConfiguration configuration)
    {
        var hangfireOptions = configuration
            .GetSection(HangfireOptions.ConfigurationKey)
            .Get<HangfireOptions>();

        var supportedLanguages = configuration
            .GetSection("SupportedLanguages")
            .Get<string[]>();

        var supportedCultures = supportedLanguages
            .Select(language => new CultureInfo(language))
            .ToList();

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        app
            .UseStaticFiles() // to access wwwroot files
            .UseMiddleware<LocalizationMiddleware>()
            .UseRequestLocalization(localizationOptions)
            .UseExceptionHandler(options => { })
            .UseHttpsRedirection()
            .UseCorrelationId()
            .UseAuthentication()
            .UseAuthorization();

        if (hangfireOptions.IsEnabled)
            app.UseHangfireDashboard();

        return app;
    }

    public static IAppSettingsKeyManagement ConfigureAppSettings(IConfiguration configuration)
    {
        ElasticApmOptions elasticApmOptions = new();
        ElasticSearchOptions elasticSearchOptions = new();
        HangfireOptions hangfireOptions = new();
        KeycloakOptions keycloakOptions = new();
        RabbitMqOptions rabbitMqOptions = new();
        RedisOptions redisOptions = new();
        SmtpEmailOptions smtpEmailOptions = new();
        BasicAuthenticationOptions basicAuthenticationOptions = new();
        AzurBlobServiceOptions azurBlobServiceOptions = new();
        RateLimitOptions rateLimitOptions = new();
        Dictionary<string, bool> appFeatures = [];
        FirebaseOptions firebaseOptions = new();
        IntegrationApisOptions integrationApisOptions = new();

        configuration
            .GetSection(ElasticApmOptions.ConfigurationKey)
            .Bind(elasticApmOptions);
        configuration
            .GetSection(ElasticSearchOptions.ConfigurationKey)
            .Bind(elasticSearchOptions);
        configuration
            .GetSection(HangfireOptions.ConfigurationKey)
            .Bind(hangfireOptions);
        configuration
            .GetSection(KeycloakOptions.ConfigurationKey)
            .Bind(keycloakOptions);
        configuration
            .GetSection(RabbitMqOptions.ConfigurationKey)
            .Bind(rabbitMqOptions);
        configuration
            .GetSection(RedisOptions.ConfigurationKey)
            .Bind(redisOptions);
        configuration
            .GetSection(SmtpEmailOptions.ConfigurationKey)
            .Bind(smtpEmailOptions);
        configuration
            .GetSection(BasicAuthenticationOptions.ConfigurationKey)
            .Bind(basicAuthenticationOptions);
        configuration
            .GetSection(AzurBlobServiceOptions.ConfigurationKey)
            .Bind(azurBlobServiceOptions);
        configuration
            .GetSection(RateLimitOptions.ConfigurationKey)
            .Bind(rateLimitOptions);
        configuration
            .GetSection("AppFeatures")
            .Bind(appFeatures);
        configuration
           .GetSection(FirebaseOptions.ConfigurationKey)
           .Bind(firebaseOptions);
        configuration
           .GetSection(IntegrationApisOptions.ConfigurationKey)
           .Bind(integrationApisOptions);

        return new AppSettingsKeyManagement(elasticApmOptions,
                                            elasticSearchOptions,
                                            hangfireOptions,
                                            keycloakOptions,
                                            rabbitMqOptions,
                                            redisOptions,
                                            smtpEmailOptions,
                                            basicAuthenticationOptions,
                                            azurBlobServiceOptions,
                                            rateLimitOptions,
                                            appFeatures,
                                            firebaseOptions,
                                            integrationApisOptions);
    }
}