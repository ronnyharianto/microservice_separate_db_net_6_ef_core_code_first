using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Notifications.Domain.Models.Builders
{
    public class ReadNotifEntityBuilder : EntityBaseBuilder<ReadNotif>
    {
        public override void Configure(EntityTypeBuilder<ReadNotif> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.NotifId);

            builder
                .Property(e => e.UserNotifId);

            builder
                .HasOne(e => e.Notification)
                .WithMany(e => e.ReadNotif)
                .HasForeignKey(e => e.NotifId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.UserNotif)
                .WithMany(e => e.ReadNotif)
                .HasForeignKey(e => e.UserNotifId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
