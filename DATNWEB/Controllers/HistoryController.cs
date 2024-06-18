using DATNWEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();

        [HttpGet]
        public IActionResult history(int? page)
        {
            const int pageSize = 15;
            var sessionInfoJson = HttpContext.Session.GetString("SessionInfo");
            var sessionInfo = JsonConvert.DeserializeObject<dynamic>(sessionInfoJson);
            string userId = sessionInfo.UID;

            var latestViews = (
                                from v in db.Views
                                join e in db.Episodes on v.EpisodeId equals e.EpisodeId
                                join a in db.Animes on e.AnimeId equals a.AnimeId
                                where v.UserId == userId
                                group new { v, e, a } by e.AnimeId into g
                                select new
                                {
                                    AnimeId = g.Key,
                                    EpisodeId = g.OrderByDescending(vea => vea.v.ViewDate).FirstOrDefault().e.EpisodeId,
                                    Ep = g.OrderByDescending(vea => vea.v.ViewDate).FirstOrDefault().e.Ep,
                                    max_ngayxem = g.Max(vea => vea.v.ViewDate),
                                    TotalEpisode = g.FirstOrDefault().a.TotalEpisode,
                                    ImageVUrl = g.FirstOrDefault().a.ImageVUrl,
                                    AnimeName = g.FirstOrDefault().a.AnimeName,
                                    Permission = g.FirstOrDefault().a.Permission,
                                    Genres = (
                                           from fg in db.FilmGenres
                                           join ge in db.Genres on fg.GenreId equals ge.GenreId
                                           where fg.AnimeId == g.Key
                                           select new
                                           {
                                               GenreId = ge.GenreId,
                                               GenreName = ge.GenreName
                                           }
                                       ).Distinct().ToList(),
                                }
                            ).ToList();
            var idanimes = latestViews.OrderByDescending(x => x.max_ngayxem).ToList();

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
                              a.Genres,
                              a.Permission,
                              a.max_ngayxem,
                              a.Ep
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
           // return Ok(idanimes);
        }
    }
}
