(function () {
    'use strict';
    
    angular.module('app').provider('menuProvider', [function () {
        
        var menus = [
            { 
                "text":     "Principal",
                "items":    [{
                        "text":     "Clientes",
                        "path":     "/cliente"
                    },{
                        "text":     "Produtos",
                        "path":     "/produto"
                    },{
                        "text":     "Pedidos",
                        "path":     "/pedido"
                    }
                ]
            },
        ];
        
        this.$get = function () {
            return {
                menus: menus
            };
        };
        
    }]);
})();