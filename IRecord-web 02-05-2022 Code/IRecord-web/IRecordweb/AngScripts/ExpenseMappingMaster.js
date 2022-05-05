var app = angular.module("myApp", ["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        // alert("hdsfh");
        $scope.ViewBBILLFORMAT();
        $scope.ViewADDLESS();
        $scope.ViewVIEWEXPSES();
        // $scope.GetAllOperatorVIEWBYID();
        //viewbankstatmentdata();
        $scope.btntext="Add";
        $scope.ViewDATAList();
        $scope.page = 1;
        $scope.ID = 0;
    }
    $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        //$scope.displayItems = $scope.totalItems.slice(startPos, startPos + 3);
        console.log($scope.page);
    };

    $scope.ViewBBILLFORMAT = function () {
        //alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/ExpenseMappingMaster/ViewBBILLFORMAT", "M_BBILLFORMAT", $scope, $http);
    };

    $scope.ViewADDLESS = function () {
        //   alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/ExpenseMappingMaster/ViewADDLESS", "M_ADDLESS", $scope, $http);
    };
    $scope.ViewVIEWEXPSES = function () {
        //   alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/ExpenseMappingMaster/ViewVIEWEXPSES", "M_VIEWEXPSES", $scope, $http);
    };
    $scope.ViewDATAList = function () {
        //   alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/ExpenseMappingMaster/ViewAllData", "DATALIST", $scope, $http);
    };
    
    $scope.FnReset = function () {
        window.location = "/ExpenseMappingMaster/Index";
    }
    $scope.FnEditItems = function (details) {
        //  alert(details.ID)
        $scope.ID = details.ID;
        $scope.ddlformat = details.FORMAT_SR_NO;
        $scope.ddlExpense = details.EXPENSE_ID;
        $scope.ddladd_less = details.ADD_LESS_STATUS;
        $scope.txt_filetaxName = details.FILE_TAX_NAME;
        $scope.btntext = "Update";
    }
    $scope.FndeleteItems = function (details) {
        //  alert(details.ID)
     
        var r = confirm("Are you sure delete this item");
        if (r == true) {
            //alert(data.Trans_no)
            var posturl = "/ExpenseMappingMaster/DeleteConfig?ID=" + details.ID;
            $http({
                method: "post",
                url: posturl

            }).then(function (response) {
                alert("Record Delete Successfully !!");
                $scope.ViewDATAList();
            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });


        } else {

        }


     

    }
  
    $scope.FnFinalSubmit = function () {


        if ($scope.btntext == "Update") {
            debugger;
            var BankStatement_Index = {};
            BankStatement_Index.ID = $scope.ID;
            BankStatement_Index.ddlformat = $scope.ddlformat;
            BankStatement_Index.ddlformat = $scope.ddlformat;
            BankStatement_Index.ddlExpense = $scope.ddlExpense;
            BankStatement_Index.ddladd_less = $scope.ddladd_less;

            BankStatement_Index.txt_filetaxName = $scope.txt_filetaxName;



            var posturl = "/ExpenseMappingMaster/FnSaveEntry";
            $http({
                method: "post",
                url: posturl,
                data: { 'JsonData': JSON.stringify(BankStatement_Index) }
            }).then(function (response) {
                $scope.ddladd_less = null;
                $scope.txt_filetaxName = "";
                $scope.btntext = "Add"
                $scope.ViewDATAList();
                alert("Record Update Successfully !!");
              

            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });
        }
        else {
            var BankStatement_Index = {};
            BankStatement_Index.ddlformat = $scope.ddlformat;
            BankStatement_Index.ddlExpense = $scope.ddlExpense;
            BankStatement_Index.ddladd_less = $scope.ddladd_less;

            BankStatement_Index.txt_filetaxName = $scope.txt_filetaxName;



            var posturl = "/ExpenseMappingMaster/FnSaveEntry";
            $http({
                method: "post",
                url: posturl,
                data: { 'JsonData': JSON.stringify(BankStatement_Index) }
            }).then(function (response) {
                alert("Record Save Successfully !!");
                $scope.ViewDATAList();
            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });
        }


      
     

    }
});
