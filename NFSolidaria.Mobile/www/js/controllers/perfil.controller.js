angular
    .module('nfsolidariaApp.controllers')
    .controller('PerfilController', CupomController)

CupomController.$inject = ['$scope', 'Api', 'AccountService', '$timeout', 'CacheService']

function CupomController($scope, Api, AccountService, $timeout, CacheService) {

    if (!AccountService.IsLogged())
        return;

    var vm = this;


}