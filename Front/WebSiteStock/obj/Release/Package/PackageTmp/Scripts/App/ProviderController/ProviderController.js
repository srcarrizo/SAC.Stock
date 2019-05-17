stockApp.controller('ProviderController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, $mdDialog) {
            var self = $scope;
            self.loading = true;
            self.telcoList = null;
            self.countryList = null;
            self.stateList = null;
            self.cityList = null;
            self.uidCode = null;
            self.resultProvider = null;

            self.columns = [{ field: 'Name', name: 'Proveedor' },
            { field: 'FirstName', name: 'Nombre' }, { field: 'LastName', name: 'Apellido' },
            { field: 'UidCodeSerie', name: 'Dni/Cuit' }];
            
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

            $http({
                method: 'POST',
                url: 'GetProviders',
                data: queryInfoService.ReturnInitialQueryInfo()
            }).then(function successCallback(response) {                

                response.data.forEach(function (row, index) {                    
                    row.CreatedDate = moment(row.CreatedDate).format('DD/MM/YYYY');
                });


                response.data.forEach(function (row, index) {                    
                    row.CreatedDate = moment(row.CreatedDate).format('DD/MM/YYYY');
                });

                $scope.gridOptions.data = response.data;
            }, function errorCallback(response) {
                $log.info(response);
                });

            function DialogController($scope, $mdDialog) {

                $scope.hide = function () {
                    newUser();
                    $mdDialog.hide();
                };

                $scope.Cancel = function () {
                    newProvider();
                    $mdDialog.cancel();
                };

                $scope.confirmProvider = function (confirmProvider) {
                    var isvalid = true;
                    $scope.error = null;

                    if ($scope.editProvider.FirstName == null) {
                        $scope.error = "Ingrese el nombre.";
                        return;
                    }

                    if ($scope.editProvider.LastName == null) {
                        $scope.error = "Ingrese el apellido.";
                        return;
                    }

                    if ($scope.editProvider.BirthDate == '') {
                        $scope.error = "Ingrese el Ingrese la fecha de nacimiento.";
                        return;
                    }

                    if ($scope.editProvider.Email == null) {
                        $scope.error = "Ingrese el email.";
                        return;
                    }

                    if ($scope.editProvider.UidCode == null) {
                        $scope.error = "Seleccione el tipo de documento.";
                        return;
                    }

                    if ($scope.editProvider.UidSerie == null) {
                        $scope.error = "Ingrese el número de documento.";
                        return;
                    }

                    if ($scope.editProvider.Phones.length > 0) {
                        for (var i = 0; i < $scope.editProvider.Phones.length; i++) {
                            if ($scope.editProvider.Phones[i].CountryCode == null || $scope.editProvider.Phones[i].AreaCode == null
                                || $scope.editProvider.Phones[i].Number == null || $scope.editProvider.Phones[i].TelcoId == null) {
                                isvalid = false;
                            }
                        }
                    }

                    if ($scope.editProvider.Address != null) {
                        if ($scope.editProvider.Address.Street == null) {
                            $scope.error = "Ingrese la calle.";
                            return;
                        }

                        if ($scope.editProvider.Address.StreetNumber == null) {
                            $scope.error = "Ingrese el número.";
                            return;
                        }

                        if ($scope.editProvider.Address.ZipCode == null) {
                            $scope.error = "Ingrese el código postal.";
                            return;
                        }

                        if ($scope.editProvider.Address.LocationId == null) {
                            $scope.error = "Seleccione una localidad.";
                            return;
                        }
                    }

                    if (!isvalid) {
                        $scope.error = "Debe completar los datos en cada telefono.";
                        return;
                    }

                    $mdDialog.hide(confirmProvider);
                };
            };

            DialogController.$inject = ['$scope', '$mdDialog'];

            self.newProvider = function (ev) {
                //self.editProvider = angular.copy(self.box);

                $mdDialog.show({
                    controller: DialogController,
                    scope: $scope.$new(),
                    templateUrl: 'dialogOpenClose',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmProvider) {
                    if (self.editProvider.$invalid) {
                        return;
                    }

                    self.editProvider.UidCode = self.editProvider.UidCode.Code;

                    $http({
                        method: 'POST',
                        url: 'SaveProvider',
                        data: self.editProvider
                    }).then(function successCallback(response) {
                        $scope.gridOptions.data.push(response.data);
                        self.resultProvider = response.data;
                        viewResult(ev);
                        newProvider();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }, function () {

                    //alert('You cancelled the dialog.');
                });
            };

            self.addPhone = function () {
                self.editProvider.Phones.push(createPhoneObject());
            };

            self.removePhone = function (param) {
                self.editProvider.Phones.pop();
            };

            self.addAddress = function () {
                self.editProvider.Address =
                    {
                        Street: null,
                        StreetNumber: null,
                        Floor: null,
                        Apartment: null,
                        Neighborhood: null,
                        ZipCode: null,
                        LocationCountry: null,
                        LocationState: null,
                        LocationCity: null,
                        LocationId: null
                    };
            };

            self.removeAddress = function () {
                self.editProvider.Address = null;
            };

            self.setTelco = function (phone) {
                if (phone != null) {
                    phone.TelcoId = phone.Telco.Id;
                }
            };

            self.setCountryAsLocation = function (address) {
                if (address != null) {
                    if ((address.LocationCountry != null) && (address.cityList == null)) {
                        getChildLocations(address.LocationCountry);
                        address.LocationId = address.LocationCountry.Id;
                    }
                }
            };

            self.setStateAsLocation = function (address) {
                if (address != null) {
                    if ((address.LocationState != null) && (address.cityList == null)) {
                        getChildLocations(address.LocationState);
                        address.LocationId = address.LocationState.Id;
                    }
                }
            };

            self.setCityAsLocation = function (address) {
                if (address != null) {
                    if ((address.LocationCity != null)) {
                        address.LocationId = address.LocationCity.Id;
                    }
                }
            };

            viewResult = function (ev) {
                function DialogControllerResult($scope, $mdDialog) {

                    $scope.hide = function () {
                        self.resultProvider = null;
                        $mdDialog.hide();
                    };

                    $scope.Cancel = function () {
                        self.resultProvider = null;
                        $mdDialog.cancel();
                    };

                    $scope.acept = function (acept) {
                        $mdDialog.hide(acept);
                    };
                };

                DialogControllerResult.$inject = ['$scope', '$mdDialog'];

                $mdDialog.show({
                    controller: DialogControllerResult,
                    scope: $scope.$new(),
                    templateUrl: 'dialogOpenCloseResult',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (acept) {
                    //self.resultProvider = null;
                }, function () {
                });
            };

            getChildLocations = function (location) {
                $http({
                    method: 'POST',
                    url: 'GetChildLocations',
                    data: location
                }).then(function successCallback(response) {
                    if (location.LocationTypeCode == "Country") {
                        self.stateList = response.data;
                    }
                    else {
                        self.cityList = response.data;
                    }

                }, function errorCallback(response) {
                    $log.info(response);
                });
            };

            getData = function () {
                if (self.telcoList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetTelcoList'
                    }).then(function successCallback(response) {
                        self.telcoList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

                if (self.countryList == null) {
                    $http({
                        method: 'POST',
                        url: 'GetCountryList'
                    }).then(function successCallback(response) {
                        self.countryList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

                if (self.uidCode == null) {
                    $http({
                        method: 'POST',
                        url: 'GetUidCode'
                    }).then(function successCallback(response) {
                        self.uidCode = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });

                }
            };

            createPhoneObject = function () {
                var itemPhone = {
                    CountryCode: "+54",
                    AreaCode: null,
                    Number: null,
                    Mobile: false,
                    Name: null,
                    Telco: null,
                    TelcoId: null
                };

                return itemPhone;
            };

            loading = function () {
                if ((self.telcoList != null) && (self.countryList != null) && (self.uidCode != null)) {
                    self.loading = false;
                }
            };

            newProvider = function () {
                self.editProvider =
                    {
                        DeactivatedDate: null,
                        DeactivateNote: null,
                        FirstName: null,
                        LastName: null,
                        BirthDate: moment(new Date()),
                        Email: null,
                        UidSerie: null,
                        UidCode: null,
                        Phones: [],
                        Address: null,
                        CreatedDate: null
                    };
            };

            getData();
            newProvider();
        }
    ]
);