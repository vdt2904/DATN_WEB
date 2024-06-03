using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DATNWEB.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();

        [HttpGet]
        public IActionResult category(string id, int? page)
        {
            const int pageSize = 15;
            var idanimes = (from a in db.Animes
                            join f in db.FilmGenres on a.AnimeId equals f.AnimeId
                            where f.GenreId == id
                            orderby a.BroadcastSchedule descending
                            select new
                            {
                                a.AnimeId,
                                a.AnimeName,
                                a.ImageVUrl,
                                a.TotalEpisode,
                                Genres = (
                                    from fg in db.FilmGenres
                                    join g in db.Genres on fg.GenreId equals g.GenreId
                                    where fg.AnimeId == a.AnimeId
                                    select new
                                    {
                                        GenreId = g.GenreId,
                                        GenreName = g.GenreName
                                    }
                                ).ToList(),
                                a.Permission
                            }).ToList();
            var totalc = (from a in idanimes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                          from e in episodeGroup.DefaultIfEmpty()
                          join c in db.Comments on e != null ? e.EpisodeId : null equals c.EpisodeId into commentGroup
                          from c in commentGroup.DefaultIfEmpty()
                          group new { a, e, c } by new
                          {
                              a.AnimeId,
                          } into grouped
                          select new
                          {
                              AnimeId = grouped.Key.AnimeId,
                              Totalc = grouped.Sum(x => x.c != null ? 1 : 0),
                          }).ToList();

            var totalv = (from a in idanimes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                          from e in episodeGroup.DefaultIfEmpty()
                          join v in db.Views on e != null ? e.EpisodeId : null equals v.EpisodeId into viewGroup
                          from v in viewGroup.DefaultIfEmpty()
                          where v == null || v.IsView == 1
                          group new { a, e, v } by new
                          {
                              a.AnimeId,
                          } into grouped
                          select new
                          {
                              AnimeId = grouped.Key.AnimeId,
                              Total = grouped.Sum(x => x.v != null ? 1 : 0),
                          }).ToList();
            var maxEpisodes = (from a in idanimes
                               join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                               from e in episodeGroup.DefaultIfEmpty()
                               group e by new
                               {
                                   AnimeId = a.AnimeId,
                               } into grouped
                               select new
                               {
                                   AnimeId = grouped.Key.AnimeId,
                                   MaxEpisode = grouped.Any() ? grouped.Max(x => x != null ? x.Ep : -1) : -1,
                               }).ToList();
            var animes = (from a in idanimes
                          join e in maxEpisodes on a.AnimeId equals e.AnimeId
                          join v in totalv on a.AnimeId equals v.AnimeId
                          join c in totalc on a.AnimeId equals c.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageVUrl,
                              a.TotalEpisode,
                              Maxep = e.MaxEpisode,
                              v.Total,
                              c.Totalc,
                              a.Genres,
                              a.Permission
                          }).Distinct().ToList();
            var totalAnimes = animes.Count;
            var totalPages = (int)Math.Ceiling(totalAnimes / (double)pageSize);
            var pageNumber = page ?? 1;
            var pagedAnimes = animes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var paginationInfo = new
            {
                TotalPages = totalPages,
                CurrentPage = pageNumber
            };

            return Ok(new { PagedAnimes = pagedAnimes, PaginationInfo = paginationInfo });
        }
        [Route("trending")]
        [HttpGet]
        public IActionResult trending(int? page)
        {
            const int pageSize = 15;
            DateTime today = DateTime.UtcNow.AddHours(7);
            DateTime oneWeekAgo = today.AddDays(-60);
            var idanimes = (from a in db.Animes
                            join e in db.Episodes on a.AnimeId equals e.AnimeId
                            join v in db.Views on e.EpisodeId equals v.EpisodeId
                            where v.ViewDate.HasValue && v.ViewDate.Value.Date >= oneWeekAgo.Date && v.ViewDate.Value.Date <= today.Date && v.IsView == 1
                            group new { a, e, v } by new
                            {
                                a.AnimeId,
                                a.TotalEpisode,
                                a.ImageVUrl,
                                a.AnimeName,
                                a.Permission
                            } into grouped
                            orderby grouped.Count() descending
                            select new
                            {
                                AnimeId = grouped.Key.AnimeId,
                                TotalEpisode = grouped.Key.TotalEpisode,
                                AnimeName = grouped.Key.AnimeName,
                                ImageVUrl = grouped.Key.ImageVUrl,
                                Permission = grouped.Key.Permission
                            }
                         ).Distinct().ToList();
            var newid = (from a in idanimes
                         select new
                         {
                             a.AnimeId,
                             a.TotalEpisode,
                             a.ImageVUrl,
                             a.AnimeName,
                             Genres = (
                               from fg in db.FilmGenres
                               join g in db.Genres on fg.GenreId equals g.GenreId
                               where fg.AnimeId == a.AnimeId
                               select new
                               {
                                   GenreId = g.GenreId,
                                   GenreName = g.GenreName
                               }
                           ).Distinct().ToList(),
                             a.Permission
                         }).Distinct().ToList();
            var totalv = (from a in db.Animes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId
                          join v in db.Views on e.EpisodeId equals v.EpisodeId
                          where v.IsView == 1
                          group new { a, e, v } by new
                          {
                              a.AnimeId,
                          } into grouped
                          orderby grouped.Count() descending
                          select new
                          {
                              AnimeId = grouped.Key.AnimeId,
                              Total = grouped.Count(),
                          }
                         ).ToList();
            var totalc = (from a in db.Animes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId
                          join v in db.Comments on e.EpisodeId equals v.EpisodeId into commentGroup
                          from c in commentGroup.DefaultIfEmpty()
                          group new { a, e, c } by new
                          {
                              a.AnimeId,
                          } into grouped
                          select new
                          {
                              AnimeId = grouped.Key.AnimeId,
                              Totalc = grouped.Sum(x => x.c != null ? 1 : 0),
                          }
             ).ToList();
            var episodesList = db.Episodes.ToList();

            var animes = (from a in newid
                          join e in episodesList on a.AnimeId equals e.AnimeId
                          join t in totalv on a.AnimeId equals t.AnimeId
                          join c in totalc on a.AnimeId equals c.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageVUrl,
                              a.TotalEpisode,
                              Maxep = db.Episodes.Where(ep => ep.AnimeId == a.AnimeId).Max(ep => ep.Ep),
                              t.Total,
                              c.Totalc,
                              a.Genres,
                              a.Permission
                          }
                         ).Distinct().ToList();
            var totalAnimes = animes.Count;
            var totalPages = (int)Math.Ceiling(totalAnimes / (double)pageSize);
            var pageNumber = page ?? 1;
            var pagedAnimes = animes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var paginationInfo = new
            {
                TotalPages = totalPages,
                CurrentPage = pageNumber
            };
            return Ok(new { PagedAnimes = pagedAnimes, PaginationInfo = paginationInfo });
        }
        [Route("popular")]
        [HttpGet]
        public IActionResult popular(int? page)
        {
            const int pageSize = 15;
            var idanimes = (from a in db.Animes
                            join e in db.Episodes on a.AnimeId equals e.AnimeId
                            join v in db.Views on e.EpisodeId equals v.EpisodeId into viewGroup
                            from v in viewGroup.DefaultIfEmpty()
                            group new { a, e, v } by new
                            {
                                a.AnimeId,
                                a.TotalEpisode,
                                a.ImageVUrl,
                                a.AnimeName,
                                a.Permission
                            } into grouped
                            orderby grouped.Sum(x => x.v != null && x.v.IsView == 1 ? 1 : 0) descending
                            select new
                            {
                                AnimeId = grouped.Key.AnimeId,
                                TotalEpisode = grouped.Key.TotalEpisode,
                                AnimeName = grouped.Key.AnimeName,
                                ImageVUrl = grouped.Key.ImageVUrl,
                                Total = grouped.Sum(x => x.v != null && x.v.IsView == 1 ? 1 : 0),
                                Permission = grouped.Key.Permission
                            }
             ).Take(6).ToList();

            var totalc = (from a in db.Animes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId
                          join v in db.Comments on e.EpisodeId equals v.EpisodeId into commentGroup
                          from c in commentGroup.DefaultIfEmpty()
                          group new { a, e, c } by new
                          {
                              a.AnimeId,
                          } into grouped
                          select new
                          {
                              AnimeId = grouped.Key.AnimeId,
                              Totalc = grouped.Sum(x => x.c != null ? 1 : 0),
                          }
             ).ToList();
            var animes = (from a in idanimes
                          join t in totalc on a.AnimeId equals t.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageVUrl,
                              a.TotalEpisode,
                              Maxep = db.Episodes.Where(ep => ep.AnimeId == a.AnimeId).Max(ep => ep.Ep),
                              a.Total,
                              t.Totalc,
                              Genres = (
                                    from fg in db.FilmGenres
                                    join g in db.Genres on fg.GenreId equals g.GenreId
                                    where fg.AnimeId == a.AnimeId
                                    select new
                                    {
                                        GenreId = g.GenreId,
                                        GenreName = g.GenreName
                                    }
                                ).ToList(),
                              a.Permission
                          }
                         ).Distinct().ToList();
            var totalAnimes = animes.Count;
            var totalPages = (int)Math.Ceiling(totalAnimes / (double)pageSize);
            var pageNumber = page ?? 1;
            var pagedAnimes = animes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var paginationInfo = new
            {
                TotalPages = totalPages,
                CurrentPage = pageNumber
            };
            return Ok(new { PagedAnimes = pagedAnimes, PaginationInfo = paginationInfo });
        }
        [Route("recently")]
        [HttpGet]
        public IActionResult recently(int? page)
        {
            const int pageSize = 15;
            var idanimes = db.Animes.OrderByDescending(x => x.BroadcastSchedule).ToList();
            var totalc = (from a in idanimes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                          from e in episodeGroup.DefaultIfEmpty()
                          join c in db.Comments on e != null ? e.EpisodeId : null equals c.EpisodeId into commentGroup
                          from c in commentGroup.DefaultIfEmpty()
                          group new { a, e, c } by new
                          {
                              a.AnimeId,
                          } into grouped
                          select new
                          {
                              AnimeId = grouped.Key.AnimeId,
                              Totalc = grouped.Sum(x => x.c != null ? 1 : 0),
                          }).ToList();

            var totalv = (from a in idanimes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                          from e in episodeGroup.DefaultIfEmpty()
                          join v in db.Views on e != null ? e.EpisodeId : null equals v.EpisodeId into viewGroup
                          from v in viewGroup.DefaultIfEmpty()
                          group new { a, e, v } by new
                          {
                              a.AnimeId,
                          } into grouped
                          select new
                          {
                              AnimeId = grouped.Key.AnimeId,
                              Total = grouped.Sum(x => x.v != null && x.v.IsView == 1 ? 1 : 0),
                          }).ToList();
            var maxEpisodes = (from a in idanimes
                               join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                               from e in episodeGroup.DefaultIfEmpty()
                               group e by new
                               {
                                   AnimeId = a.AnimeId,
                               } into grouped
                               select new
                               {
                                   AnimeId = grouped.Key.AnimeId,
                                   MaxEpisode = grouped.Any() ? grouped.Max(x => x != null ? x.Ep : -1) : -1,
                               }).ToList();
            var animes = (from a in idanimes
                          join e in maxEpisodes on a.AnimeId equals e.AnimeId
                          join v in totalv on a.AnimeId equals v.AnimeId
                          join c in totalc on a.AnimeId equals c.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageVUrl,
                              a.TotalEpisode,
                              Maxep = e.MaxEpisode,
                              v.Total,
                              c.Totalc,
                              Genres = (
                                      from fg in db.FilmGenres
                                      join g in db.Genres on fg.GenreId equals g.GenreId
                                      where fg.AnimeId == a.AnimeId
                                      select new
                                      {
                                          GenreId = g.GenreId,
                                          GenreName = g.GenreName
                                      }
                                  ).ToList(),
                              a.Permission
                          }).Distinct().ToList();
            var totalAnimes = animes.Count;
            var totalPages = (int)Math.Ceiling(totalAnimes / (double)pageSize);
            var pageNumber = page ?? 1;
            var pagedAnimes = animes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var paginationInfo = new
            {
                TotalPages = totalPages,
                CurrentPage = pageNumber
            };
            return Ok(new { PagedAnimes = pagedAnimes, PaginationInfo = paginationInfo });
        }
    }

}
