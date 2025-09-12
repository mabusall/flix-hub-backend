namespace FlixHub.Core.Api.Tasks;

internal class SyncContents(IFlixHubDbUnitOfWork uow,
                            TmdbService tmdbService,
                            OmdbService omdbService,
                            TraktService traktService,
                            IManagedCancellationToken appToken)
    : IHangfireJob
{
    public async Task ExecuteAsync()
    {
        const int movieId = 603; // Example: The Matrix
        const string lang = "en";
        var ct = appToken.Token;

        // Fetch movie with all extras
        var movie = await tmdbService.GetMovieDetailsAsync(
            movieId,
            lang,
            appendToResponse: "credits,images,videos,external_ids"
        );
        if (movie is null) return;

        // Upsert Content
        var content = await uow.ContentsRepository
            .AsQueryable(true)
            .FirstOrDefaultAsync(x => x.TmdbId == movie.Id, ct);

        var isNew = content is null;
        content ??= new Content();

        content.TmdbId = movie.Id;
        content.Type = ContentType.Movie;
        content.ImdbId = movie.ExternalIds?.ImdbId;
        content.TraktId = movie.ExternalIds?.TraktId;
        content.Title = movie.Title;
        content.OriginalTitle = movie.OriginalTitle;
        content.Overview = TrimTo(movie.Overview, 500);
        content.OriginalLanguage = movie.OriginalLanguage;
        content.ReleaseDate = SafeDate(movie.ReleaseDate);
        content.Status = MapStatus(movie.Status);
        content.Runtime = movie.Runtime;
        content.Popularity = movie.Popularity;
        content.VoteAverage = movie.VoteAverage;
        content.VoteCount = movie.VoteCount;
        content.Budget = movie.Budget;
        content.PosterPath = movie.PosterPath;
        content.BackdropPath = movie.BackdropPath;
        content.LogoPath = BestLogo(movie.Images?.Logos, lang);

        if (isNew) uow.ContentsRepository.Insert(content);
        else uow.ContentsRepository.Update(content);

        // Genres
        if (movie.Genres is not null)
        {
            content.Genres.Clear();
            foreach (var g in movie.Genres)
            {
                var genre = await EnsureGenreAsync(g.Id, g.Name, ct);
                content.Genres.Add(new ContentGenre { ContentId = content.Id, GenreId = genre.Id });
            }
        }

        // Cast
        if (movie.Credits?.Cast is not null)
        {
            content.Casts.Clear();
            foreach (var c in movie.Credits.Cast.OrderBy(c => c.Order).Take(20))
            {
                var person = await EnsurePersonAsync(c.Id, c.Name, c.Gender, c.KnownForDepartment, c.ProfilePath, ct);
                content.Casts.Add(new ContentCast
                {
                    ContentId = content.Id,
                    PersonId = person.Id,
                    Character = c.Character,
                    Order = c.Order
                });
            }
        }

        // Crew
        if (movie.Credits?.Crew is not null)
        {
            content.Crews.Clear();
            foreach (var crew in movie.Credits.Crew.Where(x =>
                x.Job is "Director" or "Writer" or "Screenplay" or "Producer"))
            {
                var person = await EnsurePersonAsync(crew.Id, crew.Name, crew.Gender, crew.Department, crew.ProfilePath, ct);
                content.Crews.Add(new ContentCrew
                {
                    ContentId = content.Id,
                    PersonId = person.Id,
                    Department = crew.Department,
                    Job = crew.Job
                });
            }
        }

        // Images
        content.Images.Clear();
        foreach (var p in (movie.Images?.Posters ?? Enumerable.Empty<TmdbImage>()).Take(6))
            content.Images.Add(new ContentImage { ContentId = content.Id, Type = ImageType.Poster, FilePath = p.FilePath, Language = p.Iso_639_1, Width = p.Width, Height = p.Height });
        foreach (var b in (movie.Images?.Backdrops ?? Enumerable.Empty<TmdbImage>()).Take(6))
            content.Images.Add(new ContentImage { ContentId = content.Id, Type = ImageType.Backdrop, FilePath = b.FilePath, Language = b.Iso_639_1, Width = b.Width, Height = b.Height });
        foreach (var l in (movie.Images?.Logos ?? Enumerable.Empty<TmdbImage>()).Take(6))
            content.Images.Add(new ContentImage { ContentId = content.Id, Type = ImageType.Logo, FilePath = l.FilePath, Language = l.Iso_639_1, Width = l.Width, Height = l.Height });

        // Videos
        content.Videos.Clear();
        foreach (var v in movie.Videos?.Results ?? Enumerable.Empty<TmdbVideo>())
        {
            if (!string.Equals(v.Site, "YouTube", StringComparison.OrdinalIgnoreCase)) continue;

            var mappedType = v.Type?.ToLowerInvariant() switch
            {
                "trailer" => VideoType.Trailer,
                "teaser" => VideoType.Teaser,
                "clip" => VideoType.Clip,
                "featurette" => VideoType.Featurette,
                _ => VideoType.Clip
            };

            content.Videos.Add(new ContentVideo
            {
                ContentId = content.Id,
                Type = mappedType,
                Site = VideoSite.YouTube,
                Key = v.Key!,
                Name = v.Name!,
                IsOfficial = v.Official ?? false
            });
        }

        // Ratings from OMDb
        content.Ratings.Clear();
        if (!string.IsNullOrWhiteSpace(content.ImdbId))
        {
            var om = await omdbService.GetByImdbIdAsync(content.ImdbId);
            if (om is not null)
            {
                content.Awards = TrimTo(om.Awards, 500);
                foreach (var r in om.Ratings ?? Enumerable.Empty<OmdbRating>())
                {
                    var source = r.Source?.ToLowerInvariant() switch
                    {
                        "internet movie database" or "imdb" => RatingSource.InternetMovieDatabase,
                        "metacritic" => RatingSource.Metacritic,
                        _ => RatingSource.RottenTomatoes,
                    };

                    content.Ratings.Add(new ContentRating { ContentId = content.Id, Source = source, Value = r.Value });
                }
            }
        }

        // Enrich with Trakt if we have a TraktId
        if (content.TraktId is not null)
        {
            var trakt = await traktService.GetMovieSummaryAsync(content.TraktId!);
            if (trakt is not null)
            {
                content.Tagline = trakt.Tagline;
                content.Homepage = trakt.Homepage;
                content.Certification = trakt.Certification;

                // Ratings from Trakt
                if (trakt.Rating.HasValue)
                {
                    content.Ratings.Add(new ContentRating
                    {
                        ContentId = content.Id,
                        Source = RatingSource.Trakt,
                        Value = $"{trakt.Rating:0.0}/10"
                    });
                }

                // Add trailer if available and not already in videos
                if (!string.IsNullOrWhiteSpace(trakt.Trailer))
                {
                    var exists = content.Videos.Any(v => v.Key == trakt.Trailer);
                    if (!exists)
                    {
                        content.Videos.Add(new ContentVideo
                        {
                            ContentId = content.Id,
                            Type = VideoType.Trailer,
                            Site = VideoSite.YouTube,
                            Key = trakt.Trailer,  // Usually a YouTube URL or key
                            Name = $"{content.Title} Trailer (Trakt)",
                            IsOfficial = true
                        });
                    }
                }
            }
        }

        if (content.VoteAverage.HasValue)
        {
            content.Ratings.Add(new ContentRating
            {
                ContentId = content.Id,
                Source = RatingSource.TMDb,
                Value = $"{content.VoteAverage:0.0}/10"
            });
        }

        await uow.SaveChangesAsync(ct);
    }

    // ---------------- helpers ----------------

    private static DateTime? SafeDate(string? s)
        => DateTime.TryParse(s, out var d) ? d : null;

    private static string? TrimTo(string? s, int max)
        => string.IsNullOrEmpty(s) ? s : s.Length <= max ? s : s[..max];

    private static ContentStatus? MapStatus(string? tmdbStatus) => tmdbStatus?.ToLowerInvariant() switch
    {
        "rumored" => ContentStatus.Rumored,
        "planned" => ContentStatus.Planned,
        "in production" => ContentStatus.InProduction,
        "post production" => ContentStatus.PostProduction,
        "released" => ContentStatus.Released,
        "canceled" or "cancelled" => ContentStatus.Canceled,
        _ => null
    };

    private static string? BestLogo(IEnumerable<TmdbImage>? logos, string lang)
    {
        if (logos is null) return null;
        var inLang = logos.Where(l => string.Equals(l.Iso_639_1, lang, StringComparison.OrdinalIgnoreCase));
        var pick = inLang.OrderByDescending(l => l.VoteAverage).ThenByDescending(l => l.Width).FirstOrDefault()
               ?? logos.OrderByDescending(l => l.VoteAverage).ThenByDescending(l => l.Width).FirstOrDefault();
        return pick?.FilePath;
    }

    private async Task<Genre> EnsureGenreAsync(int tmdbId, string name, CancellationToken ct)
    {
        var g = await uow.GenresRepository
            .AsQueryable(trackChanges: true)
            .FirstOrDefaultAsync(x => x.TmdbId == tmdbId, ct);
        if (g is null)
        {
            g = new Genre { TmdbId = tmdbId, Name = name };
            uow.GenresRepository.Insert(g);
        }
        else
        {
            if (!string.Equals(g.Name, name, StringComparison.Ordinal)) g.Name = name;
            uow.GenresRepository.Update(g);
        }
        return g;
    }

    private async Task<Person> EnsurePersonAsync(int tmdbId, string name, int? tmdbGender, string? dept, string? profilePath, CancellationToken ct)
    {
        var p = await uow.PeopleRepository
            .AsQueryable(trackChanges: true)
            .FirstOrDefaultAsync(x => x.TmdbId == tmdbId, ct);

        if (p is null)
        {
            p = new Person
            {
                Name = name,
                Gender = MapGender(tmdbGender),
                KnownForDepartment = dept,
                ProfilePath = profilePath
            };
            uow.PeopleRepository.Insert(p);
        }
        else
        {
            p.Name = name;
            p.Gender = MapGender(tmdbGender);
            p.KnownForDepartment = dept;
            p.ProfilePath = profilePath;
            uow.PeopleRepository.Update(p);
        }

        return p;
    }

    private static GenderType MapGender(int? g) => g switch
    {
        1 => GenderType.Female,
        2 => GenderType.Male,
        3 => GenderType.NonBinary,
        _ => GenderType.NotSpecified
    };
}
