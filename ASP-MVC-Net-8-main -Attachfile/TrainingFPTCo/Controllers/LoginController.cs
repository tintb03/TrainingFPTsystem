using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;

namespace TrainingFPTCo.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        } 

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            model = new LoginQuery().CheckUserLogin(model.UserName, model.Password);
            if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Email))
            {
                // nguoi dung dang nhap linh tinh
                // co mot thong bao loi ra view
                ViewData["MessageLogin"] = "Account invalid";
                return View(model);
            }

            // luu thong tin nguoi dung vao session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                HttpContext.Session.SetString("SessionUserId", model.Id);
                HttpContext.Session.SetString("SessionUsername", model.UserName);
                HttpContext.Session.SetString("SessionRolesId", model.RolesId);
                HttpContext.Session.SetString("SessionEmail", model.Email);
            }

            // Chuyển hướng người dùng đến trang tương ứng với vai trò của họ
            switch (model.RolesId)
            {
                case "1": // Administrator
                    return RedirectToAction(nameof(DashboardController.IndexAdministrator), "Dashboard");
                case "2": // Training staff
                    return RedirectToAction(nameof(DashboardController.IndexTrainingstaff), "Dashboard");
                case "3": // Trainer
                    return RedirectToAction(nameof(DashboardController.IndexTrainer), "Dashboard");
                case "4": // Trainee
                    return RedirectToAction(nameof(DashboardController.IndexTrainee), "Dashboard");
                default: // Nếu không thuộc bất kỳ vai trò nào khác
                    return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // xoa session da tao ra o login
            // quay ve trang dang nhap
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                // xoa session
                HttpContext.Session.Remove("SessionUserId");
                HttpContext.Session.Remove("SessionUsername");
                HttpContext.Session.Remove("SessionRolesId");
                HttpContext.Session.Remove("SessionEmail");
            }
            // quay ve trang dang nhap
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}
