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
                    Title = "Version",
                    Content = "Version Terbaru Tersedia",
                    Code = "UpgradeVersion",
                    Description = ""
                });

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Partner Telah Disetujui",
                    Content = "Partner [PartnerName] telah disetujui oleh [Jabatan]",
                    Code = "ApprovePartner",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Partner",
                    Content = "Telah Di Reject",
                    Code = "RejectPartner",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Visit Plan",
                    Content = "Sudah Di Approve",
                    Code = "ApproveVisitPlan",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Visit Plan",
                    Content = "Telah Di Reject",
                    Code = "RejectVisitPlan",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "POA",
                    Content = "Sudah Di Approve",
                    Code = "ApprovePOA",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "POA",
                    Content = "Telah Di Reject",
                    Code = "RejectPOA",
					Description = ""
				});

            builder
                .HasData(new NotificationTemplate()
                {
                    Id = Guid.NewGuid(),
                    Title = "Outlet",
                    Content = "Request Approval",
                    Code = "CreateOutlet",
					Description = ""
				});
        }
    }
}
