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
                .Property(e => e.TotalAudience);

            DataSeeding(builder);

        }

        private static void DataSeeding(EntityTypeBuilder<Notification> builder)
        {
            builder
                .HasData(new Notification()
                {
                    Id = Guid.NewGuid(),
                    Target = "e7TrrjY3RG2Awi826w3wMP:APA91bHGwuLjRDO3eSIwMhT-abrX9CgdRoTNM-JdxU0nHMnF8zPKqvzqeN8k3N9rojKo-HC0Y3jVdq6UZ53hpijcjgmkOc2dqKVOAyG0nIhk7zNhCJh7-ol15FV4DQf2FYBQQFqRo6pc",
                    ReceiveUserId = 110,
                    Title = "Visit Plan",
                    Content = "Visit Plan 110 Telah Di Reject",
                    NotificationCode = "RejectVisitPlan",
                    Category = "Activity",
                    TotalAudience = 1

                });

            builder
                .HasData(new Notification()
                {
                    Id = Guid.NewGuid(),
                    Target = "e1qOn6kCS6uh1rXyYz5wyn:APA91bGMZBok5XB-hpB8D-OY7--QaJ-n_K_CPAw-C-sZZLkiMNJV3CQ6HXiVVzJoSUESS-pprzvG-pqqB8PHHtI4d75QBHydzvYlW_eLqMxKB8aGu3t-tf-LC2EP4iJP7lpCi8s1hbj7",
                    ReceiveUserId = 111,
                    Title = "POA",
                    Content = "POA 111 Sudah Di Approve",
                    NotificationCode = "ApprovePOA",
                    Category = "Activity",
                    TotalAudience = 1
                });

            
        }
    }
}
