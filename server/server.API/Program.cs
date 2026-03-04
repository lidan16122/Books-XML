
using server.API.Middlewares;
using server.BL.Services;
using server.Models.Constants;

namespace server.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                DotNetEnv.Env.Load();
                var builder = WebApplication.CreateBuilder(args);

                string? clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL");
                if (string.IsNullOrEmpty(clientUrl))
                {
                    throw new Exception("client url not found");
                }

                string? enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (string.IsNullOrEmpty(enviroment) )
                {
                    throw new Exception("enviroment not found");
                }

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("allowOrigins", builder =>
                    {
                        builder.WithOrigins(clientUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
                });

                builder.Services.AddControllers();
                builder.Services.AddSwaggerGen();

                builder.Services.AddSingleton<BooksXML>();
                builder.Services.AddTransient<BooksService>();

                var app = builder.Build();

                string? url = Environment.GetEnvironmentVariable("APP_URL");
                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("app url not found");
                }
                app.Urls.Clear();
                app.Urls.Add(url);

                app.UseCors("allowOrigins");
                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseAuthorization();
                app.MapControllers();
                app.UseMiddleware<RequestLoggingMiddleware>();
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
