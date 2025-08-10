namespace FilmProject.Models.SearchMovieApi
{
    public class Rootobject
    {
        public string status { get; set; }
        public string msg { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public Seoonpage seoOnPage { get; set; }
        public Breadcrumb[] breadCrumb { get; set; }
        public string titlePage { get; set; }
        public object items { get; set; }
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
        public object og_image { get; set; }
        public string og_url { get; set; }
    }

    public class Params
    {
        public string type_slug { get; set; }
        public string keyword { get; set; }
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
        public bool isCurrent { get; set; }
        public int position { get; set; }
    }

}
