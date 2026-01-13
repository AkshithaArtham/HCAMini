<<<<<<< HEAD
using HCAMiniEHR.Data;
using HCAMiniEHR.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbSeeder.Seed(context); // Ensure seeding is called
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();


=======
using HCAMiniEHR.Data;
using HCAMiniEHR.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<DoctorService>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbSeeder.Seed(context); // Ensure seeding is called
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();


>>>>>>> de0c1979792b9fba70a0d3608ff20cf61cb6a43b
