using FilmProject.Models.MovieCategoryApi;

namespace FilmProject.Models.MovieDetailApi
{
    public class Rootobject
    {
        public bool status { get; set; }
        public string msg { get; set; }

        // Trường hợp cũ: dữ liệu phim nằm ở đây
        public Movie? movie { get; set; }

        // Trường hợp mới: dữ liệu phim nằm trong data
        public Data? data { get; set; }

        public Episode[] episodes { get; set; }
    }

    // Thêm class Data để khớp với cấu trúc JSON mới
    public class Data
    {   
        public Movie item { get; set; } // Đối tượng phim nằm ở đây
    }


    public class Movie
    {
        public Tmdb tmdb { get; set; }
        public Imdb imdb { get; set; }
        public Created created { get; set; }
        public Modified modified { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string origin_name { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string poster_url { get; set; }
        public string thumb_url { get; set; }
        public bool is_copyright { get; set; }
        public bool sub_docquyen { get; set; }
        public bool chieurap { get; set; }
        public string trailer_url { get; set; }
        public string time { get; set; }
        public string episode_current { get; set; }
        public string episode_total { get; set; }
        public string quality { get; set; }
        public string lang { get; set; }
        public string notify { get; set; }
        public string showtimes { get; set; }
        public int year { get; set; }
        public int view { get; set; }
        public string[] actor { get; set; }
        public string[] director { get; set; }
        public Category[] category { get; set; }
        public Country[] country { get; set; }
    }

    public class Tmdb
    {
        public string type { get; set; }
        public string id { get; set; }
        public object season { get; set; }
        //public int vote_average { get; set; }
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

    public class Episode
    {
        public string server_name { get; set; }
        public Server_Data[] server_data { get; set; }
    }

    public class Server_Data
    {
        public string name { get; set; }
        public string slug { get; set; }
        public string filename { get; set; }
        public string link_embed { get; set; }
        public string link_m3u8 { get; set; }
    }

}


