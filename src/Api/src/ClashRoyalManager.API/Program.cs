using ClashRoyalManager.Application;
using ClashRoyalManager.Infrastructure;
using ClashRoyalManager.API.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        
        .AddHttpContextAccessor()

        .ConfigureAutheticationServices(builder.Configuration)
        .SwaggerDocumentatiobService();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("MyPolicy",
            builder =>
            {
                builder.WithOrigins("http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            });
    });

    builder.Services.AddControllers();
}

var app = builder.Build();
{
    app.UseCors("MyPolicy");

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
    
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseStatusCodePages();
    
    app.Run();
}