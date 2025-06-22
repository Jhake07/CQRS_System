using CleanArchitectureSystem.Application.Contracts.Interface;
using FluentValidation;
namespace CleanArchitectureSystem.Application.Features.BatchSerial.Commands.UpdateBatchSerials
{
    public class UpdateBatchSerialCommandValidatorForExistingContract : AbstractValidator<UpdateBatchSerialCommand>
    {

        public UpdateBatchSerialCommandValidatorForExistingContract(IBatchSerialRepository batchSerialRepository)
        {
            RuleFor(p => p.ContractNo)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            //.NotNull().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.DocNo)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            //.NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}
