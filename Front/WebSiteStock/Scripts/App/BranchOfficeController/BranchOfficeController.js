stockApp.controller('BranchOfficeController',
    ['$scope', '$http', '$log', 'queryInfoService', 'uiGridConstants', 'uiGridGroupingConstants', 'moment',
        function ($scope, $http, $log, queryInfoService, uiGridConstants, uiGridGroupingConstants, moment)
        {
            var self = $scope; 
            self.branchOffice = null;
            self.loading = true;
            self.listView = true;
            self.editView = false;
            self.resultView = false;
            self.newView = false;         
           
            self.columns = [{ field: 'Name', name: 'Sucursal'},
                { field: 'Description', name: 'Descripcion'},
                { field: 'ActivatedDate', name: 'Fecha de activación'}];

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
                url: 'GetBranchOffices',
                data: queryInfoService.ReturnInitialQueryInfo()
            }).then(function successCallback(response) {

                response.data.forEach(function (row, index) {                    
                    row.ActivatedDate = moment(row.ActivatedDate).format('DD/MM/YYYY');
                    row.CreatedDate = moment(row.CreatedDate).format('DD/MM/YYYY');
                });

                $scope.gridOptions.data = response.data;
            }, function errorCallback(response) {
                $log.info(response);
                });

            self.addPhone = function () {
                self.branchOffice.Phones.push(createPhoneObject());
            };

            self.removePhone = function (param) {
                self.branchOffice.Phones.pop();
            };  

            self.addAddress = function () {
                self.branchOffice.Address =
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
                self.branchOffice.Address = null;
            };

            self.changeViews = function (param, operation) {
                self.listView = param == 'list';
                self.newView = param == 'new';

                if (typeof (param) == 'object') {
                    if (operation == 'result') {                        
                        self.resultView = true;                       
                        self.branchOffice = angular.copy(param.entity);                        
                    }
                    else if (operation == 'edit') {
                        self.newView = true;
                        self.branchOffice = angular.copy(param.entity);
                    }
                }
                else {
                    self.editView = false;                    
                    self.resultView = false;
                }

                if (param == 'new') {
                    newUser();
                }
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

            self.saveNew = function () {
                var invalid = false;
                if (self.newBranchOffice.$invalid) {
                    return;
                }                  

                $http({
                    method: 'POST',
                    url: 'SaveBranchOffice',
                    data: self.branchOffice
                }).then(function successCallback(response) {

                    response.data.CreatedDate = moment(response.data.CreatedDate).format('DD/MM/YYYY');
                    if (response.data.DeactivatedDate != null) {
                        response.data.DeactivatedDate = moment(response.data.DeactivatedDate).format('DD/MM/YYYY');
                    }

                    if (self.branchOffice.Id == null) {
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

            newUser = function () {
                self.branchOffice =
                    {
                        Id: 0,
                        Name: null,
                        Description: null,
                        Address: null,
                        Phones: []
                    };
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
            };

            loading = function () {
                if ((self.telcoList != null) && (self.countryList != null)) {
                    self.loading = false;
                }
            };

            getData();
            newUser();           
        }
    ]);