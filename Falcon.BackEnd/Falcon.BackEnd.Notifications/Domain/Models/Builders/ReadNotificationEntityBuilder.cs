using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Notifications.Domain.Models.Builders
{
    public class ReadNotificationEntityBuilder : EntityBaseBuilder<ReadNotification>
    {
        public override void Configure(EntityTypeBuilder<ReadNotification> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.NotificationId);

            builder
                .Property(e => e.UserNotificationId);

            builder
                .HasOne(e => e.Notification)
                .WithMany(e => e.ReadNotification)
                .HasForeignKey(e => e.NotificationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.UserNotification)
                .WithMany(e => e.ReadNotification)
                .HasForeignKey(e => e.UserNotificationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
