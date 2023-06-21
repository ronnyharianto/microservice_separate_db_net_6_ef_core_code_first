﻿using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Notifications.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.HasDefaultSchema("notification");
        }
    }
}
