# FlixHub API Integration Documentation

This document provides comprehensive information about the external API integrations implemented in FlixHub for synchronizing movie and TV show data.

---

## 🌐 Overview

FlixHub integrates with multiple external APIs to provide rich content metadata:
- **TMDb (The Movie Database)** - Primary source for movies, TV shows, cast, crew, and media
- **OMDb (Open Movie Database)** - Additional ratings and awards data
- **Trakt** - Future integration for user tracking and social features

---

## 🎬 TMDb API Integration

### Authentication
- **Method**: Bearer Token Authentication
- **Configuration**: Managed via `IAppSettingsKeyManagement` with encrypted tokens
- **Header**: `Authorization: Bearer {encrypted_token}`

### Rate Limits
- **40 requests per 10 seconds per IP**
- **1,000 requests daily (free tier)**
- **Balanced allocation**: 500 requests for Movies, 500 for TV

---

### 🎭 Movie Services (`TmdbMovieService`)

| Method | Endpoint | Purpose | Response Type |
|--------|----------|---------|---------------|
| `GetDiscoverAsync()` | `/discover/movie` | Discover movies with filters | `TmdbMediaListResponse` |
| `GetDetailsAsync(id)` | `/movie/{id}` | Movie details (runtime, budget, status) | `MovieDetailsResponse` |
| `GetCreditsAsync(id)` | `/movie/{id}/credits` | Cast & crew information | `TmdbCreditsResponse` |
| `GetImagesAsync(id)` | `/movie/{id}/images` | Posters, backdrops, logos | `TmdbImagesResponse` |
| `GetVideosAsync(id)` | `/movie/{id}/videos` | Trailers, teasers, clips | `TmdbVideosResponse` |
| `GetExternalIdsAsync(id)` | `/movie/{id}/external_ids` | IMDb, social media IDs | `TmdbExternalIdsResponse` |
| `GetKeywordsAsync(id)` | `/movie/{id}/keywords` | Movie keywords/tags | `MovieKeywordsResponse` |
| `GetGenresAsync()` | `/genre/movie/list` | All movie genres | `TmdbGenreResponse` |
| `GetUpcomingsAsync()` | `/movie/upcoming` | Upcoming movie releases | `MovieUpcomingResponse` |
| `GetTrendingAsync()` | `/trending/movie/{timeWindow}` | Trending movies | `TmdbMediaListResponse` |
| `GetTopRatedAsync()` | `/movie/top_rated` | Top rated movies | `TmdbMediaListResponse` |
| `GetPopularAsync()` | `/movie/popular` | Popular movies | `TmdbMediaListResponse` |
| `GetChangesAsync()` | `/movie/changes` | Recent movie changes | `TmdbChangesResponse` |

#### Example Usage:
```csharp
// Discover movies by year/month for sync
var query = new Dictionary<string, string>
{
    ["region"] = "US",
    ["with_release_type"] = "2|3|4|5|6",
    ["primary_release_date.gte"] = "2024-01-01",
    ["primary_release_date.lte"] = "2024-01-31",
    ["sort_by"] = "primary_release_date.asc",
    ["include_adult"] = "false"
};

var movies = await tmdbService.Movies.GetDiscoverAsync("en-US", query, page: 1);
```

---

### 📺 TV Services (`TmdbTvService`)

| Method | Endpoint | Purpose | Response Type |
|--------|----------|---------|---------------|
| `GetDiscoverAsync()` | `/discover/tv` | Discover TV shows with filters | `TmdbMediaListResponse` |
| `GetDetailsAsync(id)` | `/tv/{id}` | TV series details | `TvDetailsResponse` |
| `GetCreditsAsync(id)` | `/tv/{id}/credits` | Show-level cast & crew | `TmdbCreditsResponse` |
| `GetImagesAsync(id)` | `/tv/{id}/images` | Series posters, backdrops | `TmdbImagesResponse` |
| `GetVideosAsync(id)` | `/tv/{id}/videos` | Series trailers, promos | `TmdbVideosResponse` |
| `GetExternalIdsAsync(id)` | `/tv/{id}/external_ids` | External platform IDs | `TmdbExternalIdsResponse` |
| `GetKeywordsAsync(id)` | `/tv/{id}/keywords` | TV show keywords | `TvKeywordsResponse` |
| `GetGenresAsync()` | `/genre/tv/list` | All TV genres | `TmdbGenreResponse` |
| `GetSeasonDetailsAsync(id, seasonNumber)` | `/tv/{id}/season/{seasonNumber}` | Season information | `TvSeasonResponse` |
| `GetEpisodeDetailsAsync(id, seasonNumber, episodeNumber)` | `/tv/{id}/season/{seasonNumber}/episode/{episodeNumber}` | Episode details | `TvEpisodeResponse` |
| `GetTrendingAsync()` | `/trending/tv/{timeWindow}` | Trending TV shows | `TmdbMediaListResponse` |
| `GetChangesAsync()` | `/tv/changes` | Recent TV changes | `TmdbChangesResponse` |

#### Example Usage:
```csharp
// Discover TV shows by air date
var query = new Dictionary<string, string>
{
    ["first_air_date.gte"] = "2024-01-01",
    ["first_air_date.lte"] = "2024-01-31",
    ["sort_by"] = "first_air_date.asc",
    ["include_adult"] = "false"
};

var tvShows = await tmdbService.Tv.GetDiscoverAsync("en-US", query, page: 1);

// Get detailed season/episode data
var season = await tmdbService.Tv.GetSeasonDetailsAsync(tvId, 1);
var episode = await tmdbService.Tv.GetEpisodeDetailsAsync(tvId, 1, 1);
```

---

### 👥 People Services (`TmdbPeopleService`)

| Method | Endpoint | Purpose | Response Type |
|--------|----------|---------|---------------|
| `GetPersonAsync(id)` | `/person/{id}` | Person details, biography, images | `PersonResponse` |

#### Example Usage:
```csharp
var person = await tmdbService.People.GetPersonAsync("123", "en-US");
```

---

## 🏆 OMDb API Integration

### Authentication
- **Method**: API Key Parameter
- **Configuration**: Encrypted API key via `IAppSettingsKeyManagement`
- **Parameter**: `apikey={encrypted_key}`

### Services (`OmdbService`)

| Method | Purpose | Parameters | Response Type |
|--------|---------|------------|---------------|
| `GetMovieDetailsAsync(imdbId)` | Get detailed ratings, awards, Metacritic scores | IMDb ID | `OmdbMovieDetailsResponse` |

#### Example Usage:
```csharp
var omdbData = await omdbService.GetMovieDetailsAsync("tt0111161");
// Returns: IMDb rating, Rotten Tomatoes, Metacritic, awards, etc.
```

---

## 🎯 Trakt API Integration (Future)

### Services (`TraktService`)
- **Status**: Basic structure implemented
- **Current**: `GetMovieDetailsAsync(imdbId)`
- **Future**: User tracking, watch history, social features

---

## 🏗️ Service Architecture

### Main Service Aggregator
```csharp
internal sealed class TmdbService
{
    public TmdbMovieService Movies { get; }
    public TmdbTvService Tv { get; }
    public TmdbPeopleService People { get; }
}
```

### Dependency Injection Registration
```csharp
services
    .AddScoped<TmdbService>()
    .AddScoped<OmdbService>()
    .AddScoped<TraktService>();
```

### Common Features
- ✅ **Cancellation Token Support** via `IManagedCancellationToken`
- ✅ **Encrypted Configuration** via `IAppSettingsKeyManagement`
- ✅ **Typed HTTP Responses** with comprehensive DTOs
- ✅ **Error Handling** infrastructure
- ✅ **Rate Limiting** awareness

---

## 📊 Data Flow Integration

### Sync Process Integration
The API services integrate with the **ContentSyncLog** strategy:

1. **Discovery Phase**:
```csharp
// Movies
var movies = await tmdbService.Movies.GetDiscoverAsync(language, query, page);

// TV Shows
var tvShows = await tmdbService.Tv.GetDiscoverAsync(language, query, page);
```

2. **Enrichment Phase** (per content item):
```csharp
var details = await tmdbService.Movies.GetDetailsAsync(movieId);
var credits = await tmdbService.Movies.GetCreditsAsync(movieId);
var images = await tmdbService.Movies.GetImagesAsync(movieId);
var videos = await tmdbService.Movies.GetVideosAsync(movieId);
var externalIds = await tmdbService.Movies.GetExternalIdsAsync(movieId);

// Additional ratings from OMDb
if (!string.IsNullOrEmpty(externalIds.ImdbId))
{
    var omdbData = await omdbService.GetMovieDetailsAsync(externalIds.ImdbId);
}
```

3. **TV-Specific Enrichment**:
```csharp
var tvDetails = await tmdbService.Tv.GetDetailsAsync(tvId);

// Fetch seasons and episodes
for (int seasonNum = 1; seasonNum <= tvDetails.NumberOfSeasons; seasonNum++)
{
    var season = await tmdbService.Tv.GetSeasonDetailsAsync(tvId, seasonNum);
    
    foreach (var episode in season.Episodes)
    {
        var episodeDetails = await tmdbService.Tv.GetEpisodeDetailsAsync(
            tvId, seasonNum, episode.EpisodeNumber);
    }
}
```

---

## 🔧 Configuration Requirements

### appsettings.json Structure
```json
{
  "IntegrationApis": {
    "Apis": {
      "TMDB": {
        "BaseUrl": "https://api.themoviedb.org/3/",
        "Token": "{encrypted_bearer_token}"
      },
      "OMDB": {
        "BaseUrl": "https://www.omdbapi.com/",
        "Token": "{encrypted_api_key}"
      },
      "TRAKT": {
        "BaseUrl": "https://api.trakt.tv/",
        "Token": "{encrypted_api_key}"
      }
    }
  }
}
```

---

## 📈 Performance Considerations

### Rate Limiting Strategy
- **TMDb**: 40 req/10sec, 1000/day
- **Balanced allocation**: 500 movies + 500 TV daily
- **Retry logic**: Implemented via Polly (recommended)
- **Circuit breaker**: For API availability

### Caching Strategy
- **Stable data**: Cache genres, external IDs, images
- **TTL recommendations**: 
  - Genres: 24 hours
  - Images: 12 hours  
  - Details: 6 hours
  - Changes: 1 hour

---

## 🐛 Error Handling

### Common Error Scenarios
1. **Rate limit exceeded** (429)
2. **Invalid API key** (401)
3. **Resource not found** (404)
4. **Service unavailable** (503)

### Recommended Patterns
```csharp
// Implement retry with exponential backoff
await Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
    .ExecuteAsync(async () => 
    {
        return await tmdbService.Movies.GetDetailsAsync(movieId);
    });
```

---

## 🔮 Future Enhancements

### Phase 1 - Optimization
- [ ] Implement comprehensive retry policies
- [ ] Add response caching layer
- [ ] Implement circuit breaker patterns
- [ ] Add structured logging for API calls

### Phase 2 - Advanced Features
- [ ] Implement `/changes` endpoint for incremental updates
- [ ] Add batch processing for multiple requests
- [ ] Implement request deduplication
- [ ] Add API usage analytics

### Phase 3 - Extended Integration
- [ ] Full Trakt.tv integration
- [ ] Additional rating providers
- [ ] Real-time webhook processing
- [ ] Advanced content filtering

---

## 📚 Related Documentation
- [SyncContentLog.md](./SyncContentLog.md) - Sync strategy and orchestration
- [common commands.md](./common%20commands.md) - Development setup
- [README.md](./README.md) - Project overview

---

*Last updated: Septempre 2025*
*Version: 1.0*