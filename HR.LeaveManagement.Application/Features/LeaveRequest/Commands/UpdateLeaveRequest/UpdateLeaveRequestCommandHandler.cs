using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly ILeaveTypeRepository leaveTypeRepository;
        private readonly IEmailSender emailSender;
        private readonly IMapper mapper;
        private readonly IAppLogger<UpdateLeaveRequestCommandHandler> appLogger;

        public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                ILeaveTypeRepository leaveTypeRepository,
                                                IEmailSender emailSender,
                                                IMapper mapper,
                                                IAppLogger<UpdateLeaveRequestCommandHandler> appLogger)
        {
            this.leaveRequestRepository = leaveRequestRepository;
            this.leaveTypeRepository = leaveTypeRepository;
            this.emailSender = emailSender;
            this.mapper = mapper;
            this.appLogger = appLogger;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest is null)
                throw new NotFoundException(nameof(LeaveRequest), request.Id);


            var validator = new UpdateLeaveRequestCommandValidator(leaveTypeRepository, leaveRequestRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid Leave Request", validationResult);

            mapper.Map(request, leaveRequest);

            await leaveRequestRepository.UpdateAsync(leaveRequest);

            try
            {
                // send confirmation email
                var email = new EmailMessage
                {
                    To = string.Empty, /* Get email from employee record */
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                            $"has been updated successfully.",
                    Subject = "Leave Request Updated"
                };

                await emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                appLogger.LogWarning(ex.Message);
            }

            return Unit.Value;
        }
    }
}
