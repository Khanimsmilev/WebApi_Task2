using Application.CQRS.Products.Commands.Requests;
using Application.CQRS.Products.Commands.Responses;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers.CommandHandlers;

public class CreateProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductRequest, Result<CreateProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<CreateProductResponse>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        Product product = new()
        {
            Name = request.Name
        };

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return new Result<CreateProductResponse>
            {
                Data = null,
                Errors = ["Product Name bosh olmamalidir"],
                IsSuccess = false
            };
        }


        await _unitOfWork.ProductRepository.AddAsync(product);

        CreateProductResponse response = new()
        {
            Id = product.Id,
            Name = request.Name,
        };

        return new Result<CreateProductResponse> { Data = response, Errors = [], IsSuccess = true };

    }
}
