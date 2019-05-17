namespace SAC.Stock.Service.PersonContext
{
    using SAC.Membership.Service.UserManagement;
    using SAC.Seed.NLayer.Data;
    using SAC.Stock.Domain.PersonContext;
    using SAC.Stock.Service.BaseDto;
    using System;

    internal class PersonApplicationService : IPersonApplicationService
    {
        public IDataContext StockCtx { get; set; }
        public IUserManagementApplicationService UserManagementSvc { get; set; }

        public PersonDto GetPerson(Guid id)
        {
            var svc = new PersonService(StockCtx);
            return svc.GetPerson(id).AdaptPersonDto();
        }

        public PersonDto GetPerson(string uidCode, string uidSerie)
        {
            var svc = new PersonService(StockCtx);
            return svc.GetPerson(uidCode, uidSerie).AdaptPersonDto();
        }

        public PersonDto ModifyPerson(PersonDto personInfo)
        {
            var svc = new PersonService(StockCtx);
            return svc.ModifyPerson(personInfo).AdaptPersonDto();
        }

        public PersonDto ModifyPersonMember(PersonDto personInfo, Guid userId)
        {
            var svc = new PersonService(StockCtx);
            return svc.ModifyPersonMember(personInfo, UserManagementSvc, userId).AdaptPersonDto();
        }
    }
}
