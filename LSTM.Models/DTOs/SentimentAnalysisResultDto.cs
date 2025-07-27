namespace LSTM.Models.DTOs
{
    public class SentimentAnalysisResultDto
    {
        public string Text { get; set; } = string.Empty;
        public SentimentType PredictedSentiment { get; set; }
        public double Confidence { get; set; }
        public string SentimentLabel => PredictedSentiment switch
        {
            // 基础情感
            SentimentType.Positive => "积极",
            SentimentType.Negative => "消极",
            SentimentType.Neutral => "中性",
            
            // 扩展情感类别
            SentimentType.Joy => "喜悦",
            SentimentType.Anger => "愤怒",
            SentimentType.Sadness => "悲伤",
            SentimentType.Fear => "恐惧",
            SentimentType.Surprise => "惊讶",
            SentimentType.Disgust => "厌恶",
            SentimentType.Love => "爱",
            SentimentType.Hate => "恨",
            SentimentType.Anxiety => "焦虑",
            SentimentType.Excitement => "兴奋",
            SentimentType.Disappointment => "失望",
            SentimentType.Satisfaction => "满意",
            SentimentType.Confusion => "困惑",
            SentimentType.Gratitude => "感激",
            SentimentType.Enthusiasm => "热情",
            SentimentType.Frustration => "沮丧",
            SentimentType.Relief => "宽慰",
            SentimentType.Curiosity => "好奇",
            SentimentType.Pride => "自豪",
            SentimentType.Shame => "羞愧",
            SentimentType.Hope => "希望",
            SentimentType.Despair => "绝望",
            SentimentType.Admiration => "钦佩",
            SentimentType.Contempt => "轻蔑",
            SentimentType.Amusement => "娱乐",
            SentimentType.Boredom => "无聊",
            SentimentType.Nostalgia => "怀旧",
            SentimentType.Optimism => "乐观",
            SentimentType.Pessimism => "悲观",
            _ => "未知"
        };
        public DateTime AnalyzedAt { get; set; } = DateTime.Now;
    }
} 