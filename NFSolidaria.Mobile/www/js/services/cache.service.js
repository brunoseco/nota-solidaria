
angular.module('nfsolidariaApp.services')
    .service('CacheService', CacheService);

function CacheService() {

    var init = function (o) {

        var nomeLocalStorage = o;

        this.Get = _get;
        this.Add = _add;
        this.Update = _update;
        this.Remove = _remove;

        var self = this;

        function _get(value, field) {
            if (value !== undefined && field !== undefined)
                return _getByValue(value, field);
            var data = window.localStorage.getItem(nomeLocalStorage);
            data = JSON.parse(data);
            return data;

        };

        function _getByValue(value, field) {
            var data = _get();

            if (data != null)
                for (var i = 0 ; i < data.length ; i++) {
                    if (data[i][field] == value) {
                        return data[i];
                    }
                }

            return null;
        };

        function _add(data) {
            sendData = JSON.stringify(data);
            window.localStorage.setItem(nomeLocalStorage, sendData);
        };

        function _update(data) {
            sendData = JSON.stringify(data);
            window.localStorage.setItem(nomeLocalStorage, sendData);
        };

        function _remove() {
            window.localStorage.removeItem(nomeLocalStorage);
        };

    }

    this.clear = function () {
        window.localStorage.clear();
    }

    this.obj = function (o) {
        return new init(o);
    }

};