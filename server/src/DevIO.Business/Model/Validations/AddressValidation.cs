using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Model.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("The field {PropertyName} is required")
                .Length(2, 20).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength}");

            RuleFor(x => x.Neighborhood)
                .NotEmpty().WithMessage("The field {PropertyName} is required");

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("The field {PropertyName} is required")
                .Length(8).WithMessage("The field {PrpertyName} max length {MaxLength}");
            
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("The field {PropertyName} is require")
                .Length(2, 100).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength}");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("The field {PropertyName} is require")
                .Length(2, 50).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength}");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("The field {PropertyName} is require")
                .Length(1, 50).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength}");
        }
    }
}
