namespace FlixHub.Core.Api.Services.Dtos;

internal sealed record MovieDetailsResponse
{
    [JsonPropertyName("adult")]
    public bool Adult { get; init; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; init; }

    [JsonPropertyName("belongs_to_collection")]
    public TmdbMovieCollection? BelongsToCollection { get; init; }

    [JsonPropertyName("budget")]
    public int Budget { get; init; }

    [JsonPropertyName("genres")]
    public IList<TmdbGenre> Genres { get; set; } = [];

    [JsonPropertyName("homepage")]
    public string? Homepage { get; init; }

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; init; }

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; init; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; init; }

    [JsonPropertyName("overview")]
    public string? Overview { get; init; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; init; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; init; }

    [JsonPropertyName("production_companies")]
    public IList<TmdbProductionCompany> ProductionCompanies { get; set; } = [];

    [JsonPropertyName("production_countries")]
    public IList<TmdbProductionCountry> ProductionCountries { get; set; } = [];

    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; init; }

    [JsonPropertyName("revenue")]
    public long Revenue { get; init; }

    [JsonPropertyName("runtime")]
    public int? Runtime { get; init; }

    [JsonPropertyName("spoken_languages")]
    public IList<TmdbSpokenLanguage> SpokenLanguages { get; set; } = [];

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("tagline")]
    public string? Tagline { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("video")]
    public bool Video { get; init; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; init; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; init; }
}

internal sealed record TmdbMovieCollection
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; init; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; init; }
}