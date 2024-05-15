using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DATNWEB.Models;
using Microsoft.AspNetCore.Authorization;

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult advertisement()
        {
            var season = db.Seasons.OrderByDescending(x => x.PostingDate).FirstOrDefault();
            var animes = (from a in db.Animes
                          join e in db.Episodes on a.AnimeId equals e.AnimeId
                          where a.SeasonId == season.SeasonId
                          select new
                          {
                              AnimeId = a.AnimeId,
                              Img = a.ImageHUrl,
                              info = a.Information,
                              epid = e.EpisodeId,
                              a.AnimeName,
                              a.Permission,
                              genreid = (
                                 from fg in db.FilmGenres
                                 join g in db.Genres on fg.GenreId equals g.GenreId
                                 where fg.AnimeId == a.AnimeId
                                 select new
                                 {
                                     GenreId = g.GenreId,
                                     GenreName = g.GenreName
                                 }
                             ).ToList()
                          }).ToList();
            var randomAnimes = animes.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
            return Ok(randomAnimes);
        }
        [Route("topday")]
        [HttpGet]
        public IActionResult topday()
        {
            DateTime today = DateTime.Now;
            var idanimes = (from a in db.Animes
                            join e in db.Episodes on a.AnimeId equals e.AnimeId
                            join v in db.Views on e.EpisodeId equals v.EpisodeId
                            where v.ViewDate.HasValue && v.ViewDate.Value.Date == today.Date && v.IsView == 1
                            group new { a, e, v } by new
                            {
                                a.AnimeId,
                                a.TotalEpisode,
                                a.ImageHUrl,
                                a.AnimeName,
                                a.Permission
                            } into grouped
                            orderby grouped.Count() descending
                            select new
                            {
                                AnimeId = grouped.Key.AnimeId,
                                TotalEpisode = grouped.Key.TotalEpisode,
                                AnimeName = grouped.Key.AnimeName,
                                ImageHUrl = grouped.Key.ImageHUrl,
                                Permission = grouped.Key.Permission
                            }
                         ).Take(5).ToList();
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
            var episodesList = db.Episodes.ToList();
            var animes = (from a in idanimes
                          join e in episodesList on a.AnimeId equals e.AnimeId
                          join t in totalv on a.AnimeId equals t.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageHUrl,
                              a.TotalEpisode,
                              a.Permission,
                              Maxep = db.Episodes.Where(ep => ep.AnimeId == a.AnimeId).Max(ep => ep.Ep),
                              t.Total
                          }
                         ).Distinct().ToList();
            return Ok(animes);
        }
        [Route("topweek")]
        [HttpGet]
        public IActionResult topweek()
        {
            DateTime today = DateTime.Now;
            DateTime oneWeekAgo = today.AddDays(-7);
            var idanimes = (from a in db.Animes
                            join e in db.Episodes on a.AnimeId equals e.AnimeId
                            join v in db.Views on e.EpisodeId equals v.EpisodeId
                            where v.ViewDate.HasValue && v.ViewDate.Value.Date >= oneWeekAgo.Date && v.ViewDate.Value.Date <= today.Date && v.IsView == 1
                            group new { a, e, v } by new
                            {
                                a.AnimeId,
                                a.TotalEpisode,
                                a.ImageHUrl,
                                a.AnimeName,
                                a.Permission
                            } into grouped
                            orderby grouped.Count() descending
                            select new
                            {
                                AnimeId = grouped.Key.AnimeId,
                                TotalEpisode = grouped.Key.TotalEpisode,
                                AnimeName = grouped.Key.AnimeName,
                                ImageHUrl = grouped.Key.ImageHUrl,
                                Permission = grouped.Key.Permission
                            }
                         ).Take(5).ToList();
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
            var episodesList = db.Episodes.ToList();
            var animes = (from a in idanimes
                          join e in episodesList on a.AnimeId equals e.AnimeId
                          join t in totalv on a.AnimeId equals t.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageHUrl,
                              a.TotalEpisode,
                              Maxep = db.Episodes.Where(ep => ep.AnimeId == a.AnimeId).Max(ep => ep.Ep),
                              t.Total,
                              a.Permission
                          }
                         ).Distinct().ToList();
            return Ok(animes);
        }
        [Route("topmonth")]
        [HttpGet]
        public IActionResult topmonth()
        {
            DateTime today = DateTime.Now;
            DateTime oneWeekAgo = today.AddDays(-30);
            var idanimes = (from a in db.Animes
                            join e in db.Episodes on a.AnimeId equals e.AnimeId
                            join v in db.Views on e.EpisodeId equals v.EpisodeId
                            where v.ViewDate.HasValue && v.ViewDate.Value.Date >= oneWeekAgo.Date && v.ViewDate.Value.Date <= today.Date && v.IsView == 1
                            group new { a, e, v } by new
                            {
                                a.AnimeId,
                                a.TotalEpisode,
                                a.ImageHUrl,
                                a.AnimeName,
                                a.Permission
                            } into grouped
                            orderby grouped.Count() descending
                            select new
                            {
                                AnimeId = grouped.Key.AnimeId,
                                TotalEpisode = grouped.Key.TotalEpisode,
                                AnimeName = grouped.Key.AnimeName,
                                ImageHUrl = grouped.Key.ImageHUrl,
                                Permission = grouped.Key.Permission
                            }
                         ).Take(5).ToList();
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
            var episodesList = db.Episodes.ToList();
            var animes = (from a in idanimes
                          join e in episodesList on a.AnimeId equals e.AnimeId
                          join t in totalv on a.AnimeId equals t.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageHUrl,
                              a.TotalEpisode,
                              Maxep = db.Episodes.Where(ep => ep.AnimeId == a.AnimeId).Max(ep => ep.Ep),
                              t.Total,
                              a.Permission
                          }
                         ).Distinct().ToList();
            return Ok(animes);
        }
        [Route("topyear")]
        [HttpGet]
        public IActionResult topyear()
        {
            DateTime today = DateTime.Now;
            DateTime oneWeekAgo = today.AddDays(-365);
            var idanimes = (from a in db.Animes
                            join e in db.Episodes on a.AnimeId equals e.AnimeId
                            join v in db.Views on e.EpisodeId equals v.EpisodeId
                            where v.ViewDate.HasValue && v.ViewDate.Value.Date >= oneWeekAgo.Date && v.ViewDate.Value.Date <= today.Date && v.IsView == 1
                            group new { a, e, v } by new
                            {
                                a.AnimeId,
                                a.TotalEpisode,
                                a.ImageHUrl,
                                a.AnimeName,
                                a.Permission
                            } into grouped
                            orderby grouped.Count() descending
                            select new
                            {
                                AnimeId = grouped.Key.AnimeId,
                                TotalEpisode = grouped.Key.TotalEpisode,
                                AnimeName = grouped.Key.AnimeName,
                                ImageHUrl = grouped.Key.ImageHUrl,
                                Permission = grouped.Key.Permission
                            }
                         ).Take(5).ToList();
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
            var episodesList = db.Episodes.ToList();
            var animes = (from a in idanimes
                          join e in episodesList on a.AnimeId equals e.AnimeId
                          join t in totalv on a.AnimeId equals t.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageHUrl,
                              a.TotalEpisode,
                              Maxep = db.Episodes.Where(ep => ep.AnimeId == a.AnimeId).Max(ep => ep.Ep),
                              t.Total,
                              a.Permission
                          }
                         ).Distinct().ToList();
            return Ok(animes);
        }
        [Route("trending")]
        [HttpGet]
        public IActionResult trending()
        {
            DateTime today = DateTime.Now;
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
                         ).Distinct().Take(6).ToList();
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
            return Ok(animes);
        }
        [Route("popular")]
        [HttpGet]
        public IActionResult popular()
        {
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
            return Ok(animes);
        }
        [Route("recently")]
        [HttpGet]
        public IActionResult recently()
        {
            var idanimes = db.Animes.OrderByDescending(x => x.BroadcastSchedule).Take(6).ToList();
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
            return Ok(animes);
        }
        [Route("newcomment")]
        [HttpGet]
        public IActionResult newcomment()
        {
            var idanimes = (from a in db.Animes
                            join e in db.Episodes on a.AnimeId equals e.AnimeId
                            join c in db.Comments on e.EpisodeId equals c.EpisodeId
                            orderby c.CommentDate descending
                            select new
                            {
                                a.Permission,
                                a.AnimeId,
                                a.AnimeName,
                                a.ImageVUrl,
                                Genres = (
                                    from fg in db.FilmGenres
                                    join g in db.Genres on fg.GenreId equals g.GenreId
                                    where fg.AnimeId == a.AnimeId
                                    select new
                                    {
                                        GenreId = g.GenreId,
                                        GenreName = g.GenreName
                                    }
                                ).ToList()
                            }).GroupBy(a => new { a.AnimeId, a.AnimeName, a.ImageVUrl })
                              .Select(group => group.First())
                              .Take(6)
                              .ToList();

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

            var animes = (from a in idanimes
                          join v in totalv on a.AnimeId equals v.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageVUrl,
                              Genres = a.Genres,
                              TotalViews = v.Total,
                              a.Permission
                          }).ToList();

            return Ok(animes);
        }
    }
}
