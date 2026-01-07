using Microsoft.EntityFrameworkCore;
using TodoApi2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 이 설정이 TodoContext 생성자의 options로 들어감
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList")); // 메모리 DB 사용
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
