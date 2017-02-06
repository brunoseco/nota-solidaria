angular
    .module('nfsolidariaApp.controllers')
    .controller('SignInController', SignInController)

SignInController.$inject = ['$scope', 'AccountService', '$ionicModal', '$state']

function SignInController($scope, AccountService, $ionicModal, $state) {

    var vm = this;

    if (AccountService.IsLogged()) {
        $state.go('home.cupom');
    }

    vm.SignInModel = {};
    vm.SignUpModel = {};

    $ionicModal.fromTemplateUrl('views/account/signup.html', {
        animation: 'slide-in-up',
        scope: $scope,
        focusFirstInput: false,
    }).then(function (modal) {
        vm.modalSignUp = modal;
    });

    vm.openModalSignUp = function () {
        vm.modalSignUp.show();
    };

    vm.closeModalSignUp = function () {
        vm.modalSignUp.hide();
    };

    vm.SignUp = function (dadosSignUp) {
        AccountService.SignUp(dadosSignUp);
        vm.closeModalSignUp();
    };

    vm.SignIn = function (dadosSignIn) {
        AccountService.SignIn(dadosSignIn.Email, dadosSignIn.SenhaMD5);
    };
}