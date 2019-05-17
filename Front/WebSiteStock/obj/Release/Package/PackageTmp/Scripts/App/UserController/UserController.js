stockApp.controller('UserController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', '$mdDialog',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, $mdDialog) {
            var self = $scope;
            self.loading = true;
            self.rolesComposition = null;
            self.profilesRoles = [];
            self.otherRoles = [];
            self.selectedRoles = [];
            self.selectedProfile = null;
            self.telcoList = null;
            self.countryList = null;
            self.stateList = null;
            self.cityList = null;
            self.branchOfficeList = null;
            self.uidCode = null;
            self.resultUser = null;

            self.columns = [{ field: 'Staff.Person.FirstName', name: 'Nombre' },
                { field: 'Staff.Person.LastName', name: 'Apellido' },
                { field: 'Staff.Person.Email', name: 'Email' },
                { field: 'Staff.Person.UidCode', name: 'Tipo Dni' },
                { field: 'Staff.Person.UidSerie', name: 'Numero Dni' },
                { field: 'StaffRoleName', name: 'Perfil de usuario' },
                { field: 'UserName', name: 'Nombre de usuario' }               
            ];

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

            self.isChecked = function (param) {
                return self.selectedRoles.length === param.length;
            };

            self.toggleAll = function (param) {
                if (self.selectedRoles.length === param.length) {
                    self.selectedRoles = [];
                } else if (self.selectedRoles.length === 0 || self.selectedRoles.length > 0) {
                    self.selectedRoles = param.slice(0);
                }
            };

            self.toggle = function (item, list) {
                var idx = list.indexOf(item);
                if (idx > -1) {
                    list.splice(idx, 1);
                }
                else {
                    list.push(item);
                }
            };

            self.exists = function (item, list) {
                return list.indexOf(item) > -1;
            };

            self.fillRoles = function (code) {
                self.profilesRoles = [];
                self.otherRoles = [];

                self.rolesComposition.forEach(function (item) {                    
                    if (item.StaffRoleItem.Code == code) {
                        self.selectedProfile = item.StaffRoleItem.Name;
                        item.Roles.forEach(function (rol) {
                            self.profilesRoles.push(angular.copy(rol));                            
                        });
                    }
                });                                

                self.rolesComposition.forEach(function (item) {
                    if (item.StaffRoleItem.Code != code) {
                        item.Roles.forEach(function (rol) {
                            var exists = false;
                            self.profilesRoles.forEach(function (rolAdded) {
                                if (rol.RoleCode == rolAdded.RoleCode) {
                                    exists = true;
                                }
                            });

                            if (!exists) {
                                self.otherRoles.push(angular.copy(rol));
                            }
                        });
                    }
                });

                self.toggleAll(self.profilesRoles);

                self.editUser.StaffRoleCode = code;
            };

            $http({
                method: 'POST',
                url: 'GetUsers',
                data: queryInfoService.ReturnInitialQueryInfo()
            }).then(function successCallback(response) {
                $scope.gridOptions.data = response.data;
            }, function errorCallback(response) {
                $log.info(response);
            });

            self.clearArrays = function () {
                self.selectedProfile = null;
                self.profilesRoles = [];
                self.otherRoles = [];
            };

            function DialogController($scope, $mdDialog) {

                $scope.hide = function () {
                    newUser();
                    $mdDialog.hide();
                };

                $scope.Cancel = function () {
                    self.clearArrays();
                    newUser();
                    $mdDialog.cancel();
                };

                $scope.confirmUser = function (confirmUser) {
                    var isvalid = true;
                    $scope.error = null;                    
                    $scope.staffRoleCodeError = null;

                    if ($scope.editUser.Staff.Person.FirstName == null) {
                        $scope.error = "Ingrese el nombre.";
                        return;
                    }

                    if ($scope.editUser.Staff.Person.LastName == null) {
                        $scope.error = "Ingrese el apellido.";
                        return;
                    }

                    if ($scope.editUser.Staff.Person.BirthDate == '') {
                        $scope.error = "Ingrese el Ingrese la fecha de nacimiento.";
                        return;
                    }

                    if ($scope.editUser.Staff.Person.Email == null) {
                        $scope.error = "Ingrese el email.";
                        return;
                    }

                    if ($scope.editUser.Staff.Person.UidCode == null) {
                        $scope.error = "Seleccione el tipo de documento.";
                        return;
                    }

                    if ($scope.editUser.Staff.Person.UidSerie == null) {
                        $scope.error = "Ingrese el número de documento.";
                        return;
                    }

                    if ($scope.editUser.StaffRoleCode == null) {
                        $scope.staffRoleCodeError = "Debe seleccionar un perfil de usuario.";
                        return;
                    }

                    if ($scope.selectedRoles.length == 0) {
                        $scope.staffRoleCodeError = "Debe seleccionar al menos un rol.";
                        return;
                    }
                    
                    if ($scope.editUser.Staff.Person.Phones.length > 0)
                    {
                        for (var i = 0; i < $scope.editUser.Staff.Person.Phones.length; i++) {
                            if ($scope.editUser.Staff.Person.Phones[i].CountryCode == null || $scope.editUser.Staff.Person.Phones[i].AreaCode == null
                                || $scope.editUser.Staff.Person.Phones[i].Number == null || $scope.editUser.Staff.Person.Phones[i].TelcoId == null) {
                                isvalid = false;
                            }
                        }
                    }                    

                    if ($scope.editUser.Staff.Person.Address != null)
                    {
                        if ($scope.editUser.Staff.Person.Address.Street == null)
                        {
                            $scope.error = "Ingrese la calle.";
                            return;
                        }

                        if ($scope.editUser.Staff.Person.Address.StreetNumber == null) {
                            $scope.error = "Ingrese el número.";
                            return;
                        }

                        if ($scope.editUser.Staff.Person.Address.ZipCode == null) {
                            $scope.error = "Ingrese el código postal.";
                            return;
                        }

                        if ($scope.editUser.Staff.Person.Address.LocationId == null) {
                            $scope.error = "Seleccione una localidad.";
                            return;
                        }
                    }

                    if (!isvalid) {
                        $scope.error = "Debe completar los datos en cada telefono.";
                        return;
                    }

                    $mdDialog.hide(confirmUser);
                };
            };

            DialogController.$inject = ['$scope', '$mdDialog'];

            self.newUser = function (ev) {
                //self.editUser = angular.copy(self.box);          

                $mdDialog.show({
                    controller: DialogController,
                    scope: $scope.$new(),
                    templateUrl: 'dialogOpenClose',
                    targetEvent: ev,
                    clickOutsideToClose: false,
                    fullscreen: true,
                    bindToController: true
                }).then(function (confirmUser) {
                    if (self.editUser.$invalid) {
                        return;
                    }

                    self.selectedRoles.forEach(function (item) {                        
                        self.editUser.Roles.push(item.RoleCode);
                    });

                    self.editUser.Staff.Person.UidCode = self.editUser.Staff.Person.UidCode.Code;

                    $http({
                        method: 'POST',
                        url: 'SaveStaff',
                        data: self.editUser
                    }).then(function successCallback(response) {
                        $scope.gridOptions.data.push(response.data);
                        self.resultUser = response.data;
                        viewResult(ev);
                        newUser();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });

                    self.clearArrays();
                }, function () {

                  //alert('You cancelled the dialog.');
                });
            };            

            self.addPhone = function () {
                self.editUser.Staff.Person.Phones.push(createPhoneObject());
            };

            self.removePhone = function (param) {
                self.editUser.Staff.Person.Phones.pop();                
            };  

            self.addAddress = function () {
                self.editUser.Staff.Person.Address =
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
                self.editUser.Staff.Person.Address = null;
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
                        self.resultUser = null;
                        $mdDialog.hide();
                    };

                    $scope.Cancel = function () {          
                        self.resultUser = null;
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
                    //self.resultUser = null;
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
                if (self.rolesComposition == null) {
                    $http({
                        method: 'POST',
                        url: 'GetRolesComposition'
                    }).then(function successCallback(response) {
                        self.rolesComposition = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

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

                if (self.branchOfficeList == null)
                {
                    $http({
                        method: 'POST',
                        url: 'GetBranchOffices',
                        data: queryInfoService.ReturnInitialQueryInfo()
                    }).then(function successCallback(response) {
                        self.branchOfficeList = response.data;
                        loading();
                    }, function errorCallback(response) {
                        $log.info(response);
                    });
                }

                if (self.uidCode == null)
                {
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
                if ((self.rolesComposition != null) && (self.telcoList != null) && (self.countryList != null) && (self.branchOfficeList != null) && (self.uidCode != null)) {
                    self.loading = false;
                }
            };

            newUser = function () {
                self.editUser =
                    {
                        DeactivatedDate: null,
                        DeactivateNote: null,
                        Staff:
                            {
                                Person:
                                    {
                                        FirstName: null,
                                        LastName: null,
                                        BirthDate: moment(new Date()),
                                        Email: null,
                                        UidSerie: null,
                                        UidCode: null,
                                        Phones: [],
                                        Address: null
                                    }
                            },
                        BranchOffice: null,
                        StaffRoleCode: null,
                        UserId: null,
                        CreatedDate: null,
                        Roles: []
                    };
            };

            getData();
            newUser();
        }
    ]
);