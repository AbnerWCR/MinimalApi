using DemoMinimalApi.Data;
using DemoMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinimalContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/fornecedor", async (
    MinimalContextDb context) =>
    await context.Fornecedores.ToListAsync())
    .WithName("GetAllFornecedor")
    .WithTags("Fornecedor");

app.MapGet("/fornecedor/{id}", async(
    Guid id, 
    MinimalContextDb context) => 
    await context.Fornecedores.FindAsync(id)
        is Fornecedor fornecedor
            ? Results.Ok(fornecedor)
            : Results.NotFound())
    .Produces<Fornecedor>(StatusCodes.Status200OK)
    .Produces<Fornecedor>(StatusCodes.Status404NotFound)
    .WithName("GetFornecedorById")
    .WithTags("Fornecedor");

app.MapPost("/fornecedor", async (
    MinimalContextDb context,
    Fornecedor fornecedor) => // o ideal seria VM, utilizando Modelo para fins de aprendizagem
    {
        context.Fornecedores.Add(fornecedor);
        var result = await context.SaveChangesAsync();
    })
    .Produces<Fornecedor>(StatusCodes.Status201Created)
    .Produces<Fornecedor>(StatusCodes.Status400BadRequest)
    .WithName("PostFornecedor")
    .WithTags("Fornecedor");

app.UseHttpsRedirection();

app.Run();