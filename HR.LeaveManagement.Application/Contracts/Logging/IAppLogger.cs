using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Contracts.Logging
{
    public interface IAppLogger<T>
    {
        void LoqInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
    }
}
