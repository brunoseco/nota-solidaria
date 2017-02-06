
angular
    .module('nfsolidariaApp.directives')
    .directive('myDirectory', ['$parse', function ($parse) {

        function link(scope, element, attrs) {
            var model = $parse(attrs.myDirectory);
            console.log("afasdadasdsdasds")
            element.on('change', function (event) {
                scope.data = [];
                model(scope, { file: event.target.files });

            });
        };

        return {
            link: link
        }
    }]);

angular
    .module('nfsolidariaApp.directives')
    .directive('currency', function ($filter) {
    return {
        require: 'ngModel',
        link: function (elem, $scope, attrs, ngModel) {
            ngModel.$formatters.push(function (val) {
                return $filter('currency')(val)
            });

            ngModel.$parsers.push(function (val) {
                return $filter('currency')(val, '')
            });
        }
    }
})