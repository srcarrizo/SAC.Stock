stockApp.controller('TransactionController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment, $mdDialog) {
            var self = $scope;

            self.transaction = null;            
            self.viewTransaction = null; 
            self.billsList = null;
            self.error = null;
            self.errorDetail = null;
            self.loading = true;

            self.listView = true;            
            self.resultView = false;
            self.newView = false;

            self.TotalIn = 0;
            self.TotalOut = 0;

            var statusTemplate = '<div>{{COL_FIELD ? "Ingreso" : "Gasto"}}</div>';
            var statusTemplate2 = '<div>{{grid.appScope.getRowTotal(row.entity)}}</div>';

            self.columns = [
                { field: 'Name', name: 'Nombre' },
                { field: 'TransactionDate', name: 'Fecha' },
                { field: 'TransactionTypeInOut', name: 'Tipo', cellTemplate: statusTemplate },
                { field: 'BranchOffice.Name', name: 'Sucursal' },
                { field: '', name: 'Total', cellTemplate: statusTemplate2 },
            ];

            self.gridOptions = {
                enableSorting: true,
                columnDefs: self.columns,
                enablePaginationControls: false,
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            };

            self.gridOptionsHistorial = {
                enableSorting: true,
                columnDefs: $scope.columns,
                enablePaginationControls: false,
                paginationPageSizes: [25, 50, 75],
                paginationPageSize: 10,
                onRegisterApi: function (gridApi) {
                    self.gridApi2 = gridApi;
                }
            };

            self.getRowTotal = function (row) {
                var total = 0;
                if (row == null) {
                    return 0;
                }

                for (var i = 0; i < row.Detail.length; i++) {
                    var money = row.Detail[i].Amount * row.Detail[i].Bill.Value;
                    total += row.Detail[i].Bill.BillUnitType.IsDecimal ? (money / 100) : money;
                }
                
                return total;
            };

            self.getTotal = function () {
                var total = 0;
                if (self.transaction == null)
                {
                    return total;
                }
                
                for (var i = 0; i < self.transaction.Detail.length; i++) {
                    var detail = self.transaction.Detail[i];
                    if (detail.Bill == null)
                    {
                        return;
                    }

                    if (detail.Bill.BillUnitType.IsDecimal) {
                        total += (detail.Bill.Value * detail.Amount) / 100;
                    }
                    else {
                        total += (detail.Bill.Value * detail.Amount);
                    }
                }

                return total;
            };
      
            $http({
                method: 'POST',
                url: 'GetTransaction'
            }).then(function successCallback(response) {
                
                response.data.Transactions.forEach(function (row, index) {
                    row.TransactionDate = moment(row.TransactionDate).format('DD/MM/YYYY');
                    if (row.DeactivatedDate != null) {
                        row.DeactivatedDate = moment(row.DeactivatedDate).format('DD/MM/YYYY');
                    }                    
                });

                self.TotalIn = response.data.TotalIn;
                self.TotalOut = response.data.TotalOut;

                self.gridOptions.data = response.data.Transactions;               
            }, function errorCallback(response) {
                $log.info(response);
            });   

            $http({
                method: 'POST',
                url: 'GetTransactionHistory'
            }).then(function successCallback(response) {

                response.data.forEach(function (row, index) {
                    row.TransactionDate = moment(row.TransactionDate).format('DD/MM/YYYY');
                    if (row.DeactivatedDate != null) {
                        row.DeactivatedDate = moment(row.DeactivatedDate).format('DD/MM/YYYY');
                    }
                });                

                self.gridOptionsHistorial.data = response.data;
            }, function errorCallback(response) {
                $log.info(response);
            });  

            self.addDetail = function () {
                self.transaction.Detail.push(createDetailObject());
            };

            self.removeDetail = function (index) {
                self.transaction.Detail.splice(index, 1);
            };

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

                        self.transaction = param.entity;                        
                    }                   
                }
                else {                    
                    self.viewView = false;
                    self.resultView = false;
                }

                if (param == 'new') {
                    createObject();
                }
            };

            self.saveNew = function () {  
                if (self.transaction.Name == null)
                {
                    return;
                }

                if (self.transaction.BranchOffice == null)
                {
                    return;
                }

                if (self.transaction.BranchOfficeStaff == null)
                {
                    return;
                }

                if (self.transaction.Detail.length > 0)
                {
                    for (var i = 0; i < self.transaction.Detail.length; i++) {
                        if (self.transaction.Detail[i].Bill == null)
                        {
                            return;
                        }

                        if (self.transaction.Detail[i].Amount == null)
                        {
                            return;
                        }
                    }
                }

                if (self.transaction.$invalid) {
                    return;
                }                

                $http({
                    method: 'POST',
                    url: 'SaveTransaction',
                    data: self.transaction
                }).then(function successCallback(response) {
                    response.data.Transaction.TransactionDate = moment(response.data.TransactionDate).format('DD/MM/YYYY');
                    if (response.data.Transaction.DeactivatedDate != null) {
                        response.data.Transaction.DeactivatedDate = moment(response.data.DeactivatedDate).format('DD/MM/YYYY');
                    }

                    self.gridOptions.data.push(response.data.Transaction);

                    self.TotalIn += response.data.TotalIn;
                    self.TotalOut += response.data.TotalOut;
                    
                    var objectResult = { data: null, entity: response.data.Transaction };
                    self.changeViews(objectResult, 'result');
                    self.$broadcast('angucomplete-alt:clearInput');
                }, function errorCallback(response) {
                    $log.info(response);
                });
            };

            createObject = function () {
                self.transaction = {
                    BranchOffice: null,
                    BranchOfficeStaff: null,
                    TransactionDate: moment(new Date()),
                    Id: null,  
                    TransactionTypeInOut: false,
                    Detail: []
                };

                self.transaction.Detail.push(createDetailObject());
            };    

            createDetailObject = function () {
               
                var itemDetail = {
                    Amount: null,
                    Bill: null
                };

                return itemDetail;
            };

            getData = function () {
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
            };

            loading = function () {
                if ((self.employeesList != null) && (self.branchOfficesList != null) && (self.billsList != null)) {
                    self.loading = false;
                }
            };

            getData();
        }
    ]);