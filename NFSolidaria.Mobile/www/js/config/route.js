angular
    .module('nfsolidariaApp.config')
    .config(RouteConfig);

RouteConfig.$inject = ['$stateProvider', '$urlRouterProvider'];


function RouteConfig($stateProvider, $urlRouterProvider) {
    $stateProvider

        .state('signin', {
            url: '/signin',
            templateUrl: 'views/account/signin.html',
            controller: 'SignInController as vm'
        })
        
        .state('home', {
            url: "/home",
            abstract: true,
            templateUrl: "views/shared/home.html",
            controller: 'SharedController as vm'
        })

        .state('home.cupom', {
            url: '/cupom',
            views: {
                'tab-content': {
                    templateUrl: 'views/cupom/index.html',
                    controller: 'CupomController as vm'
                }
            },
        })

        .state('home.cupom-criar', {
            url: '/cupom-criar',
            views: {
                'tab-content': {
                    templateUrl: 'views/cupom/create.html',
                    controller: 'CupomCriarController as vm'
                }
            },
        })

        .state('home.favorita', {
            url: '/favorita',
            views: {
                'tab-content': {
                    templateUrl: 'views/favorita/index.html',
                    controller: 'FavoritaController as vm'
                }
            },
        })


    $urlRouterProvider.otherwise('/signin');

};