using Dapper;
using OsirisOnlineBetting.Constants;
using OsirisOnlineBetting.Models.Requests;
using OsirisOnlineBetting.Models.Responses;
using System.Data;

namespace OsirisOnlineBetting.Repositories
{
    public class CasinoRepository(IDbConnection dbConnection) : ICasinoRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<CasinoWagerResponse> GetPlayerWagers(PlayerWagersRequest request)
        {
            var results =  await _dbConnection.QueryAsync<CasinoResponse>(
            StoredProcedure.GetPlayerWagers, 
            request,
            commandType: CommandType.StoredProcedure);

            return new CasinoWagerResponse
            {
                Data = results.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList(),
                Page = request.PageNumber,
                PageSize = request.PageSize,
                Total = results.Count(),
                TotalPages = (int)Math.Ceiling( (double)results.Count() / request.PageSize)
            };
        }

        public async Task<IEnumerable<TopSpenderResponse>> GetTopSpenders(TopSpenderRequest request)
        {
            return await _dbConnection.QueryAsync<TopSpenderResponse>(
                StoredProcedure.GetTopSpenders, 
                request, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task InsertCasinoWagerAsync(CasinoWagerRequest request)
        {
            await _dbConnection.ExecuteAsync(
                StoredProcedure.InsertPlayerCasinoWager, 
                request, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task InsertPlayerAsync(InsertPlayerRequest request)
        {
            await _dbConnection.ExecuteAsync(
                StoredProcedure.InsertPlayer, 
                request, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PlayerResponse>> GetPlayer(GetPlayerRequest player)
        {
            return await _dbConnection.QueryAsync<PlayerResponse>(
                StoredProcedure.GetPlayer, 
                player, 
                commandType: CommandType.StoredProcedure);
        }
    }
}
