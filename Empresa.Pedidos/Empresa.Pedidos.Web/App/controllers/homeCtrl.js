(function () {
    "use strict";

    angular.module("app").controller("homeCtrl", ["$rootScope", "$scope", "$location", "$http", "resources",
        function ($rootScope, $scope, $location, $http, resources) {
            if (!$rootScope.isLoggedIn) {
                $location.url("/login");
            }
        }
    ]);

})();