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

// ������ ���?ӝ�� �� ����?�� DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // ����� �� �?���?� SQL

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))); // ����� �� Redis
builder.Services.AddScoped<IRepository<User>, UserRepository>(); // ������ ���� �������
builder.Services.AddScoped<IDataProcessorService, DataProcessorService>(); // ������ ���?� ������ ����
builder.Services.AddScoped<IMqttService, MqttService>(); // ������ ���?� MQTT
builder.Services.AddScoped<IAuthService, AuthService>(); // ������ ���?� ����� ��?�
builder.Services.AddScoped<RedisService>(); // ������ ���?� Redis
builder.Services.AddScoped<ApiService>(); // ������ ���?� API
builder.Services.AddScoped<LoggingService>(); // ������ ���?� �ǐ?�
builder.Services.AddScoped<IMessageQueueService>(provider => new MessageQueueService(builder.Configuration["QueueSettings:HostName"])); // ������ ���?� �� �?��
builder.Services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>(); // ������ ���������? ������� ����

builder.Services.AddHttpClient(); // ������ HttpClient ���? ����� ������ʝ��

builder.Services.AddControllers(); // ������ ��������
builder.Services.AddEndpointsApiExplorer(); // ������ Explorer ���? ���� �����??
builder.Services.AddSwaggerGen(); // ������ Swagger ���? ��������? API

var app = builder.Build();

// �?�����? �� ���� �������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // �������? Swagger �� ��?� �����
    app.UseSwaggerUI(); // �������? UI Swagger
}

app.UseHttpsRedirection(); // �������? HTTPS
app.UseAuthorization(); // �������? ������
app.MapControllers(); // ���������? ��������

app.Run(); // ����? ������