using Application.Services;
using Domain.Abstractions.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Infrastructure / DI ---
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Repositories ---
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

// --- Application Services ---
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<RoomService>();

// --- Controllers & Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CORS ---
builder.Services.AddCors();

var app = builder.Build();

// --- Middleware ---
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();