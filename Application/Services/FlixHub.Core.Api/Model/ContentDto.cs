namespace FlixHub.Core.Api.Model;

public record ContentDto : AuditableDto
{
    public int TmdbId { get; set; }
    public string? ImdbId { get; set; }
    public string? TraktId { get; set; }
    public ContentType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? Overview { get; set; }
    public string? OriginalLanguage { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public ContentStatus? Status { get; set; }
    public string? Country { get; set; }
    public int? Runtime { get; set; }
    public decimal? PopularityTmdb { get; set; }
    public decimal? VoteAverage { get; set; }
    public int? VoteCount { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public string? LogoPath { get; set; }
    public IList<ContentGenreDto> Genres { get; set; } = [];
    public IList<ContentCastDto> Casts { get; set; } = [];
    public IList<ContentCrewDto> Crews { get; set; } = [];
}