using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DeviceDataProcessor.Data;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using DeviceDataProcessor.Models;
using DeviceDataProcessor.DTOs;

var builder = WebApplication.CreateBuilder(args);

// «›“Êœ‰ ”—Ê?”ùÂ« »Â ò«‰ ?‰— DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // « ’«· »Â œ? «»?” SQL

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))); // « ’«· »Â Redis
builder.Services.AddScoped<IRepository<User>, UserRepository>(); // «›“Êœ‰ „Œ“‰ ò«—»—«‰
builder.Services.AddScoped<IDataProcessorService, DataProcessorService>(); // «›“Êœ‰ ”—Ê?” Å—œ«“‘ œ«œÂ
builder.Services.AddScoped<IMqttService, MqttService>(); // «›“Êœ‰ ”—Ê?” MQTT
builder.Services.AddScoped<IAuthService, AuthService>(); // «›“Êœ‰ ”—Ê?” «Õ—«“ ÂÊ? 
builder.Services.AddScoped<RedisService>(); // «›“Êœ‰ ”—Ê?” Redis
builder.Services.AddScoped<ApiService>(); // «›“Êœ‰ ”—Ê?” API
builder.Services.AddScoped<LoggingService>(); // «›“Êœ‰ ”—Ê?” ·«ê?‰ê
builder.Services.AddScoped<IMessageQueueService>(provider => new MessageQueueService(builder.Configuration["QueueSettings:HostName"])); // «›“Êœ‰ ”—Ê?” ’› Å?«„
builder.Services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>(); // «›“Êœ‰ «⁄ »«—”‰Ã? œ—ŒÊ«”  Ê—Êœ

builder.Services.AddHttpClient(); // «›“Êœ‰ HttpClient »—«? «—”«· œ—ŒÊ«” ùÂ«

builder.Services.AddControllers(); // «›“Êœ‰ ò‰ —·—Â«
builder.Services.AddEndpointsApiExplorer(); // «›“Êœ‰ Explorer »—«? ‰ﬁ«ÿ «‰ Â«??
builder.Services.AddSwaggerGen(); // «›“Êœ‰ Swagger »—«? „” ‰œ”«“? API

var app = builder.Build();

// Å?ò—»‰œ? Œÿ ·Ê·Â œ—ŒÊ«” 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // ›⁄«·ù”«“? Swagger œ— „Õ?ÿ  Ê”⁄Â
    app.UseSwaggerUI(); // ›⁄«·ù”«“? UI Swagger
}

app.UseHttpsRedirection(); // ›⁄«·ù”«“? HTTPS
app.UseAuthorization(); // ›⁄«·ù”«“? „ÃÊ“Â«
app.MapControllers(); // ‰ﬁ‘Âù»—œ«—? ò‰ —·—Â«

app.Run(); // «Ã—«? »—‰«„Â