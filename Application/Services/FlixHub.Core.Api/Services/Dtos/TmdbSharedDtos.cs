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

internal sealed record TmdbMediaListResponse : TmdbPagedResponse<TmdbMediaListItem>;

internal sealed record TmdbChangesResponse : TmdbPagedResponse<TmdbChange>;

internal sealed record TmdbChange
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("adult")]
    public bool Adult { get; set; }
}

internal sealed record TmdbExternalIdsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }

    [JsonPropertyName("freebase_mid")]
    public string? FreebaseMid { get; set; }

    [JsonPropertyName("freebase_id")]
    public string? FreebaseId { get; set; }

    [JsonPropertyName("tvdb_id")]
    public int? TvdbId { get; set; }

    [JsonPropertyName("tvrage_id")]
    public int? TvrageId { get; set; }

    [JsonPropertyName("wikidata_id")]
    public string? WikidataId { get; set; }

    [JsonPropertyName("facebook_id")]
    public string? FacebookId { get; set; }

    [JsonPropertyName("instagram_id")]
    public string? InstagramId { get; set; }

    [JsonPropertyName("twitter_id")]
    public string? TwitterId { get; set; }
}

internal sealed record TmdbCreditsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("cast")]
    public IList<TmdbCast> Cast { get; set; } = [];

    [JsonPropertyName("crew")]
    public IList<TmdbCrew> Crew { get; set; } = [];
}

internal sealed record TmdbImagesResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

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
    public int Id { get; set; }

    [JsonPropertyName("results")]
    public IList<TmdbVideo> Results { get; set; } = [];
}

internal sealed record PersonResponse
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("also_known_as")]
    public IList<string> AlsoKnownAs { get; set; } = [];

    [JsonPropertyName("biography")]
    public string? Biography { get; set; }

    [JsonPropertyName("birthday")]
    [JsonConverter(typeof(FlexibleNullableDateConverter))]
    public DateTime? Birthday { get; set; }

    [JsonPropertyName("deathday")]
    [JsonConverter(typeof(FlexibleNullableDateConverter))]
    public DateTime? Deathday { get; set; }

    [JsonPropertyName("gender")]
    public GenderType Gender { get; set; }

    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("place_of_birth")]
    public string? PlaceOfBirth { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }
}

internal sealed record TmdbMediaListItem
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

    [JsonPropertyName("name")]
    public string? Name { get; set; }

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

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; } // null for Search, filled for Discover

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}

internal sealed record TmdbGenre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal sealed record TmdbCrew
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("gender")]
    public GenderType Gender { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("job")]
    public string? Job { get; set; }
}

internal sealed record TmdbCast
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("gender")]
    public GenderType Gender { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? KnownForDepartment { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("profile_path")]
    public string? ProfilePath { get; set; }

    [JsonPropertyName("character")]
    public string? Character { get; set; }

    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }
}

internal sealed record TmdbImage
{
    [JsonPropertyName("aspect_ratio")]
    public double AspectRatio { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("iso_639_1")]
    public string? Iso6391 { get; set; }

    [JsonPropertyName("file_path")]
    public string? FilePath { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }
}

internal sealed record TmdbKeyword
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal sealed record TmdbVideo
{
    [JsonPropertyName("iso_639_1")]
    public string? Iso6391 { get; set; }

    [JsonPropertyName("iso_3166_1")]
    public string? Iso31661 { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("site")]
    public string? Site { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("official")]
    public bool Official { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

internal sealed record TmdbNetwork
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("logo_path")]
    public string? LogoPath { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("origin_country")]
    public string? OriginCountry { get; set; }
}

internal sealed record TmdbProductionCompany
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("logo_path")]
    public string? LogoPath { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("origin_country")]
    public string? OriginCountry { get; set; }
}

internal sealed record TmdbProductionCountry
{
    [JsonPropertyName("iso_3166_1")]
    public string? Iso31661 { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

internal sealed record TmdbSpokenLanguage
{
    [JsonPropertyName("english_name")]
    public string? EnglishName { get; set; }

    [JsonPropertyName("iso_639_1")]
    public string? Iso6391 { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}