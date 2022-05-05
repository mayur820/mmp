var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.getinfo();
        $scope.GetAllSAccount();
        $scope.SaveBREntryMap = {};
        $scope.Multimodel = [];

    }

    $scope.getinfo = function () {

        fnGetDataUsingGetRequestWithModel("/BrokerwiseFormatMap/GetListofBrokerFormat", "Info", $scope, $http);

    }
    $scope.GetAllSAccount = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerwiseFormatMap/GetAllAccountList", "Accounts", $scope, $http);

    }

    $scope.EditItems = function (data) {
        $scope.SaveBREntryMap.InvestmentType = data.InvestmentType;
        $scope.SaveBREntryMap.Sr_No = data.Sr_No;
        $scope.SaveBREntryMap.BrokerID = data.BrokerID;
        $scope.SaveBREntryMap.BrokerName = "Demo";
        $scope.SaveBREntryMap.uploadfileurl = $("#FileUpload").val();
        $scope.Multimodel.push($scope.SaveBREntryMap);

        $http({
            method: "post",
            url: "/BrokerwiseFormatMap/FnUpdateBrokerFormatMap",
            data: { 'JsonData': JSON.stringify($scope.Multimodel) }
        }).then(function (response) {
            alert("Update Data Successfully");
            window.location.href = "/BrokerwiseFormatMap/BrokerMappingList";
        }, function (data) {
            alert("Error Occur During This Request Of Post Data");
        })

    }
    $scope.Isdeleted = function (data) {
        var r = confirm("Are you sure delete this item");
        if (r == true) {
            $http({
                method: "get",
                url: "/ReceiptPaymentEntry/DeleteHeadPart?Trans_No=" + data.Trans_No
            }).then(function (response) {
                $scope.getinfo();

            }, function (data) {
                alert("Error Occur During This Request" + geturl);
            })

        } else {

        }
    }
});

