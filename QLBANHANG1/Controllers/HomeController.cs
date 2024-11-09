using QLBANHANG1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBANHANG1.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataClasses1DataContext db = new DataClasses1DataContext("Data Source=YENNGTH-0803\\MSSQLSERVER01;Initial Catalog=QLBANHANG1;Integrated Security=True");

        // Action để hiển thị trang chủ
        public ActionResult Index()
        {
            var products = db.Products.ToList(); // Lấy tất cả sản phẩm từ cơ sở dữ liệu
            return View(products); // Trả về View với danh sách sản phẩm
        }

        // Action để xử lý tìm kiếm sản phẩm
        public ActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                var products = db.Products.ToList(); // Trả về tất cả sản phẩm nếu không có tìm kiếm
                return View("Index", products); // Trả về View Index với tất cả sản phẩm
            }

            // Tìm kiếm sản phẩm theo tên sản phẩm
            var searchedProducts = db.Products
                                       .Where(p => p.ProductName.Contains(query)) // Tìm kiếm sản phẩm có tên chứa query
                                       .ToList();
            return View("Index", searchedProducts); // Trả về kết quả tìm kiếm
        }

        // Action để hiển thị chi tiết sản phẩm
        public ActionResult Details(string id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Sanpham/Details.cshtml", product); // Chỉ định đường dẫn đầy đủ
        }

    }
}