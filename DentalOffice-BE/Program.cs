using DentalOffice_BE.Services;
using DentalOffice_BE;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var maxMemory = 1024L * 1024L * 1024L; // 1 GB
Process.GetCurrentProcess().MaxWorkingSet = (IntPtr)maxMemory;

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<AppExceptionHandler>();

builder.Services.AddServiceData(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.InizializeData();

builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(
        _ =>
        {
            _.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
            //.AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
