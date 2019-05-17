namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Provider
{
    using Seed.Dependency;    
    using Infrastructure;
    using Service.BaseDto;
    using Service.LocationContext;
    using Service.ProviderContext;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using SAC.Stock.Code;
    using System.Collections.Generic;
    using SAC.Stock.Service.SupportContext;

    internal class ImportProvider : ImportProcess
    {
        protected override void RunCommand(object[] args)
        {
            var pathProvider = args[0].ToString();
            var providerLines = File.ReadAllLines(pathProvider, new UnicodeEncoding());
            var providerSvc = DiContainerFactory.DiContainer().Resolve<IProviderApplicationService>();
            var locSvc = DiContainerFactory.DiContainer().Resolve<ILocationApplicationService>();
            var supportSvc = DiContainerFactory.DiContainer().Resolve<ISupportApplicationService>();

            Log(string.Format("Start: {0} Providers", providerLines.Count()));

            foreach (var line in providerLines)
            {
                var row = line.Split(',');
                var loc = locSvc.GetLocation(row[4].Trim().Replace("'", string.Empty), CodeConst.LocationType.City.Code);
                var telco = supportSvc.GetTelco(row[15].Trim().Replace("'", string.Empty));
                var telcoId = telco == null ? 1 : telco.Id;
                var provider = new ProviderDto
                {
                    CreatedDate = DateTimeOffset.Now,
                    Name = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                    Person = new PersonDto
                    {
                        FirstName = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                        LastName = NormalizeText.UpperCaseFirst(row[0].Trim().Replace("'", string.Empty)),
                        BirthDate = new DateTime(1979, 01, 01),
                        UidCode = NormalizeText.UpperCaseFirst(row[1].Trim().Replace("'", string.Empty)),
                        UidSerie = NormalizeText.UpperCaseFirst(row[2].Trim().Replace("'", string.Empty)),
                        Email = NormalizeText.UpperCaseFirst(row[3].Trim().Replace("'", string.Empty)),                          
                        Address = new AddressDto
                        {
                            Apartment = row[10].Trim().Replace("'", string.Empty),
                            Floor = row[9].Trim().Replace("'", string.Empty),
                            Neighborhood = row[6].Trim().Replace("'", string.Empty),
                            Street = row[7].Trim().Replace("'", string.Empty),
                            StreetNumber = row[8].Trim().Replace("'", string.Empty),
                            ZipCode = row[5].Trim().Replace("'", string.Empty),
                            Location = loc,
                            LocationId = loc.Id
                        },
                        Phones = new List<PhoneDto>
                        {
                            new PhoneDto
                            {
                                AreaCode = row[11].Trim().Replace("'", string.Empty),
                                CountryCode = "54",
                                Mobile = row[14].Trim().Replace("'", string.Empty).Equals("Celular"),
                                Name = row[13].Trim().Replace("'", string.Empty),
                                Number = row[12].Trim().Replace("'", string.Empty),
                                TelcoId = telcoId
                            },
                            new PhoneDto
                            {
                                AreaCode = "351",
                                CountryCode = "54",
                                Mobile = false,
                                Name = "Generico 2",
                                Number = "156156156",
                                TelcoId = telcoId
                            }
                        },
                        Attributes = new List<AttributeValueDto>
                        {
                            new AttributeValueDto
                            {
                                AttributeCode = CodeConst.Attribute.Scope.Code,
                                Value = CodeConst.Scope.Provider.Code
                            }
                        }
                    }                    
                };

                providerSvc.AddProvider(provider);
            }

            Lap(string.Format("Start: {0} Providers", providerLines.Count()));
        }
    }
}
