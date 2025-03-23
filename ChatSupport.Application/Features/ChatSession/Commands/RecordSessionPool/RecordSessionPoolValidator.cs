using FluentValidation;

namespace ChatSupport.Application.Features.ChatSession.Commands.RecordSessionPool
{
    public class RecordSessionPoolValidator : AbstractValidator<RecordSessionPoolCommand>
    {
        public RecordSessionPoolValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }
}
