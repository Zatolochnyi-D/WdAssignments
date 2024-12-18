using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyndMapper.Configurations.Configurations;
using MyndMapper.Configurations.Services;

namespace MyndMapper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string customCorsPolicyName = "allowAllOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: customCorsPolicyName,
                              policy =>
                              {
                                  policy.WithOrigins("null")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                              });
        });
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddRepositories();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddValidators();
        builder.Services.AddDbContext<DataModelContext>(contextOptions => contextOptions.UseSqlite("Data source=sample.db"));
        builder.Services.AddOptions<Global>().BindConfiguration("Global");
        builder.Services.AddDistributedMemoryCache();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(customCorsPolicyName);
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
