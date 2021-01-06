
//Description : Directive for datepicker
app.directive('datepicker', function () {
    return {
        scope: true,
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {

            var checker = function () {
                var dateDiff = -999999999999999;

                if (angular.isUndefined(attrs.format))
                    attrs.format = "dd/mm/yyyy";

                $(element).datepicker({
                    format: attrs.format,
                    autoclose: !0
                });
            };

            $(element).change(function (event) {
                ngModel.$setViewValue(event.target.value);
            });

            
            checker();
        }
    };
});
