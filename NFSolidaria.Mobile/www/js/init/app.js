

angular
    .module('nfsolidariaApp')
    .run(OnRun);

OnRun.$inject = ['$rootScope', '$state']

function OnRun($rootScope, $state) {
    $rootScope.$on('$stateChangeSuccess', function () {
        if ($state.current.url == "/favorita")
            $rootScope.tabactived = "favorita";
        else if ($state.current.url == "/cupom-criar")
            $rootScope.tabactived = "cupom-criar";
        else if ($state.current.url == "/cupom")
            $rootScope.tabactived = "cupom";
        else
            $rootScope.tabactived = "";
    });
}