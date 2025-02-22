using Application.CQRS.Products.Commands.Responses;
using Common.GlobalResponses.Generics;
using MediatR;

namespace Application.CQRS.Products.Commands.Requests;

public class CreateProductRequest : IRequest<Result<CreateProductResponse>>
{
    public string Name { get; set; }
}
