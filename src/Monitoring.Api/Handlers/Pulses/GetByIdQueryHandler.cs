using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries;
using Monitoring.Core.Queries.Puls;
using Monitoring.Db.Extensions;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;
namespace Monitoring.Api.Handlers.Pulses
{
    public class GetByIdQueryHandler : QueryHandler<GetByIdQuery, QueryResult>
    {
        private readonly ILogger<GetByIdQueryHandler> _logger;
        private readonly IRepository<Puls> _repository;

        public GetByIdQueryHandler(ILogger<GetByIdQueryHandler> logger, IRepository<Puls> repository)
        {
            _logger = logger;
            _repository = repository;
        }


        public override QueryResult Handle(GetByIdQuery query, IQueryDispatcher dispatcher)
        {
            var puls = _repository.Query(x => x.Id == query.Id).IncludeMultiple(i => i.PulseType, i => i.Nature).FirstOrDefault();
            return new QueryResult(puls);
        }
    }
}
