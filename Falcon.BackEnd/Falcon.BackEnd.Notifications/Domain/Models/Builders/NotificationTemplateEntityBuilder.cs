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

            DataSeeding(builder);
        }

        private static void DataSeeding(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Version",
                    Body = "Version Terbaru Tersedia",
                    Code = "UpgradeVersion"

                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Partner",
                    Body = "Sudah Di Approve",
                    Code = "ApprovePartner"
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Partner",
                    Body = "Telah Di Reject",
                    Code = "RejectPartner"
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Visit Plan",
                    Body = "Sudah Di Approve",
                    Code = "ApproveVisitPlan"
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Visit Plan",
                    Body = "Telah Di Reject",
                    Code = "RejectVisitPlan"
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "POA",
                    Body = "Sudah Di Approve",
                    Code = "ApprovePOA"
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "POA",
                    Body = "Telah Di Reject",
                    Code = "RejectPOA"
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Outlet",
                    Body = "Request Approval",
                    Code = "CreateOutlet"
                });
        }
    }
}
