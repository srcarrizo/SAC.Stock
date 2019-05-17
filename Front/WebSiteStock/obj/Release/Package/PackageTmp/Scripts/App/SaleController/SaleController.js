stockApp.controller('SaleController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment, $mdDialog) {
            var self = $scope;

            self.listView = true;
            self.editView = false;
            self.resultView = false;
            self.newView = false;
            self.viewView = false;
            self.viewObject = null;

            self.customersList = null;
            self.productsList = null;
            self.employeesList = null;
            self.branchOfficesList = null;
            self.billsList = null;
            self.stock = null;
            self.OpenedBox = false;

            self.selectedProduct = null;
            self.loading = true;

            var cellTemplate = '<div class="btn-container"><button class="btn btn-warning" ng-show="COL_FIELD == null" ng-click="grid.appScope.changeViews(row,  \'edit\')">Editar</button><div>';
            var statusTemplate = '<div>{{COL_FIELD ? "Reserva" : "Venta Directa"}}</div>';
            var statusTemplateMinor = '<div>{{COL_FIELD ? "Minorista" : "Mayorista"}}</div>';
            var statusFree = '<div class="btn-container"><button class="btn btn-danger" ng-show="COL_FIELD" ng-click="grid.appScope.freePreSale(row)">Liberar Reserva</button><div>';
            var statusComplete = '<div class="btn-container"><button class="btn btn-success" ng-show="COL_FIELD" ng-click="grid.appScope.completeSale(row)">Completar Venta</button><div>';

            self.columns = [{ field: 'SaleDate', name: 'Fecha de Venta' },                
                { field: 'Customer.Name', name: 'Cliente' },
                { field: 'MayorMinorSale', name: 'Mayorista / Minorista', cellTemplate: statusTemplateMinor },
                { field: 'PreSale', name: 'Tipo', cellTemplate: statusTemplate },
                { field: '', name: 'Ver', cellTemplate: '<div class="btn-container"><button class="btn btn-info" ng-click="grid.appScope.changeViews(row,  \'view\')">Ver</button><div>' },
                { field: 'StockId', name: 'Editar', cellTemplate: cellTemplate },                
                { field: 'PreSale', name: 'Completar Venta', cellTemplate: statusComplete },
                { field: 'PreSale', name: 'Liberar Reserva', cellTemplate: statusFree }
            ];

            self.gridOptions = {
                enableSorting: true,
                columnDefs: self.columns,
                enablePaginationControls: false,
                paginationPageSizes: [25, 50, 75],
                paginationPageSize: 15,
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            };
            
            self.columnsDetail = [{ field: 'Product.SubCategory.Name', name: 'SubCategory' },
                { field: 'Product.Code', name: 'Código de producto' },
                { field: 'Product.Name', name: 'Producto' },
                { field: 'Product.Container.ParentContainer.Amount', name: 'Embase Mayorista' },
                { field: 'Product.Container.Amount', name: 'Embase Minorista' },
                { field: 'Amount', name: 'Cantidad' },
                { field: 'Product.CurrentProductPrice.BuyMayorPrice', name: 'Precio Producto' },
                { field: 'Price', name: 'Precio Editado' }
            ];

            self.gridOptionsDetail = {
                enableSorting: true,
                columnDefs: self.columnsDetail,
                enablePaginationControls: false,
                onRegisterApi: function (gridApi) {
                    self.gridApiDetail = gridApi;
                }
            };

            $http({
                method: 'POST',
                url: 'GetSales',
                data: queryInfoService.ReturnInitialQueryInfo()
            }).then(function successCallback(response) {

                response.data.forEach(function (row) {
                    row.SaleDate = moment(row.SaleDate).format('DD/MM/YYYY');
                    if (row.DeactivatedDate != null) {
                        row.DeactivatedDate = moment(row.DeactivatedDate).format('DD/MM/YYYY');
                    }                    

                    row.Detail.forEach(function (row2) {
                        row2.Product.CurrentProductPrice.CreatedDate = moment(row2.Product.CurrentProductPrice.CreatedDate).format('DD/MM/YYYY');
                        if (row2.Product.CurrentProductPrice.DeactivatedDate != null) {
                            row2.Product.CurrentProductPrice.DeactivatedDate = moment(row2.Product.CurrentProductPrice.DeactivatedDate).format('DD/MM/YYYY');
                        }                    
                    });
                });

                self.gridOptions.data = response.data;

            }, function errorCallback(response) {
                $log.info(response);
            });

            self.freePreSale = function (param) {
                $http({
                    method: 'POST',
                    url: 'DeletePreSale',
                    data: param.entity,                    
                }).then(function successCallback(response) {

                    self.gridOptions.data.forEach(function (row, index) {
                        if (param.entity.Id == row.Id)
                        {
                            self.gridOptions.data.splice(index, 1);
                        }
                    });  

                    getProducts();
                    checkStock();

                    $scope.gridApi.core.refresh();
                }, function errorCallback(response) {
                    $log.info(response);
                });
            };

            self.completeSale = function (param) {
                $http({
                    method: 'POST',
                    url: 'CompleteSale',
                    data: param.entity,
                }).then(function successCallback(response) {

                    response.data.SaleDate = moment(response.data.SaleDate).format('DD/MM/YYYY');
                    if (response.data.DeactivatedDate != null) {
                        response.data.DeactivatedDate = moment(response.data.DeactivatedDate).format('DD/MM/YYYY');
                    }                  

                    self.gridOptions.data.forEach(function (row, index) {
                        if (response.data.Id == row.Id) {
                            self.gridOptions.data.splice(index, 1);
                        }
                    });

                    self.gridOptions.data.push(response.data);
                    $scope.gridApi.core.refresh();
                }, function errorCallback(response) {
                    $log.info(response);
                });
            };

            self.changeViews = function (param, operation) {
                self.listView = param == 'list';
                self.newView = param == 'new';                                

                if (typeof (param) == 'object') {
                    if (operation == 'view' || operation == 'result') {                        
                        if (operation == 'view') {
                            self.viewView = true;
                        }
                        else
                        {
                            self.resultView = true;
                        }

                        self.viewObject = angular.copy(param.entity);                        
                        setDetail(param.entity.Detail);
                    }
                    else if (operation == 'edit') {
                        self.newView = true;
                        self.viewObject = angular.copy(param.entity);                            
                    }
                }
                else
                {
                    self.editView = false;
                    self.viewView = false;                    
                    self.resultView = false;   
                }

                if (param == 'new') {  
                    createObject();                    
                }
            };

            self.addProduct = function () {
                self.viewObject.Detail.push(createDetailObject());
            };

            self.removeProduct = function (param) {                
                self.viewObject.Detail.pop();
                self.viewObject.Detail.forEach(function (detail)
                {
                    self.checkStock(detail); 
                });
            };      

            self.clearPrice = function (param) {
                if (!param.ProductPrice)
                {
                    param.Price = null;
                }
            };

            self.getTotal = function () {
                var total = 0;
                for (var i = 0; i < self.viewObject.Detail.length; i++) {
                    var product = self.viewObject.Detail[i];

                    if (product.Price != null) {
                        total += (product.Price * product.Amount);
                    }
                    else if ((product.Product != null) && (product.Product != "Seleccione"))
                    {
                        total += (product.Product.CurrentProductPrice.BuyMayorPrice * product.Amount);
                    }                    
                }

                return total;
            };

            self.saveNew = function () {
                var invalid = false;
                if (self.newSale.$invalid)
                {
                    return;
                }

                var product = "";
                for (var i = 0; i < self.viewObject.Detail.length; i++) {
                    if (!self.viewObject.Detail[i].Available) {
                        product += self.viewObject.Detail[i].Product.Name + ", ";                        
                        invalid = true;                        
                    }
                }                

                if (invalid)
                {
                    showDialog(product);
                    return;
                }

                $http({
                    method: 'POST',
                    url: 'SaveSale',
                    data: self.viewObject 
                }).then(function successCallback(response) {

                    response.data.SaleDate = moment(response.data.SaleDate).format('DD/MM/YYYY');
                    if (response.data.DeactivatedDate != null) {
                        response.data.DeactivatedDate = moment(response.data.DeactivatedDate).format('DD/MM/YYYY');
                    }

                    if (self.viewObject.Id == null) {
                        self.gridOptions.data.push(response.data);
                    } else {
                        self.gridOptions.data.forEach(function (row, index) {
                            if (response.data.Id == row.Id) {
                                self.gridOptions.data.splice(index, 1);
                            }
                        });  

                        self.gridOptions.data.push(response.data);
                    }
                                                                             
                    var objectResult = { data: null, entity: response.data };
                    self.changeViews(objectResult, 'result');
                    self.gridApi.core.refresh();
                }, function errorCallback(response) {
                    $log.info(response);
                });
            };

            self.checkStock = function (item) {
                if (item.Product != "Seleccione" && item.Amount != null)
                {
                    $http({
                        method: 'GET',
                        url: 'CheckProductStock',
                        params: { productId: item.Product.Id }
                    }).then(function successCallback(response) { 
                        var amount = 0;
                        self.viewObject.Detail.forEach(function (row) {
                            if (row.Product.Id == item.Product.Id)
                            {
                                amount += row.Amount;
                            }
                        });

                        if ((response.data - amount) < 0) {
                            showDialog(item.Product.Name);
                            self.viewObject.Detail.forEach(function (row) {
                                if (row.Product.Id == item.Product.Id) {
                                    row.Available = false;
                                }
                            });
                        }
                        else
                        {
                            self.viewObject.Detail.forEach(function (row) {
                                if (row.Product.Id == item.Product.Id) {
                                    row.Available = true;
                                }
                            });
                        }                 
                    }, function errorCallback(response) {

                        $log.info(response);
                    });
                }                
            };

            self.optionSelected = function(data) {
                $scope.$apply(function() {
                    $scope.main_repo = data;
                });
            };

            showDialog = function (param) {              
                $mdDialog.show(
                    $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(false)
                        .title('Stock del producto')
                        .textContent('No existe Stock del producto ' + param + '. Por favor, realize una compra.')
                        .ariaLabel('Alerta de Stock')
                        .ok('Aceptar')
                        .targetEvent()
                );
            };            

            createObject = function () {                
                self.viewObject = {
                    BranchOffice: null,
                    BranchOfficeStaff: null,
                    SaleDate:  moment(new Date()),
                    Id: null,
                    PaymentTypeCode: null,
                    Customer: null,
                    Detail: []                    
                };

                self.viewObject.Detail.push(createDetailObject());
            };            
         
            createDetailObject = function () {
                var itemDetail = {
                    Product: null,
                    Amount: null,
                    Price: null,
                    SubTotal: null,
                    EditedSubTotal: null,
                    ProductPrice: false,
                    Available: true
                };

                return itemDetail;
            };

            setDetail = function (detail) {
                self.gridOptionsDetail.data = detail;
            };

            getData = function () {
                if (self.providersList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetCustomers',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.customersList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }
                             
                if (self.employeesList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetEmployees',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.employeesList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

                if (self.branchOfficesList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetBranchOffices',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.branchOfficesList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

                if (self.billsList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetBills'
                    }).then(function successCallback(response) {
                        self.billsList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }                 
                
                $http({
                    method: 'POST',
                    url: 'CheckOpenBox'
                }).then(function successCallback(response) {
                    self.OpenedBox = response.data;                        
                }, function errorCallback(response) {
                    $log.info(response);
                    });  

                getProducts();
                checkStock();
            }; 

            function getProducts() {                
                $http({
                    method: 'POST',
                    url: 'GetProducts',
                    data: queryInfoService.ReturnInitialQueryInfo()
                }).then(function successCallback(response) {
                    self.productsList = response.data;
                    loading();
                }, function errorCallback(response) {

                    $log.info(response);
                });                
            };
           
            function checkStock() {                
                $http({
                    method: 'POST',
                    url: 'GetStock'
                }).then(function successCallback(response) {
                    self.stock = response.data;
                    loading();
                }, function errorCallback(response) {
                    $log.info(response);
                });                
            };

            loading = function () {
                if ((self.customersList != null) && (self.productsList != null) && (self.employeesList != null) && (self.branchOfficesList != null) && (self.billsList != null) && (self.stock != null))
                {
                    self.loading = false;
                    self.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                }
            };

            createObject();
            getData();
        }
    ]);