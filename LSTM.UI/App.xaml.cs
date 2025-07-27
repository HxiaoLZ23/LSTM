using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using LSTM.Data;
using LSTM.Data.Interfaces;
using LSTM.Business.Interfaces;
using LSTM.Business.Services;
using LSTM.ML.Interfaces;
using LSTM.ML.Services;
using LSTM.UI.ViewModels;
using LSTM.UI.Views;
using Serilog;

namespace LSTM.UI
{
    public partial class App : Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 配置依赖注入容器
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services, context.Configuration);
                })
                .UseSerilog((context, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration);
                })
                .Build();

            // 启动应用
            _host.Start();

            // 确保数据库创建
            using var scope = _host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LSTMDbContext>();
            dbContext.Database.EnsureCreated();
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // 数据库配置
            services.AddDbContext<LSTMDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection") 
                    ?? "Data Source=LSTM.db;Cache=Shared";
                options.UseSqlite(connectionString);
            });

            // 注册仓储和工作单元
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 注册业务服务
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISentimentAnalysisBusinessService, SentimentAnalysisBusinessService>();
            services.AddScoped<IModelTrainingService, ModelTrainingService>();

            // 注册ML服务
            services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();

            // 注册视图模型
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<MainViewModel>();

            // 注册视图
            services.AddTransient<LoginWindow>();
            services.AddTransient<RegisterWindow>();
            services.AddTransient<MainWindow>();

            // 添加AutoMapper
            services.AddAutoMapper(typeof(App));

            // 添加日志
            services.AddLogging();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host?.Dispose();
            base.OnExit(e);
        }

        public static T GetService<T>()
            where T : class
        {
            if ((Current as App)?._host?.Services is not { } services)
                throw new InvalidOperationException("Service provider is not available");

            return services.GetService<T>() ?? throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered");
        }
    }
} 