(function () {
    'use strict';
    
    var app = angular.module('app');

    app.controller('usuarioListCtrl', ['$rootScope', '$scope', 'dialogs', '$http', 'resources',
        function ($rootScope, $scope, dialogs, $http, resources) {

            $scope.load = function () {
                $scope.consulta();
            };
            
            $scope.consulta = function () {
                resources.usuario.query().$promise.then(function (dados) {
                    $scope.entidades = dados;
                });                
            };
            
            $scope.selecionar = function(entidade) {
                $scope.entidadeSelecionada = entidade;
            };
            
            $scope.remove = function () {
                var dlg = dialogs.confirm('Usuário', 'Confirma a Exclusão desse item?');
                dlg.result.then(function () {
                    $http({
                        method: 'DELETE',
                        url: $rootScope.apiPath + "api/usuario/" + $scope.entidadeSelecionada.id
                    }).
                    success(function () {
                        $scope.consulta();
                    }).
                    error(function (data) {
                        dialogs.error("Usuário", data.message);
                    });
                });
            }; 
            
            $scope.load();
        }
    ]);

})();