using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataBaseContext dataBaseContext;


        public EmployeeRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        public void Add(Employee employee)
        {
            this.dataBaseContext.Entry(employee).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public void Delete(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void Update(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Employee GetById(int employeeId)
        {
            return this.dataBaseContext.Employees.Single(it => it.Id == employeeId);
        }

        public IEnumerable<Employee> Get(Func<Employee> selector)
        {
            throw new NotImplementedException();
        }

        public  IList<Employee> GetAll()
        {
            return this.dataBaseContext.Employees.ToList();
        }
    }
}