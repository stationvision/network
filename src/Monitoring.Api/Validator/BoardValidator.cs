using FluentValidation;
using Monitoring.Db.Models;
using System.Text.RegularExpressions;

namespace Monitoring.Api.Validator
{
    public class BoardValidator : AbstractValidator<Board>
    {
        public BoardValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.IpAddress).Must(BeAValidIP);
        }

        private bool BeAValidIP(string arg)
        {
            return arg != null && Regex.IsMatch(arg, "^(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])$\r\n");
        }
    }

}


