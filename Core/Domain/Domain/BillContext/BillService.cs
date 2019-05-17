namespace SAC.Stock.Domain.BillContext
{
    using SAC.Seed.NLayer.Data;
    using SAC.Seed.NLayer.Data.Ordering;
    using SAC.Seed.NLayer.Domain;
    using SAC.Seed.NLayer.Domain.Specification;
    using SAC.Stock.Code;
    using SAC.Stock.Service.BillContext;
    using System;
    using System.Collections.Generic;
    
    internal class BillService
    {
        private readonly IDataContext context;

        public BillService(IDataContext context)
        {
            this.context = context;
        }

        private IDataView<Bill, int> ViewBills
        {
            get
            {
                return context.GetView<Bill, int>();
            }
        }

        private IDataView<BillUnitType, int> ViewBillUnitTypes
        {
            get
            {
                return context.GetView<BillUnitType, int>();
            }
        }

        public BillUnitType GetBillUnitType(int id)
        {
            return ViewBillUnitTypes.Get(id);
        }

        public IEnumerable<BillUnitType> QueryBillUnitType(int pageIndex, int pageSize, ICollection<SortInfo> sortInfo, ICollection<FilterInfo> filterInfo = null)
        {
            var orderByExpressions = new List<IOrderByExpression<BillUnitType>>();

            foreach (var info in sortInfo)
            {
                if (info.Field.Equals(SortQuery.BillUnitType.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    orderByExpressions.Add(new OrderByExpression<BillUnitType, string>(r => r.Name, info.Direction));
                }
            }

            return ViewBillUnitTypes.Query(pageIndex, pageSize, orderByExpressions.ToArray(), GetBillUnitTypeSpecification(filterInfo));
        }

        public IEnumerable<Bill> QueryBill()
        {
            return ViewBills.GetAll();
        }

        private static Specification<BillUnitType> GetBillUnitTypeSpecification(IEnumerable<FilterInfo> filterInfo)
        {
            Specification<BillUnitType> spec = new TrueSpecification<BillUnitType>();
            if (filterInfo == null)
            {
                return spec;
            }

            foreach (var info in filterInfo)
            {              
                if (info.Spec.Equals(SpecFilter.BillUnitType.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecBillUnitTypeByName(info.Value);
                }

                if (info.Spec.Equals(SpecFilter.BillUnitType.Decimal, StringComparison.InvariantCultureIgnoreCase))
                {
                    spec &= SpecBillUnitTypeByDecimal(bool.Parse(info.Value));
                }
            }

            return spec;
        }
        
        private static Specification<BillUnitType> SpecBillUnitTypeByName(string value)
        {
            return new DirectSpecification<BillUnitType>(c => c.Name.Contains(value));
        }
        private static Specification<BillUnitType> SpecBillUnitTypeByDecimal(bool value)
        {
            return new DirectSpecification<BillUnitType>(c => c.IsDecimal == value);
        }

        public Bill AddBill(BillDto billDto)
        {
            if (billDto.BillUnitType == null)
            {
                throw BusinessRulesCode.BillUnitTypeNotSelected.NewBusinessException();
            }

            if (ViewBills.Exists(b => b.Value == billDto.Value && b.BillUnitTypeId == billDto.BillUnitType.Id))
            {
                throw BusinessRulesCode.BillExists.NewBusinessException();
            }

            var bill = NewBill(billDto);
            ViewBills.Add(bill);            
            context.ApplyChanges();            

            return bill;
        }

        private Bill NewBill(BillDto billDto)
        {
            return billDto.AdaptToBill();
        }       
    }
}