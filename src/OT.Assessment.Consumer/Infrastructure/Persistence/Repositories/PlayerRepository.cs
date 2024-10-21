
namespace OT.Assessment.Consumer.Infrastructure.Persistence.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        public readonly IDbConnection _dbConnection;
        public PlayerRepository(IDbConnection dbConnection) 
        { 
            _dbConnection = dbConnection;
        }
        public async Task AddCasinoWagerAsync(CasinoWager wager)
        {
            CheckDBConnection();

            var sql = @"EXEC sp_AddCasinoWager @WagerId, @Theme, @Provider,@GameName, @TransactionId, @BrandId, @AccountId, @Username, 
                        @ExternalReferenceId, @TransactionTypeId, @Amount, @CreatedDateTime, @NumberOfBets, @CountryCode, @SessionData, @Duration";
            await _dbConnection.ExecuteAsync(sql, wager, commandTimeout: 60);
        }

        public async Task<List<TopSpender>> GetTopSpendersAsync(int count)
        {
            CheckDBConnection();

            var sql = @"EXEC sp_GetTopSpenders @count";
            var topSPenderResult = await _dbConnection.QueryAsync<TopSpender>(sql, new { count }, commandTimeout:120);

            return topSPenderResult.ToList();
        }

        public async Task<PlayerWagerPage> GetPlayerWagersAsync(string playerId, int pageSize, int page)
        {
            var playerWagerPage = new PlayerWagerPage() { Page = page, PageSize = pageSize };
            CheckDBConnection();

            var pageSql = @"SELECT WagerId, GameName AS Game, Provider, Amount, CreatedDateTime AS CreatedDate
                    FROM (
                        SELECT WagerId, GameName, Provider, Amount, CreatedDateTime, ROW_NUMBER() OVER (ORDER BY CreatedDateTime DESC) AS RowNum FROM CasinoWagers WITH (NOLOCK) 
                        WHERE AccountId = @playerId) AS PaginatedResult
                        WHERE RowNum BETWEEN @startIndex + 1 AND @startIndex + @pageSize ;";

            var sizeSql = @"SELECT COUNT(WagerId) AS TotalRecords FROM CasinoWagers WITH (NOLOCK) WHERE AccountId = @playerId;";
            var parameters = new
            {
                playerId = playerId,
                startIndex = (page - 1) * pageSize,
                pageSize = pageSize
            };

            var wagers = await _dbConnection.QueryAsync<Wager>(pageSql, parameters);
            var rowCountData = await _dbConnection.QueryAsync<int>(sizeSql, new { playerId });

            playerWagerPage.Data = wagers.ToList();
            playerWagerPage.Total = rowCountData.LastOrDefault();
            playerWagerPage.TotalPages = playerWagerPage.Total % pageSize != 0 ?
                Math.Abs(playerWagerPage.Total / pageSize) + 1 : playerWagerPage.Total / pageSize;

            return playerWagerPage;
        }

        private void CheckDBConnection()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
        }
    }
}
