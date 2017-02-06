
angular
    .module('nfsolidariaApp.config')
    .value("config", {
    baseUrl: "http://localhost:4602/api",
    NameStorageToken: "USER_AUTH_TOKEN"
});


angular
    .module('nfsolidariaApp.config')
    .factory('httpRequestInterceptor', function (config) {
    return {
        request: function (configs) {
            var item = localStorage.getItem(config.NameStorageToken);
            configs.headers['token'] = JSON.parse(item);
            return configs;
        }
    };
});

angular
    .module('nfsolidariaApp.config')
    .config(function ($httpProvider) {
    $httpProvider.interceptors.push('httpRequestInterceptor');
});