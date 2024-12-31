using Monitoring.Core.Commands;
using Monitoring.Core.Commands.Puls;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Pulses
{
    public class EditPulsHandler : CommandHandler<EditPulsCommand>
    {
        private readonly ILogger<EditPulsHandler> _logger;
        private readonly IRepository<Puls> _repository;
        private readonly IRepository<Pulsnature> _pulsrepository;
        private readonly IRepository<PulseType> _pulstyperepository;

        public EditPulsHandler(ILogger<EditPulsHandler> logger, IRepository<Puls> repository, IRepository<Pulsnature> pulsrepository, IRepository<PulseType> pulstyperepository)
        {
            _logger = logger;
            _repository = repository;
            _pulsrepository = pulsrepository;
            _pulstyperepository = pulstyperepository;
        }

        public override void Handle(EditPulsCommand command)
        {
            var puls = _repository.Query(x => x.Id == command.Id).FirstOrDefault();
            if (puls == null)
            {
                _logger.LogError($"Puls with ID {command.Name} does not exist");
                return;
            }

            var nature = _pulsrepository.Query(x => x.Name == command.Nature).FirstOrDefault();
            if (nature == null)
            {
                _logger.LogError($"PulsNature with Name {command.Nature} does not exist");
                throw new Exception($"PulsNature with Name {command.Nature} does not exist");
            }
            var pulseType = _pulstyperepository.Query(x => x.Name == command.pulseType).FirstOrDefault();
            if (pulseType == null)
            {
                _logger.LogError($"PulseType with Name {command.pulseType} does not exist");
                throw new Exception($"PulseType with Name {command.pulseType} does not exist");
            }
            puls.Name = command.Name;
            puls.Nature = nature;
            puls.BoardId = command.BoardId;
            puls.IgnoredDurationThreshold = command.IgnorableDuration;
            puls.IgnoredDifferenceThreshold = command.IgnoredDifferenceThreshold;
            puls.PulseType = pulseType;
            puls.TrackingRange = command.TrackingRange;
            puls.NumberofTrackingRange = command.NumberofTrackingRange;

            _repository.Update(puls);
            _repository.SaveChanges();
        }
    }
}
