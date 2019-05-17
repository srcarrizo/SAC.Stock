namespace SAC.Stock.Front.WebSiteStock.Models
{
    using SAC.Membership.Service.BaseDto;
    using SAC.Membership.Service.UserManagement;
    using SAC.Seed.CodeTable;
    using SAC.Seed.Dependency;
    using SAC.Seed.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Service.ProfileContext;    
    using System.Collections.Generic;
    using System.Linq;

    public class RolesComposition
    {
        private static readonly object SyncRoot = new object();

        private static volatile RolesDefinition[] rolesDefinitions;

        private static IProfileApplicationService profileSvc;

        public static RolesDefinition[] RolesDefinitions
        {
            get
            {
                if (rolesDefinitions == null)
                {
                    lock (SyncRoot)
                    {
                        if (rolesDefinitions == null)
                        {
                            var container = DiContainerFactory.DiContainer();
                            profileSvc = container.Resolve<IProfileApplicationService>();
                            Profiles = profileSvc.QueryProfile().ToList();
                            var franchiseScope = Profiles.Where(s => s.Scope.Equals(CodeConst.Scope.BranchOffice.Code));                            
                            rolesDefinitions =
                              new List<RolesDefinition>
                              {
                                        new RolesDefinition { ScopeItem = CodeConst.Scope.BranchOffice, StaffRoles = franchiseScope.Select(profileDto => new StaffRole { Hierarchy = profileDto.Hierarchy, StaffRoleItem = new CodeItem { Code = profileDto.Code, Name = profileDto.Name, Description = profileDto.Description }, Roles = profileDto.Roles.Select(rol => 
                                        new Role
                                        {
                                            CriticalRole = rol.CriticalRole,
                                            Hierarchy = rol.Hierarchy,
                                            RoleCode = rol.RoleCode,
                                            RoleName = RolesData.Roles.FirstOrDefault(r => r.Code.Equals(rol.RoleCode))?.Name,
                                            RoleDescription = RolesData.Roles.FirstOrDefault(r => r.Code.Equals(rol.RoleCode))?.Description,
                                        }).ToArray() }).ToArray() }
                              }.ToArray();                           
                        }
                    }
                }

                return rolesDefinitions;
            }
        }

        private static List<ProfileDto> Profiles { get; set; }

        public static void UnLoadRolesDefinitions()
        {
            if (rolesDefinitions == null)
            {
                return;
            }

            lock (SyncRoot)
            {
                if (rolesDefinitions != null)
                {
                    rolesDefinitions = null;
                }
            }
        }

        public class RolesDefinition
        {
            public CodeItem ScopeItem { get; set; }

            public StaffRole[] StaffRoles { get; set; }
        }

        public class StaffRole
        {
            public CodeItem StaffRoleItem { get; set; }

            public Role[] Roles { get; set; }

            public int Hierarchy { get; set; }
        }

        public class Role
        {
            public string RoleCode { get; set; }

            public bool CriticalRole { get; set; }

            public int Hierarchy { get; set; }

            public string RoleName { get; set; }

            public string RoleDescription { get; set; }

            public string GetRoleName()
            {
                var role = RolesData.Roles.FirstOrDefault(r => r.Code.Equals(this.RoleCode));
                if (role != null)
                {
                    return role.Name;
                }

                throw new AppException("No existe el Rol de aplicación seleccionado.");
            }

            public string GetRoleDescription()
            {
                var role = RolesData.Roles.FirstOrDefault(r => r.Code.Equals(this.RoleCode));
                if (role != null)
                {
                    return role.Description;
                }

                throw new AppException("No existe el Rol de aplicación seleccionado.");
            }
        }

        internal sealed class RolesData
        {        
            private static volatile ICollection<RoleDto> roles;
            public static ICollection<RoleDto> Roles
            {
                get
                {
                    if (roles == null)
                    {
                        lock (SyncRoot)
                        {
                            if (roles == null)
                            {
                                var svc = DiContainerFactory.DiContainer().Resolve<IUserManagementApplicationService>();
                                roles = svc.QueryRole(CodeConst.Application.Stock.Code, true);
                            }
                        }
                    }

                    return roles;
                }
            }
        }
    }
}