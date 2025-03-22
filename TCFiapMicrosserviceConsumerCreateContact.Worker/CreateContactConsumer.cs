using MassTransit;
using TechChallenge.SDK.Domain.Models;
using TechChallenge.SDK.Domain.ValueObjects;
using TechChallenge.SDK.Infrastructure.Message;
using TechChallenge.SDK.Infrastructure.Persistence;

namespace TCFiapMicrosserviceConsumerCreateContact.Worker
{
    public class CreateContactConsumer : IConsumer<CreateContactMessage>
    {
        private readonly ILogger<CreateContactConsumer> _logger;
        private readonly IContactRepository _contactRepository;

        public CreateContactConsumer(ILogger<CreateContactConsumer> logger, IContactRepository contactRepository)
        {
            _logger = logger;
            _contactRepository = contactRepository;
        }

        public async Task Consume(ConsumeContext<CreateContactMessage> context)
        {
            var message = context.Message;
            _logger.LogInformation($"Recebida solicitação para criar o contato com nome : {message.FirstName}");

            var name = new Name(message.FirstName, message.LastName);
            var phone = new Phone(message.DDD, message.Phone);
            var email = new Email(message.Email);


            var existingContact = _contactRepository.Query()
                .Where( x=> x.Phone.DDD == phone.DDD && x.Phone.Number == phone.Number)
                .Any();

            if (existingContact)
                _logger.LogInformation($"Contato com o numero {phone.DDD} {phone.Number} já existe!");

            var contact = new Contact(name, email, phone);

            await _contactRepository.AddAsync(contact);

            _logger.LogInformation($"Contato {message.FirstName} adicionado com sucesso!");
        }
    }
}
