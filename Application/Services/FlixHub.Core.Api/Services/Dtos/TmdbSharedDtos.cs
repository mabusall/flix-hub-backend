namespace FlixHub.Core.Api.Services.Dtos;

internal record TmdbPagedResponse<T>
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("results")]
    public IList<T> Results { get; set; } = [];

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}

internal sealed record TmdbGenreResponse
{
    [JsonPropertyName("genres")]
    public IList<TmdbGenre> Genres { get; set; } = [];
}

internal sealed record DiscoverResponse : TmdbPagedResponse<TmdbDiscover>;

internal sealed record SearchResponse : TmdbPagedResponse<TmdbSearch>;

internal sealed record TmdbExternalIdsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; init; }

    [JsonPropertyName("freebase_mid")]
    public string? FreebaseMid { get; init; }

    [JsonPropertyName("freebase_id")]
    public string? FreebaseId { get; init; }

    [JsonPropertyName("tvdb_id")]
    public int? TvdbId { get; init; }

    [JsonPropertyName("tvrage_id")]
    public int? TvrageId { get; init; }

    [JsonPropertyName("wikidata_id")]
    public string? WikidataId { get; init; }

    [JsonPropertyName("facebook_id")]
    public string? FacebookId { get; init; }

    [JsonPropertyName("instagram_id")]
    public string? InstagramId { get; init; }

    [JsonPropertyName("twitter_id")]
    public string? TwitterId { get; init; }
}

internal sealed record TmdbCreditsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("cast")]
    public IList<TmdbCast> Cast { get; set; } = [];

    [JsonPropertyName("crew")]
    public IList<TmdbCrew> Crew { get; set; } = [];
}

internal sealed record TmdbImagesResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("backdrops")]
    public IList<TmdbImage> Backdrops { get; set; } = [];

    [JsonPropertyName("posters")]
    public IList<TmdbImage> Posters { get; set; } = [];

    [JsonPropertyName("logos")]
    public IList<TmdbImage> Logos { get; set; } = [];
}

internal sealed record TmdbVideosResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("results")]
    public IList<TmdbVideo> Results { get; set; } = [];
}

internal sealed record TmdbGenre
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

internal sealed record TmdbCrew
{
    [JsonPropertyName("adult")]
    public bool Adult { get; init; }

    [JsonPropertyName("gender")]
    public GenderType Gender { get; init; }

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; init; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; init; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; init; }

    [JsonPropertyName("credit_id")]
    public string? CreditId { get; init; }

    [JsonPropertyName("department")]
    public string? Department { get; init; }

    [JsonPropertyName("job")]
    public string? Job { get; init; }
}

internal sealed record TmdbCast
{
    [JsonPropertyName("adult")]
    public bool Adult { get; init; }

    [JsonPropertyName("gender")]
    public GenderType Gender { get; init; }

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; init; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; init; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; init; }

    [JsonPropertyName("character")]
    public string? Character { get; init; }

    [JsonPropertyName("credit_id")]
    public string? CreditId { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }
}

internal sealed record TmdbImage
{
    [JsonPropertyName("aspect_ratio")]
    public double AspectRatio { get; init; }

    [JsonPropertyName("height")]
    public int Height { get; init; }

    [JsonPropertyName("iso_639_1")]
    public string? Iso6391 { get; init; }

    [JsonPropertyName("file_path")]
    public string? FilePath { get; init; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; init; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; init; }

    [JsonPropertyName("width")]
    public int Width { get; init; }
}

internal sealed record TmdbKeyword
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

internal sealed record TmdbVideo
{
    [JsonPropertyName("iso_639_1")]
    public string? Iso6391 { get; init; }

    [JsonPropertyName("iso_3166_1")]
    public string? Iso31661 { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("key")]
    public string? Key { get; init; }

    [JsonPropertyName("site")]
    public string? Site { get; init; }

    [JsonPropertyName("size")]
    public int Size { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("official")]
    public bool Official { get; init; }

    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }
}

internal sealed record TmdbDiscover
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("genre_ids")]
    public IList<int> GenreIds { get; set; } = [];

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("origin_country")]
    public IList<string?> OriginCountry { get; set; } = [];

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}

internal sealed record TmdbSearch
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("genre_ids")]
    public IList<int> GenreIds { get; set; } = [];

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("origin_country")]
    public IList<string?> OriginCountry { get; set; } = [];

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}