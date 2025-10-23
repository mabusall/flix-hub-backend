namespace FlixHub.Core.Api.Services.Dtos;

internal sealed record TvDetailsResponse
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("created_by")]
    public IList<TvCreator> CreatedBy { get; set; } = [];

    [JsonPropertyName("episode_run_time")]
    public IList<int> EpisodeRunTime { get; set; } = [];

    [JsonPropertyName("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    [JsonPropertyName("genres")]
    public IList<TmdbGenre> Genres { get; set; } = [];

    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("in_production")]
    public bool InProduction { get; set; }

    [JsonPropertyName("languages")]
    public IList<string?> Languages { get; set; } = [];

    [JsonPropertyName("last_air_date")]
    public DateTime? LastAirDate { get; set; }

    [JsonPropertyName("last_episode_to_air")]
    public TvEpisodeResponse? LastEpisodeToAir { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("next_episode_to_air")]
    public TvEpisodeResponse? NextEpisodeToAir { get; set; }

    [JsonPropertyName("networks")]
    public IList<TmdbNetwork> Networks { get; set; } = [];

    [JsonPropertyName("number_of_episodes")]
    public int? NumberOfEpisodes { get; set; }

    [JsonPropertyName("number_of_seasons")]
    public int NumberOfSeasons { get; set; }

    [JsonPropertyName("origin_country")]
    public IList<string?> OriginCountry { get; set; } = [];

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("production_companies")]
    public IList<TmdbProductionCompany> ProductionCompanies { get; set; } = [];

    [JsonPropertyName("production_countries")]
    public IList<TmdbProductionCountry> ProductionCountries { get; set; } = [];

    [JsonPropertyName("seasons")]
    public IList<TvSeasonResponse> Seasons { get; set; } = [];

    [JsonPropertyName("spoken_languages")]
    public IList<TmdbSpokenLanguage> SpokenLanguages { get; set; } = [];

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}

internal sealed record TvKeywordsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("results")]
    public IList<TmdbKeyword> Results { get; set; } = [];
}

internal sealed record TvSeasonResponse
{
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    [JsonPropertyName("episode_count")]
    public int EpisodeCount { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("episodes")]
    public IList<TvEpisodeResponse> Episodes { get; set; } = [];
}

internal sealed record TvEpisodeResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    [JsonPropertyName("episode_number")]
    public int EpisodeNumber { get; set; }

    [JsonPropertyName("episode_type")]
    public string? EpisodeType { get; set; }

    [JsonPropertyName("production_code")]
    public string? ProductionCode { get; set; }

    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    [JsonPropertyName("show_id")]
    public int ShowId { get; set; }

    [JsonPropertyName("still_path")]
    public string? StillPath { get; set; }

    [JsonPropertyName("crew")]
    public IList<TmdbCrew>? Crew { get; set; }

    [JsonPropertyName("guest_stars")]
    public IList<TmdbCast>? GuestStars { get; set; }
}

internal sealed record TvCreator
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("gender")]
    public GenderType Gender { get; set; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }
}
