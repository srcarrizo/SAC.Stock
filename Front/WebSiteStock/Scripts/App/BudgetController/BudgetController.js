stockApp.controller('BudgetController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment) {
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

            self.selectedProduct = null;
            self.loading = true;                                 

            self.columns = [{ field: 'BudgetDate', name: 'Fecha del Presupuesto' },
            { field: 'Name', name: 'Cliente' },                        
            { field: '', name: 'Ver', cellTemplate: '<div class="btn-container"><button class="btn btn-info" ng-click="grid.appScope.changeViews(row,  \'view\')">Ver</button><div>' }];

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
                url: 'GetBudgets',
                data: queryInfoService.ReturnInitialQueryInfo()
            }).then(function successCallback(response) {

                response.data.forEach(function (row, index) {
                    row.BudgetDate = moment(row.BudgetDate).format('DD/MM/YYYY');                                      
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
                        else {
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
                else {
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
                self.viewObject.Detail.forEach(function (detail) {
                    self.checkStock(detail);
                });
            };

            self.clearPrice = function (param) {
                if (!param.ProductPrice) {
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
                    else if ((product.Product != null) && (product.Product != "Seleccione")) {
                        total += (product.Product.CurrentProductPrice.BuyMayorPrice * product.Amount);
                    }
                }

                return total;
            };

            self.saveNew = function () {
                var invalid = false;
                if (self.newBudget.$invalid) {
                    return;
                }

                var product = "";
                for (var i = 0; i < self.viewObject.Detail.length; i++) {
                    if (!self.viewObject.Detail[i].Available) {
                        product += self.viewObject.Detail[i].Product.Name + ", ";
                        invalid = true;
                    }
                }

                if (invalid) {
                    showDialog(product);
                    return;
                }

                $http({
                    method: 'POST',
                    url: 'SaveBudget',
                    data: self.viewObject
                }).then(function successCallback(response) {
                    response.data.BudgetDate = moment(response.data.BudgetDate).format('DD/MM/YYYY');

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
                    self.$broadcast('angucomplete-alt:clearInput');
                    self.gridApi.core.refresh();
                }, function errorCallback(response) {
                    $log.info(response);
                });
            };

            createObject = function () {
                self.viewObject = {
                    BranchOffice: null,
                    BranchOfficeStaff: null,
                    BudgetDate: moment(new Date()),
                    NonCustomerName: null,
                    Name: null,
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

                getProducts();                
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

            loading = function () {
                if ((self.customersList != null) && (self.productsList != null) && (self.employeesList != null) && (self.branchOfficesList != null) && (self.billsList != null)) {
                    self.loading = false;
                    self.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                }
            };

            createObject();
            getData();
        }
    ]);