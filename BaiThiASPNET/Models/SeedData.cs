using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BaiThiASPNET.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HRMContext(
                serviceProvider.GetRequiredService<DbContextOptions<HRMContext>>()))
            {
                // Look for any departments.
                if (context.Department.Any())
                {
                    return;   // DB has been seeded
                }

                context.Department.AddRange(
                    new Department
                    {
                        Name = "Accounting Department",
                        Code = "ACC",
                        Location = "Building A",
                        NumberOfEmployees = 10
                    },
                    new Department
                    {
                        Name = "Production Management Department",
                        Code = "PROD",
                        Location = "Building B",
                        NumberOfEmployees = 20
                    }
                // Add more departments as needed
                );
                context.SaveChanges();

                // Look for any employees.
                if (context.Employee.Any())
                {
                    return;   // DB has been seeded
                }

                context.Employee.AddRange(
                    new Employee
                    {
                        Name = "John Doe",
                        Code = "JD001",
                        DepartmentId = 1, // Assuming 1 is the ID of the Accounting Department
                        Rank = "Manager"
                    },
                    new Employee
                    {
                        Name = "Jane Doe",
                        Code = "JD002",
                        DepartmentId = 2, // Assuming 2 is the ID of the Production Management Department
                        Rank = "Supervisor"
                    }
                // Add more employees as needed
                );
                context.SaveChanges();
            }
        }
    }
}
