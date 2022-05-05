var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        //$scope.getinfo();
        $scope.getDetails();


        //$scope.GetEmps();
        //alert("hii");
    }
   

    //};
    //$scope.getinfo = function () {
    //    fnGetSingleDataUsingGetRequestWithModel("/BrokerBillEntry/Get_Header", "Info", $scope, $http);

    //}
    $scope.getDetails = function () {
        fnGetDataUsingGetRequestWithModel("/BankStatementImport/Get_Details", "details", $scope, $http);

    }
   

});

