using AutoMapper;
using Monitoring.Core.Commands;
using Monitoring.Core.Commands.Puls;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Pulses
{
    public class DeletePulsHandler : CommandHandler<DeletePulsCommand>
    {
        private readonly ILogger<DeletePulsHandler> _logger;
        private readonly IRepository<Puls> _repository;
        private readonly IMapper _mapper;

        public DeletePulsHandler(ILogger<DeletePulsHandler> logger, IRepository<Puls> repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public override void Handle(DeletePulsCommand command)
        {
            var puls = _repository.Query(x => x.Name == command.PulsName).FirstOrDefault();
            if (puls == null)
            {
                _logger.LogError($"Puls with ID {command.Name} does not exist");
                return;
            }

            _repository.Delete(puls);
            _repository.SaveChanges();
        }
    }
}
