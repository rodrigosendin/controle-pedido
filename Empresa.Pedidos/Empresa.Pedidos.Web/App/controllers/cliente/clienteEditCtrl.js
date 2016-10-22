(function () {
    'use strict';
    
    var app = angular.module('app');

    app.controller('clienteEditCtrl', ['$rootScope', '$scope', 'dialogs', '$http', 'resources', '$routeParams', '$location',
        function ($rootScope, $scope, dialogs, $http, resources, $routeParams, $location) {

            $scope.load = function () { 
                if ($routeParams.id) {
                     resources.cliente.get({ id: $routeParams.id }).$promise.then(
                        function (entidade) {
                            $scope.entidade = entidade;
                        },
                        function (error) {
                            dialogs.error("Cliente", error.data.message);
                        });
                } else {
                    $scope.entidade = {};
                }
            };
            
            $scope.save = function () {
                var metodo = "POST";
                if ($scope.entidade.id != undefined && $scope.entidade.id > 0) {
                    metodo = "PUT";
                }                
                $http({
                    method: metodo,
                    url:    $rootScope.apiPath + "api/cliente/",
                    data:   $scope.entidade
                }).
                success(function() {
                    $location.url("cliente/");
                }).
                error(function(data) {
                    dialogs.error("Cliente", data.message);
                });                 
            };
            
            $scope.cancel = function () {
                var dlg = dialogs.confirm('Cliente', 'Tem certeza que deseja cancelar?');
                dlg.result.then(function () {
                    $location.url("cliente/");
                });                                
            };
            $scope.load();
        }
    ]);

})();