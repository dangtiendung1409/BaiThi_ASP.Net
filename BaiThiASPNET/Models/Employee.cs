using System;
namespace BaiThiASPNET.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DepartmentId { get; set; }
        public string Rank { get; set; }

        public Department Department { get; set; }
    }
}