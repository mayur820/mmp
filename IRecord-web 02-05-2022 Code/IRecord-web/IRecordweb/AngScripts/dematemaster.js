var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        $("#DIV_VIEW_MAPING").hide();
        $scope.bindallformate();
    }
    $scope.btnupload = function () {
        let photo = document.getElementById("contractnotefile").files[0];
        if (typeof photo !== 'undefined') {
            let formData = new FormData();
            formData.append("photo", photo);
            fetch('/TradeExcelMap/Upload', { method: "POST", body: formData }).then(function () {
                // $("#btnviewmaping").show();
                $scope.GETALLEXCELCOLNM();
                $scope.bindformate()
                $("#DIV_VIEW_MAPING").show();
                alert("File Upload successfully");
            });
            $scope.btnupload = false;
        }
        else {
            alert("Please Select A files");
        }

    };
    $scope.GETALLEXCELCOLNM = function () {
        fnGetDataUsingGetRequestWithModel("/TradeExcelMap/GETALLEXCELCOLNM", "GETALLEXCELCOLNM", $scope, $http);
    }
    $scope.bindbills = function () {

        fnGetDataUsingGetRequestWithModel("/TradeExcelMap/GetAllBILLS?Invtype=" + $("#ddlInvestmentType").val(), "BILLS", $scope, $http);

    }
    $scope.bindformate = function () {
        fnGetDataUsingGetRequestWithModel("/TradeExcelMap/GetGetFormate?Invtype=" + $("#ddlInvestmentType").val() + "&&BrokerBill_Format_Id=" + $scope.ContractNoteId, "BILLSFORMATE", $scope, $http);
    }
    $scope.bindallformate = function () {
        fnGetDataUsingGetRequestWithModel("/TradeExcelMap/GetGetAllMappingFormate", "datas", $scope, $http);
    }
    $scope.deletefn = function (DATA) {
        
        $http({
            method: "post",
            url: "/TradeExcelMap/DELETEFORMATE",
            data: { 'ID': DATA.ID }
        }).then(function (response) {
            alert("Delete Data Successfully");
            $scope.bindallformate();
        }, function (data) {
            alert("Error Occur During This Request Of Post Data");
        })
    }
    
    $scope.OnClickApprove = function () {

        $http({
            method: "post",
            url: "/TradeExcelMap/Save",
            data: { 'JsonData': JSON.stringify($scope.BILLSFORMATE) }
        }).then(function (response) {
            alert("Insert Data Successfully");
            window.location.href = "/TradeExcelMap/Index";
        }, function (data) {
            alert("Error Occur During This Request Of Post Data");
        })
    }
    $scope.btn_click_sumit = function () {
        debugger;


        var text = fnGetddlText("ContractNoteId")
        $scope.SaveBREntry.ContractNoteName = text;
        $scope.SaveBREntry.ContractNoteId = $scope.ContractNoteId;
        text = fnGetddlText("Demat_Ac_Id")
        $scope.SaveBREntry.Demat_Ac_Name = text;
        $scope.SaveBREntry.invstyp = $scope.ddlInvestmentType;

        $scope.SaveBREntry.Broker_Id = $scope.ddlBroker;
        $scope.SaveBREntry.Broker_Name = fnGetddlText("ddlBroker");
        ////
        $sessionStorage.TypeOfInvst = $scope.ddlInvestmentType;
        ////
        $scope.SaveBREntry.invstyptext = fnGetddlText("ddlInvestmentType");
        $scope.SaveBREntry.ConsultantCode = $scope.ddlConsultant;
        $scope.SaveBREntry.Consultant = fnGetddlText("ddlConsultant");
        $scope.SaveBREntry.HoldingTypeCode = $scope.ddlHoldingType;
        $scope.SaveBREntry.HoldingType = fnGetddlText("ddlHoldingType");
        fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/SaveBREntry", "SaveBREntry", $scope, $http);
        window.location.href = "/BrokerBillEntry/BRDetails";
    }


});

