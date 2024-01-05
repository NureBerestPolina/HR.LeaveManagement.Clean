using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails
{
    public class GetLeaveAllocationDetailsQueryHandler : IRequestHandler<GetLeaveAllocationDetailsQuery, LeaveAllocationDetailsDto>
    {
        private readonly IMapper mapper;
        private readonly ILeaveAllocationRepository allocationRepository;

        public GetLeaveAllocationDetailsQueryHandler(IMapper mapper,
                                  ILeaveAllocationRepository allocationRepository)
        {
            this.mapper = mapper;
            this.allocationRepository = allocationRepository;
        }

        public async Task<LeaveAllocationDetailsDto> Handle(GetLeaveAllocationDetailsQuery request, CancellationToken cancellationToken)
        {
            // Query the DB
            var leaveAllocation = await allocationRepository.GetLeaveAllocationWithDetails(request.Id);

            // Verify that record exists
            if (leaveAllocation == null)
            {
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);
            }

            // Convert Data object to DTO objects
            var data = mapper.Map<LeaveAllocationDetailsDto>(leaveAllocation);

            // Return DTO object
            return data;
        }
    }
}
