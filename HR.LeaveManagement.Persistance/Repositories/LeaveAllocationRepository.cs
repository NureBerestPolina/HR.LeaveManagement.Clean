using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistance.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistance.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        public LeaveAllocationRepository(HrDatabaseContext context) : base(context)
        {

        }

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {
            await context.AddRangeAsync(allocations);
        }

        public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
        {
            return await context.LeaveAllocations.AnyAsync(q => q.EmployeeId == userId
                                                          && q.LeaveTypeId == leaveTypeId
                                                          && q.Period == period);
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            var leaveAllocations = await context.LeaveAllocations
                                                .Include(q => q.LeaveType)
                                                .ToListAsync();
            return leaveAllocations;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
        {
            var leaveAllocations = await context.LeaveAllocations
                                                .Where(q => q.EmployeeId == userId)
                                                .Include(q => q.LeaveType)
                                                .ToListAsync();
            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            return await context.LeaveAllocations
                                                .Include(q => q.LeaveType)
                                                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
        {
            return await context.LeaveAllocations
                                               .Include(q => q.LeaveType)
                                               .FirstOrDefaultAsync(q => q.EmployeeId == userId
                                                                    && q.LeaveTypeId == leaveTypeId);
        }
    }
}
