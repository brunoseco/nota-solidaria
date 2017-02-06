
angular
    .module('nfsolidariaApp.controllers')
    .controller('SharedController', SharedController);

SharedController.$inject = ['AccountService', '$state', '$ionicPopover', '$ionicHistory', '$scope', '$timeout', '$ionicModal', 'CacheService', 'Api', '$ionicPopup', '$filter', '$cordovaStatusbar']

function SharedController(AccountService, $state, $ionicPopover, $ionicHistory, $scope, $timeout, $ionicModal, CacheService, Api, $ionicPopup, $filter, $cordovaStatusbar) {

    if (!AccountService.IsLogged())
        return;

    var vm = this;

    ionic.Platform.ready(function () {
        if (ionic.Platform.platform() != "win32") {
            $cordovaStatusbar.overlaysWebView(true);
            $cordovaStatusbar.style(1);
            $cordovaStatusbar.styleHex('#009BDB');
        }
    });

    $ionicPopover.fromTemplateUrl('views/shared/popover-menu.html', {
        scope: $scope
    }).then(function (popover) {
        vm.PopOverMenu = popover;
    });

    vm.DoBack = function () {
        $ionicHistory.goBack();
    }

    $timeout(function () {

        vm.OpenPopOverMenu = function ($event) {
            vm.PopOverMenu.show($event);
        };

        vm.SignOut = function () {
            vm.PopOverMenu.hide();
            AccountService.SignOut();
        };

        vm.EditPerfil = function () {
            vm.PopOverMenu.hide();
            vm.modalEditPerfil.show();
            vm.Usuario = new CacheService.obj("usuario").Get();
            vm.Usuario.DataNascimento = $filter('date')(vm.Usuario.DataNascimento, "dd/MM/yyyy")
        };

        vm.SavePerfil = function (usuario) {

            var cupomApi = new Api.resourse("Usuario");
            cupomApi.Data = usuario;
            cupomApi.SuccessHandle = function (res) {

                var usuarioCache = new CacheService.obj("usuario");
                usuarioCache.Add(res.Data);

                $ionicPopup.confirm({
                    title: 'Sucesso',
                    cssClass: '',
                    template: 'Perfil atualizado com sucesso',
                    buttons: [
                        {
                            text: 'Fechar',
                            type: 'button-block button-stable',
                            onTap: function (e) {
                                vm.modalEditPerfil.hide();
                                $state.go('home.cupom');
                            }
                        },
                    ]
                });

            };

            cupomApi.Post();
        }

        $ionicModal.fromTemplateUrl('views/perfil/edit.html', {
            animation: 'slide-in-up',
            scope: $scope,
            focusFirstInput: true,
        }).then(function (modal) {
            vm.modalEditPerfil = modal;
        });

    }, 1500)
}