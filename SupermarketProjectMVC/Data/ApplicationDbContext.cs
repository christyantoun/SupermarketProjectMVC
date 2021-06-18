using Microsoft.EntityFrameworkCore;
using SupermarketProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketProjectMVC.Data
{
    public class ApplicationDbContext : DbContext
    {



    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

    public DbSet<SupermarketProjectMVC.Models.Producer> Producer { get; set; }


     public DbSet<SupermarketProjectMVC.Models.Item> Item { get; set; }

            public DbSet<SupermarketProjectMVC.Models.Category> Category { get; set; }

    }
    }