using DemoAPI.Hub;
using DemoAPI.ServiceBrokerManager;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

//Add SignalR
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("https://localhost:7130") // Add your frontend domain here
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();

        });
});
builder.Services.AddSignalR();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceBrokerQueueManager, ServiceBrokerQueueManager>();
var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var myDependency = services.GetRequiredService<IServiceBrokerQueueManager>();
    //Use the service
    myDependency.runServiceBrokerListener();

}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ApiHub>("/apiHub");


app.Run();
