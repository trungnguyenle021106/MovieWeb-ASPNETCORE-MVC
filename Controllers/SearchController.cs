using FilmProject.Models.GeneralMovieApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FilmProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        // Sử dụng Dependency Injection cho ILogger và IHttpClientFactory
        public SearchController(ILogger<SearchController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string keyword, string? category, string? country, int? year, string? language, int page = 1)
        {
            // Kiểm tra xem keyword có rỗng không
            if (string.IsNullOrEmpty(keyword))
            {
                // Nếu keyword rỗng, bạn có thể trả về một view trống hoặc chuyển hướng đến trang chủ
                return View("Index");
            }

            // Khởi tạo HttpClient
            var httpClient = _httpClientFactory.CreateClient();
            var baseUrl = "https://phimapi.com/v1/api/tim-kiem";

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"?keyword={Uri.EscapeDataString(keyword)}");
            queryBuilder.Append($"&page={page}&limit=24");

            // Thêm các tham số bộ lọc vào query string
            if (!string.IsNullOrEmpty(category))
            {
                queryBuilder.Append($"&category={category}");
            }
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

            // API tìm kiếm cũng có thể có các tham số sort_field và sort_type nếu bạn muốn
            // queryBuilder.Append("&sort_field=_id");
            // queryBuilder.Append("&sort_type=desc");

            string apiUrl = baseUrl + queryBuilder.ToString();

            try
            {
                var movieTask = httpClient.GetStringAsync(apiUrl);
                var countriesApiUrl = "https://phimapi.com/quoc-gia";
                var countriesTask = httpClient.GetStringAsync(countriesApiUrl);

                await Task.WhenAll(movieTask, countriesTask);

                var apiResponse = await movieTask;
                var searchData = JsonConvert.DeserializeObject<Rootobject>(apiResponse);

                var countriesResponse = await countriesTask;
                var countryList = JsonConvert.DeserializeObject<List<Country>>(countriesResponse);


                if (searchData?.status == "success" && searchData.data?.items == null)
                {
                    return Content("Không tìm thấy kết quả nào cho từ khóa: " + keyword);
                }

                    if ( searchData?.status == "success" && searchData.data?.items != null)
                {
                    var cdnDomain = searchData.data.APP_DOMAIN_CDN_IMAGE;
                    foreach (var movie in searchData.data.items)
                    {
                        if (!string.IsNullOrEmpty(cdnDomain))
                        {
                            movie.thumb_url = $"{cdnDomain}/{movie.thumb_url}";
                        }
                    }

                    ViewBag.CurrentPage = searchData.data._params?.pagination?.currentPage ?? 1;
                    ViewBag.TotalPages = searchData.data._params?.pagination?.totalPages ?? 1;

                    ViewBag.CurrentKeyword = keyword;
                    ViewBag.SearchData = searchData.data;
                    ViewBag.CurrentCategory = category;
                    ViewBag.CurrentCountry = country;
                    ViewBag.CurrentYear = year;
                    ViewBag.CurrentLanguage = language;
                    ViewBag.Countries = countryList;

                    // Xử lý yêu cầu AJAX
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        _logger.LogInformation($"AJAX request received. Page: {page}, Keyword: {keyword}");
                    }
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return PartialView("_MovieListPartial", searchData.data.items.ToList());
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tìm kiếm với từ khóa: {keyword}");
            }

            return View("Error");
        }

    }
}
