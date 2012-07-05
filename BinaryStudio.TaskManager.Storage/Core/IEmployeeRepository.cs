using System;
using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        
        void Delete(int id);
        
        void Update(Employee employee);

        Employee GetById(int employeeId);

        IEnumerable<Employee> Get(Func<Employee> selector);

        IList<Employee> GetAll();
    }
}