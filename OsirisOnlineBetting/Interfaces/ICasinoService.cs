using OsirisOnlineBetting.Models.Requests;
using OsirisOnlineBetting.Models.Responses;

namespace OsirisOnlineBetting.Interfaces
{
    public interface ICasinoService
    {
        Task InsertCasinoWager(CasinoRequest wager);
        Task<CasinoWagerResponse> GetPlayerWagers(PlayerWagersRequest request);
        Task<IEnumerable<TopSpenderResponse>> GetTopSpenders(TopSpenderRequest request);
    }
}
