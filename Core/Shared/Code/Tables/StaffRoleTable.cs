namespace SAC.Stock.Code.Tables
{
    using SAC.Seed.CodeTable;
    public class StaffRoleTable : CodeTable
    {
        public readonly CodeItem Employee = new CodeItem { Code = "Employee", Name = "Empleado" };
        public readonly CodeItem Owner = new CodeItem { Code = "Owner", Name = "Dueño" };
        public readonly CodeItem SACAministrator = new CodeItem { Code = "SACAministrator", Name = "Aministrador de SAC" };
    }
}
