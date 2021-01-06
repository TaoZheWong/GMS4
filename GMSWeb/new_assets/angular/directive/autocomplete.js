

app.directive('customAutoComplete', function ($http, $filter) {

    return {
        scope: {
            keywordLength: '@',
            src: '=',
            tags: '=',
            hiddenValue: '=',
            searchObject : '='
        },
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {

            function gettingSource(req, resp) {
                if (scope.tags) {
                    resp($filter('filter')(scope.tags, { 'label': ngModel.$modelValue }))
                } else {
                    if (!scope.src) {
                        alert("src attribute is need in autocomplete directive");
                    } else {
                        $http({
                            url: scope.src,
                            data: scope.searchObject,
                            dataType: 'JSON',
                            contentType: 'application/json; characterset=utf-8',
                            method: 'POST'
                        }).then(function (result) {
                            result.data = result.data.hasOwnProperty('d') ? result.data.d : result.data;
                            resp(result.data.Params.data);
                        });
                    }
                }
            }

            $(element).autocomplete({
                minLength: scope.keywordLength ? scope.keywordLength : 0,
                source: function (req, resp) {
                    gettingSource(req, resp);
                },
                focus: function (event, ui) {
                    this.value = ui.item.label;
                    return false;
                },
                select: function (event, ui) {
                    this.value = ui.item.label;
                    ngModel.$setViewValue(ui.item.label);
                    ngModel.$render();
                    if (angular.isDefined(scope.hiddenValue)) {
                        scope.hiddenValue = ui.item.value;
                    }
                    scope.$apply();
                    return false;
                },
                close: function (event, ui) {
                    //event when complete list close;
                }
            }).keyup(function(e) {
                //when search input change, reset the hidden value to null;
                if(e.keyCode != 13 && scope.hiddenValue)
                    scope.hiddenValue = "";
            }).focus(function (e) {
                e.target.value = "";
                ngModel.$setViewValue("");
                ngModel.$render();
                if (angular.isDefined(scope.hiddenValue))
                    scope.hiddenValue = "";
                scope.$apply();
                $(this).autocomplete("search");
            }).focusout(function (e) {
             /*
                if (scope.hiddenValue && scope.hiddenValue == "") {
                    e.target.value = "";
                    ngModel.$setViewValue("");
                    ngModel.$render();
                }*/
            });
        }
    };
});