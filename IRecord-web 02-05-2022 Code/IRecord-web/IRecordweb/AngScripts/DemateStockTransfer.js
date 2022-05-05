var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {

        $scope.GetAllDemate();

        $scope.GetAllConsultant();

    }



    $scope.FILLGRIDVIEW = function () {
        fnGetDataUsingGetRequestWithModel("/dematstocktransfer/GetGridDAta?DemateId=" + $scope.detail.ddl_from_Demat_id, "GRIDDATA", $scope, $http);
    };
    $scope.GetAllConsultant = function () {
        fnGetDataUsingGetRequestWithModel("/dematstocktransfer/GetAllConsultant", "AllConsultant", $scope, $http);
    };



    $scope.GetInvestment = function () {
        //alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/dematstocktransfer/GetInvestment", "AllInvestment", $scope, $http);
    };
    $scope.GetAllDemate = function () {
        fnGetDataUsingGetRequestWithModel("/dematstocktransfer/GetAllDemate", "FILLALLDEMATE", $scope, $http);
    };

    $scope.checkValidQty = function (data) {
        if(data.AvailableQty<data.TransferQty)
        {
            alert("Qty Not Valid!.");
            data.TransferQty = 0;
        }
    };

    


    $scope.SubmitData = function () {
        // alert("hdsfh");ID, OperatorName, Mobile, Email, ManagerId, Sucriberid, Ismanager, Createddate, Createdby

        $scope.detail.Investment = $("#ddlInvestment").val();


        var i = JSON.stringify($scope.GRIDDATA);
        var posturl = "/dematstocktransfer/SaveData";
        $http({
            method: "post",
            url: posturl,
            data: { 'JsonData': '[' + JSON.stringify($scope.detail) + ']', 'JsonData2': i }
        }).then(function (response) {
            alert("Save");
            window.location.href = "/dematstocktransfer/index";
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });


    }


});

