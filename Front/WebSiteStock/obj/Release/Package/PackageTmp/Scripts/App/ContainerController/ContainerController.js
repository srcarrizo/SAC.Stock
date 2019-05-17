stockApp.controller('ContainerController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment, $mdDialog) {
            var self = $scope;

            self.container = null;            
            self.parentContainerList = [];
            self.containerList = [];

            self.containerMSG = { Text: null, Operation: false };            
            self.loading = true;           

            self.columnsParantContainer = [                
                { field: 'Name', name: 'Contenedor Minorista' },
                { field: 'Amount', name: 'Cantidad Contenedor Minorista' }
            ];

            self.columnsContainer = [
                { field: 'ParentContainer.Name', name: 'Contenedor Mayorista', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' } },
                { field: 'ParentContainer.Amount', name: 'Cantidad Contenedor Mayorista', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' } },
                { field: 'ParentContainer.Amount', name: 'Cantidad Contenedor Mayorista' },
                { field: 'Name', name: 'Contenedor Minorista' },
                { field: 'Amount', name: 'Cantidad Contenedor Minorista' },
            ];

            self.gridParentContainer = {
                enableSorting: true,
                columnDefs: $scope.columnsParantContainer,
                enablePaginationControls: false,               
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            };    

            self.gridContainer = {
                enableSorting: true,
                columnDefs: $scope.columnsContainer,
                enablePaginationControls: false,
                onRegisterApi: function (gridApi) {
                    self.gridApi = gridApi;
                }
            }; 

            getData = function () {
                if (self.parentContainerList.length == 0) {
                    $http({
                        method: 'POST',
                        url: 'GetContainers',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {

                        response.data.forEach(function (row) {
                            if (row.ParentContainer != null) {
                                self.containerList.push(row);
                            }
                            else
                            {
                                self.parentContainerList.push(row);
                            }
                        });
                        
                        self.gridContainer.data = self.containerList;
                        self.gridParentContainer.data = self.parentContainerList;

                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }
            }

            function DialogController($scope, $mdDialog) {

                $scope.hide = function () {
                    $mdDialog.hide();
                };

                $scope.Cancel = function () {
                    $mdDialog.cancel();
                };

                $scope.confirmContainer = function (confirmContainer) {
                    var isvalid = true;
                    $scope.error = null;

                    if ($scope.container.Name == null) {
                        $scope.error = "Debe incluir el nombre.";
                        return;
                    }

                    if ($scope.container.Amount == null) {
                        $scope.error = "Debe especificar la cantidad.";
                        return;
                    }

                    if (!$scope.container.Parent) {
                        if ($scope.container.ParentContainer == 'Seleccione') {
                            $scope.error = "Debe seleccionar un embase mayorista.";
                            return;
                        }
                    } else {
                        $scope.container.ParentContainer = null;
                    }

                    $mdDialog.hide(confirmContainer);
                };
            };

            DialogController.$inject = ['$scope', '$mdDialog'];

            self.editContainer = function (ev) {
                if (ev == 'newContainer') {
                    $scope.container = { Id: 0, Name: null, ParentContainer: null };
                } else {
                    $scope.container = angular.copy(ev.entity);
                }

                $mdDialog.show({
                    controller: DialogController,
                    scope: $scope.$new(),
                    templateUrl: 'editContainer',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmContainer) {

                    if ($scope.container.$invalid) {
                        return;
                    }

                    if ($scope.container.ParentContainer != null) {
                        $scope.container.ParentContainer.Containers = null;
                    }

                    $http({
                        method: 'POST',
                        url: 'SaveContainer',
                        data: $scope.container
                    }).then(function successCallback(response) {                                                  
                        if (self.container.Id == 0) {
                            if (response.data.ParentContainer != null) {
                                self.containerList.push(response.data);
                                //self.gridContainer.data.push(response.data);
                            }
                            else {
                                self.parentContainerList.push(response.data);
                                //self.gridParentContainer.data.push(response.data);
                            }   
                            
                            self.containerMSG.Text = "El contendor ha sido dado de alta.";
                        } else {

                            self.gridContainer.data.forEach(function (row, index) {
                                if (response.data.Id == row.Id) {
                                    self.gridContainer.data.splice(index, 1);
                                }
                            });

                            self.containerList.forEach(function (row2, index2) {
                                if (response.data.Id == row2.Id) {
                                    self.containerList.splice(index2, 1);
                                }
                            });

                            self.gridContainer.data.push(response.data);
                            self.containerMSG.Text = "El embase ha sido modificado.";
                        }

                        self.containerMSG.Operation = true;
                    }, function errorCallback(response) {
                        self.containerMSG.Text = "Ha ocurrido un error.";
                        self.containerMSG.Operation = false;
                        $log.info(response);
                    });
                }, function () {

                    //alert('You cancelled the dialog.');
                });
            }; 

            loading = function () {
                if ((self.containerList.length != 0) && (self.parentContainerList.length != 0)) {
                    self.loading = false;
                }
            };

            getData();
        }
    ]);