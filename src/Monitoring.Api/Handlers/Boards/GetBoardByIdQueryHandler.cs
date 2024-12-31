using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries;
using Monitoring.Core.Queries.Boards;
using Monitoring.Db.Extensions;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Boards
{
    public class GetBoardByIdQueryHandler : QueryHandler<GetBoardByIdQuery, QueryResult>
    {
        private readonly IRepository<Board> _boardrepository;

        public GetBoardByIdQueryHandler(ILogger<GetBoardByIdQueryHandler> logger, IRepository<Board> boardrepository)
        {
            _boardrepository = boardrepository;
        }
        public override QueryResult Handle(GetBoardByIdQuery query, IQueryDispatcher dispatcher)
        {
            var board = _boardrepository.Query(x => x.Id == query.Id).IncludeMultiple(i => i.Pulses).FirstOrDefault();
            return new QueryResult(board);
        }
    }
}
