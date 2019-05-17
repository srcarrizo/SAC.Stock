namespace SAC.Stock.Front.InitializeDatabase.BaseInitializer.AreaCategorySubCategoryContainerProduct
{
    using Membership.Service.UserManagement;
    using Seed.Dependency;
    using Code;
    using Infrastructure;
    using Service.ProductContext;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal class ImportAreaCategorySubCategoryContainerProduct : ImportProcess
    {
        private AreaDto areaDto;
        private CategoryDto categoryDto;
        private SubCategoryDto subCategoryDto;
        private ContainerDto parentContainerDto;
        private ContainerDto containerDto;

        protected override void RunCommand(object[] args)
        {            
            var pathProduct = args[0].ToString();
            var productLines = File.ReadAllLines(pathProduct, new UnicodeEncoding());
            var productSvc = DiContainerFactory.DiContainer().Resolve<IProductApplicationService>();
            var userSvc = DiContainerFactory.DiContainer().Resolve<IUserManagementApplicationService>();
            ImportArea(productSvc);

            Log(string.Format("Start: {0} Products", productLines.Count()));

            foreach (var line in productLines)
            {
                var row = line.Split(',');
                if (categoryDto == null || ((categoryDto != null) && !categoryDto.Name.Equals(row[0].Trim().Replace("'", string.Empty))))
                {
                    categoryDto = new CategoryDto
                    {
                        Area = areaDto,
                        Name = row[0].Trim().Replace("'", string.Empty)
                    };

                    categoryDto = productSvc.AddCategory(categoryDto);
                    Log(string.Format("Log: Category {0} created", categoryDto.Name));
                }

                if (subCategoryDto == null || (subCategoryDto != null) && (!subCategoryDto.Name.Equals(row[1].Trim().Replace("'", string.Empty))))
                {
                    subCategoryDto = new SubCategoryDto
                    {
                        Name = row[1].Trim().Replace("'", string.Empty),
                        Category = categoryDto
                    };

                    subCategoryDto = productSvc.AddSubCategory(subCategoryDto);
                    Log(string.Format("Log: SubCategory {0} created", subCategoryDto.Name));
                }

                if (parentContainerDto == null)
                {
                    CreateContainer(Convert.ToInt32(row[3].Trim().Replace("'", string.Empty)), productSvc);
                }
                else
                {
                    var containerList = productSvc.QueryContainerByValue(Convert.ToInt32(row[3].Trim().Replace("'", string.Empty)));
                    if (containerList.Count() == 0)
                    {
                        CreateContainer(Convert.ToInt32(row[3].Trim().Replace("'", string.Empty)), productSvc);
                    } 
                    else
                    {
                        parentContainerDto = containerList.FirstOrDefault(c => c.Name.Equals("Al por mayor"));
                        if (parentContainerDto == null)
                        {
                            CreateContainer(Convert.ToInt32(row[3].Trim().Replace("'", string.Empty)), productSvc);
                        }
                    }
                }

                if (containerDto == null)
                {
                    CreateContainer(Convert.ToInt32(row[4].Trim().Replace("'", string.Empty)), productSvc, parentContainerDto);
                }
                else
                {
                    var containerList = productSvc.QueryContainerByValue(Convert.ToInt32(row[4].Trim().Replace("'", string.Empty)));
                    if (containerList.Count() == 0)
                    {
                        CreateContainer(Convert.ToInt32(row[4].Trim().Replace("'", string.Empty)), productSvc, parentContainerDto);
                    }
                    else
                    {
                        containerDto = containerList.FirstOrDefault(c => c.Name.Equals("Al por menor"));
                        if (containerDto == null)
                        {
                            CreateContainer(Convert.ToInt32(row[4].Trim().Replace("'", string.Empty)), productSvc, parentContainerDto);
                        }
                        else if(containerDto.ParentContainer.Id != parentContainerDto.Id)
                        {
                            var container = productSvc.QueryContainerByValue(Convert.ToInt32(row[4].Trim().Replace("'", string.Empty)));
                            if (!container.Any(c => c.Amount.Equals(row[4].Trim().Replace("'", string.Empty))) && !container.Any(c => c.ParentContainer.Id.Equals(parentContainerDto.Id)))
                            {
                                CreateContainer(Convert.ToInt32(row[4].Trim().Replace("'", string.Empty)), productSvc, parentContainerDto);
                            }                           
                        }
                    }
                }                

                var product = new ProductDto
                {
                    Name = row[2].Trim().Replace("'", string.Empty),                    
                    Description = row[2].Trim().Replace("'", string.Empty),
                    ForSale = true,
                    SubCategory = subCategoryDto,
                    Container = containerDto,
                    Code = row[5].Trim().Replace("'", string.Empty).Equals("Random") ? Guid.NewGuid().ToString() : row[5].Trim().Replace("'", string.Empty),
                    ProductPrices = new List<ProductPriceDto>
                    {
                        new ProductPriceDto
                        {
                            BuyMayorPrice = Convert.ToDecimal(row[6].Trim().Replace("'", string.Empty).Replace(".", ",")),
                            CreatedDate = DateTime.UtcNow,
                            MayorGainPercent = Convert.ToDecimal(row[7].Trim().Replace("'", string.Empty).Replace(".", ",")),
                            MinorGainPercent = Convert.ToDecimal(row[8].Trim().Replace("'", string.Empty).Replace(".", ",")),
                            UserId = userSvc.GetUser(UserName.StockManagement).Id                            
                        }
                    }          
                };                

                product = productSvc.AddProduct(product);
                Log(string.Format("Log: Product {0} created", product.Code));                                
                Log(string.Format("Log: Product Price {0} created", product.ProductPrices.FirstOrDefault().Id));                                
            }

            Log(string.Format("End: {0} Products", productLines.Count()));
        }

        private void CreateContainer(int amount, IProductApplicationService productSvc, ContainerDto parentContainer = null)
        {           
            if (parentContainer == null)
            {
                parentContainerDto = new ContainerDto
                {
                    Amount = amount,
                    Name = "Al por mayor",
                    Description = "Al por mayor"
                };

                parentContainerDto = productSvc.AddContainer(parentContainerDto);    
            }
            else
            {
                containerDto = new ContainerDto
                {
                    Amount = amount,
                    Name = "Al por menor",
                    Description = "Al por menor",
                    ParentContainer = parentContainer
                };

                containerDto = productSvc.AddContainer(containerDto);
            }
                        
            Log(string.Format("Log: Embase {0} created", parentContainerDto.Name));
        }

        public void ImportArea(IProductApplicationService productSvc)
        {
            Log(string.Format("Start: 1 Area"));
            areaDto = new AreaDto
            {
                Name = "Plastico, Papel y Carton"                
            };

            areaDto = productSvc.AddArea(areaDto);
            Log(string.Format("End: 1 Area"));
        }        
    }
}