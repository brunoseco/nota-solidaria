angular
    .module('nfsolidariaApp.controllers')
    .controller('CupomCriarController', CupomCriarController)

CupomCriarController.$inject = ['AccountService', 'Api', 'CacheService', '$ionicPopup', '$state', 'CameraService', '$cordovaCamera', '$filter'];


function CupomCriarController(AccountService, Api, CacheService, $ionicPopup, $state, CameraService, $cordovaCamera, $filter) {

    if (!AccountService.IsLogged())
        return;

    var vm = this;

    vm.Cupom = {
        TipoNota: "1"
    };

    vm.Imagens = [];
    vm.ImagesFromPC = [];
    vm.IsPc = ionic.Platform.platform() == "win32";

    carregaDataItemEntidades();

    vm.LerImagens = function () {
        var imagemCupomApi = new Api.resourse("ImageCupom");
        var image1 = vm.Imagens[0].replace(/^data:image\/(png|jpg);base64,/, "");
        imagemCupomApi.Data = {
            Imagem1: image1.replace(/^data:image\/(png|jpg);base64,/, "")
        };

        imagemCupomApi.SuccessHandle = function (data) {

            vm.Cupom.COO = data.Data.COO;
            vm.Cupom.Valor = data.Data.Valor;

            var _CNPJEmissor = document.getElementById("CNPJEmissor");
            _CNPJEmissor.value = data.Data.CNPJEmissor;
            angular.element(_CNPJEmissor).triggerHandler('input');

            var _dataCompra = document.getElementById("DataCompra");
            _dataCompra.value = $filter('date')(data.Data.DataCompra, "dd/MM/yyyy");
            angular.element(_dataCompra).triggerHandler('input');

        }

        imagemCupomApi.Post();
    };

    vm.LoadImagesFromPC = function () {
        vm.Imagens = [];
        vm.ImagesFromPC.forEach(function (value, index) {
            vm.Imagens.push(value.base64);
        });
        vm.ImagesFromPC = [];
    }

    vm.ImportarImagem = function () {
        var options = CameraService.getPictureOptions(0);
        carregaImagem(options);
    }

    vm.AbrirCamera = function () {
        var options = CameraService.getPictureOptions(1);
        carregaImagem(options);
    }

    vm.RemoverImagem = function (index) {
        vm.Imagens.splice(index, 1);
    }

    vm.SalvarNota = function (_cupom) {

        _cupom.EntidadeId = _cupom.Entidade.Id;
        _cupom.Entidade = null;

        var cupomApi = new Api.resourse("Cupom");

        cupomApi.Data = _cupom;

        cupomApi.SuccessHandle = function (data) {

            $ionicPopup.confirm({
                title: 'Sucesso',
                cssClass: '',
                template: 'Salvo, deseja lançar uma nota nova, ou visualizá-la?',
                buttons: [
                    {
                        text: 'Lançar Nova',
                        type: 'button-block button-balanced',
                        onTap: function (e) {
                            $state.go($state.current, {}, { reload: true })
                        }
                    },
                    {
                        text: 'Visualizar',
                        type: 'button-block button-calm',
                        onTap: function (e) {
                            $state.go('home.cupom');
                        }
                    }

                ]
            });

        };

        cupomApi.Post();

    };

    function carregaImagem(options) {

        $cordovaCamera.getPicture(options).then(function (data) {
            vm.Imagens.push(data);
        }, function () {
            
        });
    }

    function carregaDataItemEntidades() {
        var usuarioCache = new CacheService.obj("usuario").Get();
        var usuarioEntidadeFavoritaApi = new Api.resourse("UsuarioEntidadeFavorita");

        usuarioEntidadeFavoritaApi.Filter.UsuarioId = usuarioCache.UsuarioId;

        usuarioEntidadeFavoritaApi.SuccessHandle = function (data) {

            if (data.DataList.length == 0) {
                $ionicPopup.confirm({
                    title: 'Aviso',
                    cssClass: '',
                    template: 'Você ainda não selecionou nenhuma entidade como favorita.',
                    buttons: [
                        {
                            text: 'Cancelar',
                            type: 'button-block button-stable',
                            onTap: function (e) {
                                $state.go('home.cupom');
                            }
                        },
                        {
                            text: 'Favoritar agora',
                            type: 'button-block button-balanced',
                            onTap: function (e) {
                                $state.go('home.favorita');
                            }
                        }

                    ]
                });
            }

            vm.UsuarioEntidadeFavoritas = data.DataList;
        };

        usuarioEntidadeFavoritaApi.DataItem();
    }

};