using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace FilmProject.ViewComponents
{
    [ViewComponent(Name = "CategoryList")]
    public class CategoryListComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Danh sách thể loại mặc định
            var genres = new List<(string name, string slug)>
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

            // Truyền danh sách thể loại này vào View của component
            return View(genres);
        }
    }
}
