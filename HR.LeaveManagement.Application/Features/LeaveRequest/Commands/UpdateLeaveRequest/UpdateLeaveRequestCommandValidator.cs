using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
    {
        private readonly ILeaveTypeRepository leaveTypeRepository;
        private readonly ILeaveRequestRepository leaveRequestRepository;

        public UpdateLeaveRequestCommandValidator(ILeaveTypeRepository leaveTypeRepository,
                                                  ILeaveRequestRepository leaveRequestRepository)
        {
            this.leaveTypeRepository = leaveTypeRepository;
            this.leaveRequestRepository = leaveRequestRepository;

            Include(new BaseLeaveRequestValidator(leaveTypeRepository));

            RuleFor(p => p.Id)
                .NotNull()
                .MustAsync(LeaveRequestMustExist)
                .WithMessage("{PropertyName} must be present");
        }

        private async Task<bool> LeaveRequestMustExist(int id, CancellationToken cancellationToken)
        { 
            var leaveRequest = leaveRequestRepository.GetByIdAsync(id);
            return leaveRequest != null;
        }
    }
}
