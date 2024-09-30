using OsirisOnlineBetting.Models.Requests;
using OsirisOnlineBetting.Models.Responses;

namespace OsirisOnlineBetting.Repositories
{
    public interface ICasinoRepository
    {
        Task InsertPlayerAsync(InsertPlayerRequest request);
        Task InsertCasinoWagerAsync(CasinoWagerRequest wager);
        Task<CasinoWagerResponse> GetPlayerWagers(PlayerWagersRequest request);
        Task<IEnumerable<TopSpenderResponse>> GetTopSpenders(TopSpenderRequest request);
        Task<IEnumerable<PlayerResponse>> GetPlayer(GetPlayerRequest player);
    }
}
