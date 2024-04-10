using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingFPTCo.Models
{
    public class AccountViewModel
    {
        public List<AccountDetail> AccountDetailList { get; set; }
    }

    public class AccountDetail
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public string? Status { get; set; }

        public Role Role { get; set; }

        [Required(ErrorMessage = "The Username field is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The ExtraCode field is required.")]
        public string ExtraCode { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Phone field is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The FullName field is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The FirstName field is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Birthday field is required.")]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "The Gender field is required.")]
        public string Gender { get; set; }

        public string Education { get; set; }
        public string ProgramingLanguage { get; set; }
        public int? ToeicScore { get; set; }
        public string Skills { get; set; }
        public string IPCleant { get; set; }

        public DateTime? LastLogin { get; set; }
        public DateTime? LastLogout { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    // Định nghĩa model cho bảng Roles
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        // Các trường khác của bảng Roles nếu cần
    }

    public class AccountAddViewModel
    {
        public int RoleId { get; set; }
        public string? Status { get; set; }

        [Required(ErrorMessage = "The Username field is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The ExtraCode field is required.")]
        public string ExtraCode { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Phone field is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The FullName field is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The FirstName field is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field is required.")]
        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "The Gender field is required.")]
        public string Gender { get; set; }

        public string Education { get; set; }
        public string ProgramingLanguage { get; set; }
        public int? ToeicScore { get; set; }
        public string Skills { get; set; }
        public string IPCleant { get; set; }
    }
}
