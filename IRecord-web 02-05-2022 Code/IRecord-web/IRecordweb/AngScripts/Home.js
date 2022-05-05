var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.bindstate();
        $scope.GetEmps();

    }
    $scope.FnInsertData = function () {
        fnPostDataUsingPostRequestWithModel("/Home/Insertdata", "EmpModel", $scope, $http);
       
    }
    $scope.bindstate = function () {
        fnGetDataUsingGetRequestWithModel("/Home/GetAllState", "liststates", $scope, $http);
    };
    $scope.bindcity = function (id) {
        fnGetDataUsingGetRequestWithModel("/Home/GetCityByid?id=" + id, "listCitys", $scope, $http);
    };
    $scope.GetValue = function () {
        $scope.bindcity($scope.EmpModel.Stateid);
    }
    $scope.Getcitytext = function () {
        var text = fnGetddlText("ddl_City")
        $scope.EmpModel.Address = text;
    }
    $scope.GetEmps = function () {
        fnGetDataUsingGetRequestWithModel("/Home/GetAllEmp", "listemps", $scope, $http);
    };
    $scope.fnclare = function () {
        $scope.EmpModel.Stateid = "";
        $scope.EmpModel.CityId = "";
        $scope.EmpModel.Name = "";
        $scope.EmpModel.Address = "";
    }

});

