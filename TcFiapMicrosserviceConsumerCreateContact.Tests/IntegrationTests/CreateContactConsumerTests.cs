using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using TCFiapMicrosserviceConsumerCreateContact.Worker;
using TechChallenge.SDK.Domain.Models;
using TechChallenge.SDK.Domain.ValueObjects;
using TechChallenge.SDK.Infrastructure.Message;
using TechChallenge.SDK.Infrastructure.Persistence;

namespace TcFiapMicrosserviceConsumerCreateContact.Tests.IntegrationTests
{
    public class CreateContactConsumerTests
    {
        private readonly Mock<ILogger<CreateContactConsumer>> _loggerMock;
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly CreateContactConsumer _consumer;

        public CreateContactConsumerTests()
        {
            _loggerMock = new Mock<ILogger<CreateContactConsumer>>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _consumer = new CreateContactConsumer(_loggerMock.Object, _contactRepositoryMock.Object);
        }

        [Fact]
        public async Task Consume_Should_AddContact_ToRepository()
        {
            // Arrange
            var message = new CreateContactMessage("Teste", "teste", 
                11, 987654321, "teste.email@example.com");

            var contextMock = new Mock<ConsumeContext<CreateContactMessage>>();
            contextMock.Setup(c => c.Message).Returns(message);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        } 
    }
}

