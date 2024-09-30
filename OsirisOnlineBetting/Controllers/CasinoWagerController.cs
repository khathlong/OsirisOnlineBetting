using OsirisOnlineBetting.Interfaces;
using OsirisOnlineBetting.Models.Requests;

namespace OsirisOnlineBetting.Controllers
{
    public static class CasinoWagerController
    {
        public static void ConfigureCasinoWagerEndpoints(this WebApplication app)
        {
            app.MapPost("/api/player/casinowager", async (CasinoRequest request, ICasinoService casinoService) =>
            {
                await casinoService.InsertCasinoWager(request);
                return Results.Ok();
            });

            app.MapGet("/api/player/{playerId}/casino", async (Guid playerId, int page, int pageSize, ICasinoService casinoService) =>
            {
                if (page <= 0 || pageSize <= 0)
                    throw new ArgumentException("Please ensure that unput have positive number");

                var request = new PlayerWagersRequest
                {
                    PlayerId = playerId,
                    PageNumber = page,
                    PageSize = pageSize,
                };

                var results = await casinoService.GetPlayerWagers(request);
                return Results.Ok(results);
            });

            app.MapGet("/api/player/topSpenders", async (int count, ICasinoService casinoService) =>
            {
                if (count <= 0)
                    throw new ArgumentException("Please ensure count is a positive number");

                var request = new TopSpenderRequest
                {
                    Count = count
                };
                var results = await casinoService.GetTopSpenders(request);
                return Results.Ok(results);
            });
        }
    }
}
