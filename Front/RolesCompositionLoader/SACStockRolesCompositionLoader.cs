namespace SAC.Stock.Front.RolesCompositionLoader
{
    using SAC.Stock.Service.ProfileContext;    
    using System.Collections.Generic;
    using SAC.Stock.Code;
    using System;
    using SAC.Seed.Dependency;
    using SAC.Seed.NLayer.Data;
    using System.Linq;
    using SAC.Membership.Service.UserManagement;
    using SAC.Membership.Service.BaseDto;

    public class SACStockRolesCompositionLoader
    {
        public static void Run()
        {
            CreateProfiles();
        }

        private static void CreateProfiles()
        {
            try
            {
                var profileList = new List<ProfileDto>
                {
                    new ProfileDto
                    {
                        Code = CodeConst.StaffRole.Employee.Code,
                        Name = CodeConst.StaffRole.Employee.Name,
                        Description = CodeConst.StaffRole.Employee.Description,
                        Hierarchy = 2,
                        Scope = CodeConst.Scope.BranchOffice.Code,
                        Roles = new List<RolesCompositionDto>
                        {
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 1,
                                RoleCode = CodeConst.Role.BoxManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 2,
                                RoleCode = CodeConst.Role.StockManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 3,
                                RoleCode = CodeConst.Role.BuyManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 4,
                                RoleCode = CodeConst.Role.SalesManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 5,
                                RoleCode = CodeConst.Role.BudgetManager.Code
                            },
                        }
                    },
                    new ProfileDto
                    {
                        Code = CodeConst.StaffRole.Owner.Code,
                        Name = CodeConst.StaffRole.Owner.Name,
                        Description = CodeConst.StaffRole.Owner.Description,
                        Hierarchy = 1,
                        Scope = CodeConst.Scope.BranchOffice.Code,
                        Roles = new List<RolesCompositionDto>
                        {
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 1,
                                RoleCode = CodeConst.Role.AreaCategoryManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 2,
                                RoleCode = CodeConst.Role.BoxManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 3,
                                RoleCode = CodeConst.Role.BranchofficeManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 4,
                                RoleCode = CodeConst.Role.BudgetManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 5,
                                RoleCode = CodeConst.Role.BuyManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 6,
                                RoleCode = CodeConst.Role.BuysManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 7,
                                RoleCode = CodeConst.Role.ContainerManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 8,
                                RoleCode = CodeConst.Role.CustomerManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 10,
                                RoleCode = CodeConst.Role.ProductManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 11,
                                RoleCode = CodeConst.Role.ProviderManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 12,
                                RoleCode = CodeConst.Role.ReportBuySaleViewer.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 13,
                                RoleCode = CodeConst.Role.ReportGainViewer.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 14,
                                RoleCode = CodeConst.Role.SalesManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 15,
                                RoleCode = CodeConst.Role.StockManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 16,
                                RoleCode = CodeConst.Role.TransactionManager.Code
                            },
                            new RolesCompositionDto
                            {
                                CriticalRole = false,
                                Hierarchy = 17,
                                RoleCode = CodeConst.Role.UserManager.Code
                            }
                         }
                    }
                };

                SaveUpdateProfile(profileList, CodeConst.Scope.BranchOffice.Code);
                var allRoles = (from profileDto in profileList
                                select profileDto
                                  into roles
                                from r in roles.Roles
                                group r by r.RoleCode
                                    into rolesgroup
                                select new RolesCompositionDto { RoleCode = rolesgroup.Key }).Distinct().ToList();

                AddNewRoles(allRoles);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private static void SaveUpdateProfile(IEnumerable<ProfileDto> profileList, string scope)
        {
            var container = DiContainerFactory.DiContainer();
            var svc = container.Resolve<IProfileApplicationService>();
            var existingProfiles = svc.QueryProfile(
              1,
              int.MaxValue,
              new List<SortInfo> { new SortInfo { Field = SortQuery.Profile.Id, Direction = OrderDirection.Asc } },
              new FilterInfo { Spec = SpecFilter.Profile.Scope, Value = scope });

            foreach (var profileDto in profileList)
            {
                var existingProfile = existingProfiles.FirstOrDefault(p => p.Code.Equals(profileDto.Code, StringComparison.CurrentCultureIgnoreCase));
                if (existingProfile == null)
                {
                    svc.AddProfile(profileDto);
                    Console.WriteLine(@"Perfil: " + profileDto.Name + @" de alcance: " + profileDto.Scope + @" creado.");
                }
                else
                {
                    var modifed = false;
                    foreach (var rolesCompositionDto in profileDto.Roles)
                    {
                        var existingRole = existingProfile.Roles.FirstOrDefault(r => r.RoleCode.Equals(rolesCompositionDto.RoleCode, StringComparison.CurrentCultureIgnoreCase));
                        if (existingRole != null)
                        {
                            continue;
                        }

                        existingProfile.Roles.Add(rolesCompositionDto);
                        modifed = true;
                    }

                    var rolesListToDelete = new List<RolesCompositionDto>();
                    foreach (var existingRole in existingProfile.Roles)
                    {
                        var deletingRole = profileDto.Roles.FirstOrDefault(r => r.RoleCode.Equals(existingRole.RoleCode, StringComparison.CurrentCultureIgnoreCase));
                        if (deletingRole != null)
                        {
                            continue;
                        }

                        rolesListToDelete.Add(existingRole);
                    }

                    if (rolesListToDelete.Count > 0)
                    {
                        foreach (var rolesCompositionDto in rolesListToDelete)
                        {
                            existingProfile.Roles.Remove(rolesCompositionDto);
                        }

                        modifed = true;
                    }

                    if (modifed)
                    {
                        svc.ModifyProfile(existingProfile);
                        Console.WriteLine(@"Perfil: " + existingProfile.Name + @" de alcance: " + existingProfile.Scope + @" modificado.");
                    }
                }
            }

            Console.WriteLine(@"Scope: " + scope + @" finalizado.");
        }

        private static void AddNewRoles(IEnumerable<RolesCompositionDto> newRoles)
        {
            var container = DiContainerFactory.DiContainer();
            var svc = container.Resolve<IUserManagementApplicationService>();
            var existingRoles = svc.QueryRole(CodeConst.Application.Stock.Code);
            Console.WriteLine(@"Actualizando Roles en Membership");

            foreach (var rolesCompositionDto in newRoles)
            {
                var existingRole = existingRoles.FirstOrDefault(r => r.Code.Equals(rolesCompositionDto.RoleCode, StringComparison.CurrentCultureIgnoreCase));
                if (existingRole != null)
                {
                    continue;
                }

                var roleData = CodeConst.Role.Values().FirstOrDefault(r => r.Code.Equals(rolesCompositionDto.RoleCode));
                if (roleData == null)
                {
                    continue;
                }

                var addRole = new RoleDto
                {
                    AppCode = CodeConst.Application.Stock.Code,
                    Code = rolesCompositionDto.RoleCode,
                    Description = roleData.Description,
                    Name = roleData.Name
                };

                svc.AddRole(addRole);
                Console.WriteLine(@"Rol: " + addRole.Code + @" creado.");
            }
        }
    }
}
