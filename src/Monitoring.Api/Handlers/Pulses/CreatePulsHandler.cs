using Monitoring.Core.Commands;
using Monitoring.Core.Commands.Puls;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Pulses
{
    public class CreatePulsHandler : CommandHandler<CreatePulsCommand>
    {
        private readonly ILogger<CreatePulsHandler> _logger;
        private readonly IRepository<Puls> _repository;
        private readonly IRepository<Pulsnature> _naturerepository;
        private readonly IRepository<PulseType> _pulsetyperepository;

        public CreatePulsHandler(ILogger<CreatePulsHandler> logger, IRepository<Puls> repository, IRepository<Pulsnature> naturerepository, IRepository<PulseType> pulsetyperepository)
        {
            _logger = logger;
            _repository = repository;
            _naturerepository = naturerepository;
            _pulsetyperepository = pulsetyperepository;
        }
        public override void Handle(CreatePulsCommand command)
        {
            var puls = _repository.Query(x => x.Name == command.Name).Any();
            if (puls)
            {
                _logger.LogError($"Puls with Name {command.Name} already exists");
                throw new Exception($"Puls with Name {command.Name} already exists");
                return;
            }
            _repository.Add(new Puls
            {
                Name = command.Name,
                PulsenatureId = _naturerepository.Query(x => x.Name == command.Nature).FirstOrDefault().Id,
                BoardId = command.BoardId,
                IgnoredDurationThreshold = command.IgnoreDurationThreshold,
                IgnoredDifferenceThreshold = command.IgnoreDiffrenceThreshold,
                PulseTypeId = _pulsetyperepository.Query(x => x.Name == command.pulseType).FirstOrDefault().Id,
                TrackingRange = command.TrackingRange,
                NumberofTrackingRange = command.NumberofTrackingRange,
            });

            _repository.SaveChanges();

        }
    }
}
