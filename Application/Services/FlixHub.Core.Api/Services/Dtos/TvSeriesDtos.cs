namespace FlixHub.Core.Api.Services;

using System.Text.Json.Serialization;

internal record PagedResponseDto
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}

internal sealed record TvExternalIDsDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("imdb_id")]
    public string? Imdb_id { get; set; }

    [JsonPropertyName("tvdb_id")]
    public int Tvdb_id { get; set; }

    [JsonPropertyName("wikidata_id")]
    public string? Wikidata_id { get; set; }

    [JsonPropertyName("instagram_id")]
    public string? Instagram_id { get; set; }

}

internal sealed record TvCreditsDto
{
    [JsonPropertyName("cast")]
    public IList<CastItem> Cast { get; set; } = [];

internal sealed record CastItem
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("gender")]
    public int Gender { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? Known_for_department { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? Original_name { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("profile_path")]
    public string? Profile_path { get; set; }

    [JsonPropertyName("character")]
    public string? Character { get; set; }

    [JsonPropertyName("credit_id")]
    public string? Credit_id { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

}

    [JsonPropertyName("crew")]
    public IList<CrewItem> Crew { get; set; } = [];

internal sealed record CrewItem
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("gender")]
    public int Gender { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? Known_for_department { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? Original_name { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("credit_id")]
    public string? Credit_id { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("job")]
    public string? Job { get; set; }

}

    [JsonPropertyName("id")]
    public int Id { get; set; }

}

internal sealed record TvGenresDto
{
    [JsonPropertyName("genres")]
    public IList<GenreItem> Genres { get; set; } = [];

internal sealed record GenreItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

}

internal sealed record TvImagesDto
{
    [JsonPropertyName("backdrops")]
    public IList<BackdropItem> Backdrops { get; set; } = [];

internal sealed record BackdropItem
{
    [JsonPropertyName("aspect_ratio")]
    public double Aspect_ratio { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("file_path")]
    public string? File_path { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

    [JsonPropertyName("vote_count")]
    public int Vote_count { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

}

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("logos")]
    public IList<LogoItem> Logos { get; set; } = [];

internal sealed record LogoItem
{
    [JsonPropertyName("aspect_ratio")]
    public double Aspect_ratio { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    [JsonPropertyName("file_path")]
    public string? File_path { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

    [JsonPropertyName("vote_count")]
    public int Vote_count { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

}

    [JsonPropertyName("posters")]
    public IList<PosterItem> Posters { get; set; } = [];

internal sealed record PosterItem
{
    [JsonPropertyName("aspect_ratio")]
    public double Aspect_ratio { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    [JsonPropertyName("file_path")]
    public string? File_path { get; set; }

    [JsonPropertyName("vote_average")]
    public int Vote_average { get; set; }

    [JsonPropertyName("vote_count")]
    public int Vote_count { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

}

}

internal sealed record TvKeywordDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("results")]
    public IList<ResultItem> Results { get; set; } = [];

internal sealed record ResultItem
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

}

}

internal sealed record TvChangesDto
{
    [JsonPropertyName("results")]
    public IList<ResultItem> Results { get; set; } = [];

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("total_pages")]
    public int Total_pages { get; set; }

    [JsonPropertyName("total_results")]
    public int Total_results { get; set; }

}

internal sealed record TvEpisodeDetailsDto
{
    [JsonPropertyName("air_date")]
    public string? Air_date { get; set; }

    [JsonPropertyName("crew")]
    public IList<CrewItem> Crew { get; set; } = [];

    [JsonPropertyName("episode_number")]
    public int Episode_number { get; set; }

    [JsonPropertyName("episode_type")]
    public string? Episode_type { get; set; }

    [JsonPropertyName("guest_stars")]
    public IList<Guest_starItem> Guest_stars { get; set; } = [];

internal sealed record Guest_starItem
{
    [JsonPropertyName("character")]
    public string? Character { get; set; }

    [JsonPropertyName("credit_id")]
    public string? Credit_id { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("gender")]
    public int Gender { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("known_for_department")]
    public string? Known_for_department { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? Original_name { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("profile_path")]
    public string? Profile_path { get; set; }

}

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("production_code")]
    public string? Production_code { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("season_number")]
    public int Season_number { get; set; }

    [JsonPropertyName("still_path")]
    public string? Still_path { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

    [JsonPropertyName("vote_count")]
    public int Vote_count { get; set; }

}

internal sealed record TvTrendingDto
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("results")]
    public IList<ResultItem> Results { get; set; } = [];

    [JsonPropertyName("total_pages")]
    public int Total_pages { get; set; }

    [JsonPropertyName("total_results")]
    public int Total_results { get; set; }

}

internal sealed record TvDiscoverDto
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("results")]
    public IList<ResultItem> Results { get; set; } = [];

    [JsonPropertyName("total_pages")]
    public int Total_pages { get; set; }

    [JsonPropertyName("total_results")]
    public int Total_results { get; set; }

}

internal sealed record TvSearchDto
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("results")]
    public IList<ResultItem> Results { get; set; } = [];

    [JsonPropertyName("total_pages")]
    public int Total_pages { get; set; }

    [JsonPropertyName("total_results")]
    public int Total_results { get; set; }

}

internal sealed record TvDetailsDto
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? Backdrop_path { get; set; }

    [JsonPropertyName("created_by")]
    public IList<Created_byItem> Created_by { get; set; } = [];

internal sealed record Created_byItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("credit_id")]
    public string? Credit_id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? Original_name { get; set; }

    [JsonPropertyName("gender")]
    public int Gender { get; set; }

}

    [JsonPropertyName("episode_run_time")]
    public IList<string> Episode_run_time { get; set; } = [];

    [JsonPropertyName("first_air_date")]
    public string? First_air_date { get; set; }

    [JsonPropertyName("genres")]
    public IList<GenreItem> Genres { get; set; } = [];

    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("in_production")]
    public bool In_production { get; set; }

    [JsonPropertyName("languages")]
    public IList<string> Languages { get; set; } = [];

    [JsonPropertyName("last_air_date")]
    public string? Last_air_date { get; set; }

    [JsonPropertyName("last_episode_to_air")]
    public Last_episode_to_airDto Last_episode_to_air { get; set; }

internal sealed record Last_episode_to_airDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

    [JsonPropertyName("vote_count")]
    public int Vote_count { get; set; }

    [JsonPropertyName("air_date")]
    public string? Air_date { get; set; }

    [JsonPropertyName("episode_number")]
    public int Episode_number { get; set; }

    [JsonPropertyName("episode_type")]
    public string? Episode_type { get; set; }

    [JsonPropertyName("production_code")]
    public string? Production_code { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("season_number")]
    public int Season_number { get; set; }

    [JsonPropertyName("show_id")]
    public int Show_id { get; set; }

    [JsonPropertyName("still_path")]
    public string? Still_path { get; set; }

}

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("networks")]
    public IList<NetworkItem> Networks { get; set; } = [];

internal sealed record NetworkItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("logo_path")]
    public string? Logo_path { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("origin_country")]
    public string? Origin_country { get; set; }

}

    [JsonPropertyName("number_of_episodes")]
    public int Number_of_episodes { get; set; }

    [JsonPropertyName("number_of_seasons")]
    public int Number_of_seasons { get; set; }

    [JsonPropertyName("origin_country")]
    public IList<string> Origin_country { get; set; } = [];

    [JsonPropertyName("original_language")]
    public string? Original_language { get; set; }

    [JsonPropertyName("original_name")]
    public string? Original_name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("poster_path")]
    public string? Poster_path { get; set; }

    [JsonPropertyName("production_companies")]
    public IList<Production_companieItem> Production_companies { get; set; } = [];

internal sealed record Production_companieItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("logo_path")]
    public string? Logo_path { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("origin_country")]
    public string? Origin_country { get; set; }

}

    [JsonPropertyName("production_countries")]
    public IList<Production_countrieItem> Production_countries { get; set; } = [];

internal sealed record Production_countrieItem
{
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

    [JsonPropertyName("seasons")]
    public IList<SeasonItem> Seasons { get; set; } = [];

internal sealed record SeasonItem
{
    [JsonPropertyName("air_date")]
    public string? Air_date { get; set; }

    [JsonPropertyName("episode_count")]
    public int Episode_count { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("poster_path")]
    public string? Poster_path { get; set; }

    [JsonPropertyName("season_number")]
    public int Season_number { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

}

    [JsonPropertyName("spoken_languages")]
    public IList<Spoken_languageItem> Spoken_languages { get; set; } = [];

internal sealed record Spoken_languageItem
{
    [JsonPropertyName("english_name")]
    public string? English_name { get; set; }

    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

    [JsonPropertyName("vote_count")]
    public int Vote_count { get; set; }

}

internal sealed record TvSeasonDetailsDto
{
    [JsonPropertyName("_id")]
    public string? _id { get; set; }

    [JsonPropertyName("air_date")]
    public string? Air_date { get; set; }

    [JsonPropertyName("episodes")]
    public IList<EpisodeItem> Episodes { get; set; } = [];

internal sealed record EpisodeItem
{
    [JsonPropertyName("air_date")]
    public string? Air_date { get; set; }

    [JsonPropertyName("episode_number")]
    public int Episode_number { get; set; }

    [JsonPropertyName("episode_type")]
    public string? Episode_type { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("production_code")]
    public string? Production_code { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("season_number")]
    public int Season_number { get; set; }

    [JsonPropertyName("show_id")]
    public int Show_id { get; set; }

    [JsonPropertyName("still_path")]
    public string? Still_path { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

    [JsonPropertyName("vote_count")]
    public int Vote_count { get; set; }

    [JsonPropertyName("crew")]
    public IList<CrewItem> Crew { get; set; } = [];

    [JsonPropertyName("guest_stars")]
    public IList<Guest_starItem> Guest_stars { get; set; } = [];

}

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("poster_path")]
    public string? Poster_path { get; set; }

    [JsonPropertyName("season_number")]
    public int Season_number { get; set; }

    [JsonPropertyName("vote_average")]
    public double Vote_average { get; set; }

}

internal sealed record TvVideos(Trailers)Dto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("results")]
    public IList<ResultItem> Results { get; set; } = [];

}
