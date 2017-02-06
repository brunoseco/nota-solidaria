
angular.module('nfsolidariaApp.utils')
    .factory('LoadingService', LoadingService);

LoadingService.$inject = ['$ionicLoading'];

function LoadingService($ionicLoading) {

    return {
        show: _show,
        hide: _hide
    }

    function _show() {
        $ionicLoading.show({
            template: '<ion-spinner icon="android" class="ion-refreshing"></ion-spinner>',
        });
    }

    function _hide() {
        $ionicLoading.hide();
    }

};