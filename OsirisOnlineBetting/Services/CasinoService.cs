using OsirisOnlineBetting.Interfaces;
using OsirisOnlineBetting.Models.Requests;
using OsirisOnlineBetting.Models.Responses;

namespace OsirisOnlineBetting.Services
{
    public class CasinoService(IRabbitMqService rabbitMqService) : ICasinoService
    {
        private readonly IRabbitMqService _rabbitMqService = rabbitMqService;

        public async Task<CasinoWagerResponse> GetPlayerWagers(PlayerWagersRequest request)
        {
            await _rabbitMqService.Publish("casino_wager_queue", request);
            return await _rabbitMqService.ConsumePlayerWagers("casino_wager_queue");
        }

        public async Task<IEnumerable<TopSpenderResponse>> GetTopSpenders(TopSpenderRequest request)
        {
            await _rabbitMqService.Publish("casino_wager_queue", request);
            return await _rabbitMqService.ConsumeTopSpenders("casino_wager_queue");
        }

        public async Task InsertCasinoWager(CasinoRequest request)
        {
            await _rabbitMqService.Publish("casino_wager_queue", request);
            await _rabbitMqService.ConsumeInsertCasinoWager("casino_wager_queue");
        }

    }
}
