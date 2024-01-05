using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations
{
    public class GetLeaveAllocationsQueryHandler : IRequestHandler<GetLeaveAllocationsQuery, List<LeaveAllocationDto>>
    {
        private readonly IMapper mapper;
        private readonly ILeaveAllocationRepository leaveAllocationRepository;
        private readonly IAppLogger<GetLeaveAllocationsQueryHandler> logger;

        public GetLeaveAllocationsQueryHandler(IMapper mapper,
                                  ILeaveAllocationRepository leaveAllocationRepository,
                                  IAppLogger<GetLeaveAllocationsQueryHandler> logger)
        {
            this.mapper = mapper;
            this.leaveAllocationRepository = leaveAllocationRepository;
            this.logger = logger;
        }

        public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationsQuery request, CancellationToken cancellationToken)
        {
            // Query the DB
            var leaveAllocations = await leaveAllocationRepository.GetAsync();

            // Convert Data objects to DTO objects
            var data = mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

            // Return list of DTO objects
            logger.LoqInformation("Leave allocations were retrieved successfully");
            return data;
        }
    }
}
