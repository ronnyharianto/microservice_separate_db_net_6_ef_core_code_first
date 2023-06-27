using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Falcon.Libraries.DataAccess.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Falcon.BackEnd.Notifications.Domain.Models.Builders
{
    public class UserNotificationEntityBuilder : EntityBaseBuilder<UserNotification>
    {
        public override void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.UserId);

            builder
                .Property(e => e.FcmToken);

            builder
                .Property(e => e.UserName);

            DataSeeding(builder);
        }

        private static void DataSeeding(EntityTypeBuilder<UserNotification> builder)
        {
            builder
                .HasData(new UserNotification()
                {
                    Id = new Guid("420b5665-c03e-41e1-b591-c30620ac7b94"),
                    UserId = 110,
                    FcmToken = "e7TrrjY3RG2Awi826w3wMP:APA91bHGwuLjRDO3eSIwMhT-abrX9CgdRoTNM-JdxU0nHMnF8zPKqvzqeN8k3N9rojKo-HC0Y3jVdq6UZ53hpijcjgmkOc2dqKVOAyG0nIhk7zNhCJh7-ol15FV4DQf2FYBQQFqRo6pc",
                    UserName = "Putra"
                    
                });

            builder
                .HasData(new UserNotification()
                {
                    Id = new Guid("05ed93bf-c16c-4572-980d-8eb86338560f"),
                    UserId = 111,
                    FcmToken = "e1qOn6kCS6uh1rXyYz5wyn:APA91bGMZBok5XB-hpB8D-OY7--QaJ-n_K_CPAw-C-sZZLkiMNJV3CQ6HXiVVzJoSUESS-pprzvG-pqqB8PHHtI4d75QBHydzvYlW_eLqMxKB8aGu3t-tf-LC2EP4iJP7lpCi8s1hbj7",
                    UserName = "Saddam"
                });

            
        }
    }
}
