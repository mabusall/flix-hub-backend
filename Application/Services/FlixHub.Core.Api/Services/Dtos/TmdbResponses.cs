namespace FlixHub.Core.Api.Services.Dtos;

public record TmdbGenreResponse(IEnumerable<TmdbGenre> Genres);
public record TmdbGenre(int Id, string Name);
public record TmdbMovieResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? OriginalTitle { get; set; }
    public string? Overview { get; set; }
    public string? Status { get; set; }
    public string? OriginalLanguage { get; set; }
    public string? ReleaseDate { get; set; }
    public int? Runtime { get; set; }
    public decimal? Popularity { get; set; }
    public decimal? VoteAverage { get; set; }
    public int? VoteCount { get; set; }
    public long? Budget { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public TmdbExternalIds? ExternalIds { get; set; }
    public TmdbCredits? Credits { get; set; }
    public TmdbImages? Images { get; set; }
    public TmdbVideos? Videos { get; set; }
    public IEnumerable<TmdbGenre>? Genres { get; set; }
}
public record TmdbExternalIds(string? ImdbId, string? TraktId);
public record TmdbCredits(IEnumerable<TmdbCast>? Cast, IEnumerable<TmdbCrew>? Crew);
public record TmdbCast(int Id, string Name, string? Character, int? Order, int? Gender, string? KnownForDepartment, string? ProfilePath);
public record TmdbCrew(int Id, string Name, string? Job, string? Department, int? Gender, string? ProfilePath);
public record TmdbImages(IEnumerable<TmdbImage>? Posters, IEnumerable<TmdbImage>? Backdrops, IEnumerable<TmdbImage>? Logos);
public record TmdbImage(string FilePath, string? Iso_639_1, int Width, int Height, double VoteAverage);
public record TmdbVideos(IEnumerable<TmdbVideo>? Results);
public record TmdbVideo(string? Key, string? Name, string? Site, string? Type, bool? Official);
public record TmdbSeriesResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? OriginalName { get; set; }
    public string? Overview { get; set; }
    public string? Status { get; set; }
    public string? OriginalLanguage { get; set; }
    public string? FirstAirDate { get; set; }
    public string? LastAirDate { get; set; }
    public decimal? Popularity { get; set; }
    public decimal? VoteAverage { get; set; }
    public int? VoteCount { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public TmdbExternalIds? ExternalIds { get; set; }
    public TmdbCredits? Credits { get; set; }
    public TmdbImages? Images { get; set; }
    public TmdbVideos? Videos { get; set; }
    public IEnumerable<TmdbGenre>? Genres { get; set; }
}

