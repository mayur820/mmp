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

        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetListofCn", "Info", $scope, $http);

    }
    $scope.Isdeleted = function (data) {
        var r = confirm("Are you sure delete this item");
        if (r == true) {
            // alert(data.Trans_no)
            $http({
                method: "get",
                url: "/BrokerBillEntry/DeleteHeadPart?Trans_no=" + data.Trans_no
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

