using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries;
using Monitoring.Core.Queries.Boards;
using Monitoring.Db.Extensions;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;
using System.Text.Json;

namespace Monitoring.Api.Handlers.Boards
{
    public class GetAllBoardsQueryHandler : IQueryHandler<GetAllBoardQuery>
    {
        private readonly ILogger<GetAllBoardsQueryHandler> _logger;
        private readonly IRepository<Board> _boardrepository;

        public GetAllBoardsQueryHandler(ILogger<GetAllBoardsQueryHandler> logger, IRepository<Board> boardrepository)
        {
            _logger = logger;
            _boardrepository = boardrepository;
        }
        public QueryResult Handle(IQuery query, IQueryDispatcher dispatcher)
        {
            var boards = _boardrepository.Query(x => x.Id != null).IncludeMultiple(x => x.Pulses).ToList();
            var json = JsonSerializer.Serialize(boards);
            return new QueryResult(boards);
        }
    }
}
