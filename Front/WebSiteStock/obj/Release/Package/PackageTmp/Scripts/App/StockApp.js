var stockApp = angular.module('stockModule', ['ngMessages', 'ui.grid', 'ui.grid.pagination',
    'ui.grid.grouping', 'angular-loading-bar', 'ngAnimate', 'angularMoment', 'ngMaterial'])
    .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = true;
    }]).config(['$mdDateLocaleProvider', function ($mdDateLocaleProvider) {
        $mdDateLocaleProvider.months = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
            'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
        $mdDateLocaleProvider.shortDays = ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'];
        $mdDateLocaleProvider.formatDate = function (date) {
            return moment(date).format('DD/MM/YYYY');
        };    
    }]).controller('mainController', ['$scope', '$http', '$log', function ($scope, $http, $log)
    {}]);