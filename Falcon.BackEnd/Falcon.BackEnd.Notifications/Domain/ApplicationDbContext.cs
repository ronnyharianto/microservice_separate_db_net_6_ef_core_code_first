using Falcon.BackEnd.Notifications.Domain.Models.Builders;
using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Notifications.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationTemplate> NotificationTemplate { get; set; }
        public virtual DbSet<ReadNotif> ReadNotif { get; set; }
        public virtual DbSet<UserNotif> UserNotif { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.HasDefaultSchema("notification");

            new NotificationEntityBuilder().Configure(modelBuilder.Entity<Notification>());
            new NotificationTemplateEntityBuilder().Configure(modelBuilder.Entity<NotificationTemplate>());
            new ReadNotifEntityBuilder().Configure(modelBuilder.Entity<ReadNotif>());
            new UserNotifEntityBuilder().Configure(modelBuilder.Entity<UserNotif>());
        }
    }
}
