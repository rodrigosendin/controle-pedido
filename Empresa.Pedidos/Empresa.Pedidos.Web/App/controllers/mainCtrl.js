(function() {
    'use strict';

    angular.module('app').controller('mainCtrl', ['$scope', '$location', 'dialogs', 'authService',
        function ($scope, $location, dialogs, authService) {

            $scope.logout = function() {
                var dlg = dialogs.confirm('Logout', 'Confirma o Logout?');
                dlg.result.then(function() {
                    authService.logout();
                    $location.url('/login');
                });
            };            
        }
    ]);

})();