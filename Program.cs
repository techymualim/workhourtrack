using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Workhourtrack;
using Workhourtrack.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add the necessary configurations for your application

builder.Services.AddEntityFrameworkMySQL()
                .AddDbContext<HourtrackContext>(options =>
                {
                    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
var app = builder.Build();
app.MapGet("/employees", async (HourtrackContext dbContext) =>
{
    var employees = await dbContext.Employees.ToListAsync();
    return employees;
});

app.MapGet("/employees/{id}", async (int id, HourtrackContext dbContext) =>
    await dbContext.Employees.FindAsync(id)
        is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

app.MapPut("/employees/{id}", async (int id, Employee inputEmployee, HourtrackContext dbContext) =>
{
    var employee = await dbContext.Employees.FindAsync(id);

    if (employee is null) return Results.NotFound();


    employee.Id = inputEmployee.Id;
    employee.Name = inputEmployee.Name;
   

    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapGet("/workhour", async (HourtrackContext dbContext) =>
{
    var employees = await dbContext.Workhours.ToListAsync();
    return employees;

});

app.MapGet("/workhour/{id}", async (int id, HourtrackContext dbContext) =>
    await dbContext.Workhours.FindAsync(id)
        is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

app.Run();
