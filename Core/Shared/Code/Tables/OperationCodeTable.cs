namespace SAC.Stock.Code.Tables
{
    using Seed.CodeTable;
    public class OperationCodeTable : CodeTable
    {     
        public readonly CodeItem AddCustomer = new CodeItem { Code = "AddCustomer", Name = "Agregar socio." };
     
        public readonly CodeItem QueryEmployees = new CodeItem { Code = "QueryEmployees", Name = "Consulta de socios empleados." };

        public readonly CodeItem ModifyCustomer = new CodeItem { Code = "ModifyCustomer", Name = "Modificar socio." };

        public readonly CodeItem ViewCustomer = new CodeItem { Code = "ViewCustomer", Name = "Visualizar socio." };

        public readonly CodeItem ViewCustomerMovements = new CodeItem { Code = "ViewCustomerMovements", Name = "Visualizar movimientos de socio." };

        public readonly CodeItem SendMailToCustomer = new CodeItem { Code = "SendMailToCustomer", Name = "Enviar email a socio" };

        public readonly CodeItem SendFirstLoginEmail = new CodeItem { Code = "SendFirstLoginEmail", Name = "Envial email del primer ingreso al sistema" };

        public readonly CodeItem AddBranchOffice = new CodeItem { Code = "AddBranchOffice", Name = "Agregar franquicia" };

        public readonly CodeItem ModifyBranchOffice = new CodeItem { Code = "ModifyBranchOffice", Name = "Modificar franquicia" };

        public readonly CodeItem ActivateBranchOffice = new CodeItem { Code = "ActivateBranchOffice", Name = "Activar franquicia" };

        public readonly CodeItem DeactivateBranchOffice = new CodeItem { Code = "DeactivateBranchOffice", Name = "Desactivar franquicia" };

        public readonly CodeItem ViewBranchOffice = new CodeItem { Code = "ViewBranchOffice", Name = "Visualizar franquicia" };

        public readonly CodeItem AddProduct = new CodeItem { Code = "AddProduct", Name = "Agregar producto" };
        
        public readonly CodeItem ModifyProduct = new CodeItem { Code = "ModifyProduct", Name = "Modificar producto" };

        public readonly CodeItem ViewProduct = new CodeItem { Code = "ViewProduct", Name = "Visualizar producto" };

        public readonly CodeItem DiscontinueProduct = new CodeItem { Code = "DiscontinueProduct", Name = "Discontinuar producto" };

        public readonly CodeItem UpdatePersonData = new CodeItem { Code = "UpdatePersonData", Name = "Actualización de datos de persona" };

        public readonly CodeItem UpdatePersonUid = new CodeItem { Code = "UpdatePersonUid", Name = "Actualización de datos de identificacion de persona" };

        public readonly CodeItem AddCountry = new CodeItem { Code = "AddCountry", Name = "Agregar pais" };

        public readonly CodeItem AddState = new CodeItem { Code = "AddState", Name = "Agregar provincia" };

        public readonly CodeItem AddCity = new CodeItem { Code = "AddCity", Name = "Agregar cuidad" };

        public readonly CodeItem ModifyCountry = new CodeItem { Code = "ModifyCountry", Name = "Modificar pais" };

        public readonly CodeItem ModifyState = new CodeItem { Code = "ModifyState", Name = "Modificar pais" };

        public readonly CodeItem ModifyCity = new CodeItem { Code = "ModifyCity", Name = "Modificar pais" };

        public readonly CodeItem LogInUser = new CodeItem { Code = "LogInUser", Name = "Ingreso al sistema" };

        public readonly CodeItem ModifyUser = new CodeItem { Code = "ModifyUser", Name = "Modificar perfil de usuario" };

        public readonly CodeItem ModifyEmail = new CodeItem { Code = "ModifyEmail", Name = "Modificar Email de usuario" };

        public readonly CodeItem RecoverUserPassword = new CodeItem { Code = "RecoverUserPassword", Name = "Solicitud de cambio de contraseña" };

        public readonly CodeItem ValidateEmailAddress = new CodeItem { Code = "ValidateEmailAddress", Name = "Solicitud de validación de casilla de correo electrónico" };

        public readonly CodeItem ChangeUserPassword = new CodeItem { Code = "ChangeUserPassword", Name = "Cambio de contraseña" };

        public readonly CodeItem ChangeUserName = new CodeItem { Code = "ChangeUserName", Name = "Cambio de nombre de usuario" };

        public readonly CodeItem FirstLogInUser = new CodeItem { Code = "FirstLogInUser", Name = "Primer ingreso al sistema" };

        public readonly CodeItem LogOutUser = new CodeItem { Code = "LogOutUser", Name = "Salida del sistema" };
    }
}
