using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirstWebApi;

namespace FirstWebApi.Data
{
    public class FirstWebApiContext : DbContext
    {
        public FirstWebApiContext (DbContextOptions<FirstWebApiContext> options)
            : base(options)
        {
        }

        public DbSet<FirstWebApi.CustomersClass> CustomersClass { get; set; } = default!;
    }
}
