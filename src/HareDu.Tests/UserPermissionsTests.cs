namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class UserPermissionsTests :
        HareDuTestBase
    {
        [Test, Explicit]
        public async Task Verify_can_get_all_user_permissions()
        {
            Result<IEnumerable<UserPermissionsInfo>> result = await Client
                .Factory<UserPermissions>()
                .GetAll();
            
            foreach (var access in result.Data)
            {
                Console.WriteLine("VirtualHost: {0}", access.VirtualHost);
                Console.WriteLine("Configure: {0}", access.Configure);
                Console.WriteLine("Read: {0}", access.Read);
                Console.WriteLine("Write: {0}", access.Write);
                Console.WriteLine("****************************************************");
                Console.WriteLine();
            }
            
            Console.WriteLine();
        }

        [Test, Explicit]
        public async Task TestVerify_can_delete_user_permissions()
        {
            Result result = await Client
                .Factory<UserPermissions>()
                .Delete(x =>
                {
                    x.User("");
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });
        }

        [Test, Explicit]
        public async Task Verify_can_create_user_permissions()
        {
            Result result = await Client
                .Factory<UserPermissions>()
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