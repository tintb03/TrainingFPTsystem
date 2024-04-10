using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.DataDBContext;
using TrainingFPTCo.Models;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TrainingFPTCo.Controllers
{
    public class AccountController : Controller
    {
        private readonly TrainingDbContext _dbContext;

        public AccountController(TrainingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(string searchString, string filterStatus)
        {
            AccountViewModel accountModel = new AccountViewModel();
            accountModel.AccountDetailList = GetAllAccounts(searchString, filterStatus);

            ViewData["currentFilter"] = searchString;
            ViewBag.FilterStatus = filterStatus;
            return View(accountModel);
        }

        [HttpGet]
        public IActionResult IndexTrainingStaff(string searchString, string filterStatus)
        {
            AccountViewModel accountModel = new AccountViewModel();
            accountModel.AccountDetailList = GetAllAccounts(searchString, filterStatus).Where(account => IsTrainingStaffAccount(account)).ToList();

            ViewData["currentFilterTrainingStaff"] = searchString;
            ViewBag.FilterStatus = filterStatus;
            return View(accountModel);
        }

        private bool IsTrainingStaffAccount(AccountDetail account)
        {
            // Thêm logic kiểm tra xem tài khoản có phải là Training Staff, Trainer hoặc Trainee không.
            // Ví dụ: Nếu Id của Role là 2, 3 hoặc 4 thì trả về true, ngược lại trả về false.
            return /*account.RoleId == 2 || */account.RoleId == 3 || account.RoleId == 4;
        }



        private List<AccountDetail> GetAllAccounts(string searchString, string filterStatus)
        {
            List<AccountDetail> accountDetails = new List<AccountDetail>();
            var dataAccounts = new AccountQuery().GetAllAccounts(searchString, filterStatus);
            foreach (var item in dataAccounts)
            {
                var accountDetail = item;
                accountDetail.Role = new AccountQuery().GetRoleById(accountDetail.RoleId);
                accountDetails.Add(new AccountDetail
                {
                    Id = item.Id,
                    RoleId = item.RoleId,
                    Role = item.Role,
                    Username = item.Username,
                    ExtraCode = item.ExtraCode,
                    Password = item.Password,
                    Email = item.Email,
                    Phone = item.Phone,
                    Address = item.Address,
                    FullName = item.FullName,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Birthday = item.Birthday,
                    Gender = item.Gender,
                    Education = item.Education,
                    ProgramingLanguage = item.ProgramingLanguage,
                    ToeicScore = item.ToeicScore,
                    Skills = item.Skills,
                    IPCleant = item.IPCleant,
                    LastLogin = item.LastLogin,
                    LastLogout = item.LastLogout,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    DeletedAt = item.DeletedAt
                });
            }
            return accountDetails;
        }


        [HttpGet]
        public IActionResult Add()
        {
            AccountAddViewModel account = new AccountAddViewModel();
            PopulateRolesInViewBag();
            return View(account);
        }

        [HttpGet]
        public IActionResult AddTrainingStaff()
        {
            AccountAddViewModel account = new AccountAddViewModel();
            PopulateTrainingStaffRolesInViewBag();
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AccountAddViewModel account)
        {
            return await AddAccount(account, "Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrainingStaff(AccountAddViewModel account)
        {
            return await AddAccount(account, "IndexTrainingStaff");
        }

        private async Task<IActionResult> AddAccount(AccountAddViewModel account, string redirectToAction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int idAccount = new AccountQuery().InsertAccount(
                        account.RoleId,
                        account.Username,
                        account.Password,
                        account.ExtraCode,
                        account.Email,
                        account.Phone,
                        account.Address,
                        account.FullName,
                        account.FirstName,
                        account.LastName,
                        account.Birthday,
                        account.Gender,
                        account.Education,
                        account.ProgramingLanguage,
                        account.ToeicScore,
                        account.Skills,
                        account.IPCleant
                    );

                    if (idAccount > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    return RedirectToAction(redirectToAction, "Account");
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }

            PopulateRolesInViewBag();
            return View(account);
        }

        private void PopulateRolesInViewBag()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            var dataRoles = new RoleQuery().GetAllRoles();
            foreach (var role in dataRoles)
            {
                roles.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name
                });
            }
            ViewBag.Roles = roles;
        }

        private void PopulateTrainingStaffRolesInViewBag()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            var trainingStaffRoles = new RoleQuery().GetTrainingStaffRoles(); // Đổi phương thức này để lấy chỉ các vai trò Trainer và Trainee từ CSDL
            foreach (var role in trainingStaffRoles)
            {
                roles.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name
                });
            }
            ViewBag.Roles = roles;
        }



        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            AccountDetail accountDetail = new AccountQuery().GetAccountById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataRoles = new RoleQuery().GetAllRoles(); // Thay thế GetAllRoles bằng phương thức lấy danh sách các vai trò từ cơ sở dữ liệu của bạn
            foreach (var role in dataRoles)
            {
                items.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name
                });
            }
            ViewBag.Roles = items;

            // Set viewType cho view Update.cshtml
            ViewData["ViewType"] = "Default";

            return View(accountDetail);
        }

        [HttpGet]
        public IActionResult UpdateTrainingStaff(int id = 0)
        {
            AccountDetail accountDetail = new AccountQuery().GetAccountById(id);
            List<SelectListItem> items = new List<SelectListItem>();
            var dataRoles = new RoleQuery().GetTrainingStaffRoles(); // Lấy chỉ các vai trò Training Staff từ cơ sở dữ liệu
            foreach (var role in dataRoles)
            {
                items.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name
                });
            }
            ViewBag.Roles = items;

            // Set viewType cho view UpdateTrainingStaff.cshtml
            ViewData["ViewType"] = "TrainingStaff";

            return View(accountDetail);
        }




        [HttpPost]
        public IActionResult Update(AccountDetail accountDetail, string viewType)
        {
            try
            {
                bool updateAccount = new AccountQuery().UpdateAccountById(
                    accountDetail.RoleId,
                    accountDetail.Username,
                    accountDetail.Password,
                    accountDetail.ExtraCode,
                    accountDetail.Email,
                    accountDetail.Phone,
                    accountDetail.Address,
                    accountDetail.FullName,
                    accountDetail.FirstName,
                    accountDetail.LastName,
                    accountDetail.Birthday,
                    accountDetail.Gender,
                    accountDetail.Education,
                    accountDetail.ProgramingLanguage,
                    accountDetail.ToeicScore,
                    accountDetail.Skills,
                    accountDetail.IPCleant,
                    accountDetail.Id
                );

                if (updateAccount)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }

                if (viewType == "TrainingStaff")
                {
                    return RedirectToAction(nameof(AccountController.IndexTrainingStaff), "Account");
                }
                else
                {
                    return RedirectToAction(nameof(AccountController.Index), "Account");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var accountDetail = new AccountQuery().GetAccountById(id);
            accountDetail.Role = new AccountQuery().GetRoleById(accountDetail.RoleId);
            var accountViewModel = new AccountViewModel
            {
                AccountDetailList = new List<AccountDetail> { accountDetail }
            };
            return View(accountViewModel);
        }

        [HttpGet]
        public IActionResult DetailsTrainingStaff(int id)
        {
            var accountDetail = new AccountQuery().GetAccountById(id);
            accountDetail.Role = new AccountQuery().GetRoleById(accountDetail.RoleId);
            var accountViewModel = new AccountViewModel
            {
                AccountDetailList = new List<AccountDetail> { accountDetail }
            };
            return View("DetailsTrainingstaff", accountViewModel); // Sử dụng cùng view với Details
        }




        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool deleteAccount = new AccountQuery().DeleteAccountById(id);
            if (deleteAccount)
            {
                TempData["statusDelete"] = true; // Deleted successfully
            }
            else
            {
                TempData["statusDelete"] = false; // Delete failed
            }
            return RedirectToAction(nameof(Index), "Account");
        }

        [HttpGet]
        public IActionResult DeleteTrainingStaff(int id = 0)
        {
            bool deleteAccount = new AccountQuery().DeleteAccountById(id);
            if (deleteAccount)
            {
                TempData["statusDelete"] = true; // Deleted successfully
            }
            else
            {
                TempData["statusDelete"] = false; // Delete failed
            }
            return RedirectToAction(nameof(IndexTrainingStaff), "Account");
        }

    }
}
