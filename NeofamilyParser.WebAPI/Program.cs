using Microsoft.EntityFrameworkCore;
using NeofamilyParser.DAL;
using NeofamilyParser.DAL.Repository;
using NeofamilyParser.WebAPI.Configuration;

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
            //todo
        }

        private static void ConfigureAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
