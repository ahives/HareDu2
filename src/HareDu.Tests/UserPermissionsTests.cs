namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class UserPermissionsTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Should_be_able_to_get_all_user_permissions()
        {
            var result = await Client
                .Resource<UserPermissions>()
                .GetAll();
            
            foreach (var access in result.Select(x => x.Data))
            {
                Console.WriteLine("VirtualHost: {0}", access.VirtualHost);
                Console.WriteLine("Configure: {0}", access.Configure);
                Console.WriteLine("Read: {0}", access.Read);
                Console.WriteLine("Write: {0}", access.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
        }

        [Test, Explicit]
        public async Task TestVerify_can_delete_user_permissions()
        {
            var result = await Client
                .Resource<UserPermissions>()
                .Delete(x =>
                {
                    x.User("");
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });
        }

        [Test, Explicit]
        public async Task Verify_can_create_user_permissions()
        {
            var result = await Client
                .Resource<UserPermissions>()
                .Create(x =>
                {
                    x.User("");
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern("");
                        c.UsingReadPattern("");
                        c.UsingWritePattern("");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });
        }
    }
}