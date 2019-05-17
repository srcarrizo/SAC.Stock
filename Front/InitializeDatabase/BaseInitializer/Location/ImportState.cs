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

    internal class ImportState : ImportProcess
    {
        protected override void RunCommand(object[] args)
        {
            var supportSvc = DiContainerFactory.DiContainer().Resolve<ILocationApplicationService>();
            var pathToCountries = args[0].ToString();
            var filesPath = Directory.GetFiles(pathToCountries, "*.csv", SearchOption.AllDirectories);

            this.Log(string.Format("Start: {0} States.", filesPath.Count()));
            var markGlobal = this.GetLapMark();

            foreach (var file in filesPath)
            {
                TextReader objstream = new StreamReader(file);
                var lines = File.ReadLines(file, new UnicodeEncoding()).ToArray();
                LocationDto parent = null;

                this.Log(string.Format("Start: {0} States in {1}", lines.Count(), Path.GetFileNameWithoutExtension(file)));
                var mark = this.GetLapMark();

                foreach (var line in lines)
                {
                    if (parent == null)
                    {
                        parent = supportSvc.GetLocation(NormalizeText.CodeToCode(line));
                        continue;
                    }

                    var row = line.Split(',');
                    var state = new LocationDto
                    {
                        Name = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                        Description = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                        LocationTypeCode = CodeConst.LocationType.State.Code,
                        Code = parent.Code + "." + NormalizeText.NameToCode(row[0]),
                        ParentLocationId = parent.Id
                    };
                  
                    supportSvc.AddLocation(state);
                }

                this.Lap(string.Format("End: {0} States in {1}", lines.Count(), Path.GetFileNameWithoutExtension(file)), mark);

                objstream.Close();
            }

            this.Lap(string.Format("End: {0} States.", filesPath.Count()), markGlobal);
        }
    }
}
