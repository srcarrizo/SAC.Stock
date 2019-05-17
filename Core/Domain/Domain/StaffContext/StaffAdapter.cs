namespace SAC.Stock.Domain.StaffContext
{
    using PersonContext;
    using Service.BaseDto;  
    internal static class StaffAdapter
    {
        public static Staff AdaptToStaff(this StaffDto dto)
        {
            return dto == null ? null : new Staff { Id = dto.Id, Person = dto.Person.AdaptPerson() };
        }

        public static StaffDto AdaptToStaffDto(this Staff entity)
        {
            return entity == null ? null : new StaffDto { Id = entity.Id, Person = entity.Person.AdaptPersonDto() };
        }
    }
}
