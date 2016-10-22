(function() {
    'use strict';

    angular.module('app').directive('kiteResize', ['$window',
        function($window) {
            return function (scope) {
                var w = angular.element($window);
                scope.getWindowDimensions = function() {
                    return { 'h': w.height(), 'w': w.width() };
                };
                scope.$watch(scope.getWindowDimensions, function(newValue) {
                    scope.windowHeight = newValue.h;
                    scope.style = function (margem) {
                        if (!margem) margem = 0;
                        return {
                            'height': (newValue.h - 160 - margem) + 'px',
                        };
                    };

                }, true);

                w.bind('resize', function () {
                    scope.$apply();
                });
            };
        }
    ]);

    angular.module('app').directive('kiteGridResize', ['$window',
        function($window) {
            return function(scope, element) {
                var w = angular.element($window);
                scope.getWindowDimensions = function() {
                    return { 'h': w.height(), 'w': w.width() };
                };
                scope.$watch(scope.getWindowDimensions, function (newValue) {
                    var grid = element.data("kendoGrid");
                    if (grid) {

                        var tamanhoPagina = Math.floor((newValue.h - 240) / 27) - 1;
                        grid.dataSource.query({
                            page: 1,
                            pageSize: tamanhoPagina,
                            //sort: { field: "_id", dir: "asc" }
                        });                        
                    }

                }, true);

                w.bind('resize', function() {
                    scope.$apply();
                });
            };
        }
    ]);


})();