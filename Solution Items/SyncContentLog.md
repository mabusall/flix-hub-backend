# SyncContentLog Design & Usage

## Purpose
The `ContentSyncLog` table is the **central orchestrator** for synchronizing Movies and TV shows from TMDb.  
It ensures that sync is incremental, resumable, and within API limits while capturing all necessary related data (genres, cast, crew, people, ratings, images, videos, seasons, episodes).

---

## Entity Structure

| Column                | Type        | Description |
|-----------------------|-------------|-------------|
| **Type**              | Enum        | Content type: Movie or TV |
| **Year**              | int         | Sync year |
| **Month**             | int         | Sync month |
| **IsCompleted**       | bool        | Whether all pages for this year+month are synced |
| **LastCompletedPage** | int         | Last successfully fetched page |
| **TotalPages**        | int?        | Total pages reported by TMDb |
| **Notes**             | string(500) | Optional notes (errors, remarks) |

---

## Sync Strategy

### API Limits
- **40 requests per 10 seconds per IP**
- **1,000 requests daily (free key)**
- Balanced **50/50 split**: 500 requests for Movies, 500 for TV.

### Incremental Sync
- **Movies**: Iterated **per Year + Month + Page**.  
- **TV**: Iterated **per Year + Month + Page**.  
- Each log entry corresponds to one `(Type, Year, Month)` batch.  
- Progress is tracked page-by-page until `IsCompleted=true`.

### Resume Safety
- If job stops midway:  
  - `LastCompletedPage` ensures resume from the next page.  
- If completed:  
  - `IsCompleted=true`, move to next `(Year, Month)`.

---

## Fetch Methods & Endpoints

### 1. FetchNextMoviesBatch()

**Steps:**
1. Check if daily **movie quota (500)** is exceeded → if yes, stop.  
2. Find next incomplete Movie log (`IsCompleted=false`).  
3. If none found, create new log row for next `(Year, Month)`.  
4. Determine `NextPage = LastCompletedPage+1`.  
5. Call TMDb **Discover Movies** endpoint with filters:  

```
/discover/movie
?language=en-US
&region=US
&with_release_type=2|3|4|5|6
&primary_release_date.gte={Year}-{Month}-01
&primary_release_date.lte={Year}-{Month}-{LastDay}
&sort_by=primary_release_date.asc
&include_adult=false
&include_video=false
&page={NextPage}
```

**Enrichment Endpoints (per movie):**
- `/movie/{id}` → details (runtime, status, **budget**, revenue, homepage).  
- `/movie/{id}/credits` → **ContentCast, ContentCrew**, and linked **Person**.  
- `/person/{id}` → enrich people profile & **PersonImage**.  
- `/movie/{id}/images` → `ContentImage`.  
- `/movie/{id}/videos` → `ContentVideo`.  
- `/movie/{id}/external_ids` → IMDb, Facebook, Instagram, Twitter, Wikidata.  
- `/movie/{id}/keywords` → `ContentKeyword`.  
- **OMDb API** → `ContentRating`, `ContentAwards` (IMDb rating, Rotten Tomatoes, Metascore, awards).  

**Log Update:**
- Update `TotalPages` if not set.  
- Increment `LastCompletedPage`.  
- If `LastCompletedPage == TotalPages`, mark `IsCompleted=true`.  
- Save `LastSyncedAt`.  
- Return results for processing.

---

### 2. FetchNextSeriesBatch()

**Steps:**
1. Check if daily **TV quota (500)** is exceeded → if yes, stop.  
2. Find next incomplete TV log (`IsCompleted=false`).  
3. If none found, create new log row for next `(Year, Month)`.  
4. Determine `NextPage = LastCompletedPage+1`.  
5. Call TMDb **Discover TV** endpoint with filters:  

```
/discover/tv
?language=en-US
&first_air_date.gte={Year}-{Month}-01
&first_air_date.lte={Year}-{Month}-{LastDay}
&sort_by=first_air_date.asc
&include_adult=false
&include_video=false
&page={NextPage}
&with_release_type=2|3|4|5|6
```

**Enrichment Endpoints (per series):**
- `/tv/{id}` → details (status, number of seasons, episode runtime).  
- `/tv/{id}/credits` → **ContentCast, ContentCrew** (overall show-level).  
- `/person/{id}` → enrich people profile & **PersonImage**.  
- `/tv/{id}/images` → `ContentImage`.  
- `/tv/{id}/videos` → `ContentVideo`.  
- `/tv/{id}/external_ids` → IMDb, Facebook, Instagram, Twitter, Wikidata.  
- `/tv/{id}/keywords` → `ContentKeyword`.  
- `/tv/{id}/season/{season_number}` → `ContentSeason`.  
- `/tv/{id}/season/{season_number}/episode/{episode_number}` → `Episode`.  
- `/tv/{id}/season/{season_number}/episode/{episode_number}/credits` → `EpisodeCast`, `EpisodeCrew`.  
- **OMDb API** → `ContentRating`, `ContentAwards`.  

**Log Update:**
- Update `TotalPages` if not set.  
- Increment `LastCompletedPage`.  
- If `LastCompletedPage == TotalPages`, mark `IsCompleted=true`.  
- Save `LastSyncedAt`.  
- Return results for processing.

---

## Balancing Movies & TV
- `SyncContents` alternates between `FetchNextMoviesBatch()` and `FetchNextSeriesBatch()`.  
- Ensures balanced 50/50 usage of daily quota.  
- Example: Fetch 1 movie page → fetch 1 TV page → repeat until quotas reached.

---

## Future Enhancements
- Integrate `/changes` endpoint for incremental updates once base sync is complete.  
- Add provider enrichment from **Trakt** for watch history, user ratings, popularity trends.  
- Optimize by caching stable data (keywords, external IDs, images) to avoid redundant requests.  
- Add hash/checksum comparison for smarter re-sync decisions.
