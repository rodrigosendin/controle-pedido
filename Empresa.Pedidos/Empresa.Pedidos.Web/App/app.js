(function () {
    'use strict';

    angular.module('app', ['ngRoute', 'ngResource', 'ngSanitize', 'ui.bootstrap', 'dialogs.main'])
        .config(function($routeProvider, $locationProvider, $httpProvider) {

            // ROTAS
            $routeProvider.when('/',                    { templateUrl: './app/templates/home.html',                 controller: 'homeCtrl',        isPrivate: false });
            $routeProvider.when('/401',                 { templateUrl: './app/templates/util/401.html',             controller: 'utilCtrl',        isPrivate: false });
            $routeProvider.when('/404',                 { templateUrl: './app/templates/util/404.html',             controller: 'utilCtrl',        isPrivate: false });
            $routeProvider.when('/503',                 { templateUrl: './app/templates/util/503.html',             controller: 'utilCtrl',        isPrivate: false });
            $routeProvider.when('/dbOffline',           { templateUrl: './app/templates/util/dbOffline.html',       controller: 'utilCtrl',        isPrivate: false });
            $routeProvider.when('/termosUso',           { templateUrl: './app/templates/util/termosUso.html',       controller: 'termosUsoCtrl',   isPrivate: false });
            $routeProvider.when('/login',               { templateUrl: './app/templates/usuario/login.html',        controller: 'loginCtrl',       isPrivate: false });
            $routeProvider.when('/trocaSenha',          { templateUrl: './app/templates/usuario/trocaSenha.html',   controller: 'trocaSenhaCtrl',  isPrivate: true  });            
            $routeProvider.when('/usuario',             { templateUrl: './app/templates/usuario/usuarioList.html',  controller: 'usuarioListCtrl', isPrivate: true  });
            $routeProvider.when('/usuario/add',         { templateUrl: './app/templates/usuario/usuarioEdit.html',  controller: 'usuarioEditCtrl', isPrivate: true  });
            $routeProvider.when('/usuario/edit/:id',    { templateUrl: './app/templates/usuario/usuarioEdit.html',  controller: 'usuarioEditCtrl', isPrivate: true  });            
            
            $routeProvider.when('/cliente', { templateUrl: './app/templates/cliente/clienteList.html', controller: 'clienteListCtrl', isPrivate: true });
            $routeProvider.when('/cliente/add', { templateUrl: './app/templates/cliente/clienteEdit.html', controller: 'clienteEditCtrl', isPrivate: true });
            $routeProvider.when('/cliente/edit/:id', { templateUrl: './app/templates/cliente/clienteEdit.html', controller: 'clienteEditCtrl', isPrivate: true });
            $routeProvider.when('/produto', { templateUrl: './app/templates/produto/produtoList.html', controller: 'produtoListCtrl', isPrivate: true });
            $routeProvider.when('/produto/add', { templateUrl: './app/templates/produto/produtoEdit.html', controller: 'produtoEditCtrl', isPrivate: true });
            $routeProvider.when('/produto/edit/:id', { templateUrl: './app/templates/produto/produtoEdit.html', controller: 'produtoEditCtrl', isPrivate: true });
            $routeProvider.when('/pedido', { templateUrl: './app/templates/pedido/pedidoList.html', controller: 'pedidoListCtrl', isPrivate: true });
            $routeProvider.when('/pedido/add', { templateUrl: './app/templates/pedido/pedidoEdit.html', controller: 'pedidoEditCtrl', isPrivate: true });
            $routeProvider.when('/pedido/edit/:id', { templateUrl: './app/templates/pedido/pedidoEdit.html', controller: 'pedidoEditCtrl', isPrivate: true });

            $routeProvider.otherwise({ redirectTo: '/404' });
            $locationProvider.html5Mode(true);
            $httpProvider.defaults.timeout = 1000;
                        
            // INTERCEPTADOR DO HTTP
            $httpProvider.interceptors.push(['$rootScope', '$q', '$window', '$location',
                function($rootScope, $q, $window, $location) {
                    var numLoadings = 0;

                    return {
                        request: function(config) {

                            // Mostra Loading em todos requests
                            numLoadings++;
                            $rootScope.$broadcast("loader_show");

                            // Incluí Token em todos os requests
                            config.headers = config.headers || {};
                            if ($window.sessionStorage.token) {
                                config.headers.Token = $window.sessionStorage.token;
                            }

                            return config;
                        },
                        requestError: function(request) {
                            if ((--numLoadings) === 0) {
                                // Esconde loading
                                $rootScope.$broadcast("loader_hide");
                            }

                            return $q.reject(request);
                        },
                        response: function(response) {
                            if ((--numLoadings) === 0) {
                                // Esconde loading
                                $rootScope.$broadcast("loader_hide");
                            }

                            return response || $q.when(response);
                        },
                        responseError: function(response) {
                            if ((--numLoadings) === 0) {
                                // Esconde loading
                                $rootScope.$broadcast("loader_hide");
                            }

                            // Essa é uma validação que vem do servidor. 
                            // Se o usuario não pode acessar determinado recurso, vai cair aqui
                            if (response && response.status === 401) {
                                delete $window.sessionStorage.token;
                                delete $rootScope.userLogin;
                                delete window.sessionStorage.userLogin;
                                delete $rootScope.userNome;
                                delete window.sessionStorage.userNome;
                                delete $rootScope.isLoggedIn;
                                $location.url('/login');
                            }
                            if (response && response.status === 404) {
                                $location.url('/404');
                            }
                            if (response && response.status === 503) {
                                $location.url('/503');
                            }
                            if (response && response.status >= 500) {
                            }
                            return $q.reject(response);
                        }
                    };
                }]);

            // Convertendo Datas
            // ISO 8601 Date Pattern: YYYY-mm-ddThh:MM:ss
            var dateMatchPattern = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/;

            var convertDates = function (obj) {
                for (var key in obj) {
                    if (!obj.hasOwnProperty(key)) continue;

                    var value = obj[key];
                    var typeofValue = typeof (value);

                    if (typeofValue === 'object') {
                        // If it is an object, check within the object for dates.
                        convertDates(value);
                    } else if (typeofValue === 'string') {
                        if (dateMatchPattern.test(value)) {
                            obj[key] = new Date(value);
                        }
                    }
                }
            }

            $httpProvider.defaults.transformResponse.push(function (data) {
                if (typeof (data) === 'object') {
                    convertDates(data);
                }
                return data;
            });
        })
        // VALIDAÇÃO DE USUÁRIO LOGADO
        .run(['$rootScope', '$location', '$window', 'authService', '$templateCache', 'menuProvider',
            function ($rootScope, $location, $window, authService, $templateCache, menuProvider) {

                $rootScope.apiPath = "http://localhost:59684/";

                // Atualiza info de usuário logado - no caso de um load da app com usuário logado
                $rootScope.isLoggedIn = authService.isLoggedIn();
                $rootScope.userLogin  = $window.sessionStorage.userLogin;
                $rootScope.userNome   = $window.sessionStorage.userNome;
                
                $rootScope.menus = menuProvider.menus;

                $rootScope.$on("$routeChangeStart", function(event, next, current) {

                    // Essa é uma validação no Client, para impedir navegar em uma rota privada (que exige login)
                    if (next.isPrivate && !$rootScope.isLoggedIn) {
                        delete $window.sessionStorage.token;
                        delete $rootScope.userLogin;
                        delete window.sessionStorage.userLogin;
                        delete $rootScope.userNome;
                        delete window.sessionStorage.userNome;
                        delete $rootScope.isLoggedIn;
                        $location.path('/login');
                    }

                    $rootScope.currentPath = $location.path();
                    if (current && current.$$route) {
                        $rootScope.previousPath = current.$$route.originalPath;
                    }
                });
            }]);
})();