using System;
using System.Collections.Generic;
using TODOLIST.Models;
using Microsoft.EntityFrameworkCore;
using TODOLIST.Models;
using System.Diagnostics;

namespace TODOLIST.assets
{
    public class TableContext : DbContext
    {
        public TableContext(DbContextOptions<TableContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestData> RequestDatas { get; set; }

        public DbSet<Person> Persons { get; set; }
    }
}
