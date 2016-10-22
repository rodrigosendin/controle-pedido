(function () {
    "use strict";

    var app = angular.module("app");
    app.factory("resources", ['$rootScope', '$resource', 
        function ($rootScope, $resource) {

        return {
            cliente: $resource($rootScope.apiPath + "api/cliente/:id"),
            produto: $resource($rootScope.apiPath + "api/produto/:id"),
            pedido: $resource($rootScope.apiPath + "api/pedido/:id"),
            pedidoItem: $resource($rootScope.apiPath + "api/pedidoItem/:id"),
        	pedidoStatus: $resource($rootScope.apiPath + "api/pedidoStatus/:id"),
            usuario: $resource($rootScope.apiPath + "api/usuario/:id")
        };
      }
    ]);

})();