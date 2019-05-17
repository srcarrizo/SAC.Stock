stockApp.controller('ProductController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, $mdDialog) {
            var self = $scope;

            self.subCategoryList == null;
            self.contianerList == null;
            self.error = null;
            self.productMSG = { Text: null, Operation: false };

            self.loading = true;

            self.productPrice =
                {
                    Id: 0,
                    BuyMayorPrice: 0,
                    MayorGainPercent: 0,
                    MinorGainPercent: 0,
                    CreatedDate: null,
                    DisabledDate: null,
                    DisableNote: null
                };

            self.product =
                {
                    Id: 0,
                    Code: null,
                    Name: null,
                    Description: null,
                    DisabledDate: null,
                    DisableNote: null,
                    ForSale: true,
                    SubCategory: null,
                    Container: null,
                    CurrentProductPrice: self.productPrice
                };

            var cellTemplateEditShowConditional = '<div class="btn-container"><button class="btn btn-warning" ng-show="COL_FIELD != null" ng-click="grid.appScope.editProduct(row,  \'edit\')">Editar</button><div>';            
            var cellTemplateEdit = '<div class="btn-container"><button class="btn btn-warning" ng-click="grid.appScope.editProduct(row,  \'edit\')">Editar</button><div>';            

            self.columnsGrouped = [{ field: 'SubCategory.Category.Name', name: 'Categoria', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' } },
            { field: 'SubCategory.Name', name: 'SubCategoria', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' } },
            { field: 'Code', name: 'Código' },
            { field: 'Name', name: 'Nombre' },
            { field: 'Container.ParentContainer.Amount', name: 'Embase mayorista' },
            { field: 'Container.Amount', name: 'Embase minorista' },
            { field: 'CurrentProductPrice.BuyMayorPrice', name: 'Precio Compra mayorista' },
                { field: 'Id', name: 'Editar', cellTemplate: cellTemplateEditShowConditional }];

            self.columns = [{ field: 'SubCategory.Category.Name', name: 'Categoria' },
            { field: 'SubCategory.Name', name: 'SubCategoria' },
            { field: 'Code', name: 'Código' },
            { field: 'Name', name: 'Nombre' },
            { field: 'Container.ParentContainer.Amount', name: 'Embase mayorista' },
            { field: 'Container.Amount', name: 'Embase minorista' },
            { field: 'CurrentProductPrice.BuyMayorPrice', name: 'Precio Compra mayorista' },
            { field: '', name: 'Editar', cellTemplate: cellTemplateEdit }];

            self.gridOptions = {
                enableSorting: true,
                columnDefs: $scope.columns,
                enablePaginationControls: false,
                paginationPageSizes: [25, 50, 75],
                paginationPageSize: 10,
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            };

            self.gridOptionsGrouped = {
                enableSorting: true,
                columnDefs: $scope.columnsGrouped,
                enablePaginationControls: false,
                paginationPageSize: 10,
                onRegisterApi: function (gridApi) {
                    self.gridApi2 = gridApi;
                }
            };

            $http({
                method: 'POST',
                url: 'GetProducts',
                data: queryInfoService.ReturnInitialQueryInfo()
            }).then(function successCallback(response) {
                $log.info(response);
                $scope.gridOptionsGrouped.data = response.data;
                $scope.gridOptions.data = response.data;
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

                $scope.confirmProduct = function (confirmProduct) {
                    var isvalid = true;
                    $scope.error = null;

                    if ($scope.product.Code == null) {
                        $scope.error = "Debe incluir el código.";
                        return;
                    }

                    if ($scope.product.Name == null) {
                        $scope.error = "Debe incluir el nombre.";
                        return;
                    }

                    if ($scope.product.Container == null) {
                        $scope.error = "Debe elegir un embase.";
                        return;
                    }

                    if ($scope.product.SubCategory == null) {
                        $scope.error = "Debe elegir una subcategoría.";
                        return;
                    }

                    if ($scope.product.CurrentProductPrice.BuyMayorPrice == 0) {
                        $scope.error = "Debe incluir el precio de compra mayorista.";
                        return;
                    }

                    if ($scope.product.CurrentProductPrice.MayorGainPercent == 0) {
                        $scope.error = "Debe incluir el porcentaje de ganacia mayorista.";
                        return;
                    }

                    if ($scope.product.CurrentProductPrice.MinorGainPercent == 0) {
                        $scope.error = "Debe incluir el porcentaje de ganacia minorista.";
                        return;
                    }

                    $mdDialog.hide(confirmProduct);
                };
            };

            DialogController.$inject = ['$scope', '$mdDialog'];

            self.editProduct = function (ev) {
                if (ev == 'newProduct') {
                    $scope.product = { Id: 0, Name: null };
                } else {
                    $scope.product = angular.copy(ev.entity);
                }

                $mdDialog.show({
                    controller: DialogController,
                    scope: $scope.$new(),
                    templateUrl: 'editProduct',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmProduct) {

                    if ($scope.product.$invalid) {
                        return;
                    }

                    $scope.product.Container.ParentContainer = null;
                    $scope.product.Container.Products = null;                    
                    $scope.product.SubCategory.Products = null;

                    $http({
                        method: 'POST',
                        url: 'SaveProduct',
                        data: $scope.product
                    }).then(function successCallback(response) {

                        if (self.product.Id == 0) {
                            self.gridOptions.data.push(response.data);
                            self.gridOptionsGrouped.data.push(response.data);
                            self.productMSG.Text = "El producto ha sido dado de alta.";
                        } else {
                            self.gridOptions.data.forEach(function (row, index) {
                                if (response.data.Id == row.Id) {
                                    self.gridOptions.data.splice(index, 1);
                                }
                            });

                            self.gridOptionsGrouped.data.forEach(function (row, index) {
                                if (response.data.Id == row.Id) {
                                    self.gridOptionsGrouped.data.splice(index, 1);
                                }
                            });

                            self.gridOptions.data.push(response.data);
                            self.gridOptionsGrouped.data.push(response.data);
                            self.productMSG.Text = "El producto ha sido modificado.";
                        }

                        self.productMSG.Operation = true;
                    }, function errorCallback(response) {
                        self.productMSG.Text = "Ha ocurrido un error.";
                        self.productMSG.Operation = false;
                        $log.info(response);
                    });
                }, function () {

                    //alert('You cancelled the dialog.');
                });
            };

            getData = function () {
                if (self.contianerList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetContainers',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.contianerList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

                if (self.subCategoryList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetSubCategories',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.subCategoryList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }
            }

            loading = function () {
                if ((self.subCategoryList != null) && (self.contianerList != null)) {
                    self.loading = false;
                }
            };

            getData();
        }
    ]
);