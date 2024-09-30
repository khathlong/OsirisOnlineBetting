using OsirisOnlineBetting.Models.Responses;

namespace OsirisOnlineBetting.Interfaces
{
    public interface IRabbitMqService
    {
        Task Publish<T>(string queue, T message);
        Task<CasinoWagerResponse> ConsumePlayerWagers(string queueName);
        Task<IEnumerable<TopSpenderResponse>> ConsumeTopSpenders(string queueName);
        Task<bool> ConsumeInsertCasinoWager(string queueName);
    }
}
