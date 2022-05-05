var app = angular.module("myApp", ["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.GetAllBANKFORMATE();
      //  $scope.GetAllDATA();
        $scope.GetAllBANK(); 
        $scope.btntext = "Submit";
        $scope.ddl_File_Type = "EXCEL";
        $("#ddlbank").focus();
        $scope.page = 1;
    }
    $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        //$scope.displayItems = $scope.totalItems.slice(startPos, startPos + 3);
        console.log($scope.page);
    };
  
    $scope.GetAllBANKFORMATE = function () {
        fnGetDataUsingGetRequestWithModel("/BankStConfig/ViewBankFormatedata", "bankformates", $scope, $http);

    }
    $scope.GetAllBANK = function () {
        fnGetDataUsingGetRequestWithModel("/BankStConfig/GetBank", "bank", $scope, $http);

    }
 
  
    $scope.FndeleteItems = function (dataS) {
        console.log(dataS);
        var r = confirm("Are you sure delete this item?");
        if (r == true) {
            $http({
                method: "get",
                url: "/BankStConfig/BankFormatDeleteConfig?ID=" + dataS.Sr_No,
              
            }).then(function (response) {
                $scope.GetAllBANKFORMATE();
            }, function (data) {
                alert("Error Occur During This Request Of Post Data");
            })


        
        } else {

        }



    }
    $scope.FnFinalSubmit = function () {
        $scope.Multimodel = [];
        $scope.singlemodel = {};
        $scope.singlemodel.ddlbank = $scope.ddlbank;
        $scope.singlemodel.txt_bankFormateNumber = $scope.txt_bankFormateNumber;
        $scope.singlemodel.ddl_File_Type = $scope.ddl_File_Type;
        $scope.singlemodel.ddl_File_Format_Type = $scope.ddl_File_Format_Type;


        $scope.Multimodel.push($scope.singlemodel);

        $("#loader-1").show();
        $scope.btntext = "Please Wait..";
        $http({
            method: "post",
            url: "/BankStConfig/FnBankFormatMasterSaveEntry",
            data: { 'JsonData': JSON.stringify($scope.Multimodel) }
        }).then(function (response) {
            alert("Insert Data Successfully");
            window.location.href = "/BankStConfig/BankFormatMaster";
        }, function (data) {
            alert("Error Occur During This Request Of Post Data");
        })
    }
    $scope.FnReset = function () {
        var r = confirm("Are you sure Reset This Page?");
        if (r == true) {
            window.location.href = "/BankStConfig/BankFormatMaster";
        } else {

        }
    }
});

