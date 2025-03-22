# TCFiap Microsservice Consumer Create Contact

Este projeto é um serviço .NET que consome mensagens para criação de contatos através do MassTransit e RabbitMQ. Ao receber uma mensagem do tipo `CreateContactMessage`, o serviço cria um novo contato com os dados fornecidos (nome, telefone, e-mail).

## Sumário

- [Visão Geral](#visão-geral)
- [Recursos](#recursos)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Pré-requisitos](#pré-requisitos)
- [Como Executar o Projeto](#como-executar-o-projeto)
  - [Executando Localmente](#executando-localmente)
  - [Utilizando Docker](#utilizando-docker)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Testes](#testes)
- [Configuração](#configuração)

## Visão Geral

O TCFiap Microsservice Consumer Create Contact é responsável por consumir mensagens de criação de contatos. Após um atraso inicial de 15 segundos para garantir que todas as dependências estejam prontas, o serviço:

- Configura a conexão com o banco de dados e o host do RabbitMQ a partir de variáveis de ambiente.
- Registra o módulo do TechChallenge.SDK com a connection string.
- Configura o MassTransit para consumir mensagens do endpoint `create-contact-queue`.
- Ao receber uma mensagem `CreateContactMessage`, cria um novo contato e adiciona-o ao repositório, registrando as operações via logs.

## Recursos

- **Consumo de Mensagens:** Processa mensagens para criação de contatos.
- **Integração com RabbitMQ:** Configura o endpoint `create-contact-queue` para receber as mensagens.
- **Criação de Contatos:** Mapeia os dados da mensagem para instanciar e persistir um novo contato.
- **Logs Detalhados:** Registra informações sobre o processamento das mensagens e a execução do worker.
- **Testes:** Inclui testes de integração e unitários utilizando Moq para simulação de dependências.

## Tecnologias Utilizadas

- .NET 8
- MassTransit
- RabbitMQ
- Docker
- XUnit
- Moq
- TechChallenge.SDK

## Pré-requisitos

- .NET SDK 8.0
- RabbitMQ (disponível localmente ou via container)
- Variável de ambiente para a connection string:
  - `CONNECTION_DATABASE`: string de conexão com o banco de dados.
- Variável de ambiente para o host do RabbitMQ:
  - `RABBITMQ_HOST` (opcional, padrão: `localhost`).
- Docker, se desejar executar via container.

## Como Executar o Projeto

### Executando Localmente

Clone o repositório:

```
git clone https://seurepositorio.com/TCFiapMicrosserviceConsumerCreateContact.git
cd TCFiapMicrosserviceConsumerCreateContact
```

Configure as variáveis de ambiente:

- `CONNECTION_DATABASE`: string de conexão com o banco de dados.
- `RABBITMQ_HOST`: (opcional) host do RabbitMQ.

Restaure os pacotes e compile o projeto:

```
dotnet restore
dotnet build
```

Execute a aplicação:

```
dotnet run --project TCFiapMicrosserviceConsumerCreateContact.Worker
```

### Utilizando Docker

O projeto contém um Dockerfile que utiliza um processo de build multi-stage.

Defina o argumento de senha do NuGet (caso necessário) e construa a imagem Docker:

```
docker build --build-arg ARG_SECRET_NUGET_PACKAGES=SuaSenhaAqui -t tcfiap-microservice-create-contact .
```

Execute o container, definindo as variáveis de ambiente necessárias:

```
docker run -d -p 8080:8080 --env CONNECTION_DATABASE="SuaConnectionString" --env RABBITMQ_HOST="SeuHostRabbitMQ" tcfiap-microservice-create-contact
```

## Estrutura do Projeto

- **TCFiapMicrosserviceConsumerCreateContact.Worker:** Projeto principal contendo:
  - **Program.cs:** Configura o host, registra o TechChallenge.SDK e o MassTransit com o consumidor `CreateContactConsumer`.
  - **CreateContactConsumer.cs:** Consumidor que processa mensagens `CreateContactMessage` e cria um novo contato.
  - **Worker.cs:** Serviço de background que inicia e finaliza o bus do MassTransit.
- **Testes:**
  - **TcFiapMicrosserviceConsumerCreateContact.Tests.IntegrationTests:** Testes de integração para validar o fluxo de criação de contatos.
- **Dockerfile:** Configuração para build multi-stage e publicação da aplicação.

## Testes

O projeto inclui testes que garantem o funcionamento correto da criação de contatos:

- **Testes de Integração:** Validam o fluxo completo de consumo de mensagens e a adição de contatos ao repositório usando Moq.


## Configuração

**MassTransit & RabbitMQ:**

- **Endpoint de Recebimento:** `create-contact-queue`

**SDK:**

- O módulo do TechChallenge.SDK é registrado via o método `RegisterSdkModule`, utilizando a connection string do banco de dados.

**Variáveis de Ambiente:**

- `CONNECTION_DATABASE`: String de conexão com o banco de dados.
- `RABBITMQ_HOST`: Host do RabbitMQ (padrão: `localhost`).
