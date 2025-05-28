using CharacterMicroservice.Application.Commands.CharacterItemsCommands;
using CharacterMicroservice.Application.Interfaces.IRepositories;
using CharacterMicroservice.Domain.Models.Entity.Write;
using MediatR;
using MessageBroker;
using MessageBroker.Interfaces;
using MessageBroker.Requests;
using SagaCoordinator.Domain.ResponseModels;

namespace CharacterMicroservice.Infrastructure.Services;

public class CharacterMessageHandler : IMessageHandler
{
    private readonly IMessageBroker _messageBroker;
    private readonly IMediator _mediator;

    public CharacterMessageHandler(IMessageBroker messageBroker, IMediator mediator)
    {
        _messageBroker = messageBroker;
        _mediator = mediator;
    }


    public async Task HandleMessageAsync(IMessage message)
    {
        switch (message)
        {
            case ItemCraftedResponse crafted:
                await HandleItemCrafted(crafted);
                break;
        }
    }

    private async Task HandleItemCrafted(ItemCraftedResponse message)
    {
        try
        {
            var charItem = new CharacterItems
            {
                ItemId = message.ItemId,
                CharacterId = message.CharacterId
            };
            await _mediator.Send(new CreateCharacterItemCommand(charItem));
            var response = new AcknowledgeResponse
            {
                SagaId = message.SagaId,
                CharacterId = message.CharacterId,
                IsAcknowledged = true

            };
            await _messageBroker.Publish(QueueNames.CharacterServiceQueueOut, response);
        }
        catch (Exception e)
        {
            var failure = new RequestFailed
            {
                SagaId = message.SagaId,
                CharacterId = message.CharacterId,
                ErrorMessage = e.Message
            };
            await _messageBroker.Publish(QueueNames.CharacterCompensationQueueOut, failure);
        }
    }
}