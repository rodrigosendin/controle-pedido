(function() {
    'use strict';

    angular.module('app').directive('isPrivate', ['$rootScope',
        function($rootScope) {
            return {
                restrict: 'A',
                link: function (scope, element) {
                    var prevDisp = element.css('display');
                    
                    $rootScope.$watch('isLoggedIn', function (isLoggedIn) {
                        if (!isLoggedIn) {
                            element.css('display', 'none');
                        } else {
                            element.css('display', prevDisp);
                        }
                    });
                }
            };
        }
    ]).directive('onlyAnon', ['$rootScope',
        function ($rootScope) {
            return {
                restrict: 'A',
                link: function (scope, element) {
                    var prevDisp = element.css('display');

                    $rootScope.$watch('isLoggedIn', function (isLoggedIn) {
                        if (!isLoggedIn) {
                            element.css('display', prevDisp);
                        } else {
                            element.css('display', 'none');                            
                        }
                    });
                }
            };
        }
    ]).directive("loader", function ($rootScope) {
        return function ($scope, element, attrs) {
            $scope.$on("loader_show", function () {
                return element.show();
            });
            return $scope.$on("loader_hide", function () {
                return element.hide();
            });
        };
    }
    );

})();