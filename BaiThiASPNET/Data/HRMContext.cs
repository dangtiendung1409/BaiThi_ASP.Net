using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BaiThiASPNET.Models;

    public class HRMContext : DbContext
    {
        public HRMContext (DbContextOptions<HRMContext> options)
            : base(options)
        {
        }

        public DbSet<BaiThiASPNET.Models.Department> Department { get; set; } = default!;

        public DbSet<BaiThiASPNET.Models.Employee> Employee { get; set; } = default!;
    }
