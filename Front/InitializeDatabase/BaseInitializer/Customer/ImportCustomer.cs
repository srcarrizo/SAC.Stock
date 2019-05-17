namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.Customer
{
    using SAC.Seed.Dependency;
    using SAC.Stock.Code;
    using SAC.Stock.Front.InitializeDatabase.Infrastructure;
    using SAC.Stock.Service.BaseDto;
    using SAC.Stock.Service.CustomerContext;
    using SAC.Stock.Service.LocationContext;
    using SAC.Stock.Service.SupportContext;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    
    internal class ImportCustomer : ImportProcess
    {
        protected override void RunCommand(object[] args)
        {
            var pathCustomer = args[0].ToString();
            var customerLines = File.ReadAllLines(pathCustomer, new UnicodeEncoding());
            var customerSvc = DiContainerFactory.DiContainer().Resolve<ICustomerApplicationService>();
            var locSvc = DiContainerFactory.DiContainer().Resolve<ILocationApplicationService>();
            var supportSvc = DiContainerFactory.DiContainer().Resolve<ISupportApplicationService>();

            Log(string.Format("Start: {0} Customers", customerLines.Count()));

            foreach (var line in customerLines)
            {
                var row = line.Split(',');
                var loc = locSvc.GetLocation(row[4].Trim().Replace("'", string.Empty), CodeConst.LocationType.City.Code);
                var telco = supportSvc.GetTelco(row[15].Trim().Replace("'", string.Empty));
                var telcoId = telco == null ? 1 : telco.Id;
                var customer = new CustomerDto
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
                                TelcoId = 1
                            }
                        },
                        Attributes = new List<AttributeValueDto>
                        {
                            new AttributeValueDto
                            {
                                AttributeCode = CodeConst.Attribute.Scope.Code,
                                Value = CodeConst.Scope.Customer.Code
                            }
                        }
                    }
                };

                customerSvc.AddCustomer(customer);
            }

            Lap(string.Format("End: {0} Customers", customerLines.Count()));
        }
    }
}
