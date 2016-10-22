(function () {
    'use strict';
    
    var app = angular.module('app');

    app.controller('usuarioEditCtrl', ['$rootScope', '$scope', 'dialogs', '$http', 'resources', '$routeParams', '$location',
        function ($rootScope, $scope, dialogs, $http, resources, $routeParams, $location) {

            $scope.load = function () {
                if ($routeParams.id) {
                    resources.usuario.get({ id: $routeParams.id }).$promise.then(
                        function (entidade) {
                            $scope.entidade = entidade;
                        },
                        function (error) {
                            dialogs.error("Usuário", error.data.message);
                        }
                    );
                }
            };
            
            $scope.save = function () {
                var metodo = "POST";
                if ($scope.entidade.id != undefined && $scope.entidade.id > 0) {
                    metodo = "PUT";
                }                
                $http({
                    method: metodo,
                    url:    $rootScope.apiPath + "api/usuario/",
                    data:   $scope.entidade
                }).
                success(function() {
                    $location.url("usuario/");
                }).
                error(function(data) {
                    dialogs.error("Usuário", data.message);
                });                 
            };
            
            $scope.cancel = function () {
                var dlg = dialogs.confirm('Usuário', 'Tem certeza que deseja cancelar?');
                dlg.result.then(function () {
                    $location.url("usuario/");
                });                                
            };
            
            $scope.load();
        }
    ]);

})();