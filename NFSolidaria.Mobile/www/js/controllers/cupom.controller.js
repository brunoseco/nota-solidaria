angular
    .module('nfsolidariaApp.controllers')
    .controller('CupomController', CupomController)

CupomController.$inject = ['$scope', 'Api', 'AccountService', '$timeout', 'CacheService', '$ionicModal', '$ionicPopup']

function CupomController($scope, Api, AccountService, $timeout, CacheService, $ionicModal, $ionicPopup) {

    if (!AccountService.IsLogged())
        return;

    var vm = this;

    vm.Cupons = [];
    vm.Cupom = {};

    vm.PageIndex = 0;
    vm.PodePaginar = false;
    vm.NomePesquisa = "";

    $timeout(function () {
        carregaCupons(function () {
            $scope.$broadcast('scroll.infiniteScrollComplete')
        })
    }, 1000);

    vm.carregarCuponsAntigos = function () {
        vm.PageIndex++;

        carregaCupons(function () {
            $scope.$broadcast('scroll.infiniteScrollComplete')
        })
    }

    vm.carregarCuponsNovos = function () {
        vm.PageIndex = 0;
        carregaCupons(function () {
            $scope.$broadcast('scroll.refreshComplete')
        })
    }
    
    vm.AbrirCupom = function (id) {
        vm.modalDetailsCupom.show()
        
        var cupom = new Api.resourse("cupom");
        cupom.Filter.Id = id;

        cupom.SuccessHandle = function (data) {
            vm.modalDetailsCupom.show();
            vm.Cupom = data.Data;
        };

        cupom.Get();
    }

    vm.CancelarCupom = function (id) {

        $ionicPopup.confirm({
            title: 'Confirmar',
            cssClass: '',
            template: 'Tem certeza que deseja cancelar este cupom?',
            buttons: [
                {
                    text: 'Cancelar',
                    type: 'button-block button-balanced',
                    onTap: function (e) {
                        cancela()
                    }
                },
                {
                    text: 'Fechar',
                    type: 'button-block button-stable',
                    onTap: function (e) {

                    }
                },
            ]
        });

        function cancela() {
            var cupom = new Api.resourse("cupom");

            cupom.Filter.CupomId = id;

            cupom.SuccessHandle = function (data) {

                $ionicPopup.confirm({
                    title: 'Sucesso',
                    cssClass: '',
                    template: 'Cupom foi cancelado com sucesso',
                    buttons: [
                        {
                            text: 'Fechar',
                            type: 'button-block button-stable',
                            onTap: function (e) {
                                vm.modalDetailsCupom.hide();
                                carregaCupons(function () {
                                    $scope.$broadcast('scroll.infiniteScrollComplete')
                                })
                            }
                        },

                    ]
                });

            };
            cupom.Delete();

        }
              

    }

    function carregaCupons(onDone) {

        var usuarioCache = new CacheService.obj("usuario").Get();
        var cupom = new Api.resourse("cupom");

        cupom.Filter.UsuarioId = usuarioCache.UsuarioId;
        cupom.Filter.OrderFields = ['DataLancamento'];
        cupom.Filter.PageIndex = vm.PageIndex;
        cupom.Filter.QueryOptimizerBehavior = "ListaCuponsAplicativo";
        cupom.Filter.COONomeEntidade = vm.NomePesquisa;

        cupom.SuccessHandle = function (data) {
            vm.Cupons = vm.PageIndex > 0 ? data.DataList.concat(vm.Cupons) : data.DataList;
            vm.PodePaginar = vm.Cupons.length < data.Summary.Total;
            onDone();
        };

        cupom.Get();
    }

    $ionicModal.fromTemplateUrl('views/cupom/details.html', {
        animation: 'slide-in-up',
        scope: $scope,
        focusFirstInput: true,
    }).then(function (modal) {
        vm.modalDetailsCupom = modal;
    });

}