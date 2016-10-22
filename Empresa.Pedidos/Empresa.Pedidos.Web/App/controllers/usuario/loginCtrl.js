(function() {
    'use strict';

    angular.module('app').controller('loginCtrl', ['$scope', '$location', 'dialogs', 'authService',
        function ($scope, $location, dialogs, authService) {

            $scope.submit = function () {
                authService.login($scope.user,
                    function () {
                        $location.url("/login");
                    },
                    function (msg) {
                        dialogs.error("Login", msg);
                    }
                );
            };

        }
    ]);

})();