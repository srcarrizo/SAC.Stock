namespace SAC.Stock.Domain.StaffContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Domain;
    using SAC.Seed.NLayer.Domain.Specification;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.PersonContext;
    using SAC.Stock.Service.BaseDto;
    using System;    
    using System.Linq;    
    internal class StaffService
    {
        private readonly IDataContext context;

        public StaffService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Staff, Guid> ViewStaff
        {
            get
            {
                return context.GetView<Staff, Guid>();
            }
        }

        public Staff GetStaff(Guid id)
        {
            return ViewStaff.Get(id);
        }

        public Staff GetStaff(string uidCode, string uidSerie)
        {
            return ViewStaff.Query(SpecStaffByUid(uidCode, uidSerie)).FirstOrDefault();
        }

        public Staff AddStaff(StaffDto staffInfo)
        {
            if (GetStaff(staffInfo.Person.UidCode, staffInfo.Person.UidSerie) != null)
            {
                throw new BusinessRulesException(BusinessRulesCode.StaffPrevious.Message, BusinessRulesCode.StaffPrevious.Code);
            }

            var staff = NewStaff(staffInfo);
            ViewStaff.Add(staff);
            context.ApplyChanges();

            return staff;
        }

        public Staff UpdateStaff(StaffDto staffInfo, Staff staff = null)
        {
            staff = staff ?? this.GetStaff(staffInfo.Id);

            if (staff == null)
            {
                throw new BusinessRulesException(BusinessRulesCode.StaffNotExists.Message, BusinessRulesCode.StaffNotExists.Code);
            }

            var personSvc = new PersonService(context);
            personSvc.UpdatePerson(staffInfo.Person, staff.Person);

            return staff;
        }

        public Staff NewStaff(StaffDto staffInfo)
        {
            var svcPerson = new PersonService(context);
            var person = svcPerson.GetOrNewPerson(staffInfo.Person, true);
            var staff = new Staff { Id = person.Id, Person = person };

            return staff;
        }

        public Staff GetOrNewStaff(StaffDto staffInfo)
        {
            var staff = (GetStaff(staffInfo.Id) ?? GetStaff(staffInfo.Person.UidCode, staffInfo.Person.UidSerie)) ?? NewStaff(staffInfo);

            return staff;
        }

        private static Specification<Staff> SpecStaffByUid(string uidCode, string uidSerie)
        {
            return new DirectSpecification<Staff>(
                p => p.Person.UidCode.Equals(uidCode, StringComparison.InvariantCultureIgnoreCase) && p.Person.UidSerie.Equals(uidSerie, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
