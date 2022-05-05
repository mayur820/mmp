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

        fnGetDataUsingGetRequestWithModel("/AccountMaster/GetListofAccount", "Info", $scope, $http);
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

