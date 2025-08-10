using FilmProject.Models.MovieDetailApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class MovieDetailController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MovieDetailController> _logger;

    public MovieDetailController(IHttpClientFactory httpClientFactory, ILogger<MovieDetailController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var httpClient = _httpClientFactory.CreateClient();
        var movieDetailUrl = $"https://phimapi.com/phim/{slug}";

        try
        {
            var apiResponse = await httpClient.GetStringAsync(movieDetailUrl);

            // Sử dụng class Rootobject để deserialize JSON
            var movieDetail = JsonConvert.DeserializeObject<Rootobject>(apiResponse);
            Console.WriteLine(movieDetail);
            Movie filmObject = movieDetail.movie ?? movieDetail.data?.item;

            if (movieDetail?.status == true && filmObject != null)
            {
                return View(movieDetail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Lỗi khi lấy dữ liệu phim với slug: {slug}");
        }

        return NotFound();
    }

    public async Task<IActionResult> WatchMovie(string slug, string episodeId)
    {
        if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(episodeId))
        {
            return NotFound();
        }

        var httpClient = _httpClientFactory.CreateClient();
        var movieDetailUrl = $"https://phimapi.com/phim/{slug}";

        try
        {
            var apiResponse = await httpClient.GetStringAsync(movieDetailUrl);
            var movieData = JsonConvert.DeserializeObject<Rootobject>(apiResponse);

            // Lấy đối tượng phim từ một trong hai vị trí
            var movieDetail = movieData?.movie ?? movieData?.data?.item;

            if (movieDetail == null)
            {
                return NotFound();
            }

            // Tìm kiếm link embed của tập phim
            var currentEpisode = movieData.episodes
                ?.SelectMany(server => server.server_data)
                .FirstOrDefault(ep => ep.slug == episodeId);

            if (currentEpisode == null)
            {
                return NotFound();
            }

            // Đóng gói dữ liệu cần thiết cho View vào ViewBag
            ViewBag.MovieDetail = movieDetail;
            ViewBag.CurrentEpisodeLink = currentEpisode.link_embed;
            ViewBag.CurrentEpisodeId = episodeId;
            ViewBag.Episodes = movieData.episodes; // Truyền danh sách tất cả các tập

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Lỗi khi lấy dữ liệu xem phim với slug: {slug} và episodeId: {episodeId}");
        }

        return NotFound();
    }
}