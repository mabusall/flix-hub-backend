namespace FlixHub.Core.Api.Services.Dtos;

public record TraktMovieResponse
{
    public string? Title { get; set; }
    public int? Year { get; set; }
    public TraktIds? Ids { get; set; }
    public string? Tagline { get; set; }
    public string? Overview { get; set; }
    public string? Released { get; set; }
    public int? Runtime { get; set; }
    public string? Country { get; set; }
    public string? Trailer { get; set; }
    public string? Homepage { get; set; }
    public string? Status { get; set; }
    public double? Rating { get; set; }
    public int? Votes { get; set; }
    public int? Comment_Count { get; set; }
    public DateTime? Updated_At { get; set; }
    public string? Language { get; set; }
    public IEnumerable<string>? Languages { get; set; }
    public IEnumerable<string>? Available_Translations { get; set; }
    public IEnumerable<string>? Genres { get; set; }
    public IEnumerable<string>? Subgenres { get; set; }
    public string? Certification { get; set; }
    public string? Original_Title { get; set; }
    public bool? After_Credits { get; set; }
    public bool? During_Credits { get; set; }
}

public record TraktIds
{
    public int? Trakt { get; set; }
    public string? Slug { get; set; }
    public string? Imdb { get; set; }
    public int? Tmdb { get; set; }
}
