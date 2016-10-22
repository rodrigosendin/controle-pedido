(function () {
    'use strict';
    
    var app = angular.module('app');

    app.controller('produtoEditCtrl', ['$rootScope', '$scope', 'dialogs', '$http', 'resources', '$routeParams', '$location',
        function ($rootScope, $scope, dialogs, $http, resources, $routeParams, $location) {

            $scope.load = function () { 
                if ($routeParams.id) {
                     resources.produto.get({ id: $routeParams.id }).$promise.then(
                        function (entidade) {
                            $scope.entidade = entidade;
                        },
                        function (error) {
                            dialogs.error("Produto", error.data.message);
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
                    url:    $rootScope.apiPath + "api/produto/",
                    data:   $scope.entidade
                }).
                success(function() {
                    $location.url("produto/");
                }).
                error(function(data) {
                    dialogs.error("Produto", data.message);
                });                 
            };
            
            $scope.cancel = function () {
                var dlg = dialogs.confirm('Produto', 'Tem certeza que deseja cancelar?');
                dlg.result.then(function () {
                    $location.url("produto/");
                });                                
            };
            $scope.load();
        }
    ]);

})();