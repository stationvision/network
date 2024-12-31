using Monitoring.Core.Commands;
using Monitoring.Core.Commands.Boards;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Models;

namespace Monitoring.Api.Handlers.Boards
{
    public class DeleteBaordHandler : CommandHandler<DeleteBoardCommand>
    {
        private readonly ILogger _logger;
        private readonly IRepository<Board> _boardrepository;

        public DeleteBaordHandler(ILogger<DeleteBaordHandler> logger, IRepository<Board> boardrepository)
        {
            _logger = logger;
            _boardrepository = boardrepository;
        }
        public override void Handle(DeleteBoardCommand command)
        {
            var board = _boardrepository.Query(x => x.Id == command.BoardId).FirstOrDefault();
            if (board != null)
            {
                _boardrepository.Delete(board);
                _boardrepository.SaveChanges();
                _logger.LogWarning($"Board : {command.BoardId} has been deleted");
            }
            _logger.LogInformation($"Board : {command.BoardId} Cannot Find");
        }
    }
}
