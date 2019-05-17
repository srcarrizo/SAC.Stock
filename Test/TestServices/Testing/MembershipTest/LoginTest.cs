namespace SAC.Stock.Testing.MembershipTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SAC.Membership.Service.UserAccess;
    using SAC.Stock.Infrastucture;
    using SAC.Seed.Dependency;
    using SAC.Membership.Data.Context;

    [TestClass]
    public class LoginTest
    {
        string userName = "STKADMIN";
        string password = "12345678";

        [TestMethod]
        public void TestLoginMethod()
        {
            var userServ = NewContainer().Resolve<IUserAccessApplicationService>();
            var user = userServ.LoginUser("SAC.Stock", AuthHelper.GenerateAuthAttribute(userName, password));

            Assert.IsNotNull(user);
        }

        private static IDiContainer NewContainer()
        {
            return DiContainerFactory.DiContainer().BeginScope();
        }
    }
}
