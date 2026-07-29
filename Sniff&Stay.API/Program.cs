using SniffAndStay.API;
using SniffAndStay.Application;
using SniffAndStay.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sniff & Stay API v1");
    });

}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
