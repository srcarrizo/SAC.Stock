namespace SAC.Stock.Code
{
    using Seed.NLayer.ExceptionHandling;

    internal class BusinessRulesCode
    {
        private static BusinessRuleData @default;
        private static BusinessRuleData locationExists;
        private static BusinessRuleData locationNotExists;
        private static BusinessRuleData failedConnectDatabase;
        private static BusinessRuleData billExists;
        private static BusinessRuleData billUnitTypeNotSelected;
        private static BusinessRuleData noUserLoggedIn;
        private static BusinessRuleData personExists;
        private static BusinessRuleData personNotExists;
        private static BusinessRuleData invalidDateOfBirth;
        private static BusinessRuleData locationNotCity;
        private static BusinessRuleData providerNotExists;
        private static BusinessRuleData customerNotExists;
        private static BusinessRuleData customerExists;
        private static BusinessRuleData providerExists;
        private static BusinessRuleData containerNotExists;
        private static BusinessRuleData productNotExists;
        private static BusinessRuleData productDeactivateNote;

        private static BusinessRuleData areaNotExists;
        private static BusinessRuleData categoryNotExists;
        private static BusinessRuleData subCategoryNotExists;
        private static BusinessRuleData productPriceNotExists;
        private static BusinessRuleData productPriceDeactivateNote;
        private static BusinessRuleData staffPrevious;
        private static BusinessRuleData staffNotExists;
        private static BusinessRuleData branchOfficeStaffPrevious;        
        private static BusinessRuleData userExists;
        private static BusinessRuleData branchOfficeStaffNotExists;
        private static BusinessRuleData branchOfficeByIdNotExists;
        private static BusinessRuleData branchOfficeShouldNotChangeActivate;
        private static BusinessRuleData branchOfficeDeactivateWithoutActivate;
        private static BusinessRuleData duplicateCode;
        private static BusinessRuleData buyWithoutBranchOffice;
        private static BusinessRuleData buyWithoutBranchOfficeStaff;
        private static BusinessRuleData buyWithoutProvider;
        private static BusinessRuleData buyNotExists;        
        private static BusinessRuleData transactionWithoutBill;
        private static BusinessRuleData profileNotExists;
        private static BusinessRuleData transactionDoesNotExist;
        private static BusinessRuleData transactionWithoutBranchOffice;
        private static BusinessRuleData transactionWithoutBranchOfficeStaff;
        private static BusinessRuleData saleWithoutBranchOffice;
        private static BusinessRuleData saleWithoutBranchOfficeStaff;
        private static BusinessRuleData saleWithoutCustomer;
        private static BusinessRuleData saleNotExists;
        private static BusinessRuleData lowStock;
        private static BusinessRuleData noStock;

        private static BusinessRuleData boxNotExists;
        private static BusinessRuleData boxOpeningWithoutOpenDate;
        private static BusinessRuleData boxOpeningWithoutOpenNote;

        private static BusinessRuleData boxClosingingWithoutCloseDate;
        private static BusinessRuleData boxClosingWithoutClosingNote;
        private static BusinessRuleData notPreSale;

        private static BusinessRuleData budgetWithoutBranchOffice;
        private static BusinessRuleData budgetWithoutBranchOfficeStaff;
        private static BusinessRuleData budgetWithoutProvider;
        private static BusinessRuleData budgetNotExists;
        private static BusinessRuleData budgetWithoutCustomer;

        public static BusinessRuleData NotPreSale
        {
            get
            {
                return notPreSale
                       ?? (notPreSale =
                           new BusinessRuleData
                           {
                               Code = "NotPreSale",
                               Message = "No se trata de una reserva."
                           });
            }
        }

        public static BusinessRuleData BoxClosingingWithoutCloseDate
        {
            get
            {
                return boxClosingingWithoutCloseDate
                       ?? (boxClosingingWithoutCloseDate =
                           new BusinessRuleData
                           {
                               Code = "BoxClosingingWithoutCloseDate",
                               Message = "Cierre de caja sin fecha de cierre."
                           });
            }
        }

        public static BusinessRuleData BoxClosingWithoutClosingNote
        {
            get
            {
                return boxClosingWithoutClosingNote
                       ?? (boxClosingWithoutClosingNote =
                           new BusinessRuleData
                           {
                               Code = "BoxClosingWithoutClosingNote",
                               Message = "Cierre de caja sin nota de cierre."
                           });
            }
        }

        public static BusinessRuleData BoxOpeningWithoutOpenDate
        {
            get
            {
                return boxOpeningWithoutOpenDate
                       ?? (boxOpeningWithoutOpenDate =
                           new BusinessRuleData
                           {
                               Code = "BoxOpeningWithoutOpenDate",
                               Message = "Apertura de caja sin fecha de apertura."
                           });
            }
        }

        public static BusinessRuleData BoxOpeningWithoutOpenNote
        {
            get
            {
                return boxOpeningWithoutOpenNote
                       ?? (boxOpeningWithoutOpenNote =
                           new BusinessRuleData
                           {
                               Code = "BoxOpeningWithoutOpenNote",
                               Message = "Apertura de caja sin nota de apertura."
                           });
            }
        }

        public static BusinessRuleData BoxNotExists
        {
            get
            {
                return boxNotExists
                       ?? (boxNotExists =
                           new BusinessRuleData
                           {
                               Code = "BoxNotExists",
                               Message = "La entrada de caja no existe."
                           });
            }
        }

        public static BusinessRuleData LowStock
        {
            get
            {
                return lowStock
                       ?? (lowStock =
                           new BusinessRuleData
                           {
                               Code = "LowStock",
                               Message = "El stock de este producto esta bajo."
                           });
            }
        }

        public static BusinessRuleData NoStock
        {
            get
            {
                return noStock
                       ?? (noStock =
                           new BusinessRuleData
                           {
                               Code = "NoStock",
                               Message = "No hay Stock para este producto."
                           });
            }
        }

        public static BusinessRuleData SaleNotExists
        {
            get
            {
                return saleNotExists
                       ?? (saleNotExists =
                           new BusinessRuleData
                           {
                               Code = "SaleNotExists",
                               Message = "La compra no existe."
                           });
            }
        }

        public static BusinessRuleData SaleWithoutCustomer
        {
            get
            {
                return saleWithoutCustomer
                       ?? (saleWithoutCustomer = new BusinessRuleData { Code = "SaleWithoutCustomer", Message = "Debe especificar un proveedor." });
            }
        }

        public static BusinessRuleData SaleWithoutBranchOfficeStaff
        {
            get
            {
                return saleWithoutBranchOfficeStaff
                       ?? (saleWithoutBranchOfficeStaff = new BusinessRuleData { Code = "SaleWithoutBranchOfficeStaff", Message = "Debe especificar un empleado." });
            }
        }

        public static BusinessRuleData SaleWithoutBranchOffice
        {
            get
            {
                return saleWithoutBranchOffice
                       ?? (saleWithoutBranchOffice = new BusinessRuleData { Code = "SaleWithoutBranchOffice", Message = "Debe especificar una sucursal." });
            }
        }

        public static BusinessRuleData TransactionWithoutBranchOffice
        {
            get
            {
                return transactionWithoutBranchOffice
                       ?? (transactionWithoutBranchOffice =
                           new BusinessRuleData
                           {
                               Code = "TransactionWithoutBranchOffice",
                               Message = "La transacción debe tener una sucursal."
                           });
            }
        }

        public static BusinessRuleData TransactionWithoutBranchOfficeStaff
        {
            get
            {
                return transactionWithoutBranchOfficeStaff
                       ?? (transactionWithoutBranchOfficeStaff =
                           new BusinessRuleData
                           {
                               Code = "TransactionWithoutBranchOfficeStaff",
                               Message = "La transacción debe tener un empleado."
                           });
            }
        }

        public static BusinessRuleData TransactionDoesNotExist
        {
            get
            {
                return transactionDoesNotExist
                       ?? (transactionDoesNotExist =
                           new BusinessRuleData
                           {
                               Code = "TransactionDoesNotExist",
                               Message = "La transacción no existe."
                           });
            }
        }

        public static BusinessRuleData BuyNotExists
        {
            get
            {
                return buyNotExists
                       ?? (buyNotExists =
                           new BusinessRuleData
                           {
                               Code = "BuyNotExists",
                               Message = "La compra no existe."
                           });
            }
        }

        public static BusinessRuleData BuyWithoutProvider
        {
            get
            {
                return buyWithoutProvider
                       ?? (buyWithoutProvider = new BusinessRuleData { Code = "BuyWithoutProvider", Message = "Debe especificar un proveedor." });
            }
        }

        public static BusinessRuleData BuyWithoutBranchOfficeStaff
        {
            get
            {
                return buyWithoutBranchOfficeStaff
                       ?? (buyWithoutBranchOfficeStaff = new BusinessRuleData { Code = "BuyWithoutBranchOfficeStaff", Message = "Debe especificar un empleado." });
            }
        }

        public static BusinessRuleData BuyWithoutBranchOffice
        {
            get
            {
                return buyWithoutBranchOffice
                       ?? (buyWithoutBranchOffice = new BusinessRuleData { Code = "BuyWithoutBranchOffice", Message = "Debe especificar una sucursal." });
            }
        }

        public static BusinessRuleData ProfileNotExists
        {
            get
            {
                return profileNotExists
                       ?? (profileNotExists =
                           new BusinessRuleData
                           {
                               Code = "ProfileNotExists",
                               Message = "El perfil seleccionado no existe en la base de datos."
                           });
            }
        }

        public static BusinessRuleData TransactionWithoutBill
        {
            get
            {
                return transactionWithoutBill
                       ?? (transactionWithoutBill = new BusinessRuleData { Code = "TransactionWithoutBill", Message = "Transacci{on sin valores monetarios incluidos." });
            }
        }

        public static BusinessRuleData UserExists
        {
            get
            {
                return userExists
                       ?? (userExists = new BusinessRuleData { Code = "UserExists", Message = "El usuario ya existe en la base de datos." });
            }
        }
        public static BusinessRuleData BranchOfficeStaffNotExists
        {
            get
            {
                return branchOfficeStaffNotExists
                       ?? (branchOfficeStaffNotExists = new BusinessRuleData { Code = "BranchOfficeStaffNotExists", Message = "No existe información del colaborador." });
            }
        }

        public static BusinessRuleData BranchOfficeByIdNotExists
        {
            get
            {
                return branchOfficeByIdNotExists
                       ?? (branchOfficeByIdNotExists = new BusinessRuleData { Code = "BranchOfficeByIdNotExists", Message = "La sucursal no existe.\nId de sucursal: {0}." });
            }
        }
      
        public static BusinessRuleData BranchOfficeShouldNotChangeActivate
        {
            get
            {
                return branchOfficeShouldNotChangeActivate
                       ?? (branchOfficeShouldNotChangeActivate =
                           new BusinessRuleData
                           {
                               Code = "BranchOfficeShouldNotChangeActivate",
                               Message = "No se puede modificar la fecha de activación."
                           });
            }
        }

        public static BusinessRuleData BranchOfficeDeactivateWithoutActivate
        {
            get
            {
                return branchOfficeDeactivateWithoutActivate
                       ?? (branchOfficeDeactivateWithoutActivate =
                           new BusinessRuleData
                           {
                               Code = "BranchOfficeDeactivateWithoutActivate",
                               Message = "No se puede desactivar una sucursal sin previamente haberla activado."
                           });
            }
        }

        public static BusinessRuleData DuplicateCode
        {
            get
            {
                return duplicateCode
                       ?? (duplicateCode =
                           new BusinessRuleData { Code = "DuplicateCode", Message = "El código ingresado ya esta en uso. Especifique uno diferente." });
            }
        }

        public static BusinessRuleData BranchOfficeStaffPrevious
        {
            get
            {
                return branchOfficeStaffPrevious
                       ?? (branchOfficeStaffPrevious = new BusinessRuleData { Code = "BranchOfficeStaffPrevious", Message = "El colaborador que se intenta ingresar ya es colaborador registrado." });
            }
        }

        public static BusinessRuleData StaffNotExists
        {
            get
            {
                return staffNotExists
                       ?? (staffNotExists = new BusinessRuleData { Code = "StaffNotExists", Message = "No hay datos del personal de staff solitiado." });
            }
        }

        public static BusinessRuleData StaffPrevious
        {
            get
            {
                return staffPrevious
                       ?? (staffPrevious = new BusinessRuleData { Code = "StaffPrevious", Message = "El personal del staff que se intenta ingresar ya es colaborador registrado." });
            }
        }

        public static BusinessRuleData AreaNotExists
        {
            get
            {
                return areaNotExists
                       ?? (areaNotExists =
                           new BusinessRuleData
                           {
                               Code = "areaNotExists",
                               Message = "El area no existe en la base de datos."
                           });
            }
        }

        public static BusinessRuleData CategoryNotExists
        {
            get
            {
                return categoryNotExists
                       ?? (categoryNotExists =
                           new BusinessRuleData
                           {
                               Code = "categoryNotExists",
                               Message = "La categoria no existe en la base de datos."
                           });
            }
        }

        public static BusinessRuleData SubCategoryNotExists
        {
            get
            {
                return subCategoryNotExists
                       ?? (subCategoryNotExists =
                           new BusinessRuleData
                           {
                               Code = "subCategoryNotExists",
                               Message = "La subcategoria no existe en la base de datos."
                           });
            }
        }

        public static BusinessRuleData ProductPriceNotExists
        {
            get
            {
                return productPriceNotExists
                       ?? (productPriceNotExists =
                           new BusinessRuleData
                           {
                               Code = "productPriceNotExists",
                               Message = "El precio del producto no existe en la base de datos."
                           });
            }
        }

        public static BusinessRuleData ProductPriceDeactivateNote
        {
            get
            {
                return productPriceDeactivateNote
                       ?? (productPriceDeactivateNote =
                           new BusinessRuleData
                           {
                               Code = "productPriceDeactivateNote",
                               Message = "Debe especificar un motivo de desactivación del precio del producto."
                           });
            }
        }

        public static BusinessRuleData ProductNotExists
        {
            get
            {
                return productNotExists
                       ?? (productNotExists =
                           new BusinessRuleData
                           {
                               Code = "ProductNotExists",
                               Message = "El producto no existe en la base de datos."
                           });
            }
        }

        public static BusinessRuleData ProductDeactivateNote
        {
            get
            {
                return productDeactivateNote
                       ?? (productDeactivateNote =
                           new BusinessRuleData
                           {
                               Code = "productDeactivateNote",
                               Message = "Debe especificar un motivo de desactivación del producto."
                           });
            }
        }

        public static BusinessRuleData ContainerNotExists
        {
            get
            {
                return containerNotExists
                       ?? (containerNotExists =
                           new BusinessRuleData
                           {
                               Code = "ContainerNotExists",
                               Message = "El embase no existe en la base de datos."
                           });
            }
        }
        public static BusinessRuleData ProviderExists
        {
            get
            {
                return providerExists
                       ?? (providerExists =
                           new BusinessRuleData
                           {
                               Code = "ProviderExists",
                               Message = "El proveedor ya se encuentra dado de alta en la base de datos."
                           });
            }
        }

        public static BusinessRuleData CustomerExists
        {
            get
            {
                return customerExists
                       ?? (customerExists =
                           new BusinessRuleData
                           {
                               Code = "CustomerExists",
                               Message = "El cliente ya se encuentra dado de alta en la base de datos."
                           });
            }
        }

        public static BusinessRuleData ProviderNotExists
        {
            get
            {
                return providerNotExists
                       ?? (providerNotExists = new BusinessRuleData { Code = "ProviderNotExists", Message = "El proveedor solicitado no esta registrado." });
            }
        }

        public static BusinessRuleData CustomerNotExists
        {
            get
            {
                return customerNotExists
                       ?? (customerNotExists = new BusinessRuleData { Code = "CustomerNotExists", Message = "El cliente solicitado no esta registrado." });
            }
        }

        public static BusinessRuleData LocationNotCity
        {
            get
            {
                return locationNotCity
                       ?? (locationNotCity =
                           new BusinessRuleData { Code = "LocationNotCity", Message = "La cuidad no fue ingresada correctamente." });
            }
        }

        public static BusinessRuleData InvalidDateOfBirth
        {
            get
            {
                return invalidDateOfBirth
                       ?? (invalidDateOfBirth =
                       new BusinessRuleData
                       {
                           Code = "InvalidDateOfBirth",
                           Message = "La fecha de nacimiento debe ser mayor a 01/01/1900."
                       });
            }
        }

        public static BusinessRuleData PersonNotExists
        {
            get
            {
                return personNotExists
                       ?? (personNotExists = new BusinessRuleData { Code = "PersonNotExists", Message = "No hay datos de la persona solicitada." });
            }
        }

        public static BusinessRuleData PersonExists
        {
            get
            {
                return personExists
                       ?? (personExists = new BusinessRuleData { Code = "PersonExists", Message = "Los datos personales ya existen en el sistema." });
            }
        }

        public static BusinessRuleData NoUserLoggedIn
        {
            get
            {
                return noUserLoggedIn
                       ?? (noUserLoggedIn =
                           new BusinessRuleData
                           {
                               Code = "NoUserLoggedIn",
                               Message = "No se registra un usuario válido."
                           });
            }
        }

        public static BusinessRuleData BillUnitTypeNotSelected
        {
            get
            {
                return billUnitTypeNotSelected
                       ?? (billUnitTypeNotSelected =
                           new BusinessRuleData { Code = "billUnitTypeNotSelected", Message = "Debe seleccionar un tipo de moneda" });
            }
        }

        public static BusinessRuleData BillExists
        {
            get
            {
                return billExists
                       ?? (billExists =
                           new BusinessRuleData { Code = "billExists", Message = "El billete o moneda ya existe" });
            }
        }

        public static BusinessRuleData FailedConnectDatabase
        {
            get
            {
                return failedConnectDatabase ?? (failedConnectDatabase = new BusinessRuleData { Code = "FailedConnectDatabase", Message = "Falló la conexión a la Base de datos." });
            }
        }

        public static BusinessRuleData LocationNotExists
        {
            get
            {
                return locationNotExists
                       ?? (locationNotExists =
                           new BusinessRuleData { Code = "LocationNotExists", Message = "La Localidad (País/Provincia/Cuidad) no se encuentra en la base de datos." });
            }
        }

        public static BusinessRuleData LocationExists
        {
            get
            {
                return locationExists
                       ?? (locationExists =
                           new BusinessRuleData { Code = "LocationExists", Message = "La Localidad (País/Provincia/Cuidad) ya se encuentra en la base de datos." });
            }
        }

        public static BusinessRuleData Default
        {
            get
            {
                return @default
                       ?? (@default =
                           new BusinessRuleData
                           {
                               Code = "Default",
                               Message =
                                   "Se presentó una condición de falla. Por favor, reintente y si se repite el inconveniente comuníquelo al soporte técnico del sistema."
                           });
            }
        }

        //--//-- 
        public static BusinessRuleData BudgetNotExists
        {
            get
            {
                return budgetNotExists
                       ?? (budgetNotExists =
                           new BusinessRuleData
                           {
                               Code = "BudgetNotExists",
                               Message = "El presupuesto no existe."
                           });
            }
        }

        public static BusinessRuleData BudgetWithoutProvider
        {
            get
            {
                return budgetWithoutProvider
                       ?? (budgetWithoutProvider = new BusinessRuleData { Code = "BudgetWithoutProvider", Message = "Debe especificar un proveedor." });
            }
        }

        public static BusinessRuleData BudgetWithoutBranchOfficeStaff
        {
            get
            {
                return budgetWithoutBranchOfficeStaff
                       ?? (budgetWithoutBranchOfficeStaff = new BusinessRuleData { Code = "BudgetWithoutBranchOfficeStaff", Message = "Debe especificar un empleado." });
            }
        }

        public static BusinessRuleData BudgetWithoutBranchOffice
        {
            get
            {
                return budgetWithoutBranchOffice
                       ?? (budgetWithoutBranchOffice = new BusinessRuleData { Code = "BudgetWithoutBranchOffice", Message = "Debe especificar una sucursal." });
            }
        }

        public static BusinessRuleData BudgetWithoutCustomer
        {
            get
            {
                return budgetWithoutCustomer
                       ?? (budgetWithoutCustomer = new BusinessRuleData { Code = "BudgetWithoutCustomer", Message = "Debe especificar un cliente." });
            }
        }        

internal class BusinessRuleData
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public string FormatMessage(params object[] args)
            {
                return args.Length > 0 ? string.Format(this.Message, args) : this.Message;
            }

            public BusinessRulesException NewBusinessException(params object[] args)
            {
                return new BusinessRulesException(this.FormatMessage(args), this.Code);
            }
        }
    }
}