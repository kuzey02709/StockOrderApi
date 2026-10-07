using FluentValidation;
using StockOrderApi.DTOs;

namespace StockOrderApi.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Geçerli bir Müşteri ID girilmelidir.");

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("Siparişte en az bir ürün bulunmalıdır.");

        RuleFor(x => x.OrderItems)
            .ForEach(item =>
            {
                item.SetValidator(new CreateOrderItemDtoValidator());
            });
    }
}

public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Geçerli bir Ürün ID girilmelidir.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Sipariş adedi 0'dan büyük olmalıdır.");
    }
}