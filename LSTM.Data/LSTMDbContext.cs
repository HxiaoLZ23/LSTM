using Microsoft.EntityFrameworkCore;
using LSTM.Models;

namespace LSTM.Data
{
    public class LSTMDbContext : DbContext
    {
        public LSTMDbContext(DbContextOptions<LSTMDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SentimentAnalysis> SentimentAnalyses { get; set; }
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<TrainingData> TrainingData { get; set; }
        public DbSet<MLModel> MLModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 用户表配置
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // 情感分析表配置
            modelBuilder.Entity<SentimentAnalysis>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.SentimentAnalyses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.SentimentAnalyses)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // 数据集表配置
            modelBuilder.Entity<Dataset>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Datasets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 训练数据表配置
            modelBuilder.Entity<TrainingData>(entity =>
            {
                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.TrainingData)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 模型表配置
            modelBuilder.Entity<MLModel>(entity =>
            {
                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 添加种子数据
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // 创建默认用户
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }
            );

            // 创建默认数据集
            modelBuilder.Entity<Dataset>().HasData(
                new Dataset
                {
                    Id = 1,
                    Name = "默认数据集",
                    Description = "系统默认的情感分析数据集",
                    UserId = 1,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }
            );

            // 添加一些示例训练数据
            var sampleData = new[]
            {
                new TrainingData { Id = 1, Text = "这个产品真的很棒，我很喜欢！", ActualSentiment = SentimentType.Positive, DatasetId = 1 },
                new TrainingData { Id = 2, Text = "这个服务太差了，完全不推荐。", ActualSentiment = SentimentType.Negative, DatasetId = 1 },
                new TrainingData { Id = 3, Text = "这个东西还行吧，没什么特别的。", ActualSentiment = SentimentType.Neutral, DatasetId = 1 },
                new TrainingData { Id = 4, Text = "非常满意这次的购买体验！", ActualSentiment = SentimentType.Positive, DatasetId = 1 },
                new TrainingData { Id = 5, Text = "质量不行，浪费钱。", ActualSentiment = SentimentType.Negative, DatasetId = 1 }
            };

            modelBuilder.Entity<TrainingData>().HasData(sampleData);
        }
    }
} 