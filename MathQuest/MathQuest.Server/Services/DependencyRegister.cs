using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Data;
using Data.Interfaces;
using Data.Repositories;
using IdentityApp2.Services;
using System.Text.Encodings.Web;

namespace MathQuestWebApi.Services
{
    public static class DependencyRegister
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        { 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<ITwoFactorService, TwoFactorService>();
            services.AddSingleton<UrlEncoder>(UrlEncoder.Default);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            services.AddScoped<IFriendshipService, FriendshipService>();

            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IGradeService, GradeService>();

            services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
            services.AddScoped<ILeaderboardService, LeaderboardService>();

            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            services.AddScoped<IConsoleEmailSenderService, ConsoleEmailSenderService>(); // Temporary

            services.Configure<MailSettingsModel>(configuration.GetSection("Mail"));
            services.AddTransient<IEmailSenderService, EmailSenderService>();
        }
    }
}
