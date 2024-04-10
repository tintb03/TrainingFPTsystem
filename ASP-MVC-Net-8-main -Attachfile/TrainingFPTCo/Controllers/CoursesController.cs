using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPTCo.Helpers;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;

namespace TrainingFPTCo.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet]
        public IActionResult Index(string SearchString, string Status)
        {
            CourseViewModel course = new CourseViewModel();
            course.CourseDetailsList = new List<CourseDetail>();
            var dataCourses = new CourseQuery().GetAllDataCourses(SearchString, Status);
            var categoryQuery = new CategoryQuery(); // Khởi tạo CategoryQuery
            foreach (var data in dataCourses)
            {
                string categoryName = categoryQuery.GetCategoryNameById(data.CategoryId); // Lấy tên category
                course.CourseDetailsList.Add(new CourseDetail
                {
                    Id = data.Id,
                    Name = data.Name,
                    CategoryId = data.CategoryId,
                    Description = data.Description,
                    Status = data.Status,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    ViewImageCourse = data.ViewImageCourse,
                    ViewCategoryName = categoryName // Sử dụng tên category ở đây
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(course);
        }



        [HttpGet]
        public IActionResult Add()
        {
            CourseAddViewModel course = new CourseAddViewModel();
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseAddViewModel course, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // return Ok(course);
                    string imageCourse = UploadFileHelper.UpLoadFile(Image);
                    // return Ok(course);
                    int idCourse = new CourseQuery().InsetDataCourse(
                        course.Name,
                        course.CategoryId,
                        course.Description,
                        course.StartDate,
                        course.EndDate,
                        course.Status,
                        imageCourse
                    );

                    if (idCourse > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(nameof(CoursesController.Index), "Courses");
                }
                catch (Exception ex)
                {
                    // neu co loi
                    return Ok(ex.Message);
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(course);
        }



        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            CourseDetail courseDetail = new CourseQuery().GetDataCouseById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCategories = new CategoryQuery().GetAllCategories(null, null);
            foreach (var category in dataCategories)
            {
                items.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }
            ViewBag.Categories = items;
            return View(courseDetail);
        }
        [HttpPost]
        public IActionResult Update(CourseDetail courseDetail, IFormFile Image)
        {
            try
            {
                //courseDetail.CategoryId = 2;
                var detail = new CourseQuery().GetDataCouseById(courseDetail.Id);
                // return Ok(courseDetail);
                string uniqueImage = detail.ViewImageCourse;
                if (courseDetail.Image != null)
                {
                    uniqueImage = UploadFileHelper.UpLoadFile(Image);
                }
                bool update = new CourseQuery().UpdateCourseById(
                    courseDetail.Name,
                    courseDetail.Description,
                    uniqueImage,
                    courseDetail.CategoryId,
                    courseDetail.StartDate,
                    courseDetail.EndDate,
                    courseDetail.Status,
                    courseDetail.Id);
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(CoursesController.Index), "Courses");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                // return View(courseDetail);
            }

        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new CourseQuery().DeleteItemCourse(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(CoursesController.Index), "Courses");
        }

    }
}
