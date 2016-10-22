(function () {
    'use strict';
    
    var app = angular.module('app');

    app.controller('pedidoEditCtrl', ['$rootScope', '$scope', 'dialogs', '$http', 'resources', '$routeParams', '$location',
        function ($rootScope, $scope, dialogs, $http, resources, $routeParams, $location) {

            $scope.load = function () { 
                resources.cliente.query().$promise.then(function (dados) {
                    $scope.clienteList = dados;                   
                });
                
                resources.pedidoStatus.query().$promise.then(function (dados) {
                    $scope.pedidoStatusList = dados;                   
                });
                
                resources.produto.query().$promise.then(function (dados) {
                    $scope.itensProdutoList = dados;
                });
                
                if ($routeParams.id) {
                     resources.pedido.get({ id: $routeParams.id }).$promise.then(
                        function (entidade) {
                            $scope.entidade = entidade;
                        },
                        function (error) {
                            dialogs.error("Pedido", error.data.message);
                        });
                } else {
                    $scope.entidade = {};
                }
                $scope.datePickerDataPedidoOpened = false;
                
                $scope.itensSelecionado = {};
            };
            
            $scope.save = function () {
                var metodo = "POST";
                if ($scope.entidade.id != undefined && $scope.entidade.id > 0) {
                    metodo = "PUT";
                }                
                $http({
                    method: metodo,
                    url:    $rootScope.apiPath + "api/pedido/",
                    data:   $scope.entidade
                }).
                success(function() {
                    $location.url("pedido/");
                }).
                error(function(data) {
                    dialogs.error("Pedido", data.message);
                });                 
            };
            
            $scope.cancel = function () {
                var dlg = dialogs.confirm('Pedido', 'Tem certeza que deseja cancelar?');
                dlg.result.then(function () {
                    $location.url("pedido/");
                });                                
            };

            $scope.openDatePickerDataPedido = function() {
                $scope.datePickerDataPedidoOpened = true;
            };
            
            $scope.novoItens = function () {
                $scope.itensSelecionado = {};
            };

            $scope.salvarItens = function () {
                if($scope.entidade.itens == undefined){
                    $scope.entidade.itens = [];
                }
                var index = $scope.entidade.itens.indexOf($scope.itensSelecionado);
                if (index == -1) {
                    $scope.entidade.itens.push($scope.itensSelecionado);
                    $scope.novoItens();
                }
            };

            $scope.removeItens = function () {
                var index = $scope.entidade.itens.indexOf($scope.itensSelecionado);
                if (index > -1) {
                    $scope.entidade.itens.splice(index, 1);
                    $scope.novoItens();
                }
            };

            $scope.selecionarItens = function(entidade) {
                $scope.itensSelecionado = entidade;
            };            
            
            $scope.load();
        }
    ]);

})();