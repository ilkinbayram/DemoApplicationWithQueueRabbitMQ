using Azure.Abstracts.Configs;
using Core.Azure.Config;
using Core.Configurations.RabbitMq;
using Queue.Data.Abstract;
using Queue.Data.RabbitMQ;
using Queue.Service.Abstract;
using Queue.Service.Concrete;
using Services.Abstract.Cloud;
using Services.Concrete.Cloud;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRMQConnectionService, RMQConnectionManager>();
builder.Services.AddScoped<IPublisherService, RabbitPublisherService>();
builder.Services.AddScoped<IQueueConnection, QueueConnectionRepository>();
builder.Services.AddScoped<IQueuePublisher, RabbitMqPublisher>();
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddScoped<IRabbitMqConfig, RabbitConfig>();
builder.Services.AddScoped<IAzureConfig, AzureConfig>();
builder.Services.Configure<RabbitConfig>(builder.Configuration);
builder.Services.Configure<AzureConfig>(builder.Configuration);

//                           /\
//________________________  //\\
//     UP IS Services    |   ||
//------------------------   ||
//                           --


//                           --
//________________________   ||
//   Down IS MiddleWare  |   ||
//------------------------  \\//
//                           \/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
