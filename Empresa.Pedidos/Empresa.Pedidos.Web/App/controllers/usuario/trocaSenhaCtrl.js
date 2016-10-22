(function() {
    'use strict';

    angular.module('app').controller('trocaSenhaCtrl', ['$scope', '$location', 'dialogs', 'authService',
        function ($scope, $location, dialogs, authService) {

            $scope.submit = function () {
                authService.trocaSenha($scope.trocaSenha,
                    function () {
                        $location.url("/");
                    },
                    function () {
                        dialogs.error("Troca de Senha", "N&atilde;o foi poss&iacute;vel completar a opera&ccedil;&atilde;o! Verifique se a senha antiga foi digitada corretamente.");
                    }
                );
            };

        }
    ]);

})();