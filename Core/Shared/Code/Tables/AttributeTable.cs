namespace SAC.Stock.Code.Tables
{
    using SAC.Seed.CodeTable;
    public class AttributeTable : CodeTable
    {
        public readonly CodeItemAttribute<StaffRoleTable> StaffRole = new CodeItemAttribute<StaffRoleTable> { Code = "StaffRole", Name = "Rol de organización" };
        public readonly CodeItemAttribute<ScopeTable> Scope = new CodeItemAttribute<ScopeTable> { Code = "Scope", Name = "Scope de la aplicación" };
        public readonly CodeItem StaffType = new CodeItem { Code = "StaffType", Name = "Tipo de staff" };        
        public readonly CodeItem StaffId = new CodeItem { Code = "StaffId", Name = "Id de Staff" };               
        public readonly CodeItem PersonId = new CodeItem { Code = "PersonId", Name = "Id de Persona" };       
        public readonly CodeItem SpecialCustomer = new CodeItem { Code = "SpecialCustomer", Name = "Cliente Especial" };
        public readonly CodeItem Email = new CodeItem { Code = Code.Email, Name = "Email" };              
        public readonly CodeItem CustomerId = new CodeItem { Code = "CustomerId", Name = "Id de customer" };
        public readonly CodeItemAttribute<GenderTable> Gender = new CodeItemAttribute<GenderTable> { Code = "Gender", Name = "Genero de la entidad" };
        public readonly CodeItem PhoneAreaCode = new CodeItem { Code = "PhoneAreaCode", Name = "Código de área de teléfono" };
        public readonly CodeItem PhoneCountryCode = new CodeItem { Code = "PhoneCountryCode", Name = "Caractarística de teléfono de país" };           
        public readonly CodeItem ZipCode = new CodeItem { Code = "ZipCode", Name = "Código postal" };
        public readonly CodeItem BirthDate = new CodeItem { Code = "BirthDate", Name = "Fecha de nacimiento" };
        public readonly CodeItem UserCurrentAppVersion = new CodeItem { Code = "UserCurrentAppVersion", Name = "Actual versión del usuario" };              
        public readonly CodeItem ValidatedEmailAddress = new CodeItem { Code = "ValidatedEmailAddress", Name = "Casilla de correo electrónico válida." };
        public readonly CodeItem BranchOfficeStaffId = new CodeItem { Code = "BranchOfficeStaffId", Name = "Id de empleado." };
        public readonly CodeItem BranchOfficeId = new CodeItem { Code = "BranchOfficeId", Name = "Id de Sucursal." };

        public static class Code
        {
            public const string Email = "Email";
        }
    }
}
