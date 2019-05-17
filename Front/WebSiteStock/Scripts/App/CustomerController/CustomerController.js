stockApp.controller('CustomerController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, $mdDialog) {
            var self = $scope;
            self.loading = true;            
            self.telcoList = null;
            self.countryList = null;
            self.stateList = null;
            self.cityList = null;            
            self.uidCode = null;
            self.resultCustomer = null;

            self.columns = [{ field: 'Name', name: 'Cliente' },
            { field: 'FirstName', name: 'Nombre' }, { field: 'LastName', name: 'Apellido' },
            { field: 'UidCodeSerie', name: 'Dni/Cuit' }];

            self.gridOptions = { enableSorting: true,
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
                url: 'GetCustomer',
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
                    newCustomer();
                    $mdDialog.cancel();
                };

                $scope.confirmCustomer = function (confirmCustomer) {
                    var isvalid = true;
                    $scope.error = null;                    

                    if ($scope.editCustomer.FirstName == null) {
                        $scope.error = "Ingrese el nombre.";
                        return;
                    }

                    if ($scope.editCustomer.LastName == null) {
                        $scope.error = "Ingrese el apellido.";
                        return;
                    }

                    if ($scope.editCustomer.BirthDate == '') {
                        $scope.error = "Ingrese el Ingrese la fecha de nacimiento.";
                        return;
                    }

                    if ($scope.editCustomer.Email == null) {
                        $scope.error = "Ingrese el email.";
                        return;
                    }

                    if ($scope.editCustomer.UidCode == null) {
                        $scope.error = "Seleccione el tipo de documento.";
                        return;
                    }

                    if ($scope.editCustomer.UidSerie == null) {
                        $scope.error = "Ingrese el número de documento.";
                        return;
                    }

                    if ($scope.editCustomer.Phones.length > 0) {
                        for (var i = 0; i < $scope.editCustomer.Phones.length; i++) {
                            if ($scope.editCustomer.Phones[i].CountryCode == null || $scope.editCustomer.Phones[i].AreaCode == null
                                || $scope.editCustomer.Phones[i].Number == null || $scope.editCustomer.Phones[i].TelcoId == null) {
                                isvalid = false;
                            }
                        }
                    }

                    if ($scope.editCustomer.Address != null) {
                        if ($scope.editCustomer.Address.Street == null) {
                            $scope.error = "Ingrese la calle.";
                            return;
                        }

                        if ($scope.editCustomer.Address.StreetNumber == null) {
                            $scope.error = "Ingrese el número.";
                            return;
                        }

                        if ($scope.editCustomer.Address.ZipCode == null) {
                            $scope.error = "Ingrese el código postal.";
                            return;
                        }

                        if ($scope.editCustomer.Address.LocationId == null) {
                            $scope.error = "Seleccione una localidad.";
                            return;
                        }
                    }

                    if (!isvalid) {
                        $scope.error = "Debe completar los datos en cada telefono.";
                        return;
                    }

                    $mdDialog.hide(confirmCustomer);
                };
            };

            DialogController.$inject = ['$scope', '$mdDialog'];

            self.newCustomer = function (ev) {
                //self.editCustomer = angular.copy(self.box);

                $mdDialog.show({
                    controller: DialogController,
                    scope: $scope.$new(),
                    templateUrl: 'dialogOpenClose',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmCustomer) {
                    if (self.editCustomer.$invalid) {
                        return;
                    }

                    self.editCustomer.UidCode = self.editCustomer.UidCode.Code;

                    $http({
                        method: 'POST',
                        url: 'SaveCustomer',
                        data: self.editCustomer
                    }).then(function successCallback(response) {
                        $scope.gridOptions.data.push(response.data);
                        self.resultCustomer = response.data;
                        viewResult(ev);
                        newCustomer();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });                    
                }, function () {

                    //alert('You cancelled the dialog.');
                });
            };     

            self.addPhone = function () {
                self.editCustomer.Phones.push(createPhoneObject());
            };

            self.removePhone = function (param) {
                self.editCustomer.Phones.pop();
            };

            self.addAddress = function () {
                self.editCustomer.Address =
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
                self.editCustomer.Address = null;
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
                        self.resultCustomer = null;
                        $mdDialog.hide();
                    };

                    $scope.Cancel = function () {
                        self.resultCustomer = null;
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
                    //self.resultCustomer = null;
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

            newCustomer = function () {
                self.editCustomer =
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
            newCustomer();           
        }
    ]
);