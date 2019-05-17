namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Location
{
    using SAC.Seed.Dependency;
    using SAC.Stock.Code;
    using SAC.Stock.Front.InitializeDatabase.Infrastructure;
    using SAC.Stock.Service.BaseDto;
    using SAC.Stock.Service.LocationContext;    
    using System.IO;
    using System.Linq;
    using System.Text;
    internal class ImportCity : ImportProcess
    {
        protected override void RunCommand(object[] args)
        {
            var supportSvc = DiContainerFactory.DiContainer().Resolve<ILocationApplicationService>();
            var pathToStates = args[0].ToString();
            var filePaths = Directory.GetFiles(pathToStates, "*.csv", SearchOption.AllDirectories);

            this.Log(string.Format("Start: {0} Cities.", filePaths.Count()));
            var markGlobal = this.GetLapMark();

            foreach (var file in filePaths)
            {
                TextReader objstream = new StreamReader(file);
                var linesToread = File.ReadLines(file, new UnicodeEncoding()).ToArray();
                LocationDto parent = null;

                this.Log(string.Format("Start: {0} Cities in {1}", linesToread.Count(), Path.GetFileNameWithoutExtension(file)));
                var mark = this.GetLapMark();

                foreach (var line in linesToread)
                {
                    if (parent == null)
                    {
                        parent = supportSvc.GetLocation(NormalizeText.CodeToCode(line));
                        continue;
                    }

                    var row = line.Split(',');
                    var city = new LocationDto
                    {
                        Name = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                        Description = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                        LocationTypeCode = CodeConst.LocationType.City.Code,
                        ParentLocationId = parent.Id
                    };
                  
                    supportSvc.AddLocation(city);
                }

                this.Lap(string.Format("End: {0} Cities in {1}", linesToread.Count(), Path.GetFileNameWithoutExtension(file)), mark);

                objstream.Close();
            }

            this.Lap(string.Format("End: {0} Cities.", filePaths.Count()), markGlobal);
        }
    }
}
