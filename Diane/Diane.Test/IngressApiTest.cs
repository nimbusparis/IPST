using Xunit;

namespace Diane.Test
{
    public class IngressApiTest
    {
        [Fact]
        public void SignupTest()
        {
            var target = new IngressApi();
            target.Signup();
        }
    }
}
