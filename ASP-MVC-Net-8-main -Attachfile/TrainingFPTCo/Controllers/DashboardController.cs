using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TrainingFPTCo.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }

            // Chuyển hướng người dùng đến trang tương ứng với vai trò của họ
            switch (HttpContext.Session.GetString("SessionRolesId"))
            {
                case "1": // Administrator
                    return RedirectToAction(nameof(IndexAdministrator));
                case "2": // Training staff
                    return RedirectToAction(nameof(IndexTrainingstaff));
                case "3": // Trainer
                    return RedirectToAction(nameof(IndexTrainer));
                case "4": // Trainee
                    return RedirectToAction(nameof(IndexTrainee));
                default: // Nếu không thuộc bất kỳ vai trò nào khác
                    return RedirectToAction(nameof(IndexDefault));
            }
        }

        public IActionResult IndexAdministrator()
        {
            // Các logic xử lý cho vai trò Administrator
            return View();
        }

        public IActionResult IndexTrainingstaff()
        {
            // Các logic xử lý cho vai trò Training staff
            return View();
        }

        public IActionResult IndexTrainer()
        {
            // Các logic xử lý cho vai trò Trainer
            return View();
        }

        public IActionResult IndexTrainee()
        {
            // Các logic xử lý cho vai trò Trainee
            return View();
        }

        public IActionResult IndexDefault()
        {
            // Trang mặc định cho các vai trò không được xác định
            return View();
        }
    }
}
