using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.Notifications.Domain.Models.Builders
{
    public class UserNotifEntityBuilder : EntityBaseBuilder<UserNotif>
    {
        public override void Configure(EntityTypeBuilder<UserNotif> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.UserId);

            builder
                .Property(e => e.FcmToken);

            builder
                .Property(e => e.UserName);
        }
    }
}
