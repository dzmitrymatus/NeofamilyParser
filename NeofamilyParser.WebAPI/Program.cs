using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using NeofamilyParser.BLL.ApiClient;
using NeofamilyParser.DAL;
using NeofamilyParser.DAL.Repository;
using NeofamilyParser.WebAPI.Configuration;
using RestSharp;

namespace NeofamilyParser.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureAppDb(builder.Services, builder.Configuration);
            ConfigureBusinessServices(builder.Services, builder.Configuration);
            ConfigureAppServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static void ConfigureAppDb(IServiceCollection services, IConfiguration configuration)
        {
            string? connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<NeofamilyParserDbContext>(options => options.UseSqlServer(connection));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        private static void ConfigureBusinessServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<IHtmlParser, HtmlParser>();
            services.AddScoped<INeofamilyApiClient, NeofamilyApiClient>();
        }

        private static void ConfigureAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
