using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Model.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required!")
                .Length(2, 200).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength}");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The field {PropertyName} is required!")
                .Length(2, 1000).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength}");

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("The field {PropertyName} must be greather than {ComparisonValue}");

        }
    }
}
