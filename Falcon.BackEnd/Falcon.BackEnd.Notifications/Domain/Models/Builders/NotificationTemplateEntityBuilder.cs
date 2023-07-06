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
				.Property(e => e.Code)
                .HasMaxLength(20);

			builder
                .Property(e => e.Title)
                .HasMaxLength(100);

            builder
                .Property(e => e.Description)
                .HasMaxLength(250);

            DataSeeding(builder);
        }

        private static void DataSeeding(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Upgrade Version",
                    Content = "Version Terbaru Tersedia",
                    Code = "UpgradeVersion",
                    Description = ""
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Partner Telah Disetujui",
                    Content = "Sudah Di Approve Oleh",
                    Code = "ApprovePartner",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Partner Telah Di Reject",
                    Content = "Telah Di Reject Oleh",
                    Code = "RejectPartner",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Visit Plan Sudah Di Approve",
                    Content = "Sudah Di Approve Oleh",
                    Code = "ApproveVisitPlan",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Visit Plan Telah Di Reject",
                    Content = "Telah Di Reject Oleh",
                    Code = "RejectVisitPlan",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "POA Sudah Di Approve",
                    Content = "Sudah Di Approve Oleh",
                    Code = "ApprovePOA",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "POA Telah Di Reject",
                    Content = "Telah Di Reject Oleh",
                    Code = "RejectPOA",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Outlet Request Approval",
                    Content = "Request Approval Kepada",
                    Code = "CreateOutlet",
					Description = ""
				});
        }
    }
}
