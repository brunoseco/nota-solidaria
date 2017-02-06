angular
    .module('nfsolidariaApp.controllers')
    .controller('FavoritaController', FavoritaController)

FavoritaController.$inject = ['$scope', 'Api', 'AccountService', '$timeout', 'CacheService']

function FavoritaController($scope, Api, AccountService, $timeout, CacheService) {

    if (!AccountService.IsLogged())
        return;

    var vm = this;

    var usuarioCache = new CacheService.obj("usuario").Get();

    vm.UsuarioEntidadeFavoritas = [];
    vm.Entidades = [];

    vm.PageIndex = 0;
    vm.PodePaginar = false;
    vm.NomePesquisa = "";

    $timeout(function () {
        carregarUsuarioEntidadeFavoritas(function () { });

        carregarEntidades(function () {
            $scope.$broadcast('scroll.infiniteScrollComplete')
        })
    }, 1000);

    vm.RemoveUsuarioEntidadeFavorita = function (_entidadeId) {
        var usuarioEntidadeFavoritaApi = new Api.resourse("UsuarioEntidadeFavorita");

        usuarioEntidadeFavoritaApi.Filter.UsuarioId = usuarioCache.UsuarioId;
        usuarioEntidadeFavoritaApi.Filter.EntidadeId = _entidadeId;

        usuarioEntidadeFavoritaApi.SuccessHandle = function (data) {

            carregarEntidades(function () {
                $scope.$broadcast('scroll.infiniteScrollComplete')
            })

            vm.UsuarioEntidadeFavoritas = vm.UsuarioEntidadeFavoritas.filter(function (item) {
                return item.EntidadeId != _entidadeId;
            });
        };

        usuarioEntidadeFavoritaApi.Delete();
    }

    vm.AdicionaUsuarioEntidadeFavorita = function (_entidadeId) {
        var usuarioEntidadeFavoritaApi = new Api.resourse("UsuarioEntidadeFavorita");

        usuarioEntidadeFavoritaApi.Data = {
            UsuarioId: usuarioCache.UsuarioId,
            EntidadeId: _entidadeId
        };

        usuarioEntidadeFavoritaApi.SuccessHandle = function (data) {
            carregarUsuarioEntidadeFavoritas(function () {
                vm.Entidades = vm.Entidades.filter(function (item) {
                    return item.EntidadeId != _entidadeId;
                });
            })

        };

        usuarioEntidadeFavoritaApi.Post();
    }

    vm.carregarEntidadesAntigos = function () {
        vm.PageIndex++;

        carregarEntidades(function () {
            $scope.$broadcast('scroll.infiniteScrollComplete')
        })
    }

    vm.carregarUsuarioEntidadeFavoritaNovas = function () {
        vm.PageIndex = 0;

        carregarUsuarioEntidadeFavoritas(function () { });

        carregarEntidades(function () {
            $scope.$broadcast('scroll.refreshComplete')
        })
    }

    function carregarUsuarioEntidadeFavoritas(onDone) {

        var usuarioEntidadeFavoritaApi = new Api.resourse("UsuarioEntidadeFavorita");

        usuarioEntidadeFavoritaApi.Filter.UsuarioId = usuarioCache.UsuarioId;
        usuarioEntidadeFavoritaApi.Filter.OrderFields = ['Entidade.Usuario.Nome'];
        usuarioEntidadeFavoritaApi.Filter.IsPaginate = false;
        usuarioEntidadeFavoritaApi.Filter.OrderByType = 0;
        usuarioEntidadeFavoritaApi.Filter.Nome = vm.NomePesquisa;

        usuarioEntidadeFavoritaApi.SuccessHandle = function (data) {
            vm.UsuarioEntidadeFavoritas = data.DataList;
            onDone();
        };

        usuarioEntidadeFavoritaApi.Get();
    }

    function carregarEntidades(onDone) {

        var entidadeApi = new Api.resourse("Entidade");

        entidadeApi.Filter.OrderFields = ['Usuario.Nome'];
        entidadeApi.Filter.OrderByType = 0;
        entidadeApi.Filter.PageIndex = vm.PageIndex;
        entidadeApi.Filter.NaoFavoritadaPeloUsuarioId = usuarioCache.UsuarioId;
        entidadeApi.Filter.Nome = vm.NomePesquisa;

        entidadeApi.SuccessHandle = function (data) {
            vm.Entidades = vm.PageIndex > 0 ? data.DataList.concat(vm.Entidades) : data.DataList;
            vm.PodePaginar = vm.Entidades.length < data.Summary.Total;
            onDone();
        };

        entidadeApi.Get();
    }

}