var app = angular.module("myApp", ["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.getinfo();
        $scope.page = 1;

    }
    $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        //$scope.displayItems = $scope.totalItems.slice(startPos, startPos + 3);
        console.log($scope.page);
    };

    $scope.getinfo = function () {

        fnGetDataUsingGetRequestWithModel("/ConsultantMaster/GetListofConsultant", "Info", $scope, $http);

    }



});

