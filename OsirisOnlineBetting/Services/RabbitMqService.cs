using OsirisOnlineBetting.Interfaces;
using OsirisOnlineBetting.Models.Requests;
using OsirisOnlineBetting.Models.Responses;
using OsirisOnlineBetting.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OsirisOnlineBetting.Services
{
    public class RabbitMqService(IConnectionFactory connectionFactory, ICasinoRepository casinoRepository) : IRabbitMqService
    {
        private readonly IConnectionFactory _connectionFactory = connectionFactory;
        private readonly ICasinoRepository _casinoRepository = casinoRepository;

        public Task Publish<T>(string queue, T message)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: messageBody);
            return Task.CompletedTask;
        }

        public async Task<CasinoWagerResponse> ConsumePlayerWagers(string queueName)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            var tcs = new TaskCompletionSource<CasinoWagerResponse>();

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var request = JsonSerializer.Deserialize<PlayerWagersRequest>(Encoding.UTF8.GetString(body));
                var results = await _casinoRepository.GetPlayerWagers(request);
                tcs.SetResult(results);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return await tcs.Task;
        }

        public async Task<IEnumerable<TopSpenderResponse>> ConsumeTopSpenders(string queueName)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            var tcs = new TaskCompletionSource<IEnumerable<TopSpenderResponse>>();

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var request = JsonSerializer.Deserialize<TopSpenderRequest>(Encoding.UTF8.GetString(body));
                var results = await _casinoRepository.GetTopSpenders(request);
                tcs.SetResult(results);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return await tcs.Task;
        }

        public async Task<bool> ConsumeInsertCasinoWager(string queueName)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            var tcs = new TaskCompletionSource<bool>();

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var request = JsonSerializer.Deserialize<CasinoRequest>(Encoding.UTF8.GetString(body));

                var players = await _casinoRepository.GetPlayer(new GetPlayerRequest { AccountId = request.AccountId });
                if (!players.Any())
                    await _casinoRepository.InsertPlayerAsync(new InsertPlayerRequest
                    {
                        AccountId = request.AccountId,
                        Username = request.Username
                    });

                var payload = new CasinoWagerRequest
                {
                    WagerId = request.WagerId,
                    GameName = request.GameName,
                    Provider = request.Provider,
                    AccountId = request.AccountId,
                    Amount = request.Amount,
                    CreatedDateTime = request.CreatedDateTime,
                    Theme = request.Theme,
                    TransactionId = request.TransactionId,
                    BrandId = request.BrandId,
                    ExternalReferenceId = request.ExternalReferenceId,
                    TransactionTypeId = request.TransactionTypeId,
                    NumberOfBets = request.NumberOfBets,
                    CountryCode = request.CountryCode,
                    SessionData = request.SessionData,
                    Duration = request.Duration,
                };

                await _casinoRepository.InsertCasinoWagerAsync(payload);
                tcs.SetResult(true);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return await tcs.Task;
        }

    }
}
