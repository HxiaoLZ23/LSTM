using System.ComponentModel.DataAnnotations;

namespace LSTM.Models
{
    public class SentimentAnalysis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public SentimentType PredictedSentiment { get; set; }

        [Range(0.0, 1.0)]
        public double Confidence { get; set; }

        public DateTime AnalyzedAt { get; set; } = DateTime.Now;

        // 外键
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int? ModelId { get; set; }
        public virtual MLModel? Model { get; set; }
    }

    public enum SentimentType
    {
        // 基础情感
        Negative = -1,
        Neutral = 0,
        Positive = 1,
        
        // 扩展情感类别
        Joy = 2,           // 喜悦
        Anger = 3,         // 愤怒
        Sadness = 4,       // 悲伤
        Fear = 5,          // 恐惧
        Surprise = 6,      // 惊讶
        Disgust = 7,       // 厌恶
        Love = 8,          // 爱
        Hate = 9,          // 恨
        Anxiety = 10,      // 焦虑
        Excitement = 11,   // 兴奋
        Disappointment = 12, // 失望
        Satisfaction = 13,   // 满意
        Confusion = 14,      // 困惑
        Gratitude = 15,      // 感激
        Enthusiasm = 16,     // 热情
        Frustration = 17,    // 沮丧
        Relief = 18,         // 宽慰
        Curiosity = 19,      // 好奇
        Pride = 20,          // 自豪
        Shame = 21,          // 羞愧
        Hope = 22,           // 希望
        Despair = 23,        // 绝望
        Admiration = 24,     // 钦佩
        Contempt = 25,       // 轻蔑
        Amusement = 26,      // 娱乐
        Boredom = 27,        // 无聊
        Nostalgia = 28,      // 怀旧
        Optimism = 29,       // 乐观
        Pessimism = 30       // 悲观
    }
} 