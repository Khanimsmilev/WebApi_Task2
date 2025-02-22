using Application.CQRS.Customers.Commands.Requests;
using Application.CQRS.Customers.Commands.Responses;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Customers.Handlers.CommandHandlers
{
    internal class CreateCustomerHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCustomerRequest, Result<CreateCustomerResponse>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<CreateCustomerResponse>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            Customer customer = new()
            {
                Name = request.Name
            };

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new Result<CreateCustomerResponse>
                {
                    Data = null,
                    Errors = ["Customer Name bosh olmamalidir"],
                    IsSuccess = false
                };
            }


            await _unitOfWork.CustomerRepository.AddAsync(customer);

            CreateCustomerResponse response = new()
            {
                Id = customer.Id,
                Name = request.Name,
            };

            return new Result<CreateCustomerResponse> { Data = response, Errors = [], IsSuccess = true };

        }
    }
}
