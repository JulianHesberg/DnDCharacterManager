using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Infrastructure.Presistance;
using CharacterMicroservice.Infrastructure.Presistance.Mongo;
using CharacterMicroservice.Infrastructure.Presistance.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICharacterSheetRepository, CharacterSheetRepository>();
builder.Services.AddScoped<ICharacterItemsRepository, CharacterItemsRepository>();
builder.Services.AddScoped<ICharacterSkillsRepository, CharacterSkillsRepository>();
builder.Services.AddScoped<ICharacterNotesRepository, CharacterNotesRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<ICharacterReadRepository, CharacterReadRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
