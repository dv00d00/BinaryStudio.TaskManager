using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IClientConnectionManager
    {
        ClientConnection GetClientByEmployeeId(int employeeId);       
    }
    
}
