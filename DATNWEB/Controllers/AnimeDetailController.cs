﻿using DATNWEB.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeDetailController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public IActionResult AnimeDetail(string id)
        {
            var idanime = db.Animes.Where(x=>x.AnimeId==id).ToList();
            if (idanime == null)
            {
                return BadRequest();
            }
            else
            {
                var totalv = (from a in db.Animes
                              join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                              from e in episodeGroup.DefaultIfEmpty()
                              join v in db.Views on e != null ? e.EpisodeId : null equals v.EpisodeId into viewGroup
                              from v in viewGroup.DefaultIfEmpty()
                              where v == null || v.IsView == 1 && a.AnimeId == id
                              group new { a, e, v } by new
                              {
                                  a.AnimeId,
                              } into grouped
                              select new
                              {
                                  AnimeId = grouped.Key.AnimeId,
                                  Total = grouped.Sum(x => x.v != null ? 1 : 0),
                              }).ToList();
                var totalc = (from a in db.Animes
                              join e in db.Episodes on a.AnimeId equals e.AnimeId into episodeGroup
                              from e in episodeGroup.DefaultIfEmpty()
                              join c in db.Comments on e != null ? e.EpisodeId : null equals c.EpisodeId into commentGroup
                              from c in commentGroup.DefaultIfEmpty()
                              where a.AnimeId == id
                              group new { a, e, c } by new
                              {
                                  a.AnimeId,
                              } into grouped
                              select new
                              {
                                  AnimeId = grouped.Key.AnimeId,
                                  Totalc = grouped.Sum(x => x.c != null ? 1 : 0),
                              }).ToList();
                var detail = (from a in idanime
                             join v in totalv on a.AnimeId equals v.AnimeId
                             join c in totalc on a.AnimeId equals c.AnimeId
                             where a.AnimeId == id
                             select new
                             {
                                 a.AnimeId,
                                 a.ImageVUrl,
                                 a.AnimeName,
                                 genre = (from fg in db.FilmGenres
                                          join g in db.Genres on fg.GenreId equals g.GenreId
                                          where fg.AnimeId == a.AnimeId
                                          select new
                                             {
                                                 GenreId = g.GenreId,
                                                 GenreName = g.GenreName
                                             }
                                         ).ToList(),
                                a.BroadcastSchedule,
                                a.Information,
                                a.Permission,
                                v.Total,
                                c.Totalc,
                                direc = db.Directors.Where(x => x.DirectorId == a.DirectorId).Select(x=>x.DirectorName).FirstOrDefault()
                             }).FirstOrDefault();
                return Ok(detail);
            }
        }
        [Route("mikelike")]
        [HttpGet]
        public IActionResult Mikelike(string id)
        {
            var genre = (from fg in db.FilmGenres
                        join g in db.Genres on fg.GenreId equals g.GenreId
                        where fg.AnimeId == id
                        select new
                        {
                            GenreId = g.GenreId,
                            GenreName = g.GenreName
                        }
                        ).ToList();
            var genreIds = genre.Select(g => g.GenreId).ToList();
            var idanime = (from a in db.Animes
                           join f in db.FilmGenres on a.AnimeId equals f.AnimeId
                           where genreIds.Contains(f.GenreId) && a.AnimeId != id
                           select new
                           {
                               a.AnimeId,
                               a.AnimeName,
                               a.ImageHUrl,
                               a.TotalEpisode,
                               a.Permission
                           }).Distinct().ToList();
            var randomidAnimes = idanime.OrderBy(x => Guid.NewGuid()).Take(4).ToList();
            var totalv = (from a in db.Animes
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
            var maxEpisodes = (from a in idanime
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
            var animes = (from a in randomidAnimes
                          join e in maxEpisodes on a.AnimeId equals e.AnimeId
                          join v in totalv on a.AnimeId equals v.AnimeId
                          select new
                          {
                              a.AnimeId,
                              a.AnimeName,
                              a.ImageHUrl,
                              a.TotalEpisode,
                              Maxep = e.MaxEpisode,
                              v.Total,
                              a.Permission
                          }).Distinct().ToList();
            return Ok(animes);
        }
        [Route("review")]
        [HttpGet]
        public IActionResult review(string id,int? page)
        {
            const int pageSize = 6;
            var reviews = db.Reviews.Where(x => x.AnimeId == id).ToList();
            var rvs = (from r in reviews
                       select new
                       {
                           r.Id,
                           r.Rating,
                           r.Timestamp,
                           r.Content,
                           User = db.Users.Where(x => x.UserId == r.UserId).Select(x=>x.Username).FirstOrDefault()
                       }).OrderByDescending(x=>x.Timestamp).ToList();
            var totalreviews = rvs.Count;
            var totalPages = (int)Math.Ceiling(totalreviews / (double)pageSize);
            var pageNumber = page ?? 1;
            var pagedReview = rvs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var paginationInfo = new
            {
                TotalPages = totalPages,
                CurrentPage = pageNumber
            };
            return Ok(new { PagedReview = pagedReview, PaginationInfo = paginationInfo });
        }
        [Route("rate")]
        [HttpGet]
        public IActionResult rate(string id)
        {
            var rate = new
            {
                TotalRating = db.Reviews.Where(x => x.AnimeId == id).Sum(x => x.Rating),
                TotalReviews = db.Reviews.Count(x => x.AnimeId == id)
            };
            return Ok(rate);
        }
        [Route("addreview")]
        [HttpPost]
        public IActionResult addreview([FromBody] Review rv)
        {
            db.Reviews.Add(rv);
            db.SaveChanges();
            return Ok(rv);
        }
    }
}
