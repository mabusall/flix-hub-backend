//namespace FlixHub.Core.Api.Services;

//internal sealed record ChangesResponse
//{
//    [JsonPropertyName("results")]
//    public IList<ChangesResponseResultsItem> Results { get; set; } = [];

//    [JsonPropertyName("page")]
//    public int Page { get; set; }

//    [JsonPropertyName("total_pages")]
//    public int TotalPages { get; set; }

//    [JsonPropertyName("total_results")]
//    public int TotalResults { get; set; }

//}

//internal sealed record ChangesResponseResultsItem
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//}

//internal sealed record CreditsResponse
//{
//    [JsonPropertyName("cast")]
//    public IList<CreditsResponseCastItem> Cast { get; set; } = [];
//    [JsonPropertyName("crew")]
//    public IList<CreditsResponseCrewItem> Crew { get; set; } = [];
//    [JsonPropertyName("id")]
//    public int Id { get; set; }
//}

//internal sealed record CreditsResponseCastItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("gender")]
//    public int Gender { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("known_for_department")]
//    public string? KnownForDepartment { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }

//    [JsonPropertyName("character")]
//    public string? Character { get; set; }

//    [JsonPropertyName("credit_id")]
//    public string? CreditId { get; set; }

//    [JsonPropertyName("order")]
//    public int Order { get; set; }
//}

//internal sealed record CreditsResponseCrewItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("gender")]
//    public int Gender { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("known_for_department")]
//    public string? KnownForDepartment { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }

//    [JsonPropertyName("credit_id")]
//    public string? CreditId { get; set; }

//    [JsonPropertyName("department")]
//    public string? Department { get; set; }

//    [JsonPropertyName("job")]
//    public string? Job { get; set; }
//}

//internal sealed record DetailsResponse
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("backdrop_path")]
//    public string? BackdropPath { get; set; }

//    [JsonPropertyName("created_by")]
//    public IList<DetailsResponseCreatedByItem> CreatedBy { get; set; } = [];
//    [JsonPropertyName("episode_run_time")]
//    public IList<string> EpisodeRunTime { get; set; } = [];
//    [JsonPropertyName("first_air_date")]
//    public string? FirstAirDate { get; set; }

//    [JsonPropertyName("genres")]
//    public IList<DetailsResponseGenresItem> Genres { get; set; } = [];
//    [JsonPropertyName("homepage")]
//    public string? Homepage { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("in_production")]
//    public bool InProduction { get; set; }

//    [JsonPropertyName("languages")]
//    public IList<string> Languages { get; set; } = [];
//    [JsonPropertyName("last_air_date")]
//    public string? LastAirDate { get; set; }

//    [JsonPropertyName("last_episode_to_air")]
//    public DetailsResponseLastEpisodeToAir? LastEpisodeToAir { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("next_episode_to_air")]
//    public string? NextEpisodeToAir { get; set; }

//    [JsonPropertyName("networks")]
//    public IList<DetailsResponseNetworksItem> Networks { get; set; } = [];

//    [JsonPropertyName("number_of_episodes")]
//    public int NumberOfEpisodes { get; set; }

//    [JsonPropertyName("number_of_seasons")]
//    public int NumberOfSeasons { get; set; }

//    [JsonPropertyName("origin_country")]
//    public IList<string> OriginCountry { get; set; } = [];

//    [JsonPropertyName("original_language")]
//    public string? OriginalLanguage { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("production_companies")]
//    public IList<DetailsResponseProductionCompaniesItem> ProductionCompanies { get; set; } = [];
//    [JsonPropertyName("production_countries")]
//    public IList<DetailsResponseProductionCountriesItem> ProductionCountries { get; set; } = [];
//    [JsonPropertyName("seasons")]
//    public IList<DetailsResponseSeasonsItem> Seasons { get; set; } = [];
//    [JsonPropertyName("spoken_languages")]
//    public IList<DetailsResponseSpokenLanguagesItem> SpokenLanguages { get; set; } = [];
//    [JsonPropertyName("status")]
//    public string? Status { get; set; }

//    [JsonPropertyName("tagline")]
//    public string? Tagline { get; set; }

//    [JsonPropertyName("type")]
//    public string? Type { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record DetailsResponseCreatedByItem
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("credit_id")]
//    public string? CreditId { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("gender")]
//    public int Gender { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }
//}
//internal sealed record DetailsResponseGenresItem
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }
//}

//internal sealed record DetailsResponseLastEpisodeToAir
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }

//    [JsonPropertyName("air_date")]
//    public string? AirDate { get; set; }

//    [JsonPropertyName("episode_number")]
//    public int EpisodeNumber { get; set; }

//    [JsonPropertyName("episode_type")]
//    public string? EpisodeType { get; set; }

//    [JsonPropertyName("production_code")]
//    public string? ProductionCode { get; set; }

//    [JsonPropertyName("runtime")]
//    public int Runtime { get; set; }

//    [JsonPropertyName("season_number")]
//    public int SeasonNumber { get; set; }

//    [JsonPropertyName("show_id")]
//    public int ShowId { get; set; }

//    [JsonPropertyName("still_path")]
//    public string? StillPath { get; set; }
//}

//internal sealed record DetailsResponseNetworksItem
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("logo_path")]
//    public string? LogoPath { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("origin_country")]
//    public string? OriginCountry { get; set; }
//}

//internal sealed record DetailsResponseProductionCompaniesItem
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("logo_path")]
//    public string? LogoPath { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("origin_country")]
//    public string? OriginCountry { get; set; }
//}

//internal sealed record DetailsResponseProductionCountriesItem
//{
//    [JsonPropertyName("iso_3166_1")]
//    public string? Iso31661 { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }
//}

//internal sealed record DetailsResponseSeasonsItem
//{
//    [JsonPropertyName("air_date")]
//    public string? AirDate { get; set; }

//    [JsonPropertyName("episode_count")]
//    public int EpisodeCount { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("season_number")]
//    public int SeasonNumber { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }
//}

//internal sealed record DetailsResponseSpokenLanguagesItem
//{
//    [JsonPropertyName("english_name")]
//    public string? EnglishName { get; set; }

//    [JsonPropertyName("iso_639_1")]
//    public string? Iso6391 { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }
//}

//internal sealed record DiscoverResponse
//{
//    [JsonPropertyName("page")]
//    public int Page { get; set; }

//    [JsonPropertyName("results")]
//    public IList<DiscoverResponseResultsItem> Results { get; set; } = [];

//    [JsonPropertyName("total_pages")]
//    public int TotalPages { get; set; }

//    [JsonPropertyName("total_results")]
//    public int TotalResults { get; set; }
//}

//internal sealed record DiscoverResponseResultsItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("backdrop_path")]
//    public string? BackdropPath { get; set; }

//    [JsonPropertyName("genre_ids")]
//    public IList<string> GenreIds { get; set; } = [];
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("origin_country")]
//    public IList<string> OriginCountry { get; set; } = [];
//    [JsonPropertyName("original_language")]
//    public string? OriginalLanguage { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("first_air_date")]
//    public string? FirstAirDate { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("vote_average")]
//    public int VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record EpisodeDetailsResponse
//{
//    [JsonPropertyName("air_date")]
//    public string? AirDate { get; set; }

//    [JsonPropertyName("crew")]
//    public IList<EpisodeDetailsResponseCrewItem> Crew { get; set; } = [];
//    [JsonPropertyName("episode_number")]
//    public int EpisodeNumber { get; set; }

//    [JsonPropertyName("episode_type")]
//    public string? EpisodeType { get; set; }

//    [JsonPropertyName("guest_stars")]
//    public IList<EpisodeDetailsResponseGuestStarsItem> GuestStars { get; set; } = [];
//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("production_code")]
//    public string? ProductionCode { get; set; }

//    [JsonPropertyName("runtime")]
//    public int Runtime { get; set; }

//    [JsonPropertyName("season_number")]
//    public int SeasonNumber { get; set; }

//    [JsonPropertyName("still_path")]
//    public string? StillPath { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record EpisodeDetailsResponseCrewItem
//{
//    [JsonPropertyName("job")]
//    public string? Job { get; set; }

//    [JsonPropertyName("department")]
//    public string? Department { get; set; }

//    [JsonPropertyName("credit_id")]
//    public string? CreditId { get; set; }

//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("gender")]
//    public int Gender { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("known_for_department")]
//    public string? KnownForDepartment { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }
//}

//internal sealed record EpisodeDetailsResponseGuestStarsItem
//{
//    [JsonPropertyName("character")]
//    public string? Character { get; set; }

//    [JsonPropertyName("credit_id")]
//    public string? CreditId { get; set; }

//    [JsonPropertyName("order")]
//    public int Order { get; set; }

//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("gender")]
//    public int Gender { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("known_for_department")]
//    public string? KnownForDepartment { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }
//}

//internal sealed record ExternalIDsResponse
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("imdb_id")]
//    public string? ImdbId { get; set; }

//    [JsonPropertyName("freebase_mid")]
//    public string? FreebaseMid { get; set; }

//    [JsonPropertyName("freebase_id")]
//    public string? FreebaseId { get; set; }

//    [JsonPropertyName("tvdb_id")]
//    public int TvdbId { get; set; }

//    [JsonPropertyName("tvrage_id")]
//    public string? TvrageId { get; set; }

//    [JsonPropertyName("wikidata_id")]
//    public string? WikidataId { get; set; }

//    [JsonPropertyName("facebook_id")]
//    public string? FacebookId { get; set; }

//    [JsonPropertyName("instagram_id")]
//    public string? InstagramId { get; set; }

//    [JsonPropertyName("twitter_id")]
//    public string? TwitterId { get; set; }
//}

//internal sealed record GenresResponse
//{
//    [JsonPropertyName("genres")]
//    public IList<GenresResponseGenresItem> Genres { get; set; } = [];
//}

//internal sealed record GenresResponseGenresItem
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//}

//internal sealed record ImagesResponse
//{
//    [JsonPropertyName("backdrops")]
//    public IList<ImagesResponseBackdropsItem> Backdrops { get; set; } = [];
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("logos")]
//    public IList<ImagesResponseLogosItem> Logos { get; set; } = [];
//    [JsonPropertyName("posters")]
//    public IList<ImagesResponsePostersItem> Posters { get; set; } = [];
//}

//internal sealed record ImagesResponseBackdropsItem
//{
//    [JsonPropertyName("aspect_ratio")]
//    public double AspectRatio { get; set; }

//    [JsonPropertyName("height")]
//    public int Height { get; set; }

//    [JsonPropertyName("iso_639_1")]
//    public string? Iso6391 { get; set; }

//    [JsonPropertyName("file_path")]
//    public string? FilePath { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }

//    [JsonPropertyName("width")]
//    public int Width { get; set; }
//}

//internal sealed record ImagesResponseLogosItem
//{
//    [JsonPropertyName("aspect_ratio")]
//    public double AspectRatio { get; set; }

//    [JsonPropertyName("height")]
//    public int Height { get; set; }

//    [JsonPropertyName("iso_639_1")]
//    public string? Iso6391 { get; set; }

//    [JsonPropertyName("file_path")]
//    public string? FilePath { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }

//    [JsonPropertyName("width")]
//    public int Width { get; set; }
//}

//internal sealed record ImagesResponsePostersItem
//{
//    [JsonPropertyName("aspect_ratio")]
//    public double AspectRatio { get; set; }

//    [JsonPropertyName("height")]
//    public int Height { get; set; }

//    [JsonPropertyName("iso_639_1")]
//    public string? Iso6391 { get; set; }

//    [JsonPropertyName("file_path")]
//    public string? FilePath { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }

//    [JsonPropertyName("width")]
//    public int Width { get; set; }
//}

//internal sealed record KeywordResponse
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("keywords")]
//    public IList<KeywordResponseKeywordsItem> Keywords { get; set; } = [];
//}

//internal sealed record KeywordResponseKeywordsItem
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }
//}

//internal sealed record MovieDetailsResponse
//{
//    [JsonPropertyName("title")]
//    public string? Title { get; set; }

//    [JsonPropertyName("year")]
//    public int Year { get; set; }

//    [JsonPropertyName("ids")]
//    public MovieDetailsResponseIds? Ids { get; set; }

//    [JsonPropertyName("tagline")]
//    public string? Tagline { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("released")]
//    public string? Released { get; set; }

//    [JsonPropertyName("runtime")]
//    public int Runtime { get; set; }

//    [JsonPropertyName("country")]
//    public string? Country { get; set; }

//    [JsonPropertyName("trailer")]
//    public string? Trailer { get; set; }

//    [JsonPropertyName("homepage")]
//    public string? Homepage { get; set; }

//    [JsonPropertyName("status")]
//    public string? Status { get; set; }

//    [JsonPropertyName("rating")]
//    public double Rating { get; set; }

//    [JsonPropertyName("votes")]
//    public int Votes { get; set; }

//    [JsonPropertyName("comment_count")]
//    public int CommentCount { get; set; }

//    [JsonPropertyName("updated_at")]
//    public string? UpdatedAt { get; set; }

//    [JsonPropertyName("language")]
//    public string? Language { get; set; }

//    [JsonPropertyName("languages")]
//    public IList<string> Languages { get; set; } = [];
//    [JsonPropertyName("available_translations")]
//    public IList<string> AvailableTranslations { get; set; } = [];
//    [JsonPropertyName("genres")]
//    public IList<string> Genres { get; set; } = [];
//    [JsonPropertyName("subgenres")]
//    public IList<string> Subgenres { get; set; } = [];
//    [JsonPropertyName("certification")]
//    public string? Certification { get; set; }

//    [JsonPropertyName("original_title")]
//    public string? OriginalTitle { get; set; }

//    [JsonPropertyName("after_credits")]
//    public bool AfterCredits { get; set; }

//    [JsonPropertyName("during_credits")]
//    public bool DuringCredits { get; set; }
//}

//internal sealed record MovieDetailsResponseIds
//{
//    [JsonPropertyName("trakt")]
//    public int Trakt { get; set; }

//    [JsonPropertyName("slug")]
//    public string? Slug { get; set; }

//    [JsonPropertyName("imdb")]
//    public string? Imdb { get; set; }

//    [JsonPropertyName("tmdb")]
//    public int Tmdb { get; set; }
//}

//internal sealed record PersonResponse
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("also_known_as")]
//    public IList<string> AlsoKnownAs { get; set; } = [];

//    [JsonPropertyName("biography")]
//    public string? Biography { get; set; }

//    [JsonPropertyName("birthday")]
//    public DateTime? Birthday { get; set; }

//    [JsonPropertyName("deathday")]
//    public DateTime? Deathday { get; set; }

//    [JsonPropertyName("gender")]
//    public GenderType? Gender { get; set; }

//    [JsonPropertyName("homepage")]
//    public string? Homepage { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("imdb_id")]
//    public string? ImdbId { get; set; }

//    [JsonPropertyName("known_for_department")]
//    public string? KnownForDepartment { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("place_of_birth")]
//    public string? PlaceOfBirth { get; set; }

//    [JsonPropertyName("popularity")]
//    public float Popularity { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }
//}

//internal sealed record PopularResponse
//{
//    [JsonPropertyName("page")]
//    public int Page { get; set; }

//    [JsonPropertyName("results")]
//    public IList<PopularResponseResultsItem> Results { get; set; } = [];
//    [JsonPropertyName("total_pages")]
//    public int TotalPages { get; set; }

//    [JsonPropertyName("total_results")]
//    public int TotalResults { get; set; }
//}

//internal sealed record PopularResponseResultsItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("backdrop_path")]
//    public string? BackdropPath { get; set; }

//    [JsonPropertyName("genre_ids")]
//    public IList<int> GenreIds { get; set; } = [];
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("original_language")]
//    public string? OriginalLanguage { get; set; }

//    [JsonPropertyName("original_title")]
//    public string? OriginalTitle { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("release_date")]
//    public string? ReleaseDate { get; set; }

//    [JsonPropertyName("title")]
//    public string? Title { get; set; }

//    [JsonPropertyName("video")]
//    public bool Video { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record SearchResponse
//{
//    [JsonPropertyName("page")]
//    public int Page { get; set; }

//    [JsonPropertyName("results")]
//    public IList<SearchResponseResultsItem> Results { get; set; } = [];

//    [JsonPropertyName("total_pages")]
//    public int TotalPages { get; set; }

//    [JsonPropertyName("total_results")]
//    public int TotalResults { get; set; }
//}

//internal sealed record SearchResponseResultsItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("backdrop_path")]
//    public string? BackdropPath { get; set; }

//    [JsonPropertyName("genre_ids")]
//    public IList<int> GenreIds { get; set; } = [];

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("origin_country")]
//    public IList<string> OriginCountry { get; set; } = [];

//    [JsonPropertyName("original_language")]
//    public string? OriginalLanguage { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("first_air_date")]
//    public string? FirstAirDate { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("vote_average")]
//    public int VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record SeasonDetailsResponse
//{
//    [JsonPropertyName("_id")]
//    public string? Id { get; set; }

//    [JsonPropertyName("air_date")]
//    public string? AirDate { get; set; }

//    [JsonPropertyName("episodes")]
//    public IList<SeasonDetailsResponseEpisodesItem> Episodes { get; set; } = [];
//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("season_number")]
//    public int SeasonNumber { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }
//}

//internal sealed record SeasonDetailsResponseEpisodesItem
//{
//    [JsonPropertyName("air_date")]
//    public string? AirDate { get; set; }

//    [JsonPropertyName("episode_number")]
//    public int EpisodeNumber { get; set; }

//    [JsonPropertyName("episode_type")]
//    public string? EpisodeType { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("production_code")]
//    public string? ProductionCode { get; set; }

//    [JsonPropertyName("runtime")]
//    public int Runtime { get; set; }

//    [JsonPropertyName("season_number")]
//    public int SeasonNumber { get; set; }

//    [JsonPropertyName("show_id")]
//    public int ShowId { get; set; }

//    [JsonPropertyName("still_path")]
//    public string? StillPath { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }

//    [JsonPropertyName("crew")]
//    public IList<SeasonDetailsResponseEpisodesItemCrewItem> Crew { get; set; } = [];
//    [JsonPropertyName("guest_stars")]
//    public IList<SeasonDetailsResponseEpisodesItemGuestStarsItem> GuestStars { get; set; } = [];
//}

//internal sealed record SeasonDetailsResponseEpisodesItemCrewItem
//{
//    [JsonPropertyName("job")]
//    public string? Job { get; set; }

//    [JsonPropertyName("department")]
//    public string? Department { get; set; }

//    [JsonPropertyName("credit_id")]
//    public string? CreditId { get; set; }

//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("gender")]
//    public int Gender { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("known_for_department")]
//    public string? KnownForDepartment { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }
//}

//internal sealed record SeasonDetailsResponseEpisodesItemGuestStarsItem
//{
//    [JsonPropertyName("character")]
//    public string? Character { get; set; }

//    [JsonPropertyName("credit_id")]
//    public string? CreditId { get; set; }

//    [JsonPropertyName("order")]
//    public int Order { get; set; }

//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("gender")]
//    public int Gender { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("known_for_department")]
//    public string? KnownForDepartment { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("original_name")]
//    public string? OriginalName { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("profile_path")]
//    public string? ProfilePath { get; set; }
//}

//internal sealed record TopRatedResponse
//{
//    [JsonPropertyName("page")]
//    public int Page { get; set; }

//    [JsonPropertyName("results")]
//    public IList<TopRatedResponseResultsItem> Results { get; set; } = [];
//    [JsonPropertyName("total_pages")]
//    public int TotalPages { get; set; }

//    [JsonPropertyName("total_results")]
//    public int TotalResults { get; set; }
//}

//internal sealed record TopRatedResponseResultsItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("backdrop_path")]
//    public string? BackdropPath { get; set; }

//    [JsonPropertyName("genre_ids")]
//    public IList<int> GenreIds { get; set; } = [];
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("original_language")]
//    public string? OriginalLanguage { get; set; }

//    [JsonPropertyName("original_title")]
//    public string? OriginalTitle { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("release_date")]
//    public string? ReleaseDate { get; set; }

//    [JsonPropertyName("title")]
//    public string? Title { get; set; }

//    [JsonPropertyName("video")]
//    public bool Video { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record TrendingResponse
//{
//    [JsonPropertyName("page")]
//    public int Page { get; set; }

//    [JsonPropertyName("results")]
//    public IList<TrendingResponseResultsItem> Results { get; set; } = [];

//    [JsonPropertyName("total_pages")]
//    public int TotalPages { get; set; }

//    [JsonPropertyName("total_results")]
//    public int TotalResults { get; set; }
//}

//internal sealed record TrendingResponseResultsItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("backdrop_path")]
//    public string? BackdropPath { get; set; }

//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("title")]
//    public string? Title { get; set; }

//    [JsonPropertyName("original_title")]
//    public string? OriginalTitle { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("media_type")]
//    public string? MediaType { get; set; }

//    [JsonPropertyName("original_language")]
//    public string? OriginalLanguage { get; set; }

//    [JsonPropertyName("genre_ids")]
//    public IList<int> GenreIds { get; set; } = [];
//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("release_date")]
//    public string? ReleaseDate { get; set; }

//    [JsonPropertyName("video")]
//    public bool Video { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record UpcomingResponse
//{
//    [JsonPropertyName("dates")]
//    public UpcomingResponseDates? Dates { get; set; }

//    [JsonPropertyName("page")]
//    public int Page { get; set; }

//    [JsonPropertyName("results")]
//    public IList<UpcomingResponseResultsItem> Results { get; set; } = [];
//    [JsonPropertyName("total_pages")]
//    public int TotalPages { get; set; }

//    [JsonPropertyName("total_results")]
//    public int TotalResults { get; set; }
//}

//internal sealed record UpcomingResponseDates
//{
//    [JsonPropertyName("maximum")]
//    public string? Maximum { get; set; }

//    [JsonPropertyName("minimum")]
//    public string? Minimum { get; set; }
//}

//internal sealed record UpcomingResponseResultsItem
//{
//    [JsonPropertyName("adult")]
//    public bool Adult { get; set; }

//    [JsonPropertyName("backdrop_path")]
//    public string? BackdropPath { get; set; }

//    [JsonPropertyName("genre_ids")]
//    public IList<int> GenreIds { get; set; } = [];
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("original_language")]
//    public string? OriginalLanguage { get; set; }

//    [JsonPropertyName("original_title")]
//    public string? OriginalTitle { get; set; }

//    [JsonPropertyName("overview")]
//    public string? Overview { get; set; }

//    [JsonPropertyName("popularity")]
//    public double Popularity { get; set; }

//    [JsonPropertyName("poster_path")]
//    public string? PosterPath { get; set; }

//    [JsonPropertyName("release_date")]
//    public string? ReleaseDate { get; set; }

//    [JsonPropertyName("title")]
//    public string? Title { get; set; }

//    [JsonPropertyName("video")]
//    public bool Video { get; set; }

//    [JsonPropertyName("vote_average")]
//    public double VoteAverage { get; set; }

//    [JsonPropertyName("vote_count")]
//    public int VoteCount { get; set; }
//}

//internal sealed record VideosTrailersResponse
//{
//    [JsonPropertyName("id")]
//    public int Id { get; set; }

//    [JsonPropertyName("results")]
//    public IList<VideosTrailersResponseResultsItem> Results { get; set; } = [];
//}

//internal sealed record VideosTrailersResponseResultsItem
//{
//    [JsonPropertyName("iso_639_1")]
//    public string? Iso6391 { get; set; }

//    [JsonPropertyName("iso_3166_1")]
//    public string? Iso31661 { get; set; }

//    [JsonPropertyName("name")]
//    public string? Name { get; set; }

//    [JsonPropertyName("key")]
//    public string? Key { get; set; }

//    [JsonPropertyName("site")]
//    public string? Site { get; set; }

//    [JsonPropertyName("size")]
//    public int Size { get; set; }

//    [JsonPropertyName("type")]
//    public string? Type { get; set; }

//    [JsonPropertyName("official")]
//    public bool Official { get; set; }

//    [JsonPropertyName("published_at")]
//    public string? PublishedAt { get; set; }

//    [JsonPropertyName("id")]
//    public string? Id { get; set; }
//}
