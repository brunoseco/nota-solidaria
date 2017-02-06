
angular.module('nfsolidariaApp.services')
    .factory('AccountService', AccountService);

AccountService.$inject = ['Api', 'CacheService', '$ionicPopup', '$state', 'config', '$ionicHistory', '$timeout'];

function AccountService(Api, CacheService, $ionicPopup, $state, config, $ionicHistory, $timeout) {

    return {
        SignIn: _signin,
        SignUp: _signup,
        SignOut: _signout,
        IsLogged: _verifyIsLogged
    }

    function _signout() {
        $ionicPopup.confirm({
            title: 'Sair',
            cssClass: '',
            template: 'Deseja realmente sair?',
            buttons: [
                {
                    text: 'Sair',
                    type: 'button-block button-assertive',
                    onTap: function (e) {
                        $ionicHistory.clearCache()
                        new CacheService.clear();
                        $state.go('signin');
                    }
                },
                {
                    text: 'Fechar',
                    type: 'button-block button-stable'
                }

            ]
        });
    }

    function _signin(Email, SenhaMD5) {

        var usuario = new Api.resourse("usuario");

        usuario.Data = {
            Email: Email,
            SenhaMD5: SenhaMD5,
            AttributeBehavior: "SignIn"
        }

        usuario.SuccessHandle = function (res) {

            var usuarioCache = new CacheService.obj("usuario");
            usuarioCache.Add(res.Data);

            var tokenCache = new CacheService.obj(config.NameStorageToken);
            tokenCache.Add(res.Data.LastToken);
            $state.go('home.cupom');

        };

        usuario.Post();

    }

    function _signup(dado) {

        var usuario = new Api.resourse("usuario");

        usuario.Data = dado;
        usuario.Data.AttributeBehavior = "SignUp";

        usuario.SuccessHandle = function (res) {

            var usuarioCache = new CacheService.obj("usuario");
            usuarioCache.Add(res.Data);

            var tokenCache = new CacheService.obj(config.NameStorageToken);
            tokenCache.Add(res.Data.LastToken);

            $state.go('home.cupom');

        };

        usuario.ErrorHandle = function (res) {
            $ionicPopup.alert({ title: 'Erro', cssClass: 'popup-danger', template: res.Errors[0], buttons: [{ text: 'OK', type: 'button-dark' }] });
        };

        usuario.Post();

    }

    function _verifyIsLogged() {

        var usuario = new CacheService.obj("usuario").Get();

        if (usuario == null)
            $timeout(function () {
                $state.go('signin');
            }, 1500)
        else
            return true;

    }

};