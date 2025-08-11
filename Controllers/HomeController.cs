using FilmProject.Models;
using FilmProject.Models.GeneralMovieApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FilmProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        // Sử dụng Dependency Injection cho ILogger và IHttpClientFactory
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> Index()
        {
            // Tạo HttpClient bằng IHttpClientFactory
            var _httpClient = _httpClientFactory.CreateClient();

            var categoriesToFetch = new List<(string name, string slug)>
            {
                ("Phim Hành Động", "hanh-dong"),
                ("Phim Cổ Trang", "co-trang"),
                ("Phim Chiến Tranh", "chien-tranh"),
                ("Phim Viễn Tưởng", "vien-tuong"),
                ("Phim Kinh Dị", "kinh-di"),
                ("Phim Tài Liệu", "tai-lieu"),
                ("Phim Bí Ẩn", "bi-an"),
                ("Phim Thể Thao", "the-thao"),
                ("Phim Tình Cảm", "tinh-cam"),
                ("Phim Tâm Lý", "tam-ly"),
                ("Phim Gia Đình", "gia-dinh"),
                ("Phim Phiêu Lưu", "phieu-luu"),
                ("Phim Âm Nhạc", "am-nhac"),
                ("Phim Hình Sự", "hinh-su"),
                ("Phim Học Đường", "hoc-duong"),
                ("Phim Hài Hước", "hai-huoc"),
                ("Phim Thần Thoại", "than-thoai"),
                ("Phim Võ Thuật", "vo-thuat"),
                ("Phim Khoa Học", "khoa-hoc"),
                ("Phim Chính Kịch", "chinh-kich")
            };

            var allMovieData = new Dictionary<string, Rootobject>();
            var fetchTasks = new List<Task>();

            foreach (var category in categoriesToFetch)
            {
                fetchTasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        string apiUrl = $"https://phimapi.com/v1/api/the-loai/{category.slug}?page=1&limit=8";
                        var apiResponse = await _httpClient.GetStringAsync(apiUrl);
                        var genreData = JsonConvert.DeserializeObject<Rootobject>(apiResponse);

                        if (genreData?.data?.items != null && genreData.status == "true")
                        {
                            var cdnDomain = genreData.data.APP_DOMAIN_CDN_IMAGE;
                            foreach (var movie in genreData.data.items)
                            {
                                // Cập nhật URL hình ảnh đầy đủ
                                movie.thumb_url = !string.IsNullOrEmpty(cdnDomain)
                                    ? $"{cdnDomain}/{movie.thumb_url}"
                                    : movie.thumb_url;
                            }
                            lock (allMovieData)
                            {
                                allMovieData.Add(category.name, genreData);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Lỗi khi lấy dữ liệu thể loại {category.name}.");
                    }
                }));
            }

            await Task.WhenAll(fetchTasks);

            // Gán Dictionary vào ViewBag
            ViewBag.AllMovieData = allMovieData;

            // Trả về View mà không có model
            return View();
        }

        public IActionResult PhimBo()
        {
            // Logic ?? l?y danh sách phim b? t? API
            return View();
        }


        public IActionResult PhimLe()
        {
            // Logic ?? l?y danh sách phim l? t? API
            return View();
        }

        public IActionResult HoatHinh()
        {
            // Logic ?? l?y danh sách phim ho?t hình t? API
            return View();
        }
    }
}


