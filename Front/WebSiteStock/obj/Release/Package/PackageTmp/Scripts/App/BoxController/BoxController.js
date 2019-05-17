stockApp.controller('BoxController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment, $mdDialog) {
            var self = $scope;

            self.box = null;
            self.boxCompleted = null;
            self.editBox = null;
            self.billsList = null;
            self.error = null;
            self.errorDetail = null;

            self.openingClosing = false;
            self.customFullscreen = false;
            self.loading = true;

            var statusTemplate = '<div>{{COL_FIELD ? "Centavo(s)" : "Peso(s)"}}</div>';            
            var statusTemplate2 = '<div class="ui-grid-cell-contents">{{grid.appScope.calcTotalRow(row.entity)}}</div>';

            self.columns = [
                { field: 'Bill.BillUnitType.Name', name: 'Tipo' },
                { field: 'Bill.Value', name: 'Valor' },
                { field: 'Bill.BillUnitType.IsDecimal', name: 'Centavo', cellTemplate: statusTemplate },                   
                { field: 'Amount', name: 'Cantidad' },
                { field: '', name: 'SubTotal', cellTemplate: statusTemplate2 }
            ];            

            self.gridOptions = {
                enableSorting: true,
                columnDefs: self.columns,
                enablePaginationControls: false,                              
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            };

            $scope.calcTotalRow = function (row) {
                if (row == null) {
                    return 0;
                }

                if (row.Bill == null) {
                    return 0;
                }

                if (row.Bill.BillUnitType == null) {
                    return 0;
                }

                var money = row.Amount * row.Bill.Value;
                var total = row.Bill.BillUnitType.IsDecimal ? (money / 100) : money;
                return total;
            };

            self.changeBoxState = function () {                
                if (self.openingClosing) {
                    self.openingClosing = false;
                    return;
                }

                if (!self.openingClosing) {
                    self.openingClosing = true;
                    return;
                }
            };

            $scope.calcTotal = function (box) {
                if (box == null) {
                    return 0;
                }

                var total = 0;
                for (var i = 0; i < box.Detail.length; i++) {
                    if (box.Detail[i].Bill == null)
                    {
                        total += 0;
                    }

                    if (box.Detail[i].Bill.BillUnitType == null)
                    {
                        total += 0;
                    }

                    var money = box.Detail[i].Amount * box.Detail[i].Bill.Value;
                    total += box.Detail[i].Bill.BillUnitType.IsDecimal ? (money / 100) : money;
                }

                return total;
            };

            $http({
                method: 'POST',
                url: 'GetBox'                
            }).then(function successCallback(response) {                     
                self.openingClosing = response.data.Box.OpeningClosing;
                self.boxCompleted = response.data;

                self.box = response.data.Box;
                self.box.OpenDate = response.data.Box.OpenDate != null ? moment(response.data.Box.OpenDate).format('DD/MM/YYYY') : moment(new Date()).format('DD/MM/YYYY');
                self.box.CloseDate = response.data.Box.CloseDate != null ? moment(response.data.Box.CloseDate).format('DD/MM/YYYY') : moment(new Date()).format('DD/MM/YYYY');
                self.box.DeactivateDate = response.data.Box.DeactivateDate != null ? moment(response.data.Box.DeactivateDate).format('DD/MM/YYYY') : moment(new Date()).format('DD/MM/YYYY');                

                self.gridOptions.data = response.data.Box.Detail;
            }, function errorCallback(response) {
                $log.info(response);
            }); 
        
            function DialogController($scope, $mdDialog) {

                $scope.hide = function () {                   
                    $mdDialog.hide();                   
                };

                $scope.Cancel = function () {
                    $mdDialog.cancel();
                };

                $scope.confirmBox = function (confirmBox) {
                    var isvalid = true;
                    $scope.error = null;
                    $scope.errorDetail = null;

                    if ($scope.editBox.OpenNote == null && (!$scope.openingClosing))
                    {
                        $scope.error = "Debe incluir la nota de apertura.";
                        return;
                    }

                    if ($scope.editBox.CloseNote == null && ($scope.openingClosing)) {
                        $scope.error = "Debe incluir la nota de cierre.";
                        return;
                    }

                    if ($scope.editBox.Detail.length == 0) {
                        $scope.error = "Debe incluir al menos un detalle.";
                        return;
                    }
                                        
                    for (var i = 0; i < $scope.editBox.Detail.length; i++) {
                        if ($scope.editBox.Detail[i].Amount == null || $scope.editBox.Detail[i].Bill == "Seleccione")
                        {
                            isvalid = false;
                        }
                    }

                    if (!isvalid)
                    {
                        $scope.errorDetail = "Debe completar los datos en cada detalle: Seleccione item y cantidad.";
                        return;
                    }

                    $mdDialog.hide(confirmBox);
                };
            };

            DialogController.$inject = ['$scope', '$mdDialog'];

            self.processBox = function (ev) {  
                self.editBox = angular.copy(self.box);

                $mdDialog.show({                    
                    controller: DialogController,
                    scope: $scope.$new(),                  
                    templateUrl: 'dialogOpenClose',                                    
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,                    
                    bindToController: true                
                }).then(function (confirmBox) {
                    if (self.editBox.$invalid) {
                        return;
                    }

                    if (self.openingClosing) {
                        self.editBox.OpenDate = null;
                        self.editBox.CloseDate = moment(new Date());
                    } else {
                        self.editBox.OpenDate = moment(new Date());
                        self.editBox.CloseDate = null;
                    }

                    $http({
                        method: 'POST',
                        url: 'SaveBox',
                        data: self.editBox
                    }).then(function successCallback(response) {                        
                        self.boxCompleted = response.data;

                        self.box = response.data.Box;
                        self.box.OpenDate = response.data.Box.OpenDate != null ? moment(response.data.Box.OpenDate).format('DD/MM/YYYY'): moment(new Date()).format('DD/MM/YYYY');
                        self.box.CloseDate = response.data.Box.CloseDate != null ? moment(response.data.Box.CloseDate).format('DD/MM/YYYY') : moment(new Date()).format('DD/MM/YYYY');
                        self.box.DeactivateDate = response.data.Box.DeactivateDate != null ? moment(response.data.Box.DeactivateDate).format('DD/MM/YYYY') : moment(new Date()).format('DD/MM/YYYY');

                        self.gridOptions.data = response.data.Box.Detail;

                        self.changeBoxState();
                        }, function errorCallback(response) {
                        $log.info(response);
                    });                    
                }, function () {                        

                        //alert('You cancelled the dialog.');
                });
            }; 

            self.addDetail = function () {
                self.editBox.Detail.push(createDetailObject());
            };

            self.removeDetail = function (index) {                
                self.editBox.Detail.splice(index, 1);
            };   

            self.getTotal = function () {
                var total = 0;
                for (var i = 0; i < self.editBox.Detail.length; i++) {
                    var detail = self.editBox.Detail[i];

                    if (detail.Bill.BillUnitType.IsDecimal) {
                        total += (detail.Bill.Value * detail.Amount) / 100;
                    }
                    else
                    {
                        total += (detail.Bill.Value * detail.Amount);
                    }
                }

                return total;
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
            };

            loading = function () {
                if (self.billsList != null) {
                    self.loading = false;
                }
            };

            getData();
        }
    ]);