using System.ComponentModel.DataAnnotations;

namespace LSTM.Models
{
    public class TrainingData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public SentimentType ActualSentiment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 外键
        public int DatasetId { get; set; }
        public virtual Dataset Dataset { get; set; } = null!;
    }
} 