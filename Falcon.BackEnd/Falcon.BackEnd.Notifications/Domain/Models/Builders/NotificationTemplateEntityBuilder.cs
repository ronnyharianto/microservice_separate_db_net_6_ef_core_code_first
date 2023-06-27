using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.Notifications.Domain.Models.Builders
{
    public class NotificationTemplateEntityBuilder : EntityBaseBuilder<NotificationTemplate>
    {
        public override void Configure(EntityTypeBuilder<NotificationTemplate> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Title);

            builder
                .Property(e => e.Body);

            builder
                .Property(e => e.Code);
        }
    }
}
