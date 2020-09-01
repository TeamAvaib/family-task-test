using Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Validators.Commands
{
    public class UpdateTaskMemberCommandValidator : AbstractValidator<UpdateTaskMemberCommand>
    {
        public UpdateTaskMemberCommandValidator()
        {
            RuleFor(x => x.id).NotNull().NotEmpty();
            RuleFor(x => x.memberId).NotNull().NotEmpty();
        }
    }
}
