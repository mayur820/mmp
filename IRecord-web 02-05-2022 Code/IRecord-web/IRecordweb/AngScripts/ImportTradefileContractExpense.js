var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.getExpanse();
        // $scope.getDetails();


        //$scope.GetEmps();
        //  alert("hii");
    }
    // $scope.btnupload = function () {

    //     let photo = document.getElementById("contractnotefile").files[0];
    //     let formData = new FormData();

    //     formData.append("photo", photo);
    //     fetch('/BrokerBillEntry/Upload', { method: "POST", body: formData });
    //     alert("File Upload successfully");
    //     $scope.btnupload = false;

    //};
    ////$scope.getinfo = function () {
    ////    fnGetSingleDataUsingGetRequestWithModel("/BrokerBillEntry/Get_Header","Info", $scope, $http);

    ////}
    $scope.getExpanse = function () {

        //$http(fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/Get_Expenses", "Exps", $scope, $http)).then(function (mRes) {

        //});
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Expenses", "Exps", $scope, $http);
       // window.location.href = "/ImportTradefileContract/ImportTradefileExpense";
    }
    $scope.fnsubmit = function () {

        fnPostData_Msg_UsingPostRequestWithModel("/ImportTradefileContract/SaveAllDeailts", "msg", $scope, $http, "Insert Data Successfully", "/BrokerBillEntry/SaveBREntry");

    }

    // $scope.btn_click_sumit = function () {
    //     fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/SaveBREntry", "SaveBREntry", $scope, $http);
    //     window.location.href = "/BrokerBillEntry/BRDetails";
    // }
    //$scope.bindstate = function () {
    //    fnGetDataUsingGetRequestWithModel("/Home/GetAllState", "liststates", $scope, $http);
    //};
    //$scope.bindcity = function (id) {
    //    fnGetDataUsingGetRequestWithModel("/Home/GetCityByid?id=" + id, "listCitys", $scope, $http);
    //};
    //$scope.GetValue = function () {
    //    $scope.bindcity($scope.EmpModel.Stateid);
    //}
    //$scope.Getcitytext = function () {
    //    var text = fnGetddlText("ddl_City")
    //    $scope.EmpModel.Address = text;
    //}
    //$scope.GetEmps = function () {
    //    fnGetDataUsingGetRequestWithModel("/Home/GetAllEmp", "listemps", $scope, $http);
    //};
    //$scope.fnclare = function () {
    //    $scope.EmpModel.Stateid = "";
    //    $scope.EmpModel.CityId = "";
    //    $scope.EmpModel.Name = "";
    //    $scope.EmpModel.Address = "";
    //}

});

