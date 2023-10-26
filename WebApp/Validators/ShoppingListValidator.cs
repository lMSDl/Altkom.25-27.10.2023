using FluentValidation;
using Models;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Validators
{
    public class ShoppingListValidator : AbstractValidator<ShoppingList>
    {

        public ShoppingListValidator(IShoppingListsService service)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("puste???")
                .Length(5, 15)
                .Must(x => x.EndsWith(".")).WithMessage("Wartość musi kończyć się kropką")
                .WithName("Nazwa");

            RuleFor(x => x.Name).Must(x => !service.ReadAsync().Result.Any(xx => xx.Name == x)).WithMessage("Lista o tej nazwie już istnieje");
        }
    }
}
