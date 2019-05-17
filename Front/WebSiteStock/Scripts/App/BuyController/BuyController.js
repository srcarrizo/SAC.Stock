stockApp.controller('BuyController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment) {
            var self = $scope;
            
            self.listView = true;
            self.editView = false;
            self.resultView = false;
            self.newView = false;
            self.viewView = false;
            self.viewObject = null;

            self.providersList = null;
            self.productsList = null;
            self.employeesList = null;
            self.branchOfficesList = null;
            self.billsList = null;

            self.selectedProduct = null;
            self.loading = true;

            var cellTemplate = '<div class="btn-container"><button class="btn btn-warning" ng-show="COL_FIELD == null" ng-click="grid.appScope.changeViews(row,  \'edit\')">Editar</button><div>';

            self.columns = [{ field: 'BuyDate', name: 'Fecha de Compra' },
                { field: 'DeactivatedDate', name: 'Fecha de desactivacion' },
                { field: 'Provider.Name', name: 'Proveedor' },
                { field: '', name: 'Ver', cellTemplate: '<div class="btn-container"><button class="btn btn-info" ng-click="grid.appScope.changeViews(row,  \'view\')">Ver</button></div>' },
                { field: 'StockId', name: 'Editar', cellTemplate: cellTemplate }
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
                url: 'GetBuys',
                data: queryInfoService.ReturnInitialQueryInfo()
            }).then(function successCallback(response) {

                response.data.forEach(function (row) {
                    row.BuyDate = moment(row.BuyDate).format('DD/MM/YYYY');
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

                        self.viewObject = param.entity;
                        setDetail(param.entity.Detail);
                    }
                    else if (operation == 'edit') {
                        self.newView = true;
                        self.viewObject = param.entity;                                         
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

            self.removeProduct = function () {
                self.viewObject.Detail.pop();
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
                if (self.newBuy.$invalid)
                {
                    return;
                }

                $http({
                    method: 'POST',
                    url: 'SaveBuy',
                    data: self.viewObject 
                }).then(function successCallback(response) {
                    self.gridOptions.data.push(response.data);

                    response.data.BuyDate = moment(response.data.BuyDate).format('DD/MM/YYYY');
                    if (response.data.DeactivatedDate != null) {
                        response.data.DeactivatedDate = moment(response.data.DeactivatedDate).format('DD/MM/YYYY');
                    }

                    response.data.Detail.forEach(function (row2) {
                        row2.Product.CurrentProductPrice.CreatedDate = moment(row2.Product.CurrentProductPrice.CreatedDate).format('DD/MM/YYYY');
                        if (row2.Product.CurrentProductPrice.DeactivatedDate != null) {
                            row2.Product.CurrentProductPrice.DeactivatedDate = moment(row2.Product.CurrentProductPrice.DeactivatedDate).format('DD/MM/YYYY');
                        }
                    });

                    var objectResult = { data: null, entity: response.data };
                    self.changeViews(objectResult, 'result');                    
                }, function errorCallback(response) {
                    $log.info(response);
                });
            };

            self.optionSelected = function (data) {
                $scope.$apply(function () {
                    $scope.main_repo = data;
                });
            };

            createObject = function () {                
                self.viewObject = {
                    BranchOffice: null,
                    BranchOfficeStaff: null,
                    BuyDate:  moment(new Date()),
                    Id: null,
                    PaymentTypeCode: null,
                    Provider: null,
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
                    ProductPrice: false
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
                        url: 'GetProviders',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.providersList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

                if (self.productsList == null) {
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
            }; 

            loading = function () {
                if ((self.providersList != null) && (self.productsList != null) && (self.employeesList != null) && (self.branchOfficesList != null) && (self.billsList != null))
                {
                    self.loading = false;
                }
            };

            createObject();
            getData();
        }
    ]);