using MassTransit;
using TechChallenge.SDK.Persistence;
using TechChallengeFiap.Messages;

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
            
            ValidateContact(message);

            var contact = new TechChallenge.SDK.Models.Contact
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                PhoneDdd = message.DDD,
                PhoneNumber = message.Phone,
                EmailAddress = message.Email
            };

            await _contactRepository.AddAsync(contact);

            _logger.LogInformation($"Contato {message.FirstName} removido com sucesso!");
        }

        private void ValidateContact(CreateContactMessage message) 
        {
            if (string.IsNullOrEmpty(message.FirstName))
            {
                throw new ArgumentNullException(nameof(message.FirstName));
            }
            if (string.IsNullOrEmpty(message.LastName))
            {
                throw new ArgumentNullException(nameof(message.LastName));
            }
            if (message.DDD is < 11 or > 99)
            {
                throw new ArgumentNullException(nameof(message.DDD));
            }
            if (message.Phone.ToString().Length is < 8 or > 9)
            {
                throw new ArgumentNullException(nameof(message.Phone));
            }
            if (string.IsNullOrEmpty(message.Email))
            {
                throw new ArgumentNullException(nameof(message.Email));
            }
        }
    }
}
