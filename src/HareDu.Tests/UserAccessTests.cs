namespace HareDu.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using NUnit.Framework;

    [TestFixture]
    public class UserAccessTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test1()
        {
            Result<IEnumerable<UserAccessInfo>> result = await Client
                .Factory<UserAccess>()
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
            
            Console.WriteLine("Reason: {0}", result.Reason);
            Console.WriteLine("StatusCode: {0}", result.StatusCode);
            Console.WriteLine();

        }
    }
}