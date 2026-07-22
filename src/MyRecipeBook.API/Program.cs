using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Token;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exception;
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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Description = "Enter only your access token. Swagger will add 'Bearer' automatically.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document), []
        }
    });
});

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

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IAccessTokenProvider, HttpContextTokenProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var signingKey = builder.Configuration.GetValue<string>("Jwt:SigningKey")!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents()
        {
            OnTokenValidated = async context =>
            {
                var subject = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                             context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(subject, out var userId) == false)
                {
                    context.Fail("Invalid token subject!");
                    return;
                }
                
                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyRepository>();

                var userExists = await userRepository.ExistActiveUserWithId(userId);
                if (userExists == false)
                {
                    context.Fail("User not found or inactive!");
                }
            },
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = context.AuthenticateFailure switch
                {   
                    null => new ResponseErrorJson(ResourceMessagesException.VALIDATION_ACCESS_TOKEN_REQUIRED),
                    SecurityTokenExpiredException => new ResponseErrorJson("Token Expired!", true),
                    _ => new ResponseErrorJson(ResourceMessagesException.VALIDATION_RESOURCE_ACCESS_TOKEN)
                };
                
                await context.Response.WriteAsJsonAsync(response);
            }
        };
    });

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

public partial class Program
{
}