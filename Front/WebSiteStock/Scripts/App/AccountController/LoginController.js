stockApp.controller('LoginController', ['$scope', '$http', '$log', '$interval', function ($scope, $http, $log, $interval) {
    var self = $scope;

        self.Login = function () {    
            self.errorMessage = false;
            if (self.loginUser.$invalid) {                
                return;
            }

            $http({
                method: 'POST',
                url: 'LoginUser',
                params: { userName: self.loginUser.userName, password: self.loginUser.password }
            }).then(successFunction, errorFunction);
        };

        var successFunction = function (response) {
            $log.info(response);                        
            window.open('/', '_self');
        };

        var errorFunction = function (response) {
            self.errorMessage = true;
            $log.info(response);
        };
    }]);