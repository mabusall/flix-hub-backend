namespace FlixHub.Core.Api.Services.Dtos;

internal sealed record TraktMovieDetailsResponse
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("ids")]
    public TraktMovieDetailsResponseIds Ids { get; set; } = new();

    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("released")]
    public DateTime? Released { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("trailer")]
    public string? Trailer { get; set; }

    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("votes")]
    public int Votes { get; set; }

    [JsonPropertyName("comment_count")]
    public int CommentCount { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("languages")]
    public IList<string> Languages { get; set; } = [];

    [JsonPropertyName("available_translations")]
    public IList<string> AvailableTranslations { get; set; } = [];

    [JsonPropertyName("genres")]
    public IList<string> Genres { get; set; } = [];

    [JsonPropertyName("subgenres")]
    public IList<string> Subgenres { get; set; } = [];

    [JsonPropertyName("certification")]
    public string? Certification { get; set; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    [JsonPropertyName("after_credits")]
    public bool AfterCredits { get; set; }

    [JsonPropertyName("during_credits")]
    public bool DuringCredits { get; set; }
}

internal sealed record TraktMovieDetailsResponseIds
{
    [JsonPropertyName("trakt")]
    public int Trakt { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("imdb")]
    public string? Imdb { get; set; }

    [JsonPropertyName("tmdb")]
    public int Tmdb { get; set; }
}
