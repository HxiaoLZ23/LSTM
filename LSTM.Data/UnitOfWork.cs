using Microsoft.EntityFrameworkCore.Storage;
using LSTM.Data.Interfaces;
using LSTM.Data.Repositories;
using LSTM.Models;

namespace LSTM.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LSTMDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(LSTMDbContext context)
        {
            _context = context;
            Users = new Repository<User>(_context);
            SentimentAnalyses = new Repository<SentimentAnalysis>(_context);
            Datasets = new Repository<Dataset>(_context);
            TrainingData = new Repository<TrainingData>(_context);
            MLModels = new Repository<MLModel>(_context);
        }

        public IRepository<User> Users { get; }
        public IRepository<SentimentAnalysis> SentimentAnalyses { get; }
        public IRepository<Dataset> Datasets { get; }
        public IRepository<TrainingData> TrainingData { get; }
        public IRepository<MLModel> MLModels { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 