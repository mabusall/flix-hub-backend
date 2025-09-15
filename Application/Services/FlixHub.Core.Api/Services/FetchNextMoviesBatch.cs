namespace FlixHub.Core.Api.Tasks;

/// <summary>
/// Executes one batch of movie sync using TMDb + OMDb and updates ContentSyncLog.
/// </summary>
internal sealed class FetchNextMoviesBatch(IFlixHubDbUnitOfWork uow,
                                           TmdbMovieService tmdb,
                                           OmdbService omdb)
{
    public async Task<MovieBatchResult> ExecuteAsync(CancellationToken ct = default)
    {
        // 1. Get next incomplete movie log (oldest year+month)
        var log = await uow.ContentSyncLogsRepository
            .AsQueryable(false)
            .Where(x => x.Type == ContentType.Movie && !x.IsCompleted)
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .FirstOrDefaultAsync(ct);

        if (log is null)
            return new MovieBatchResult(null, [], "No pending movie logs");

        int nextPage = log.LastCompletedPage + 1;
        int lastDay = DateTime.DaysInMonth(log.Year, log.Month);

        // 2. Build query for discover
        var query = new Dictionary<string, string>
        {
            { "region", "US" },
            { "with_release_type", "2|3|4|5|6" },
            { "primary_release_date.gte", $"{log.Year}-{log.Month:00}-01" },
            { "primary_release_date.lte", $"{log.Year}-{log.Month:00}-{lastDay}" },
            { "sort_by", "primary_release_date.asc" },
            { "include_adult", "false" },
            { "include_video", "false" }
        };

        // 3. Call discover API
        var discover = await tmdb.GetDiscoverAsync("en-US", query, nextPage);
        if (discover == null || discover.Results.Count == 0)
        {
            log.Notes = "Discover returned no results";
            await uow.SaveChangesAsync(ct);
            return new MovieBatchResult(log, [], "Empty discover response");
        }

        if (log.TotalPages is null)
            log.TotalPages = discover.TotalPages;

        var contents = new List<Content>();

        // 4. Enrich each movie
        foreach (var movie in discover.Results)
        {
            var existing = await uow.ContentsRepository
                .AsQueryable()
                .FirstOrDefaultAsync(c => c.TmdbId == movie.Id && c.Type == ContentType.Movie, ct);

            if (existing != null)
            {
                contents.Add(existing);
                continue;
            }

            var detail = await tmdb.GetDetailsAsync(movie.Id);
            if (detail == null) continue;

            var content = new Content
            {
                Type = ContentType.Movie,
                TmdbId = detail.Id,
                ImdbId = detail.ImdbId,
                Title = detail.Title,
                OriginalTitle = detail.OriginalTitle,
                Overview = detail.Overview,
                ReleaseDate = detail.ReleaseDate,
                Runtime = detail.Runtime,
                Status = detail.Status,
                Budget = detail.Budget,
                PosterPath = detail.PosterPath,
                BackdropPath = detail.BackdropPath,
                LogoPath = detail.LogoPath,
                Popularity = detail.Popularity,
                VoteAverage = detail.VoteAverage,
                VoteCount = detail.VoteCount,
                OriginalLanguage = detail.OriginalLanguage,
                Country = detail.OriginCountry?.FirstOrDefault()
            };

            // Credits (cast & crew)
            var credits = await tmdb.GetCreditsAsync(movie.Id);
            if (credits != null)
            {
                foreach (var cast in credits.Cast)
                {
                    var person = await EnsurePersonAsync(cast.Id, ct);
                    content.Casts.Add(new ContentCast
                    {
                        Person = person,
                        Character = cast.Character,
                        Order = cast.Order
                    });
                }
                foreach (var crew in credits.Crew)
                {
                    var person = await EnsurePersonAsync(crew.Id, ct);
                    content.Crews.Add(new ContentCrew
                    {
                        Person = person,
                        Department = crew.Department,
                        Job = crew.Job
                    });
                }
            }

            // Images
            var images = await tmdb.GetImagesAsync(movie.Id);
            if (images != null)
            {
                foreach (var img in images.Backdrops.Concat(images.Posters))
                {
                    content.Images.Add(new ContentImage
                    {
                        FilePath = img.FilePath,
                        Width = img.Width,
                        Height = img.Height,
                        Language = img.Iso_639_1
                    });
                }
            }

            // Videos
            var videos = await tmdb.GetVideosAsync(movie.Id);
            if (videos != null)
            {
                foreach (var vid in videos.Results)
                {
                    content.Videos.Add(new ContentVideo
                    {
                        Key = vid.Key,
                        Name = vid.Name,
                        Site = vid.Site,
                        Type = vid.Type
                    });
                }
            }

            // External Ids → IMDb Id (if not already set)
            var ext = await tmdb.GetExternalIdsAsync(movie.Id);
            if (ext != null && !string.IsNullOrEmpty(ext.ImdbId))
                content.ImdbId = ext.ImdbId;

            // Ratings / Awards from OMDb
            if (!string.IsNullOrEmpty(content.ImdbId))
            {
                var omdbData = await omdb.GetByImdbIdAsync(content.ImdbId, ct);
                if (omdbData != null)
                {
                    if (!string.IsNullOrEmpty(omdbData.ImdbRating))
                    {
                        content.Ratings.Add(new ContentRating
                        {
                            Source = "imdb",
                            Value = omdbData.ImdbRating
                        });
                    }
                    if (!string.IsNullOrEmpty(omdbData.Metascore))
                    {
                        content.Ratings.Add(new ContentRating
                        {
                            Source = "metascore",
                            Value = omdbData.Metascore
                        });
                    }
                    if (!string.IsNullOrEmpty(omdbData.RottenTomatoes))
                    {
                        content.Ratings.Add(new ContentRating
                        {
                            Source = "rotten_tomatoes",
                            Value = omdbData.RottenTomatoes
                        });
                    }
                    if (!string.IsNullOrEmpty(omdbData.Awards))
                    {
                        content.Awards = omdbData.Awards;
                    }
                }
            }

            await uow.ContentsRepository.AddAsync(content, ct);
            contents.Add(content);
        }

        // 5. Update sync log
        log.LastCompletedPage = nextPage;
        if (log.LastCompletedPage == log.TotalPages)
            log.IsCompleted = true;
        log.LastSyncedAt = DateTime.UtcNow;

        await uow.SaveChangesAsync(ct);
        return new MovieBatchResult(log, contents, "OK");
    }

    private async Task<Person> EnsurePersonAsync(int tmdbId, CancellationToken ct)
    {
        var person = await uow.PersonsRepository
            .AsQueryable()
            .FirstOrDefaultAsync(p => p.TmdbId == tmdbId, ct);

        if (person != null)
            return person;

        var detail = await tmdb.GetPersonAsync(tmdbId, ct);
        if (detail == null)
            return new Person { TmdbId = tmdbId, Name = "Unknown" };

        person = new Person
        {
            TmdbId = detail.Id,
            Name = detail.Name,
            Biography = detail.Biography,
            Birthday = detail.Birthday,
            Deathday = detail.Deathday,
            Gender = detail.Gender,
            PlaceOfBirth = detail.PlaceOfBirth,
            ProfilePath = detail.ProfilePath
        };

        await uow.PersonsRepository.AddAsync(person, ct);
        return person;
    }
}

internal sealed record MovieBatchResult(ContentSyncLog? Log, IList<Content> Contents, string Status);
