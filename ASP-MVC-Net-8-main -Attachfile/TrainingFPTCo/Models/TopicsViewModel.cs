using System.ComponentModel.DataAnnotations;
using TrainingFPTCo.Migrations;
using TrainingFPTCo.Validations;

namespace TrainingFPT.Models
{
    public class TopicsViewModel
    {
        public List<TopicDetail> TopicDetailList { get; set; }
    }

    public class TopicDetail
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Topic's name, please")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Choose Course, please")]
        public int CourseId { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Choose documents, please")]
        [AllowExtensionFile(new string[] { ".docx", ".pdf" , ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".mpeg", ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a", ".wma", ".png", ".jpg", ".jpeg" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)]
        public IFormFile Documents { get; set; }
        public string? ViewDocuments { get; set; }


        [AllowExtensionFile(new string[] { ".docx", ".pdf", ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".mpeg", ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a", ".wma", ".png", ".jpg", ".jpeg" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)]
        public IFormFile? AttachFile { get; set; }
        public string? ViewAttachFile { get; set; }

        [Required(ErrorMessage = "Choose file, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)]
        public IFormFile PosterTopic { get; set; }
        public string? ViewPosterTopic { get; set; }

        public string? TypeDocument { get; set; }



        public string? viewCourseName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
