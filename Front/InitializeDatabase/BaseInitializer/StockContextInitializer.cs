namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer
{
    using Membership.Service.UserManagement;
    using Seed.Dependency;
    using Code;
    using Data.Context;
    using Bill;
    using Location;
    using Provider;
    using Customer;
    using AreaCategorySubCategoryContainerProduct;
    using Service.BranchOfficeContext;
    using System;
    using Service.LocationContext;
    using System.Collections.Generic;
    using Service.BaseDto;
    using System.Linq;
    using Infrastucture;

    internal static class StockContextInitializer
    {
        public static void Seed(StockContext context)
        {
            AddRolesToSfCrew();
            LocationImporter.Run();
            AddBranchOffice();
            BillImporter.Run();
            ProviderImporter.Run();
            CustomerImporter.Run();
            AreaCategorySubCategoryContainerProductImporter.Run();
        }

        private static void AddRolesToSfCrew()
        {
            var userSvc = NewContainer().Resolve<IUserManagementApplicationService>();
            var sfcrew = userSvc.GetUser(UserName.StockManagement);
            sfcrew.Roles = new[]
                             {
                                CodeConst.Role.StockSuperAdmin.Code, CodeConst.Role.AreaCategoryManager.Code,
                                CodeConst.Role.BoxManager.Code, CodeConst.Role.BranchofficeManager.Code,
                                CodeConst.Role.BudgetManager.Code, CodeConst.Role.BuyManager.Code,
                                CodeConst.Role.BuysManager.Code, CodeConst.Role.ContainerManager.Code,                                
                                CodeConst.Role.ProductManager.Code, CodeConst.Role.ProviderManager.Code,
                                CodeConst.Role.ReportBuySaleViewer.Code, CodeConst.Role.ReportGainViewer.Code,
                                CodeConst.Role.SalesManager.Code, CodeConst.Role.StockManager.Code,
                                CodeConst.Role.TransactionManager.Code, CodeConst.Role.UserManager.Code,
                                CodeConst.Role.CustomerManager.Code
                             };

            userSvc.ModifyUser(sfcrew);
        }

        private static void AddBranchOffice()
        {
            var locSvc = DiContainerFactory.DiContainer().Resolve<ILocationApplicationService>();
            var braSvc = DiContainerFactory.DiContainer().Resolve<IBranchOfficeApplicationService>();
            var loc = locSvc.GetLocation("Cordoba", CodeConst.LocationType.City.Code);

            var branchOffice = new BranchOfficeDto
            {
                ActivatedDate = DateTime.UtcNow,
                Name = "Nueva Cordoba",
                Address = new AddressDto
                {
                    Location = loc,
                    LocationId = loc.Id,
                    Street = "Rondeu",
                    StreetNumber = "500",
                    Neighborhood = "Nueva Cordoba",
                    ZipCode = "5000"
                },
                Description = "Nueva Cordoba",
                CreatedDate = DateTime.UtcNow,
                Phones = new List<PhoneDto>
                {
                    new PhoneDto
                    {
                        AreaCode = "351",
                        CountryCode = "54",
                        Mobile = true,
                        Name = "Trabajo",
                        Number = "153470241",
                        TelcoId = 1
                    },
                    new PhoneDto
                    {
                        AreaCode = "351",
                        CountryCode = "54",
                        Mobile = true,
                        Name = "Trabajo",
                        Number = "153108595",
                        TelcoId = 1
                    }
                }
            };

            var branch = braSvc.AddBranchOffice(branchOffice);
            AddStaff(branch);
        }

        private static void AddStaff(BranchOfficeDto branchOffice)
        {
            var braSvc = DiContainerFactory.DiContainer().Resolve<IBranchOfficeApplicationService>();
            var userSvc = NewContainer().Resolve<IUserManagementApplicationService>();
            var roles = userSvc.QueryRole(CodeConst.Application.Stock.Code);

            var staff = new BranchOfficeStaffSaveDto
            {
                CreatedDate = DateTime.UtcNow,
                BranchOffice = branchOffice,
                Roles = roles.Select(r => r.Code).ToArray(),
                StaffRoleCode = CodeConst.StaffRole.Owner.Code,
                Staff = new StaffDto
                {
                    Person = new PersonDto
                    {
                        Address = branchOffice.Address,
                        BirthDate = DateTime.Now,
                        Email = "c@c.com",
                        FirstName = "Lucas",
                        LastName = "Leutier",
                        UidCode = CodeConst.UidType.Cuit.Code,
                        UidSerie = "30-35222256-1",
                        Phones = new List<PhoneDto>
                        {
                            new PhoneDto
                            {
                                AreaCode = "351",
                                CountryCode = "54",
                                Mobile = true,
                                Name = "Trabajo",
                                Number = "153470241",
                                TelcoId = 1
                            },
                            new PhoneDto
                            {
                                AreaCode = "351",
                                CountryCode = "54",
                                Mobile = true,
                                Name = "Trabajo",
                                Number = "153108595",
                                TelcoId = 1
                            }
                        }
                    }
                }
            };

            var staffSaved = braSvc.AddBranchOfficeStaff(staff);
            var user = userSvc.GetUser(staffSaved.UserId);
            user.SetUserName(user.UserName);
            user.SetPassword(user.UserName);
            userSvc.ModifyUser(user);
            userSvc.ActivateUser(user);
        }        

        private static IDiContainer NewContainer()
        {
            return DiContainerFactory.DiContainer().BeginScope();
        }
    }
}
