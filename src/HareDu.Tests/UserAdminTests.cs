namespace HareDu.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class UserAdminTests :
        HareDuTestBase
    {
        [Test]
        public async Task Test()
        {
            Result result = await Client
                .Factory<UserAdmin>()
                .Create(x =>
                {
                    x.Username("testuser1");
                    x.Password("testuserpwd1");
//                    x.WithPasswordHash("");
                    x.WithTags(t =>
                    {
                        t.Administrator();
                    });
                });
        }
    }
}