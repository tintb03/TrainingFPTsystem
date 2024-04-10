using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPTCo.Migrations;
using TrainingFPTCo.Helpers;
using TrainingFPT.Models;

namespace TrainingFPT.Controllers
{
    public class TopicsController : Controller
    {

        // INDEX
        [HttpGet]
        public IActionResult Index(string SearchString, string Status)
        {
            TopicsViewModel topic = new TopicsViewModel();
            topic.TopicDetailList = new List<TopicDetail>();
            var dataTopics = new TopicQuery().GetAllDataTopics(SearchString, Status);
            var courseQuery = new CourseQuery(); // Khởi tạo CourseQuery
            foreach (var data in dataTopics)
            {
                string courseName = courseQuery.GetCourseNameById(data.CourseId); // Lấy tên của Course
                topic.TopicDetailList.Add(new TopicDetail
                {
                    Id = data.Id,
                    Name = data.Name,
                    CourseId = data.CourseId,
                    Description = data.Description,
                    ViewDocuments = data.ViewDocuments,
                    ViewAttachFile = data.ViewAttachFile,
                    ViewPosterTopic = data.ViewPosterTopic,
                    Status = data.Status,
                    viewCourseName = courseName // Gán tên của Course vào viewCourseName
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(topic);
        }


        // ADD
        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null, null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.Id.ToString(),
                    Text = course.Name
                });
            }
            ViewBag.Courses = items;
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic, IFormFile Documents, IFormFile AttachFile, IFormFile PosterTopic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string documentsTopic = UploadFileHelper.UpLoadFile(Documents);
                    string attachFileTopic = AttachFile != null ? UploadFileHelper.UpLoadFile(AttachFile) : null; // Kiểm tra AttachFile có tồn tại không
                    string posterTopic = UploadFileHelper.UpLoadFile(PosterTopic);
                    int idTopic = new TopicQuery().InsertDataTopic(
                        topic.Name,
                        topic.CourseId,
                        topic.Description,
                        documentsTopic,
                        attachFileTopic, // Sử dụng giá trị null nếu AttachFile không được chọn
                        posterTopic,
                        topic.Status
                    );
                    if (idTopic > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(nameof(TopicsController.Index), "Topics");
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null, null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.Id.ToString(),
                    Text = course.Name
                });
            }
            ViewBag.Courses = items;
            return View(topic);
        }


        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new TopicQuery().DeleteItemTopic(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(TopicsController.Index), "Topics");
        }

        // UPDATE
        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            TopicDetail topicDetail = new TopicQuery().GetDataTopicById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataCourses = new CourseQuery().GetAllDataCourses(null, null);
            foreach (var course in dataCourses)
            {
                items.Add(new SelectListItem
                {
                    Value = course.Id.ToString(),
                    Text = course.Name
                });
            }
            ViewBag.Courses = items;
            return View(topicDetail);
        }
        [HttpPost]
        public IActionResult Update(TopicDetail topicDetail, IFormFile Documents, IFormFile AttachFile, IFormFile PosterTopic)
        {
            try
            {
                var detail = new TopicQuery().GetDataTopicById(topicDetail.Id);
                // Lưu lại dữ liệu cũ đã được tải lên trước đó
                string uniqueDocuments = detail.ViewDocuments;
                string uniqueAttachFile = detail.ViewAttachFile;
                string uniquePosterTopic = detail.ViewPosterTopic;

                // Kiểm tra xem có tệp được tải lên không, nếu có thì cập nhật tên tệp
                if (Documents != null)
                {
                    uniqueDocuments = UploadFileHelper.UpLoadFile(Documents);
                }
                if (AttachFile != null)
                {
                    uniqueAttachFile = UploadFileHelper.UpLoadFile(AttachFile);
                }
                if (PosterTopic != null)
                {
                    uniquePosterTopic = UploadFileHelper.UpLoadFile(PosterTopic);
                }

                // Gọi phương thức cập nhật với dữ liệu mới và cũ
                bool update = new TopicQuery().UpdateTopicById(
                    topicDetail.Name,
                    topicDetail.CourseId,
                    topicDetail.Description,
                    uniqueDocuments,
                    uniqueAttachFile,
                    uniquePosterTopic,
                    topicDetail.Status,
                    topicDetail.Id);

                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(TopicsController.Index), "Topics");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                // return View(topicDetail);
            }
        }
    }
}
