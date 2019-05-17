namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Location
{
    using SAC.Stock.Code;
    using SAC.Seed.Dependency;
    using SAC.Stock.Front.InitializeDatabase.Infrastructure;
    using SAC.Stock.Service.BaseDto;
    using SAC.Stock.Service.LocationContext;    
    using System.IO;
    using System.Linq;
    using System.Text;    

    internal class ImportCountry : ImportProcess
    {
        protected override void RunCommand(object[] args)
        {
            var pathCountry = args[0].ToString();
            var countryLines = File.ReadAllLines(pathCountry, new UnicodeEncoding());
            var supportSvc = DiContainerFactory.DiContainer().Resolve<ILocationApplicationService>();

            this.Log(string.Format("Start: {0} Countries", countryLines.Count()));

            foreach (var line in countryLines)
            {
                var row = line.Split(',');                
                var country = new LocationDto
                {
                    Name = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                    Description = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                    LocationTypeCode = CodeConst.LocationType.Country.Code,
                    Code = NormalizeText.NameToCode(row[0])
                };
             
                supportSvc.AddLocation(country);
            }

            this.Lap(string.Format("Start: {0} Countries", countryLines.Count()));
        }
    }
}
