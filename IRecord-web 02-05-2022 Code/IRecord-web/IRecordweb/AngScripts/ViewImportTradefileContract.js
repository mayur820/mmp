var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.getinfo();
      
    }

    $scope.getinfo = function () {

        $http({
            method: "get",
            url: "/ImportTradefileContract/Get_Info"
        }).then(function (response) {
            $scope.Info= JSON.parse(response.data);
            $scope.getTredeDll_Equity();
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        })
        //fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Info", "Info", $scope, $http);

    }
    $scope.getTredeDll_Equity = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Equity?BrokerBill_Name_ID=" + $scope.Info.Broker, "Equityddl", $scope, $http);

    }
    $scope.getTredeDll_F_N_O = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Header", "Info", $scope, $http);

    }
    $scope.getTredeDll_MCX = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Header", "Info", $scope, $http);

    }
    $scope.getTredeDll_Currency = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Header", "Info", $scope, $http);

    }
    $scope.getTredeDll_NCDEX = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Header", "Info", $scope, $http);

    }



    $scope.getDetails = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/Get_Details", "details", $scope, $http);

    }
    //$scope.fnsubmit = function () {

    //    fnPostData_Msg_UsingPostRequestWithModel("/BrokerBillEntry/SaveAllDeailts", "msg", $scope, $http, "Insert Data Successfully", "/BrokerBillEntry/SaveBREntry");

    //}

    $scope.SubmitData = function () {
        //  fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/SaveBREntry", "SaveBREntry", $scope, $http);
        window.location.href = "/ImportTradefileContract/ImportTradefileExpense";
     }
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

