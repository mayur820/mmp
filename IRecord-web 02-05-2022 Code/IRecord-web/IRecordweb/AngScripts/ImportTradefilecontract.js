var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        $scope.fillallbank();
        $scope.fillalldemate();
        //viewbankstatmentdata();
    }


    $scope.fillallbank = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/GetAllBILLS", "FILLBANKS", $scope, $http);
    };

    $scope.fillalldemate = function () {
        fnGetDataUsingGetRequestWithModel("/ImportTradefileContract/GetAllDemate", "FILLALLDEMATE", $scope, $http);
    };
    $scope.btnupload = function () {
        let photo = document.getElementById("contractnotefile").files[0];
        if (typeof photo !== 'undefined') {
            let formData = new FormData();
            formData.append("photo", photo);
            fetch('/ImportTradefileContract/Upload', { method: "POST", body: formData }).then(function () {

                alert("File Upload successfully");
            });
            $scope.btnupload = false;
        }
        else {
            alert("Please Select A files");
        }

    };
    $scope.SubmitData = function () {
        var ImportTradefileContract_INDEX = {};
        ImportTradefileContract_INDEX.FromDate = formatDate($scope.FromDate);
        ImportTradefileContract_INDEX.ToDate = formatDate($scope.ToDate);
        ImportTradefileContract_INDEX.ContractNoteName = fnGetddlText("fillId");
        ImportTradefileContract_INDEX.ContractNoteId = $scope.fillId;
        ImportTradefileContract_INDEX.Demat_Ac_Name = fnGetddlText("filldementid");
        ImportTradefileContract_INDEX.Demat_Ac_Id = $scope.filldementid;
        ImportTradefileContract_INDEX.Password = $scope.Password;
        var posturl = "/ImportTradefileContract/IndexSave";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(ImportTradefileContract_INDEX)
        }).then(function (response) {
            alert("Save");
            window.location.href = "/ImportTradefileContract/ViewImportTradefileContract";
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

