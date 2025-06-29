using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Application.Interfaces.IRepositories.ReadRepositories;
using CharacterMicroservice.Application.Interfaces.UnitOfWork;
using CharacterMicroservice.Infrastructure.Presistance;
using CharacterMicroservice.Infrastructure.Presistance.Mongo;
using CharacterMicroservice.Infrastructure.Presistance.Repositories;
using CharacterMicroservice.Application.Mapping;
using CharacterMicroservice.Application.Commands.CharacterSheetCommands;
using CharacterMicroservice.Application.Queries;
using CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Character;
using MediatR;
using CharacterMicroservice.Application.Commands.CharacterItemsCommands;
using CharacterMicroservice.Application.Commands.CharacterSkillsCommands;
using CharacterMicroservice.Application.Commands.CharacterNotesCommands;
using CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Items;
using CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Skills;
using CharacterMicroservice.Infrastructure.Presistance.Mongo.EventHandlers.Notes;
using CharacterMicroservice.Infrastructure.Configurations;
using CharacterMicroservice.Infrastructure.Services;
using MessageBroker.Configuration;
using CharacterMicroservice.Infrastructure.Presistance.DbContexts;
using Microsoft.EntityFrameworkCore;
using MessageBroker.Factories;
using MessageBroker.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CharacterDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("CharacterDb")));


builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

var rabbitMqOptions = builder.Configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();

// Register IMessageBroker using the factory
builder.Services.AddSingleton<IMessageBroker>(_ => RabbitMQFactory.CreateRabbitAMPQ(rabbitMqOptions));


builder.Services.AddScoped<ICharacterSheetRepository, CharacterSheetRepository>();
builder.Services.AddScoped<ICharacterItemsRepository, CharacterItemsRepository>();
builder.Services.AddScoped<ICharacterSkillsRepository, CharacterSkillsRepository>();
builder.Services.AddScoped<ICharacterNotesRepository, CharacterNotesRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<ICharacterReadRepository, CharacterReadRepository>();

builder.Services.AddSingleton<IMessageHandler, CharacterMessageHandler>();

builder.Services.AddHostedService<RabbitMQListener>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateCharacterCommandHandler).Assembly,
    typeof(UpdateCharacterCommandHandler).Assembly,
    typeof(RemoveCharacterCommandHandler).Assembly,
    typeof(CreateCharacterItemCommandHandler).Assembly,
    typeof(RemoveCharacterItemCommandHandler).Assembly,
    typeof(CreateCharacterSkillCommandHandler).Assembly,
    typeof(RemoveCharacterSkillCommandHandler).Assembly,
    typeof(CreateCharacterNoteCommandHandler).Assembly,
    typeof(RemoveCharacterNoteCommandHandler).Assembly,

    // Query Handlers
    typeof(GetCharacterByIdQueryHandler).Assembly,
    typeof(GetAllCharactersQueryHandler).Assembly,

    // Event Handlers
    typeof(CharacterUpsertedEventHandler).Assembly,
    typeof(CharacterDeletedEventHandler).Assembly,
    typeof(CharacterItemAddedEventHandler).Assembly,
    typeof(CharacterItemRemovedEventHandler).Assembly,
    typeof(CharacterSkillAddedEventHandler).Assembly,
    typeof(CharacterSkillRemovedEventHandler).Assembly,
    typeof(CharacterNoteAddedEventHandler).Assembly,
    typeof(CharacterNoteRemovedEventHandler).Assembly
));

builder.Services.AddAutoMapper(
    typeof(CharacterMappingProfile).Assembly
);




builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CharacterDbContext>();
    // creates the DB if it doesn’t exist, or applies pending migrations
    await db.Database.MigrateAsync();
}

app.Run();
