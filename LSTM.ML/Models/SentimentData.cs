using Microsoft.ML.Data;

namespace LSTM.ML.Models
{
    public class SentimentData
    {
        [LoadColumn(0)]
        public string Text { get; set; } = string.Empty;

        [LoadColumn(1)]
        public bool Label { get; set; }
    }

    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }

    public class SentimentDataMultiClass
    {
        [LoadColumn(0)]
        public string Text { get; set; } = string.Empty;

        [LoadColumn(1)]
        public string Label { get; set; } = string.Empty;
    }

    public class SentimentPredictionMultiClass
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; } = string.Empty;

        [ColumnName("Score")]
        public float[] Score { get; set; } = Array.Empty<float>();
    }
} 