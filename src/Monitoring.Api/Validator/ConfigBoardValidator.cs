using FluentValidation;
using Monitoring.Core.Dtos;

namespace Monitoring.Api.Validator
{
    public class ConfigBoardValidator : AbstractValidator<ConfigBoardDto>
    {

        public ConfigBoardValidator()
        {
            RuleFor(x => x.BoardId1).NotEmpty();
            RuleFor(x => x.BoardId2).NotEmpty();
            RuleFor(x => x.BoardId3).NotEmpty();
            RuleFor(x => x.BoardId4).NotEmpty();

        }
    }
}
