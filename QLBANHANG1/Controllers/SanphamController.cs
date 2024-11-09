using QLBANHANG1.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBANHANG1.Controllers
{

    public class SanphamController : Controller
    {
        private readonly DataClasses1DataContext db = new DataClasses1DataContext("Data Source=YENNGTH-0803\\MSSQLSERVER01;Initial Catalog=QLBANHANG1;Integrated Security=True");

        // GET: Sanpham
        public ActionResult Index()

        {
            var products = db.Products.ToList();
            return View(products);
        }
        // Hiển thị trang thêm sản phẩm
        public ActionResult Create()
        {
            var categories = db.Categories.ToList(); // Lấy danh sách danh mục từ cơ sở dữ liệu
            ViewBag.Categories = categories; // Truyền danh sách danh mục vào ViewBag

            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]

        public ActionResult Create(FormCollection form)
        {
            using (var db = new DataClasses1DataContext("Data Source=YENNGTH-0803\\MSSQLSERVER01; Initial Catalog=QLBANHANG1; Integrated Security=True"))
            {
                // Lấy giá trị từ form
                string ProductID = form["ProductID"];
                string ProductName = form["ProductName"];
                string Description = form["Description"];
                string Price = form["Price"];
                string StockQuantity = form["StockQuantity"];
                string CategoryID = form["CategoryID"];
                string ImageUrl = form["ImageUrl"]; // Đảm bảo tên trường đúng với View

                // Chuyển đổi giá trị từ string sang kiểu dữ liệu phù hợp
                Decimal Gia = Convert.ToDecimal(Price);
                int Soluong = Convert.ToInt32(StockQuantity);

                // Tạo đối tượng sản phẩm mới
                Product newObj = new Product
                {
                    ProductID = ProductID,
                    ProductName = ProductName,
                    Description = Description,
                    Price = Gia,
                    StockQuantity = Soluong,
                    CategoryID = CategoryID,
                    ImageUrl = ImageUrl,
                    CreatedAt = DateTime.Now
                };
                // Kiểm tra xem tất cả các trường bắt buộc đã được điền đầy đủ chưa
                if (string.IsNullOrEmpty(newObj.ProductID) ||
                    string.IsNullOrEmpty(newObj.ProductName) ||
                    string.IsNullOrEmpty(newObj.Description) ||
                    newObj.Price <= 0 ||
                    newObj.StockQuantity < 0 ||
                    string.IsNullOrEmpty(newObj.CategoryID))
                {
                    ViewBag.ErrorMessage = "Bạn hãy nhập đầy đủ thông tin.";
                    var categories = db.Categories.ToList();
                    ViewBag.Categories = categories;
                    return View(newObj); // Trả lại view với dữ liệu đã nhập và lỗi
                }

                // Kiểm tra xem ProductID đã tồn tại chưa
                bool isProductIdExists = db.Products.Any(p => p.ProductID == newObj.ProductID);
                if (isProductIdExists)
                {
                    ModelState.AddModelError("", "ID bị trùng, hãy nhập lại");
                    var categories = db.Categories.ToList();
                    ViewBag.Categories = categories;
                    return View(newObj); // Trả lại view với dữ liệu đã nhập và lỗi
                }


                // Thêm sản phẩm vào cơ sở dữ liệu
                db.Products.InsertOnSubmit(newObj);
                db.SubmitChanges();




                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
        }
        public ActionResult Edit()
        {
            string abc = Request.QueryString["id"];

            Product editObj = db.Products.Where(o => o.ProductID == abc).FirstOrDefault();
            //lay ban ghi can sua va gui thong tin qua view
            if (editObj == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm cần sửa.";
                return RedirectToAction("Index");
            }
            //var categories = db.Categories.ToList(); // Lấy danh sách danh mục từ cơ sở dữ liệu
            ViewBag.Categories = db.Categories.ToList(); // Truyền danh sách danh mục vào ViewBag

            return View(editObj);//truyen qua doi tuong co ten editObj trong view
        }
 

        public ActionResult PostEdit(FormCollection form)
        {
            // Lấy giá trị từ form 
            string ProductID = form["ProductID"];
            string ProductName = form["ProductName"];
            string Description = form["Description"];
            string Price = form["Price"];
            string StockQuantity = form["StockQuantity"];
            string CategoryID = form["CategoryID"];
            string ImageUrl = form["ImageUrl"];

            // Chuyển đổi các giá trị từ chuỗi sang các kiểu dữ liệu phù hợp
            decimal Gia = Convert.ToDecimal(Price);
            int Soluong = Convert.ToInt32(StockQuantity);

            // Tìm sản phẩm hiện tại theo ID
            Product editObj = db.Products.FirstOrDefault(p => p.ProductID == ProductID);

            if (editObj != null)
            {
                // Cập nhật các thuộc tính với các giá trị từ form
                editObj.ProductName = ProductName;
                editObj.Description = Description;
                editObj.Price = Gia;
                editObj.StockQuantity = Soluong;
                editObj.CategoryID = CategoryID;
                editObj.ImageUrl = ImageUrl;
                editObj.CreatedAt = DateTime.Now; // Thiết lập thời gian cập nhật mới

                // Lưu các thay đổi vào cơ sở dữ liệu
                db.SubmitChanges();

                // Lưu thông báo thành công vào TempData
                TempData["SuccessMessage"] = "Sửa thông tin sản phẩm thành công!";

                // Trả về RedirectToAction trực tiếp
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm để cập nhật";
                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete()
        {
            string abc = Request.QueryString["id"];

            // Tìm sản phẩm hiện tại theo ID
            Product deleteObj = db.Products.Where(o => o.ProductID == abc).FirstOrDefault();
            if (deleteObj != null)
            {
                db.Products.DeleteOnSubmit(deleteObj);
                db.SubmitChanges();
            }
           

            var x = db.Products.ToList();
            return View("index", x);
        }
        public ActionResult Details(string id)
        {
            // Kiểm tra xem id có hợp lệ không
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "ID sản phẩm không hợp lệ.";
                return RedirectToAction("Index");
            }

            // Tìm sản phẩm theo ID
            Product product = db.Products.FirstOrDefault(p => p.ProductID == id);

            // Nếu sản phẩm không tồn tại, hiển thị thông báo lỗi
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm.";
                return RedirectToAction("Index");
            }

            // Truyền sản phẩm qua view để hiển thị chi tiết
            return View(product);
        }

    }


}


