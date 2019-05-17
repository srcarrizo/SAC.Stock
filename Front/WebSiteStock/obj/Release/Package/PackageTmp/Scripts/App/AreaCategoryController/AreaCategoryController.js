stockApp.controller('AreaCategoryController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment, $mdDialog) {
            var self = $scope;

            self.area = null;           
            self.areaList = null;   

            self.category = null;
            self.subCategory = null;

            self.categoryList = null;
            self.subCategoryList = null;

            self.error = null;
            self.areaMSG = { Text: null, Operation: false };
            self.categoryMSG = { Text: null, Operation: false };
            self.subCategoryMSG = { Text: null, Operation: false };

            self.loading = true;

            var cellTemplateEdit = '<div class="btn-container"><button class="btn btn-warning" ng-click="grid.appScope.editArea(row,  \'edit\')">Editar</button><div>';                                    
            var cellTemplateCategoryEdit = '<div class="btn-container"><button class="btn btn-warning" ng-click="grid.appScope.editCategory(row,  \'edit\')">Editar</button><div>';            
            var cellTemplateSubCategoryEdit = '<div class="btn-container"><button class="btn btn-warning" ng-click="grid.appScope.editSubCategory(row,  \'edit\')">Editar</button><div>';            

            self.columnsArea = [
                { field: 'Name', name: 'Nombre' },
                { field: '', name: 'Editar', cellTemplate: cellTemplateEdit }
            ];

            self.columnsCategory = [
                { field: 'Name', name: 'Nombre' },
                { field: 'Area.Name', name: 'Rubro' },                
                { field: '', name: 'Editar', cellTemplate: cellTemplateCategoryEdit }
            ];

            self.columnsSubCategory = [
                { field: 'Name', name: 'Nombre' },
                { field: 'Category.Name', name: 'Categoria' },                
                { field: '', name: 'Editar', cellTemplate: cellTemplateSubCategoryEdit }
            ];
                      
            self.gridArea = {
                enableSorting: true,
                columnDefs: $scope.columnsArea,
                enablePaginationControls: false,
                paginationPageSizes: [25, 50, 75],
                paginationPageSize: 10,
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            };

            self.gridCategory = {
                enableSorting: true,
                columnDefs: $scope.columnsCategory,
                enablePaginationControls: false,
                paginationPageSizes: [25, 50, 75],
                paginationPageSize: 10,
                onRegisterApi: function (gridApi) {
                    self.gridApi2 = gridApi;
                }
            };

            self.gridSubCategory = {
                enableSorting: true,
                columnDefs: $scope.columnsSubCategory,
                enablePaginationControls: false,
                paginationPageSizes: [25, 50, 75],
                paginationPageSize: 10,
                onRegisterApi: function (gridApi) {
                    self.gridApi3 = gridApi;
                }
            };           

            function DialogController($scope, $mdDialog) {

                $scope.hide = function () {
                    $mdDialog.hide();
                };

                $scope.Cancel = function () {
                    $mdDialog.cancel();
                };

                $scope.confirmArea = function (confirmArea) {
                    var isvalid = true;
                    $scope.error = null;                    

                    if ($scope.area.Name == null) {
                        $scope.error = "Debe incluir el nombre.";
                        return;
                    }                    
                                       
                    $mdDialog.hide(confirmArea);
                };
            };

            DialogController.$inject = ['$scope', '$mdDialog'];

            self.editArea = function (ev) {
                if (ev == 'newArea') {
                    $scope.area = { Id: 0, Name: null };
                } else {
                    $scope.area = angular.copy(ev.entity);
                }
                               
                $mdDialog.show({
                    controller: DialogController,
                    scope: $scope.$new(),
                    templateUrl: 'editArea',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmArea) {

                    if ($scope.area.$invalid) {
                        return;
                    }                    

                    $http({
                        method: 'POST',
                        url: 'SaveArea',
                        data: $scope.area
                    }).then(function successCallback(response) {    

                        if (self.area.Id == 0) {
                            self.gridArea.data.push(response.data);                            
                            self.areaMSG.Text = "El area ha sido dada de alta.";
                        } else {
                            self.gridArea.data.forEach(function (row, index) {
                                if (response.data.Id == row.Id) {
                                    self.gridArea.data.splice(index, 1);                                    
                                }
                            });

                            self.areaList.forEach(function (row2, index2) {
                                if (response.data.Id == row2.Id) {                                    
                                    self.areaList.splice(index2, 1);
                                }
                            });

                            self.gridArea.data.push(response.data);                            
                            self.areaMSG.Text = "El area ha sido modificada.";   
                        }   
                        
                        self.areaMSG.Operation = true;
                        }, function errorCallback(response) {
                            self.areaMSG.Text = "Ha ocurrido un error.";
                            self.areaMSG.Operation = false;
                        $log.info(response);
                    });
                }, function () {

                    //alert('You cancelled the dialog.');
                });
            }; 

            function DialogControllerCategory($scope, $mdDialog) {

                $scope.hide = function () {
                    $mdDialog.hide();
                };

                $scope.Cancel = function () {
                    $mdDialog.cancel();
                };

                $scope.confirmCategory = function (confirmCategory) {
                    var isvalid = true;
                    $scope.error = null;

                    if ($scope.category.Name == null) {
                        $scope.error = "Debe incluir el nombre.";
                        return;
                    }

                    if ($scope.category.Area == 'Seleccione') {
                        $scope.error = "Debe seleccionar un area.";
                        return;
                    }

                    $mdDialog.hide(confirmCategory);
                };
            };

            DialogControllerCategory.$inject = ['$scope', '$mdDialog'];

            self.editCategory = function (ev) {
                if (ev == 'newCategory') {
                    $scope.category = {
                        Id: 0, Name: null, Area: { Id: 0, Name: null }, SubCategories: null };
                } else {
                    $scope.category = angular.copy(ev.entity);
                }

                $mdDialog.show({
                    controller: DialogControllerCategory,
                    scope: $scope.$new(),
                    templateUrl: 'editCategory',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmCategory) {
                    if ($scope.category.$invalid) {
                        return;
                    }

                    $scope.category.Area.Categories = null;
                    
                    $http({
                        method: 'POST',
                        url: 'SaveCategory',
                        data: $scope.category
                    }).then(function successCallback(response) {

                        if (self.category.Id == 0) {
                            self.gridCategory.data.push(response.data);                            
                            self.categoryMSG.Text = "La categoria ha sido dada de alta.";
                        } else {
                            self.gridCategory.data.forEach(function (row, index) {
                                if (response.data.Id == row.Id) {
                                    self.gridCategory.data.splice(index, 1);
                                }
                            });

                            self.categoryList.forEach(function (row2, index2) {
                                if (response.data.Id == row2.Id) {
                                    self.categoryList.splice(index2, 1);
                                }
                            });

                            self.gridCategory.data.push(response.data);                            
                            self.categoryMSG.Text = "La categoria ha sido modificada.";
                        }

                        self.categoryMSG.Operation = true;
                    }, function errorCallback(response) {
                        self.categoryMSG.Text = "Ha ocurrido un error.";
                        self.categoryMSG.Operation = false;
                        $log.info(response);
                    });
                }, function () {

                    //alert('You cancelled the dialog.');
                });

            };
            
            function DialogControllerSubCategory($scope, $mdDialog) {

                $scope.hide = function () {
                    $mdDialog.hide();
                };

                $scope.Cancel = function () {
                    $mdDialog.cancel();
                };

                $scope.confirmSubCategory = function (confirmSubCategory) {
                    var isvalid = true;
                    $scope.error = null;

                    if ($scope.subCategory.Name == null) {
                        $scope.error = "Debe incluir el nombre.";
                        return;
                    }

                    if ($scope.subCategory.Category == 'Seleccione') {
                        $scope.error = "Debe seleccionar una Categoria.";
                        return;
                    }

                    $mdDialog.hide(confirmSubCategory);
                };
            };

            DialogControllerSubCategory.$inject = ['$scope', '$mdDialog'];

            self.editSubCategory = function (ev) {
                if (ev == 'newSubCategory') {
                    $scope.subCategory = {
                        Id: 0, Name: null, Category: { Id: 0, Name: null }
                    };
                } else {
                    $scope.subCategory = angular.copy(ev.entity);
                }

                $mdDialog.show({
                    controller: DialogControllerSubCategory,
                    scope: $scope.$new(),
                    templateUrl: 'editSubCategory',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmSubCategory) {
                    if ($scope.subCategory.$invalid) {
                        return;
                    }                    

                    $http({
                        method: 'POST',
                        url: 'SaveSubCategory',
                        data: $scope.subCategory
                    }).then(function successCallback(response) {

                        if (self.subCategory.Id == 0) {
                            self.gridSubCategory.data.push(response.data);
                            self.subCategoryMSG.Text = "La subCategoria ha sido dada de alta.";
                        } else {
                            self.gridSubCategory.data.forEach(function (row, index) {
                                if (response.data.Id == row.Id) {
                                    self.gridSubCategory.data.splice(index, 1);
                                }
                            });

                            self.subCategoryList.forEach(function (row2, index2) {
                                if (response.data.Id == row2.Id) {
                                    self.subCategoryList.splice(index2, 1);
                                }
                            });

                            self.gridSubCategory.data.push(response.data);
                            self.subCategoryMSG.Text = "La categoria ha sido modificada.";
                        }

                        self.subCategoryMSG.Operation = true;
                    }, function errorCallback(response) {
                        self.subCategoryMSG.Text = "Ha ocurrido un error.";
                        self.subCategoryMSG.Operation = false;
                        $log.info(response);
                    });
                }, function () {

                    //alert('You cancelled the dialog.');
                });

            };

            getData = function () {
                if (self.categoryList == null)
                {
                    $http({
                        method: 'POST',
                        url: 'GetAreas',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.areaList = response.data;
                        self.gridArea.data = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });    
                }

                if (self.categoryList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetCategories',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.categoryList = response.data;
                        self.gridCategory.data = response.data;
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
                        self.gridSubCategory.data = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }
            }

            loading = function () {
                if ((self.areaList != null) && (self.categoryList != null) && (self.subCategoryList != null)) {
                    self.loading = false;
                }
            };

            getData();
        }
    ]
);