(function() {
    'use strict';

    angular.module('app').service('authService', ['$http', '$rootScope', '$route', '$window', 
        function ($http, $rootScope, $route, $window) {
                                  
            function addRoute(path, route) {
                $route.routes[path] = angular.extend({
                    reloadOnSearch: true
                },
                route,
                path && pathRegExp(path, route));

                // create redirection for trailing slashes
                if (path) {
                    var redirectPath = (path[path.length - 1] == '/') ? path.substr(0, path.length - 1) : path + '/';

                    $route.routes[redirectPath] = angular.extend({
                        redirectTo: path
                    },
                    pathRegExp(redirectPath, route));
                }

                return this;
            };

            function pathRegExp(path, opts) {
                var insensitive = opts.caseInsensitiveMatch,
                    ret = {
                        originalPath: path,
                        regexp: path
                    },
                    keys = ret.keys = [];

                path = path.replace(/([().])/g, '\\$1')
                    .replace(/(\/)?:(\w+)([\?\*])?/g, function (_, slash, key, option) {
                        var optional = option === '?' ? option : null;
                        var star = option === '*' ? option : null;
                        keys.push({
                            name: key,
                            optional: !!optional
                        });
                        slash = slash || '';
                        return '' + (optional ? '' : slash) + '(?:' + (optional ? slash : '') + (star && '(.+?)' || '([^/]+)') + (optional || '') + ')' + (optional || '');
                    })
                    .replace(/([\/$\*])/g, '\\$1');

                ret.regexp = new RegExp('^' + path + '$', insensitive ? 'i' : '');
                return ret;
            };

            return {               
                checkServer: function (success, error) {
                    $http.get($rootScope.apiPath + 'api/environment')
                      .success(function (data, status, headers, config) {
                          success(JSON.parse(data));
                      })
                      .error(function (data, status, headers, config) {
                          error();
                      });
                },
                
                login: function(user, success, error) {
                    //var postString = 'grant_type=password&username=' + user.login + '&password=' + encodeURIComponent(user.password);
                    var loginRequest = {
                        login: user.login,
                        senha: encodeURIComponent(user.password)
                    }
                    $http.post($rootScope.apiPath + 'api/Token', loginRequest)
                      .success(function (data, status, headers, config) {
                          debugger;
                          $window.sessionStorage.token = data;
                          
                          $window.sessionStorage.userLogin = user.login;
                          $window.sessionStorage.userLogin = user.login;
                          $window.sessionStorage.userNome = user.login;
                          $rootScope.userLogin = $window.sessionStorage.userLogin;
                          $rootScope.userNome = $window.sessionStorage.userNome;
                          $rootScope.isLoggedIn = true;
                          
                          success();
                      })
                      .error(function (data, status, headers, config) {
                          delete $window.sessionStorage.token;
                          delete $rootScope.userLogin;
                          delete window.sessionStorage.userLogin;
                          delete $rootScope.userNome;
                          delete window.sessionStorage.userNome;
                          delete $rootScope.isLoggedIn;
                          error('Invalid user or password');
                      });
                },
                
                buscaMenu: function(success, error) {
                    $http.get($rootScope.apiPath + 'api/menu')
                      .success(function (data, status, headers, config) {
                          $rootScope.menus = [];

                          // registrando novas rotas e incluindo itens de menu
                          angular.forEach(data, function (grupo) {

                              var menuGrupo = { text: grupo.grupo, items: [] };

                              angular.forEach(grupo.itens, function (item) {

                                  // Se tiver controller, registra a rota
                                  if (item.controller) {
                                      addRoute(item.path, { templateUrl: item.templateUrl, controller: item.controller, isPrivate: true });
                                  }

                                  // Se tiver a descricao de menu, incluí no menu
                                  if (item.menu) {
                                      menuGrupo.items.push({ path: item.path, text: item.menu });
                                  }
                              });

                              $rootScope.menus.push(menuGrupo);
                          });
                          success();
                      })
                      .error(function (data, status, headers, config) {
                          error(data.message);
                      });
                },

                isLoggedIn: function() {
                    return $window.sessionStorage.token != undefined;
                },

                logout: function() {
                    delete $window.sessionStorage.token;
                    delete $rootScope.userLogin;
                    delete window.sessionStorage.userLogin;
                    delete $rootScope.userNome;
                    delete window.sessionStorage.userNome;
                    delete $rootScope.isLoggedIn;
                },
                
                trocaSenha: function(trocaSenha, success, error) {                    
                    $http.post($rootScope.apiPath + 'api/trocasenha', trocaSenha)
                      .success(function (data, status, headers, config) {
                          success();                          
                      })
                      .error(function (data, status, headers, config) {                          
                          error(data.Message);
                      });
                },
            };
        }
    ]);

})();