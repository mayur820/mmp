var app = angular.module("myApp", ["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
       // $scope.GetAllBANKFORMATE();
        $("#ddlbank").focus();
        $scope.GetAllDATA();
        $scope.GetAllBANK(); 
        $scope.btntext = "Submit";
        $scope.ddl_Format_Type = "1";
        $scope.page = 1;
    }
    $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        //$scope.displayItems = $scope.totalItems.slice(startPos, startPos + 3);
        console.log($scope.page);
    };
    $scope.fnchangeformattype = function () {
        
        if ($scope.ddl_Format_Type == "2") {
            $("#divdrcr").show();
            $("#divtype1").hide();
            
        } else {
            $("#divdrcr").hide();
            $("#divtype1").show();
            
        }
    }
  
    $scope.GetAllBANKFORMATE = function () {
        fnGetDataUsingGetRequestWithModel("/BankStConfig/GetAllFormat?bankid=" + $scope.ddlbank, "bankformates", $scope, $http);

    }
    $scope.GetAllBANK = function () {
        fnGetDataUsingGetRequestWithModel("/BankStConfig/GetBank", "bank", $scope, $http);

    }
    $scope.GetAllDATA = function () {
        fnGetDataUsingGetRequestWithModel("/BankStConfig/GetAllDATA", "MultimodelDATA", $scope, $http);
        
    }
  
    $scope.FndeleteItems = function (dataS) {
        console.log(dataS);
        var r = confirm("Are you sure delete this item?");
        if (r == true) {
            $http({
                method: "get",
                url: "/BankStConfig/DeleteConfig?ID=" + dataS.ID,
              
            }).then(function (response) {
                $scope.GetAllDATA();
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
        $scope.singlemodel.ddlbankFormat = $scope.ddlbankFormat;
        $scope.singlemodel.ddl_Format_Type = $scope.ddl_Format_Type;
        $scope.singlemodel.txt_Starting_Row = $scope.txt_Starting_Row;
        $scope.singlemodel.ddl_Date = $scope.ddl_Date;
        $scope.singlemodel.ddl_Account = $scope.ddl_Account;
        $scope.singlemodel.ddl_Cheque = $scope.ddl_Cheque;
        $scope.singlemodel.ddl_Narration = $scope.ddl_Narration;
        $scope.singlemodel.ddl_Debit = $scope.ddl_Debit;
        $scope.singlemodel.ddl_Credit = $scope.ddl_Credit;
        $scope.singlemodel.ddl_Dr_Cr = $scope.ddl_Dr_Cr;
        $scope.singlemodel.ddl_Amount = $scope.ddl_Amount;
        $scope.Multimodel.push($scope.singlemodel);

        $("#loader-1").show();
        $scope.btntext = "Please Wait..";
        $http({
            method: "post",
            url: "/BankStConfig/FnSaveEntry",
            data: { 'JsonData': JSON.stringify($scope.Multimodel) }
        }).then(function (response) {
            alert("Insert Data Successfully");
            window.location.href = "/BankStConfig/Index";
        }, function (data) {
            alert("Error Occur During This Request Of Post Data");
        })
    }
    $scope.FnReset = function () {
        var r = confirm("Are you sure Reset This Page?");
        if (r == true) {
            window.location.href = "/BankStConfig/Index";
        } else {

        }
    }
});

