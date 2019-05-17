namespace SAC.Stock.Code
{
    using SAC.Membership.Code.Tables;
    using Tables;
    using ApplicationTable = SAC.Stock.Code.Tables.ApplicationTable;
    using RoleTable = SAC.Stock.Code.Tables.RoleTable;

    public static class CodeConst
    {
        private static TelcoTable instanceTelcoTable;

        private static LocationTypeTable instanceLocationTypeTable;

        private static AuthMethodTable instanceAuthMethodTable;

        private static AuthAttributeTable instanceAuthAttributeTable;

        private static ApplicationTable instanceApplicationTable;

        private static AttributeTable instanceAttributeTable;

        private static RoleTable instanceRoleTable;

        private static UidTypeTable instanceUidTypeTable;

        private static BillUnitTypeTable instanceBillTypeTable;

        private static BillTable instanceBillTable;

        private static OperationCodeTable instanceOperationCodeTable;

        private static ScopeTable instanceScopeTable;

        private static StaffRoleTable instanceStaffRoleTable;

        private static StaffTypeTable instanceStaffTypeTable;

        public static StaffTypeTable StaffType
        {
            get
            {
                return instanceStaffTypeTable ?? (instanceStaffTypeTable = new StaffTypeTable());
            }
        }

        public static OperationCodeTable OperationCode
        {
            get
            {
                return instanceOperationCodeTable ?? (instanceOperationCodeTable = new OperationCodeTable());
            }
        }

        public static BillTable BillTable
        {
            get
            {
                return instanceBillTable ?? (instanceBillTable = new BillTable());
            }
        }

        public static BillUnitTypeTable BillUnitType
        {
            get
            {
                return instanceBillTypeTable ?? (instanceBillTypeTable = new BillUnitTypeTable());
            }
        }

        public static UidTypeTable UidType
        {
            get
            {
                return instanceUidTypeTable ?? (instanceUidTypeTable = new UidTypeTable());
            }
        }

        public static TelcoTable Telco
        {
            get
            {
                return instanceTelcoTable ?? (instanceTelcoTable = new TelcoTable());
            }
        }

        public static LocationTypeTable LocationType
        {
            get
            {
                return instanceLocationTypeTable ?? (instanceLocationTypeTable = new LocationTypeTable());
            }
        }

        public static AuthMethodTable AuthMethod
        {
            get
            {
                return instanceAuthMethodTable ?? (instanceAuthMethodTable = new AuthMethodTable());
            }
        }

        public static AuthAttributeTable AuthAttribute
        {
            get
            {
                return instanceAuthAttributeTable ?? (instanceAuthAttributeTable = new AuthAttributeTable());
            }
        }

        public static ApplicationTable Application
        {
            get
            {
                return instanceApplicationTable ?? (instanceApplicationTable = new ApplicationTable());
            }
        }

        public static AttributeTable Attribute
        {
            get
            {
                return instanceAttributeTable ?? (instanceAttributeTable = new AttributeTable());
            }
        }

        public static RoleTable Role
        {
            get
            {
                return instanceRoleTable ?? (instanceRoleTable = new RoleTable());
            }
        }

        public static ScopeTable Scope
        {
            get
            {
                return instanceScopeTable ?? (instanceScopeTable = new ScopeTable());
            }
        }        

        public static StaffRoleTable StaffRole
        {
            get
            {
                return instanceStaffRoleTable ?? (instanceStaffRoleTable = new StaffRoleTable());
            }
        }
    }
}