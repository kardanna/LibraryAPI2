using LibraryAPI.Infrastructure;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Application.Services;

namespace LibraryAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddSingleton<ILibraryRepository, LibraryRepository>();
        builder.Services.AddTransient<IBookService, BookService>();
        builder.Services.AddTransient<IAuthorService, AuthorService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
