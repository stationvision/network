using AutoMapper;
using Monitoring.Core.Commands;
using Monitoring.Core.Commands.Boards;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Boards
{
    public class CreateBoardHandler : CommandHandler<CreateBoardCommand>
    {
        private readonly ILogger<CreateBoardHandler> _logger;
        private readonly IRepository<Board> _boardrepository;
        private readonly IRepository<Pulsnature> _naturerepository;
        private readonly IRepository<PulseType> _pulsetyperepository;
        private readonly IMapper _mapper;

        public CreateBoardHandler(ILogger<CreateBoardHandler> logger, IRepository<Board> boardrepository, IRepository<Pulsnature> naturerepository, IRepository<PulseType> pulsetyperepository)
        {
            _logger = logger;
            _boardrepository = boardrepository;
            _naturerepository = naturerepository;
            _pulsetyperepository = pulsetyperepository;
            this._mapper = _mapper;
        }
        public override void Handle(CreateBoardCommand command)
        {
            var board = _boardrepository.Query(x => x.Id == command.Id || x.IpAddress == command.IpAddress).Any();
            if (board)
            {
                _logger.LogError($"Board with Id {command.Id} or IpAddress {command.IpAddress} already exists");
            }

            var mappedPulses = (from e in command.Pulses
                                select new Puls()
                                {
                                    Name = e.Name,
                                    PulsenatureId = _naturerepository.Query(x => x.Name == e.Nature.Name).FirstOrDefault().Id,
                                    PulseTypeId = _pulsetyperepository.Query(x => x.Name == e.PulseType.Name).FirstOrDefault().Id,

                                    Nature = _naturerepository.Query(x => x.Name == e.Nature.Name).FirstOrDefault(),
                                    BoardId = command.Id,
                                    IgnoredDurationThreshold = e.IgnoreDurationThreshold,
                                    IgnoredDifferenceThreshold = e.IgnoredDifferenceThreshold,
                                    TrackingRange = e.TrackingRange,
                                    NumberofTrackingRange = e.NumberofTrackingRange
                                }).ToList();


            _boardrepository.Add(new Board
            {
                Id = command.Id,
                IpAddress = command.IpAddress,
                NumberOfPulses = command.NumberOfPulses,
                Pulses = mappedPulses
            });
            _boardrepository.SaveChanges();
        }
    }
}
