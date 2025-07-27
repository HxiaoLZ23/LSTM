using System.ComponentModel.DataAnnotations;

namespace LSTM.Models
{
    public class MLModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public string ModelPath { get; set; } = string.Empty;

        public ModelStatus Status { get; set; } = ModelStatus.Created;

        public double? Accuracy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? TrainedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // 外键
        public int DatasetId { get; set; }
        public virtual Dataset Dataset { get; set; } = null!;

        // 导航属性
        public virtual ICollection<SentimentAnalysis> SentimentAnalyses { get; set; } = new List<SentimentAnalysis>();
    }

    public enum ModelStatus
    {
        Created,
        Training,
        Trained,
        Failed
    }
} 