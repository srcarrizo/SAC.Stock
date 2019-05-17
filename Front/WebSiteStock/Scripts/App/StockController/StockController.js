stockApp.controller('StockController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment, $mdDialog) {
            var self = $scope;
            self.stock = null;                        
            self.stockCreated = false;


            self.columns = [                
                { field: 'Product.Name', name: 'Producto' },
                { field: 'Amount', name: 'Cantidad' },
                { field: 'Product.Container.Amount', name: 'Embase Minorista' }                
            ];        

            self.gridOptions = {
                enableSorting: true,
                columnDefs: self.columns,
                enablePaginationControls: false,
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            };                       

            $http({
                method: 'POST',
                url: 'GetLatestStock'
            }).then(function successCallback(response) {                
                self.stockCreated = false;               
                self.stock = response.data;                
                self.stock.StockDate = moment(response.data.StockDate).format('DD/MM/YYYY');
                self.stock.DeactivatedDate = self.stock.DeactivatedDate != null ? moment(response.data.DeactivatedDate).format('DD/MM/YYYY') : null;              
                self.gridOptions.data = response.data.Detail;
            }, function errorCallback(response) {
                $log.info(response);
            });            

            self.processStock = function (ev) {
                $http({
                    method: 'POST',
                    url: 'SaveStock'
                }).then(function successCallback(response) {
                    self.stockCreated = true;
                    self.stock = response.data;
                    self.stock.StockDate = moment(response.data.StockDate).format('DD/MM/YYYY');
                    self.stock.DeactivatedDate = self.stock.DeactivatedDate != null ? moment(response.data.DeactivatedDate).format('DD/MM/YYYY') : null;

                    self.gridOptions.data = response.data.Detail;
                }, function errorCallback(response) {
                    $log.info(response);
                });              
            };
        }
    ]);