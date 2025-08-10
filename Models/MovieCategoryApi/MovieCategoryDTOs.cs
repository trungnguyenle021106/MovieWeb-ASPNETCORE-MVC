using Newtonsoft.Json;

namespace FilmProject.Models.MovieCategoryApi
{
    public class Rootobject
    {
        public bool status { get; set; }
        public string msg { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public Seoonpage seoOnPage { get; set; }
        public Breadcrumb[] breadCrumb { get; set; }
        public string titlePage { get; set; }
        public Item[] items { get; set; }
        [JsonProperty("params")]
        public Params _params { get; set; }
        public string type_list { get; set; }
        public string APP_DOMAIN_FRONTEND { get; set; }
        public string APP_DOMAIN_CDN_IMAGE { get; set; }
    }

    public class Seoonpage
    {
        public string og_type { get; set; }
        public string titleHead { get; set; }
        public string descriptionHead { get; set; }
        public string[] og_image { get; set; }
        public string og_url { get; set; }
    }

    public class Params
    {
        public string type_slug { get; set; }
        public string slug { get; set; }
        public string[] filterCategory { get; set; }
        public string[] filterCountry { get; set; }
        public string[] filterYear { get; set; }
        public string[] filterType { get; set; }
        public string sortField { get; set; }
        public string sortType { get; set; }
        public Pagination pagination { get; set; }
    }

    public class Pagination
    {
        public int totalItems { get; set; }
        public int totalItemsPerPage { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
    }

    public class Breadcrumb
    {
        public string name { get; set; }
        public string slug { get; set; }
        public bool isCurrent { get; set; }
        public int position { get; set; }
    }

    public class Item
    {
        public Tmdb tmdb { get; set; }
        public Imdb imdb { get; set; }
        public Created created { get; set; }
        public Modified modified { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string origin_name { get; set; }
        public string type { get; set; }
        public string poster_url { get; set; }
        public string thumb_url { get; set; }
        public bool sub_docquyen { get; set; }
        public bool chieurap { get; set; }
        public string time { get; set; }
        public string episode_current { get; set; }
        public string quality { get; set; }
        public string lang { get; set; }
        public int year { get; set; }
        public Category[] category { get; set; }
        public Country[] country { get; set; }
    }

    public class Tmdb
    {
        public string type { get; set; }
        public string id { get; set; }
        public object season { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class Imdb
    {
        public object id { get; set; }
    }

    public class Created
    {
        public DateTime time { get; set; }
    }

    public class Modified
    {
        public DateTime time { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

}
