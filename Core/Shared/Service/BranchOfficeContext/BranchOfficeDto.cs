using SAC.Seed.NLayer.Application;
using SAC.Stock.Service.BaseDto;
using SAC.Stock.Service.BuyContext;
using SAC.Stock.Service.SaleContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Stock.Service.BranchOfficeContext
{
    internal class BranchOfficeDto : EntityDto<Guid>
    {
        public BranchOfficeDto()
        {
            Phones = new List<PhoneDto>();
        }

        public string Name { get; set; }
        public string Description { get; set; }        
        public AddressDto Address { get; set; }
        public ICollection<PhoneDto> Phones { get; set; }
        public DateTimeOffset? ActivatedDate { get; set; }
        public DateTimeOffset? DeactivatedDate { get; set; }
        public string DeactivateNote { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public ICollection<SaleDto> Sales { get; set; }
        public ICollection<BuyDto> Buys { get; set; }
    }
}
