var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        $scope.fillbank();
        $scope.fillbankCONFIQ();
        //viewbankstatmentdata();
    }

    
    $scope.fillbank = function () {
        fnGetDataUsingGetRequestWithModel("/BankStatementImport/GetAllbank", "BANKS", $scope, $http);
    };

    $scope.fillbankCONFIQ = function () {
        fnGetDataUsingGetRequestWithModel("/BankStatementImport/GetAllbankconfiq", "BANKSCONFIQ", $scope, $http);
    };
    $scope.btnupload = function () {
        let photo = document.getElementById("contractnotefile").files[0];
        if (typeof photo !== 'undefined') {
            let formData = new FormData();
            formData.append("photo", photo);
            fetch('/BankStatementImport/Upload', { method: "POST", body: formData }).then(function () {

                alert("File Upload successfully");
            });
            $scope.btnupload = false;
        }
        else {
           alert("Please Select A files");
        }

    };
    $scope.SubmitData = function () {
        var BankStatement_Index = {};
        BankStatement_Index.FromDate = formatDate($scope.FromDate);
        BankStatement_Index.ToDate = formatDate($scope.ToDate);
        BankStatement_Index.BankName = fnGetddlText("BankId");
        BankStatement_Index.BankNameId = $scope.BankId;
        BankStatement_Index.BankConfig = fnGetddlText("Bank_Config_id");
        BankStatement_Index.BankConfigId = $scope.Bank_Config_id;
        BankStatement_Index.Password = $scope.Password;
        var posturl = "/BankStatementImport/IndexSave";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(BankStatement_Index)
        }).then(function (response) {
            alert("Save");
            window.location.href = "/BankStatementImport/ViewBankStatement";
        }, function (data) {
                alert("Error Occur During This Request" + posturl);
        });

         
       
    }

    //$scope.btnupload = function () {

    //    let photo = document.getElementById("contractnotefile").files[0];
    //    if (typeof photo !== 'undefined') {


    //        let formData = new FormData();

    //        formData.append("photo", photo);
    //        fetch('/BrokerBillEntry/Upload', { method: "POST", body: formData }).then(function () {

    //            alert("File Upload successfully");
    //        });
    //        $scope.btnupload = false;
    //    }
    //    else {
            
    //        alert("Please Select A files");
    //    }

    //};

    //$scope.bindbills = function () {


        

    //    fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllBILLS", "BILLS", $scope, $http);

    //}
    //$scope.GetAllDemate = function () {
    //    fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllDemate", "Demates", $scope, $http);

    //}
    //$scope.btn_click_sumit = function () {
    //    debugger;
    //    var text = fnGetddlText("ContractNoteId")
    //    $scope.SaveBREntry.ContractNoteName = text;
    //    $scope.SaveBREntry.ContractNoteId = $scope.ContractNoteId;
    //    text = fnGetddlText("Demat_Ac_Id")
    //    $scope.SaveBREntry.Demat_Ac_Name = text;
    //    fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/SaveBREntry", "SaveBREntry", $scope, $http);
    //    window.location.href = "/BrokerBillEntry/BRDetails";
    //}
   

});

