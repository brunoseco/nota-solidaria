
angular
    .module('nfsolidariaApp', [
        'ionic',
        'nfsolidariaApp.config',
        'nfsolidariaApp.controllers',
        'ngCordova',
        'naif.base64'
    ]);

angular
    .module('nfsolidariaApp.config', []);

angular
    .module('nfsolidariaApp.services', [
        'crud.API',
        'ngCordova'
    ]);

angular
    .module('nfsolidariaApp.directives', []);

angular
    .module('nfsolidariaApp.utils', []);

angular
    .module('nfsolidariaApp.controllers', [
        'crud.API',
        'nfsolidariaApp.services',
        'ui.mask',
        'nfsolidariaApp.directives',
        'ui.utils.masks',
        'ngCordova',
    ]);