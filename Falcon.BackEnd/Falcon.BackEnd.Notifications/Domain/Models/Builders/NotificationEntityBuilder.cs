using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.Notifications.Domain.Models.Builders
{
    public class NotificationEntityBuilder : EntityBaseBuilder<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Target);

            builder
                .Property(e => e.ReceiveUserId)
                .IsRequired(false);

            builder
                .Property(e => e.Title);

            builder
                .Property(e => e.Content);

            builder
                .Property(e => e.NotificationCode)
                .IsRequired(false);

            builder
                .Property(e => e.TotalAudiance);
            
        }
    }
}
