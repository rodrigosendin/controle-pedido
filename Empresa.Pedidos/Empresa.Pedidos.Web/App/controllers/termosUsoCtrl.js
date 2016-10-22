(function () {
    'use strict';

    angular.module('app').controller('termosUsoCtrl', ['$scope', '$location', 'authService', 
        function ($scope, $location, authService) {

            $scope.naoAceitar = function () {
                authService.logout();
                $location.url("/login");
            };

        }
    ]);

})();