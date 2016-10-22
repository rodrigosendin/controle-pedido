(function () {
    'use strict';
    
    var app = angular.module('app');

    app.controller('pedidoListCtrl', ['$rootScope', '$scope', 'dialogs', '$http', 'resources',
        function ($rootScope, $scope, dialogs, $http, resources) {

            $scope.load = function () {
                $scope.consulta();
            };
            
            $scope.consulta = function () {
                resources.pedido.query().$promise.then(function (dados) {
                    $scope.entidades = dados;
                });                
            };
            
            $scope.selecionar = function(entidade) {
                $scope.entidadeSelecionada = entidade;
            };
            
            $scope.remove = function () {
                var dlg = dialogs.confirm('Pedido', 'Confirma a Exclusão desse item?');
                dlg.result.then(function () {
                    $http({
                        method: 'DELETE',
                        url: $rootScope.apiPath + "api/Pedido/" + $scope.entidadeSelecionada.id
                    }).
                    success(function () {
                        $scope.consulta();
                    }).
                    error(function (data) {
                        dialogs.error("Pedido", data.message);
                    });
                });
            };            
                        
            $scope.abaterEstoque = function () {
                var dlg = dialogs.confirm('Pedido', 'Confirma que deseja abater o estoque desse Pedido?');
                dlg.result.then(function () {
                    $http({
                        method: 'POST',
                        url: $rootScope.apiPath + "api/PedidoAbateEstoque/" + $scope.entidadeSelecionada.id
                    }).
                    success(function () {
                        $scope.consulta();
                    }).
                    error(function (data) {
                        dialogs.error("Pedido", data.message);
                    });
                });
            };

            $scope.load();
        }
    ]);

})();