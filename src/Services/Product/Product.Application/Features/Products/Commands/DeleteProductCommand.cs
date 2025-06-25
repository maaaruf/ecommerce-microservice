using MediatR;
using Product.Application.Interfaces;

namespace Product.Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest
{
    public string Id { get; set; } = string.Empty;
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductService _productService;

    public DeleteProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _productService.DeleteProductAsync(request.Id);
    }
} 