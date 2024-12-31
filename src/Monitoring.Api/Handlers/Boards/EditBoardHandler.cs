using AutoMapper;
using Monitoring.Core.Commands;
using Monitoring.Core.Commands.Boards;
using Monitoring.Db.Extensions;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Boards
{
    public class EditBoardHandler : CommandHandler<EditBoardCommand>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Board> _boardrepository;

        public EditBoardHandler(ILogger<EditBoardHandler> logger, IRepository<Board> boardrepository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _boardrepository = boardrepository;
        }
        public override void Handle(EditBoardCommand command)
        {
            var board = _boardrepository.Query(x => x.Id == command.Id).IncludeMultiple(i => i.Pulses).FirstOrDefault();

            if (board != null)
            {

                var mappedPulses = _mapper.Map<List<Puls>>(command.Pulses);

                board.IpAddress = command.IpAddress;
                board.NumberOfPulses = command.Pulses != null ? command.NumberOfPulses : board.NumberOfPulses;
                board.Pulses = command.Pulses != null ? mappedPulses : board.Pulses;
                _boardrepository.Update(board);
                _boardrepository.SaveChanges();
                _logger.LogInformation($"Boad : {command.Id} has been updated");
            }
            _logger.LogInformation($"Boad : {command.Id} Cannot Find");
        }
    }
}
