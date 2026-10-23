using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookingContext>(options => options.UseInMemoryDatabase("BookingDb"));

var app = builder.Build();

app.Run();
