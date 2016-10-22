(function () {
    'use strict';
    
    var app = angular.module('app');

    app.controller('clienteListCtrl', ['$rootScope', '$scope', 'dialogs', '$http', 'resources',
        function ($rootScope, $scope, dialogs, $http, resources) {

            $scope.load = function () {
                $scope.consulta();
            };
            
            $scope.consulta = function () {
                resources.cliente.query().$promise.then(function (dados) {
                    $scope.entidades = dados;
                });                
            };
            
            $scope.selecionar = function(entidade) {
                $scope.entidadeSelecionada = entidade;
            };
            
            $scope.remove = function () {
                var dlg = dialogs.confirm('Cliente', 'Confirma a Exclusão desse item?');
                dlg.result.then(function () {
                    $http({
                        method: 'DELETE',
                        url: $rootScope.apiPath + "api/Cliente/" + $scope.entidadeSelecionada.id
                    }).
                    success(function () {
                        $scope.consulta();
                    }).
                    error(function (data) {
                        dialogs.error("Cliente", data.message);
                    });
                });
            };            
                        
            
            $scope.load();
        }
    ]);

})();