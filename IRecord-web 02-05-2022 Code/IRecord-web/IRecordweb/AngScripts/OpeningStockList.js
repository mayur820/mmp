var app = angular.module("myApp", ["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.getinfo();
        //  $scope.GetLoadDeatils();
        //$scope.GetAllDemate();
        //$scope.GetAllConsultant();
        //$scope.getDbscript();
        $scope.page = 1;
    }

    $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        //$scope.displayItems = $scope.totalItems.slice(startPos, startPos + 3);
        console.log($scope.page);
    };

    $scope.getinfo = function () {

        // fnGetDataUsingGetRequestWithModel("/ReceiptPaymentEntry/GetListofReceiptPay", "Info", $scope, $http);
        $http({
            method: "get",
            url: "/OpeningStock/GetListofOpeningStock"
        }).then(function (response) {
            $scope.Info = JSON.parse(response.data);


        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        })
    }

    $scope.getDbscript = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/Get_All_Scprit", "scriptmaster", $scope, $http);
    }

    $scope.GetLoadDeatils = function () {
        $http({
            method: "get",
            url: "/OpeningStock/GetOpDetailsdt"
        }).then(function (response) {
            $scope.Details = JSON.parse(response.data);

        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            //alert("Error Occur During This Request" + geturl);
        })


        // fnGetDataUsingGetRequestWithModel("/OpeningStock/GetOpDetailsdt", "Details", $scope, $http);

    }
    $scope.GetAllConsultant = function () {
        fnGetDataUsingGetRequestWithModel("/OpeningStock/GetAllConsultant", "Consultants", $scope, $http);

    }
    $scope.GetAllDemate = function () {
        fnGetDataUsingGetRequestWithModel("/OpeningStock/GetAllDemate", "Demates", $scope, $http);
        console.log($scope.Demates)
    }
    $scope.Isdeleted = function (datas) {
        var r = confirm("Are you sure delete this item");
        if (r == true) {
            //alert(data.Trans_no)
            $http({
                method: "get",
                url: "/ReceiptPaymentEntry/DeleteHeadPart?Trans_No=" + datas.Trans_no
            }).then(function (response) {
                $scope.getinfo();

            }, function (data) {
                //deferred.reject({ message: "Really bad" });
                alert("Error Occur During This Request" + geturl);
            })


        } else {

        }

        //fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetListofCn", "Info", $scope, $http);

    }


});

