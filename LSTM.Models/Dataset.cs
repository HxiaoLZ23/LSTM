using System.ComponentModel.DataAnnotations;

namespace LSTM.Models
{
    public class Dataset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastTrainedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // 外键
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        // 导航属性
        public virtual ICollection<TrainingData> TrainingData { get; set; } = new List<TrainingData>();
        public virtual ICollection<MLModel> Models { get; set; } = new List<MLModel>();
    }
} 