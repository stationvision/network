using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Queries;
using Monitoring.Core.Queries.Puls;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Pulses
{
    public class GetAllPulsQueryHandler : IQueryHandler<GetAlPulslQuery>
    {
        private readonly ILogger<GetAllPulsQueryHandler> _logger;
        private readonly IRepository<Puls> _repository;

        public GetAllPulsQueryHandler(ILogger<GetAllPulsQueryHandler> logger, IRepository<Puls> repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public QueryResult Handle(IQuery query, IQueryDispatcher dispatcher)
        {
            var pulses = _repository.GetAll().ToList();
            return new QueryResult(pulses);
        }
    }
}
