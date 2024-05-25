using CloudinaryDotNet;
using DATNWEB.Models;
using FuzzySharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // cho EF Core
// hoặc
using System.Data; // cho EF6

namespace DATNWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        [HttpGet]
        public async Task<IActionResult> suggestsearch(string keyword)
        {
            if (keyword.Length < 3)
            {
                return BadRequest("Tối thiểu 3 ký tự");
            }
            var animes = await db.Animes
                .Select(anime => new
                {
                    anime.Permission,
                    anime.AnimeId,
                    anime.AnimeName,
                    anime.ImageVUrl,
                    Similarity = Fuzz.PartialRatio(anime.AnimeName.ToLower(), keyword.ToLower()),
                    Genres = (
                                      from fg in db.FilmGenres
                                      join g in db.Genres on fg.GenreId equals g.GenreId
                                      where fg.AnimeId == anime.AnimeId
                                      select new
                                      {
                                          GenreId = g.GenreId,
                                          GenreName = g.GenreName
                                      }
                                  ).ToList(),
                })
                .Take(15)
                .ToListAsync();
            var maxEpisodes =  (from a in animes
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
            var animess = (from a in animes
                           join e in maxEpisodes on a.AnimeId equals e.AnimeId
                           select new
                           {
                               a.Permission,
                               a.AnimeId,
                               a.AnimeName,
                               a.ImageVUrl,
                               a.Similarity,
                               a.Genres,
                               e.MaxEpisode
                           }).ToList();
            // Sắp xếp theo mức độ tương đồng giảm dần
            var result =  animess
                .Where(anime => anime.Similarity > 70) // Lọc ra các kết quả có độ tương đồng lớn hơn 70
                .OrderByDescending(anime => anime.Similarity) // Sắp xếp giảm dần theo độ tương đồng
                .ToList();           
            if (result.Count == 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("Searchdata")]
        public async Task<IActionResult> searchdata(string keyword,int? page)
        {
            const int pageSize = 15;
            var animes = await db.Animes
                .Select(anime => new
                {
                    anime.TotalEpisode,
                    anime.Permission,
                    anime.AnimeId,
                    anime.AnimeName,
                    anime.ImageVUrl,
                    Similarity = Fuzz.PartialRatio(anime.AnimeName.ToLower(), keyword.ToLower()),
                    Genres = (
                                      from fg in db.FilmGenres
                                      join g in db.Genres on fg.GenreId equals g.GenreId
                                      where fg.AnimeId == anime.AnimeId
                                      select new
                                      {
                                          GenreId = g.GenreId,
                                          GenreName = g.GenreName
                                      }
                                  ).ToList(),
                })
                .ToListAsync();
            var maxEpisodes = (from a in animes
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
            var totalc = (from a in animes
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
            var totalv = (from a in animes
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
            var animess = (from a in animes
                           join e in maxEpisodes on a.AnimeId equals e.AnimeId
                           join v in totalv on a.AnimeId equals v.AnimeId
                           join c in totalc on a.AnimeId equals c.AnimeId
                           select new
                           {
                               a.AnimeId,
                               a.AnimeName,
                               a.ImageVUrl,
                               a.Similarity,
                               a.Genres,
                               e.MaxEpisode,
                               v.Total,
                               c.Totalc,
                               a.Permission,
                               a.TotalEpisode
                           }).ToList();
            // Sắp xếp theo mức độ tương đồng giảm dần
            var result = animess
                .Where(anime => anime.Similarity > 70) // Lọc ra các kết quả có độ tương đồng lớn hơn 70
                .OrderByDescending(anime => anime.Similarity) // Sắp xếp giảm dần theo độ tương đồng
                .ToList();

            var totalAnimes = result.Count;
            var totalPages = (int)Math.Ceiling(totalAnimes / (double)pageSize);
            var pageNumber = page ?? 1;
            var pagedAnimes = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var paginationInfo = new
            {
                TotalPages = totalPages,
                CurrentPage = pageNumber
            };
            return Ok(new { PagedAnimes = pagedAnimes, PaginationInfo = paginationInfo });
        }
    }
}
