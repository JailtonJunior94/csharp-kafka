using FluentValidation;
using CSharp.Kafka.Business.Domain.Dtos;

namespace CSharp.Kafka.Business.Application.Validations
{
    public class CustomerValidations : AbstractValidator<CustomerRequest>
    {
        public CustomerValidations()
        {
            RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage("O Nome é obrigatório");

            RuleFor(x => x.Email)
              .NotEmpty()
              .WithMessage("O E-mail é obrigatório")
              .EmailAddress()
              .WithMessage("O E-mail está inválido");
        }
    }
}
