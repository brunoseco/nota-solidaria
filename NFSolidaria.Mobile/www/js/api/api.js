
angular.module('crud.API', [
    'nfsolidariaApp.utils'
]);

angular.module('crud.API')
    .service('Api', Api);

Api.$inject = ['$http', '$httpParamSerializer', '$log', 'LoadingService', '$ionicPopup', '$ionicHistory', 'CacheService', '$state']

function Api($http, $httpParamSerializer, $log, LoadingService, $ionicPopup, $ionicHistory, CacheService, $state) {

    var init = function (o) {

        this.EndPoint = "http://192.168.137.1/api"
        this.Resourse = o;

        this.DefaultFilter = {
            PageSize: 10,
            PageIndex: 0,
            IsPaginate: true,
            QueryOptimizerBehavior: "",
        };

        this.EnableLoading = true;
        this.EnableErrorMessage = true;
        this.EnableLogs = false;
        this.Filter = {};
        this.Data = {};

        this.SuccessHandle = function (data) { return data; };
        this.ErrorHandle = function (err) { return err; };

        this.Get = _get;
        this.Post = _post;
        this.Put = _put;
        this.Delete = _delete;
        this.DataItem = _dataitem;
        this.Upload = _upload;

        var self = this;

        function _post() {

            ShowLoading();

            return $http
                .post(makeUri(), self.Data)
                .then(handleSuccess, handleError);
        }

        function _upload() {

            ShowLoading();

            var request = {
                method: 'POST',
                url: makeUri(),
                data: self.Data,
                headers: {
                    'Content-Type': undefined
                }
            };

            return $http(request)
                .then(handleSuccess, handleError);;
        }

        function _put() {

            ShowLoading();

            return $http
                .put(makeUri(), self.Data)
                .then(handleSuccess, handleError);
        }

        function _delete() {

            ShowLoading();

            return $http
                .delete(makeDeleteBaseUrl())
                .then(handleSuccess, handleError);
        }

        function _get() {

            ShowLoading();

            return $http
                .get(makeGetBaseUrl())
                .then(handleSuccess, handleError);
        }

        function _dataitem() {
            return $http
                .get(makeDataItemBaseUrl())
                .then(handleSuccess, handleError);
        }

        function dataPost() {
            return JSON.stringify(self.Data);
        }

        function queryStringFilter() {

            if (self.Filter.Id !== undefined)
                return self.Filter.Id;

            if (self.Filter.OrderFields !== undefined) {
                self.Filter.IsOrderByDynamic = true;
                if (self.Filter.OrderByType === undefined)
                    self.Filter.OrderByType = 1;
            }

            return String.format("?{0}", $httpParamSerializer(angular.merge({}, self.DefaultFilter, self.Filter)));
        }

        function makeGetBaseUrl() {
            return String.format("{0}/{1}", makeUri(), queryStringFilter());
        }

        function makeDataItemBaseUrl() {
            return String.format("{0}/GetDataItem/{1}", makeUri(), queryStringFilter());
        }

        function makeDeleteBaseUrl() {
            return String.format("{0}/?{1}", makeUri(), $httpParamSerializer(self.Filter));
        }

        function makeUri() {
            return String.format("{0}/{1}", self.EndPoint, self.Resourse)
        }

        function handleSuccess(response) {
            HideLoading();

            if (self.EnableLogs)
                $log.debug("sucesso na API >>", makeUri())

            self.SuccessHandle(response.data);
        }

        function handleError(err) {
            HideLoading();

            if (self.EnableLogs)
                $log.error("erro na API >>", makeUri())

            if (err.data == null) {
                $ionicPopup.alert({
                    title: 'Ops, ocorreu um erro!',
                    cssClass: '',
                    template: 'Verifique sua conexão com a internet',
                    buttons: [{
                        text: 'Fechar',
                        type: 'button-block button-stable'
                    }]
                });
                return;
            }

            if (err.data.StatusCode == 401) {
                $ionicHistory.clearCache()
                new CacheService.clear();
                $state.go('signin');
            }

            if (self.EnableErrorMessage)
                $ionicPopup.alert({
                    title: 'Ops, ocorreu um erro!',
                    cssClass: '',
                    template: err.data.Errors[0],
                    buttons: [{
                        text: 'Fechar',
                        type: 'button-block button-stable'
                    }]
                });

            self.ErrorHandle(err.data);
        }

        String.format = function () {
            var theString = arguments[0];
            for (var i = 1; i < arguments.length; i++) {
                var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
                theString = theString.replace(regEx, arguments[i]);
            }

            return theString;
        }

        function ShowLoading() {
            if (self.EnableLoading)
                LoadingService.show();
        }

        function HideLoading() {
            LoadingService.hide();
        }


    }

    this.resourse = function (o) {
        return new init(o);
    }

}
