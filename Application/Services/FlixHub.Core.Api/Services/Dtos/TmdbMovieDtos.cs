namespace FlixHub.Core.Api.Services.Dtos;

internal sealed record MovieDetailsResponse
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("belongs_to_collection")]
    public MovieCollection? BelongsToCollection { get; set; }

    [JsonPropertyName("budget")]
    public int Budget { get; set; }

    [JsonPropertyName("genres")]
    public IList<TmdbGenre> Genres { get; set; } = [];

    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

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

    [
        JsonPropertyName("release_date"),
        JsonConverter(typeof(FlexibleNullableDateConverter))
    ]
    public DateTime? ReleaseDate { get; set; }

    [JsonPropertyName("revenue")]
    public long Revenue { get; set; }

    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    [JsonPropertyName("spoken_languages")]
    public IList<TmdbSpokenLanguage> SpokenLanguages { get; set; } = [];

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}

internal sealed record MovieKeywordsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("keywords")]
    public IList<TmdbKeyword> Keywords { get; set; } = [];
}

internal sealed record MovieUpcomingResponse : TmdbPagedResponse<TmdbMediaListItem>
{
    [JsonPropertyName("dates")]
    public MovieUpcomingDates Dates { get; init; } = new();
}

internal sealed record MovieUpcomingDates
{
    [JsonPropertyName("maximum")]
    public DateTime Maximum { get; init; }

    [JsonPropertyName("minimum")]
    public DateTime Minimum { get; init; }
}

internal sealed record MovieCollection
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }
}
