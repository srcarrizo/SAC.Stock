namespace SAC.Stock.Domain.PersonContext
{
    using SAC.Membership.Service.UserManagement;
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Domain;
    using SAC.Seed.NLayer.Domain.Specification;
    using SAC.Seed.NLayer.ExceptionHandling;
    using SAC.Stock.Code;
    using SAC.Stock.Domain.AttributeValueContext;
    using SAC.Stock.Domain.Infrastructure;
    using SAC.Stock.Domain.LocationContext;
    using SAC.Stock.Domain.PhoneContext;
    using SAC.Stock.Infrastucture;
    using SAC.Stock.Service.BaseDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    internal class PersonService : DomainService
    {
        public PersonService(IDataContext context) : base(context)
        {
        }

        private IDataView<Person, Guid> ViewPerson
        {
            get
            {
                return Context.GetView<Person, Guid>();
            }
        }

        private IDataView<PersonPhone, int> ViewPersonPhone
        {
            get
            {
                return Context.GetView<PersonPhone, int>();
            }
        }

        private IDataView<PersonAttributeValue, int> ViewPersonAttributeValue
        {
            get
            {
                return Context.GetView<PersonAttributeValue, int>();
            }
        }

        public Person GetPerson(Guid id)
        {
            return ViewPerson.Get(id);
        }

        public Person GetPerson(string uidCode, string uidSerie)
        {
            return ViewPerson.Query(SpecPersonByUid(uidCode, uidSerie)).FirstOrDefault();
        }

        public Person AddPerson(PersonDto personInfo)
        {
            var person = GetPerson(personInfo.Id) ?? GetPerson(personInfo.UidCode, personInfo.UidSerie);
            if (person != null)
            {
                throw new BusinessRulesException(BusinessRulesCode.PersonExists.Message, BusinessRulesCode.PersonExists.Code);
            }

            person = NewPerson(personInfo);

            Context.ApplyChanges();

            return person;
        }

        public Person UpdatePerson(PersonDto personInfo, Person person = null)
        {
            person = person ?? GetPerson(personInfo.Id);
            if (person == null)
            {
                throw BusinessRulesCode.PersonNotExists.NewBusinessException();
            }

            if (personInfo.BirthDate < new DateTime(1900, 01, 01))
            {
                throw BusinessRulesCode.InvalidDateOfBirth.NewBusinessException();
            }

            MergePersonPhone(personInfo, person);

            MergePersonAttribute(personInfo, person);

            var locationSvc = new LocationService(Context);
            person.Address = locationSvc.UpdateOrNewAddress(personInfo.Address, person.Address);

            if (!person.UidCode.Equals(personInfo.UidCode, StringComparison.InvariantCultureIgnoreCase)
                || !person.UidSerie.Equals(personInfo.UidSerie, StringComparison.InvariantCultureIgnoreCase))
            {
                Logg.Info(
                  CodeConst.OperationCode.UpdatePersonUid.Code,
                  new Dictionary<string, string>
                    {
              { "PersonId", person.Id.ToString() },
              { "PreviousUidCode", person.UidCode },
              { "PreviousUidSerie", person.UidSerie },
              { "UidCode", personInfo.UidCode },
              { "UidSerie", personInfo.UidSerie }
                    });
            }

            person.FirstName = personInfo.FirstName;
            person.LastName = personInfo.LastName;
            person.GenderCode = personInfo.GenderCode;
            person.Email = personInfo.Email;
            person.UidCode = personInfo.UidCode;
            person.UidSerie = personInfo.UidSerie;
            person.BirthDate = personInfo.BirthDate.Date;

            ViewPerson.Modify(person);

            Logg.Verbose(
              CodeConst.OperationCode.UpdatePersonData.Code,
              new Dictionary<string, string>
                {
            { "PersonId", person.Id.ToString() },
            { "UidCode", person.UidCode },
            { "UidSerie", person.UidSerie },
            { "Email", person.Email }
                });

            return person;
        }

        public Person UpdatePersonMember(PersonDto personInfo, IUserManagementApplicationService userManagementSvc, Guid userId, Person person = null)
        {
            person = person ?? GetPerson(personInfo.Id);
            if (person == null)
            {
                throw BusinessRulesCode.PersonNotExists.NewBusinessException();
            }

            if (personInfo.BirthDate < new DateTime(1900, 01, 01))
            {
                throw BusinessRulesCode.InvalidDateOfBirth.NewBusinessException();
            }

            MergePersonPhone(personInfo, person);

            MergePersonAttribute(personInfo, person);

            var locationSvc = new LocationService(Context);

            if (!personInfo.ChangingPassword)
            {
                person.Address = locationSvc.UpdateOrNewAddress(personInfo.Address, person.Address);
            }

            if (!person.UidCode.Equals(personInfo.UidCode, StringComparison.InvariantCultureIgnoreCase)
                || !person.UidSerie.Equals(personInfo.UidSerie, StringComparison.InvariantCultureIgnoreCase))
            {
                Logg.Info(
                  CodeConst.OperationCode.UpdatePersonUid.Code,
                  new Dictionary<string, string>
                    {
              { "PersonId", person.Id.ToString() },
              { "PreviousUidCode", person.UidCode },
              { "PreviousUidSerie", person.UidSerie },
              { "UidCode", personInfo.UidCode },
              { "UidSerie", personInfo.UidSerie }
                    });
            }

            person.FirstName = personInfo.FirstName;
            person.LastName = personInfo.LastName;
            person.GenderCode = personInfo.GenderCode;
            person.Email = personInfo.Email;
            person.UidCode = personInfo.UidCode;
            person.UidSerie = personInfo.UidSerie;
            person.BirthDate = personInfo.BirthDate.Date;
            var user = userManagementSvc.GetUser(userId);
            user.Email = string.IsNullOrWhiteSpace(person.Email) ? SharedSettings.DefaultUserEmail : person.Email;
            user.UidSerie = personInfo.UidSerie;
            user.UidCode = personInfo.UidCode;
            user.FirstName = personInfo.FirstName;
            user.LastName = personInfo.LastName;
            user.Email = personInfo.Email;

            userManagementSvc.ModifyUser(user);
            ViewPerson.Modify(person);

            Logg.Verbose(
              CodeConst.OperationCode.UpdatePersonData.Code,
              new Dictionary<string, string>
                {
            { "PersonId", person.Id.ToString() },
            { "UidCode", person.UidCode },
            { "UidSerie", person.UidSerie },
            { "Email", person.Email }
                });

            return person;
        }

        public Person NewPerson(PersonDto personInfo)
        {
            if (personInfo.BirthDate < new DateTime(1900, 01, 01))
            {
                throw BusinessRulesCode.InvalidDateOfBirth.NewBusinessException();
            }

            var locationSvc = new LocationService(Context);
            var person = new Person
            {
                BirthDate = personInfo.BirthDate.Date,
                GenderCode = personInfo.GenderCode,
                Email = personInfo.Email,
                LastName = personInfo.LastName,
                FirstName = personInfo.FirstName,
                UidCode = personInfo.UidCode,
                UidSerie = personInfo.UidSerie,
                Attributes =
                               personInfo.Attributes == null
                                 ? new List<PersonAttributeValue>()
                                 : personInfo.Attributes.Select(r => new PersonAttributeValue { AttributeCode = r.AttributeCode, Value = r.Value }).ToList(),
                                Phones =
                               personInfo.Phones == null
                                 ? new List<PersonPhone>()
                                 : personInfo.Phones.Select(
                                   r =>
                                   new PersonPhone
                                   {
                                       AreaCode = r.AreaCode,
                                       CountryCode = r.CountryCode,
                                       Mobile = r.Mobile,
                                       Name = r.Name,
                                       Number = r.Number,
                                       TelcoId = r.TelcoId
                                   }).ToList(),
                Address = locationSvc.UpdateOrNewAddress(personInfo.Address)
            };

            person.GenerateNewIdentity();

            ViewPerson.Add(person);

            return person;
        }

        public Person GetOrNewPerson(PersonDto personInfo, bool update)
        {
            var person = GetPerson(personInfo.Id) ?? GetPerson(personInfo.UidCode, personInfo.UidSerie);
            if (person != null && update)
            {
                UpdatePerson(personInfo, person);
            }

            return person ?? NewPerson(personInfo);
        }

        public Person ModifyPerson(PersonDto personInfo)
        {
            var person = GetPerson(personInfo.Id) ?? GetPerson(personInfo.UidCode, personInfo.UidSerie);
            UpdatePerson(personInfo, person);
            Context.ApplyChanges();

            return person;
        }

        public Person ModifyPersonMember(PersonDto personInfo, IUserManagementApplicationService userManagementSvc, Guid userId)
        {
            var person = GetPerson(personInfo.Id) ?? GetPerson(personInfo.UidCode, personInfo.UidSerie);
            UpdatePersonMember(personInfo, userManagementSvc, userId, person);
            Context.ApplyChanges();

            return person;
        }

        private static string FirstToCamelCase(string data)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.ToLowerInvariant());
        }

        private static string FirstToCapital(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return data;
            }

            var chars = data.ToCharArray();
            chars[0] = chars[0].ToString(CultureInfo.InvariantCulture).ToUpperInvariant().ToCharArray()[0];
            return new string(chars);
        }

        private static Specification<Person> SpecPersonByUid(string uidCode, string uidSerie)
        {
            return
              new DirectSpecification<Person>(
                p => p.UidCode.Equals(uidCode, StringComparison.InvariantCultureIgnoreCase) && p.UidSerie.Equals(uidSerie, StringComparison.InvariantCultureIgnoreCase));
        }

        private void MergePersonPhone(PersonDto personInfo, Person person)
        {
            var toDelete = new List<int>();
            foreach (var phone in person.Phones)
            {
                var actual = personInfo.Phones.FirstOrDefault(r => r.Id == phone.Id);
                if (actual == null)
                {
                    toDelete.Add(phone.Id);
                }
                else
                {
                    actual.AdaptToPhone(phone);
                    ViewPersonPhone.Modify(phone);
                }
            }

            foreach (var phone in personInfo.Phones.Where(r => r.Id == 0).Select(r => r.AdaptTo<PersonPhone>()))
            {
                phone.PersonId = person.Id;
                ViewPersonPhone.Add(phone);
            }

            foreach (var id in toDelete)
            {
                ViewPersonPhone.Remove(id);
            }
        }

        private void MergePersonAttribute(PersonDto personInfo, Person person)
        {
            var toDelete = new List<int>();
            foreach (var attr in person.Attributes)
            {
                var actual = personInfo.Attributes.FirstOrDefault(r => r.Id == attr.Id);
                if (actual == null)
                {
                    toDelete.Add(attr.Id);
                }
                else
                {
                    actual.AdaptToAttributeValue(attr);
                    ViewPersonAttributeValue.Modify(attr);
                }
            }

            foreach (var attr in personInfo.Attributes.Where(r => r.Id == 0).Select(r => r.AdaptTo<PersonAttributeValue>()))
            {
                attr.PersonId = person.Id;
                ViewPersonAttributeValue.Add(attr);
            }

            foreach (var id in toDelete)
            {
                ViewPersonAttributeValue.Remove(id);
            }
        }
    }
}