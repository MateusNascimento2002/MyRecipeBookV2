using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options
            .JsonSerializerOptions
            .Converters
            .Add(new StringConverter());
    });
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    string[] supportedCultures =
    [
        "en",
        "pt-BR"
    ];

    options.SetDefaultCulture("en");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
    options.RequestCultureProviders =
    [
        new AcceptLanguageHeaderRequestCultureProvider()
    ];
});

builder.Services.AddMvc(options => { options.Filters.Add<ExceptionFilter>(); });

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddPasswordHasher();
builder.Services.AddUseCases();
builder.Services.AddRepositories();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddUnitOfWork();

var app = builder.Build();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(localizationOptions.Value);


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await ExecuteMigrations();

app.Run();

async Task ExecuteMigrations()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DatabaseMigration.ExecuteMigrations(scope.ServiceProvider);
}