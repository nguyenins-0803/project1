using QLBANHANG1.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QLBANHANG1.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly DataClasses1DataContext db = new DataClasses1DataContext("Data Source=YENNGTH-0803\\MSSQLSERVER01;Initial Catalog=QLBANHANG1;Integrated Security=True");

        // GET: DanhMuc
        public ActionResult Index()
        {
            // Truyền danh sách danh mục vào ViewBag
            ViewBag.ParentCategories = db.Categories.ToList();
            return View();
        }

        /* Action method for AJAX */
        [HttpPost]
        public JsonResult GetListDanhMuc()
        {
            var categories = db.Categories.Select(c => new
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName,
                Description = c.Description,
                ParentCategoryID = c.ParentCategoryID,
                ParentCategoryName = db.Categories
                    .Where(parent => parent.CategoryID == c.ParentCategoryID)
                    .Select(parent => parent.CategoryName)
                    .FirstOrDefault()
            }).ToList();

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            // Chỉ gán ViewBag.ParentCategories một lần
            ViewBag.ParentCategories = db.Categories.ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Create(string categoryID, string categoryName, string description, string parentCategoryID)
        {
            try
            {
                // Validate dữ liệu đầu vào
                if (string.IsNullOrEmpty(categoryID))
                {
                    return Json(new { success = false, message = "CategoryID không được để trống" });
                }

                if (string.IsNullOrEmpty(categoryName))
                {
                    return Json(new { success = false, message = "CategoryName không được để trống" });
                }

                // Kiểm tra CategoryID đã tồn tại chưa
                var existingCategory = db.Categories.FirstOrDefault(c => c.CategoryID == categoryID);
                if (existingCategory != null)
                {
                    return Json(new { success = false, message = "CategoryID đã tồn tại" });
                }

                // Kiểm tra ParentCategoryID có tồn tại không (nếu có)
                if (!string.IsNullOrEmpty(parentCategoryID))
                {
                    var parentCategory = db.Categories.FirstOrDefault(c => c.CategoryID == parentCategoryID);
                    if (parentCategory == null)
                    {
                        return Json(new { success = false, message = "Danh mục cha không tồn tại" });
                    }
                }

                var newCategory = new Category
                {
                    CategoryID = categoryID,
                    CategoryName = categoryName,
                    Description = description,
                    ParentCategoryID = string.IsNullOrEmpty(parentCategoryID) ? null : parentCategoryID
                };

                db.Categories.InsertOnSubmit(newCategory);
                db.SubmitChanges();

                return Json(new
                {
                    success = true,
                    message = "Thêm danh mục thành công",
                    data = new
                    {
                        CategoryID = newCategory.CategoryID,
                        CategoryName = newCategory.CategoryName,
                        Description = newCategory.Description,
                        ParentCategoryID = newCategory.ParentCategoryID,
                        ParentCategoryName = db.Categories
                            .Where(c => c.CategoryID == newCategory.ParentCategoryID)
                            .Select(c => c.CategoryName)
                            .FirstOrDefault()
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        public ActionResult Edit(string id)
        {
            // Tìm danh mục theo ID
            var category = db.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy danh mục
            }

            // Lấy danh sách các danh mục cha để hiển thị trong dropdown
            ViewBag.ParentCategories = db.Categories.Where(c => c.CategoryID != id).ToList();

            // Trả về view với danh mục đã tìm thấy
            return View(category);
        }
        public JsonResult GetCategoryById(string id)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category != null)
            {
                return Json(new
                {
                    success = true,
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    ParentCategoryID = category.ParentCategoryID
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Danh mục không tìm thấy." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult PostEdit(Category model)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = db.Categories.FirstOrDefault(c => c.CategoryID == model.CategoryID);
                if (existingCategory != null)
                {
                    existingCategory.CategoryName = model.CategoryName;
                    existingCategory.Description = model.Description;
                    existingCategory.ParentCategoryID = model.ParentCategoryID;

                    db.SubmitChanges();// Lưu thay đổi vào cơ sở dữ liệu

                    return Json(new { success = true, message = "Cập nhật danh mục thành công." });
                }
                else
                {
                    return Json(new { success = false, message = "Danh mục không tìm thấy." });
                }
            }
            return Json(new { success = false, message = "Thông tin không hợp lệ." });
        }
      [HttpPost]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không hợp lệ." }, JsonRequestBehavior.AllowGet);
            }

            // Tìm danh mục cần xóa theo ID
            Category deleteObj = db.Categories.FirstOrDefault(o => o.CategoryID == id);
            if (deleteObj != null)
            {
                db.Categories.DeleteOnSubmit(deleteObj);
                db.SubmitChanges();
                return Json(new { success = true, message = "Danh mục đã được xóa thành công." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Không tìm thấy danh mục." }, JsonRequestBehavior.AllowGet);
        }
        


    }
}
