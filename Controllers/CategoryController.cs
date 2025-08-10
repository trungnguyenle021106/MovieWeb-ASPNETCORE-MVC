using FilmProject.Models.GeneralMovieApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FilmProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryController(ILogger<CategoryController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string slug, string? country, int? year, string? language, int page = 1)
        {
            // === DEBUG: Hiển thị các tham số đầu vào của phương thức Index ===
            //Console.WriteLine("--- Bắt đầu phương thức Index ---");
            //Console.WriteLine($"Tham số slug: {slug}");
            //Console.WriteLine($"Tham số country: {country}");
            //Console.WriteLine($"Tham số year: {year}");
            //Console.WriteLine($"Tham số language: {language}");
            //Console.WriteLine("----------------------------------");

            if (string.IsNullOrEmpty(slug))
            {
                Console.WriteLine("Lỗi: Slug rỗng, trả về NotFound.");
                return NotFound();
            }

            // Danh sách thể loại mặc định
            var categoriesToFetch = new List<(string name, string slug)>
            {
                ("Hành Động", "hanh-dong"),
                ("Cổ Trang", "co-trang"),
                ("Chiến Tranh", "chien-tranh"),
                ("Viễn Tưởng", "vien-tuong"),
                ("Kinh Dị", "kinh-di"),
                ("Tài Liệu", "tai-lieu"),
                ("Bí Ẩn", "bi-an"),
                ("Thể Thao", "the-thao"),
                ("Tình Cảm", "tinh-cam"),
                ("Tâm Lý", "tam-ly"),
                ("Gia Đình", "gia-dinh"),
                ("Phiêu Lưu", "phieu-luu"),
                ("Âm Nhạc", "am-nhac"),
                ("Hình Sự", "hinh-su"),
                ("Học Đường", "hoc-duong"),
                ("Hài Hước", "hai-huoc"),
                ("Thần Thoại", "than-thoai"),
                ("Võ Thuật", "vo-thuat"),
                ("Khoa Học", "khoa-hoc"),
                ("Chính Kịch", "chinh-kich")
            };

            var httpClient = _httpClientFactory.CreateClient();
            var baseUrl = $"https://phimapi.com/v1/api/the-loai/{slug}";

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"?page={page}&limit=24");


            if (!string.IsNullOrEmpty(country))
            {
                queryBuilder.Append($"&country={country}");
            }
            if (year.HasValue && year.Value > 0)
            {
                queryBuilder.Append($"&year={year.Value}");
            }
            if (!string.IsNullOrEmpty(language))
            {
                queryBuilder.Append($"&sort_lang={language}");
            }

            string apiUrl = baseUrl + queryBuilder.ToString();

            // === DEBUG: Hiển thị apiUrl trước khi gọi API ===
            //Console.WriteLine($"API URL đã xây dựng: {apiUrl}");
            //Console.WriteLine("----------------------------------");

            try
            {
                var movieTask = httpClient.GetStringAsync(apiUrl);
                var countriesApiUrl = "https://phimapi.com/quoc-gia";
                var countriesTask = httpClient.GetStringAsync(countriesApiUrl);

                await Task.WhenAll(movieTask, countriesTask);

                var apiResponse = await movieTask;
                var categoryData = JsonConvert.DeserializeObject<Rootobject>(apiResponse);

                var countriesResponse = await countriesTask;
                var countryList = JsonConvert.DeserializeObject<List<Country>>(countriesResponse);
                //Console.WriteLine($"categoryData.data is null: {categoryData.data == null}");
                if (categoryData.data != null)
                {
                    Console.WriteLine($"categoryData.data._params is null: {categoryData.data._params == null}");
                    if (categoryData.data._params != null)
                    {
                        Console.WriteLine($"categoryData.data._params.pagination is null: {categoryData.data._params.pagination == null}");
                    }
                }

                // In ra giá trị totalPages để kiểm tra
                // Sử dụng null-conditional operator để tránh lỗi NullReferenceException
                //Console.WriteLine($"Total pages from API: {categoryData.data?._params?.pagination?.totalPages ?? 1}");

                if (categoryData?.status == "true" && categoryData.data?.items != null)
                {
                    string categoryName = categoriesToFetch.FirstOrDefault(c => c.slug == slug).name ?? "Phim theo thể loại";

                    // === DEBUG: Hiển thị tên thể loại đã được xác định ===
                    //Console.WriteLine($"Tên thể loại đã được tìm thấy: {categoryName}");
                    //Console.WriteLine("----------------------------------");

               

                    var cdnDomain = categoryData.data.APP_DOMAIN_CDN_IMAGE;
                    foreach (var movie in categoryData.data.items)
                    {
                        if (!string.IsNullOrEmpty(cdnDomain))
                        {
                            movie.thumb_url = $"{cdnDomain}/{movie.thumb_url}";
                        }
                    }

                    // Gán toàn bộ dữ liệu vào ViewBag
                    ViewBag.CurrentPage = categoryData.data._params?.pagination?.currentPage ?? 1;
                    ViewBag.TotalPages = categoryData.data._params?.pagination?.totalPages ?? 1;                   

                    ViewBag.CategoriesToFilter = categoriesToFetch;
                    ViewBag.CategoryData = categoryData.data;
                    ViewBag.CategoryName = categoryName;
                    ViewBag.CurrentSlug = slug;
                    ViewBag.CurrentCountry = country;
                    ViewBag.CurrentYear = year;
                    ViewBag.CurrentLanguage = language;
                    ViewBag.Countries = countryList;

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return PartialView("_MovieListPartial", categoryData.data.items.ToList());
                    }
                    //Console.WriteLine("Kết thúc phương thức Index thành công.");
                    return View();
                }
                else
                {
                    Console.WriteLine("Lỗi: Dữ liệu API không hợp lệ hoặc rỗng.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy dữ liệu thể loại phim với slug: {slug}");
                Console.WriteLine($"Lỗi trong quá trình xử lý: {ex.Message}");
            }

            Console.WriteLine("Trả về NotFound.");
            return NotFound();
        }

    }
}
