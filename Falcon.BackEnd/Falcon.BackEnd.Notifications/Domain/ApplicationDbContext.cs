
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
        public virtual DbSet<ReadNotification> ReadNotification { get; set; }
        public virtual DbSet<UserNotification> UserNotification { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.HasDefaultSchema("notification");

            new NotificationEntityBuilder().Configure(modelBuilder.Entity<Notification>());
            new NotificationTemplateEntityBuilder().Configure(modelBuilder.Entity<NotificationTemplate>());
            new ReadNotificationEntityBuilder().Configure(modelBuilder.Entity<ReadNotification>());
            new UserNotificationEntityBuilder().Configure(modelBuilder.Entity<UserNotification>());
        }
    }
}
