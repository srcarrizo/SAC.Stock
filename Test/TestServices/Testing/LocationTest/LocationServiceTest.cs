namespace SAC.Stock.Test.TestServices.Testing.LocationTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SAC.Seed.Dependency;
    using SAC.Stock.Service.BaseDto;
    using SAC.Stock.Service.LocationContext;

    [TestClass]
    public class LocationServiceTest
    {
        [TestMethod]
        public void TestLocationAdd()
        {
            var loc = new LocationDto
            {
                Code = "Cor",
                Description="Cordoba",
                LocationTypeCode="country",
                Name="Cordoba"                
            };

            var locServ = NewContainer().Resolve<ILocationApplicationService>();
            var location =  locServ.AddLocation(loc);

            Assert.IsNotNull(location);
        }

        private static IDiContainer NewContainer()
        {
            return DiContainerFactory.DiContainer().BeginScope();
        }
    } 
}