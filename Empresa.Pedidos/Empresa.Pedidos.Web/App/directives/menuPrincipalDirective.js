(function() {
    'use strict';

    angular.module('app').directive('menuPrincipal', [
        function() {
            return {
                restrict: 'A',
                link: function (scope, elem) {
                    scope.$watch('menus', function () {
                        elem.kendoPanelBar({
                            expandMode: "multiple"
                        });
                    });
                }
            };
        }]);
})();