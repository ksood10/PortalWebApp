﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalWebApp.Areas.Identity.Data;
using PortalWebApp.Models;

namespace PortalWebApp.Data
{
    public class PortalWebAppContext : IdentityDbContext<PortalWebAppUser>
    {
        public PortalWebAppContext(DbContextOptions<PortalWebAppContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; }

        public DbSet<Tank> Tank { get; set; }

        public DbSet<StrapChart> StrapChart { get; set; }
        public DbSet<TankConfig> TankConfig { get; set; }

        public DbSet<Organization> Organization { get; set; }

        public DbSet<OrganizationTree> OrganizationTree { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
