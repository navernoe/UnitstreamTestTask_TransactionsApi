using UnistreamTask.Application.Extensions;
using UnistreamTask.Application.Interfaces;
using UnistreamTask.Application.Repositories;
using UnistreamTask.Application.Settings;
using UnistreamTask.WebApi.Mappers;
using UnistreamTask.WebApi.Middlewares;
using UnistreamTask.WebApi.StartupExtensions;

namespace UnistreamTask.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
        builder.Services.AddFluentValidators();
        builder.Services.AddInMemoryDbContext(dbName: "unistream");
        builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        builder.Services.AddSingleton<TransactionMapper>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseMiddleware<ExceptionsLoggingMiddlware>();

        app.Run();
    }
}