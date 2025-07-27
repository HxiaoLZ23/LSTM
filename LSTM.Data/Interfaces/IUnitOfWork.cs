using LSTM.Models;

namespace LSTM.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<SentimentAnalysis> SentimentAnalyses { get; }
        IRepository<Dataset> Datasets { get; }
        IRepository<TrainingData> TrainingData { get; }
        IRepository<MLModel> MLModels { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 