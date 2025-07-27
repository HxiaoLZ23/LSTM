using System.ComponentModel.DataAnnotations;

namespace LSTM.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        // 导航属性
        public virtual ICollection<SentimentAnalysis> SentimentAnalyses { get; set; } = new List<SentimentAnalysis>();
        public virtual ICollection<Dataset> Datasets { get; set; } = new List<Dataset>();
    }
} 