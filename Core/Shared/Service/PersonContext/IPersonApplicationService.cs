namespace SAC.Stock.Service.PersonContext
{
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Service.BaseDto;
    using System;
    using System.ServiceModel;

    [ServiceContract]
    internal interface IPersonApplicationService
    {
        [OperationContract]
        PersonDto GetPerson(Guid id);

        [OperationContract]
        PersonDto GetPerson(string uidCode, string uidSerie);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        PersonDto ModifyPerson(PersonDto personInfo);

        [OperationContract]
        [FaultContract(typeof(BusinessRulesFaultDetail))]
        PersonDto ModifyPersonMember(PersonDto personInfo, Guid userId);
    }
}
