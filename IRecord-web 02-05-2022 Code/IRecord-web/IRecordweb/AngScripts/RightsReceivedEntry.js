var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {

        $scope.FillBank();


        //if ($("#hfchecker").val() != "0") {
        //    //in case of View only
        //    $http({
        //        method: "get",
        //        url: "/SplitEntry/GetDataWithView?Trans_No=" + $("#hfchecker").val()
        //    }).then(function (response) {
        //        // alert(JSON.parse(response.data));
        //        $scope.TblData = JSON.parse(response.data);
        //        $scope.btntext = "Process";
        //    }, function (data) {
        //        //deferred.reject({ message: "Really bad" });
        //        alert("Error Occur During This Request" + geturl);
        //    });
        //}


        $scope.btntext = "Submit";
        $scope.RRDetails = {};


        var posturl1 = "/ReceiptPaymentEntry/TypeList1"
        $http({
            method: "get",
            url: posturl1

        }).then(function (response) {
            $scope.Cashbanklist = JSON.parse(response.data);
            



        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });





        $http({
            method: "get",
            url: "/RightsReceivedEntry/GetScpirt"
        }).then(function (response) {
            $scope.SplitEntryScpirt = JSON.parse(response.data);
            // alert(JSON.parse(response.data));
            //  $("#txt_TransactionNo").val(JSON.parse(response.data));
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
            })
        $http({
            method: "get",
            url: "/RightsReceivedEntry/GetTranstionNO"
        }).then(function (response) {
          //  $scope.SplitEntryScpirt = JSON.parse(response.data);
            // alert(JSON.parse(response.data));
            $("#lblTransactionNo").html(JSON.parse(response.data));
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        })
        //$http({
        //    method: "get",
        //    url: "/SplitEntry/GetScpirt"

        //}).then(function (response) {
        //    $scope.SplitEntryScpirt = JSON.parse(response.data);
        //reverse calling function
        //if ($("#hf_CActionView").val() != "0") {
        //    //alert("sadf");
        //    $("#ddlScript").val('string:' + $("#hf_CActionView").val());
        //    $scope.ddlScript = $("#hf_CActionView").val();
        //    $scope.ChngSprt();
        //    $("#txt_Description").val("My Description");
        //    $scope.txt_Description = "My Description";
        //    $("#ddlHoldingType").val("0");
        //    $scope.ddlHoldingType = "0";
        //    setTimeout(function () {
        //        $scope.FillTable();
        //    }, 3000);


        //}
        //reverse calling function
        //}, function (data) {
        //    //deferred.reject({ message: "Really bad" });
        //    alert("Error Occur During This Request" + geturl);
        //})
        //$http({
        //    method: "get",
        //    url: "/SplitEntry/GetTranstionNO"
        //}).then(function (response) {
        //    // alert(JSON.parse(response.data));
        //    $("#txt_TransactionNo").val(JSON.parse(response.data));
        //}, function (data) {
        //    //deferred.reject({ message: "Really bad" });
        //    alert("Error Occur During This Request" + geturl);
        //})

        //document.getElementById("txt_RecordDate").readOnly = true;
        //document.getElementById("txt_NewFaceValue").readOnly = true;
        //$scope.btntext = "Process";
        //$scope.btnposttext = "Post In Respective Demat";



    }
    $scope.changecashbanklist = function () {
        $scope.GetPaymode();
     
    }

    $scope.FillBank = function () {
        fnGetDataUsingGetRequestWithModel("/RightsReceivedEntry/FillBank", "CashBAccount", $scope, $http);

    }
    $scope.GetPaymode = function () {
        fnGetDataUsingGetRequestWithModel("/ReceiptPaymentEntry/GetPaymode?Name=" + $scope.ddlCashbanklist, "paymod", $scope, $http);
    }

    $scope.SaveData = function () {
        $scope.btntext = "Please Wait.."
        $("#loader-1").show();
        debugger;
        var data_Index = {};
        data_Index.Script_id = $scope.ddlScript;
        data_Index.RecordDate = $scope.RRDetails.RecordDate;
        data_Index.TransactionNo = $("#lblTransactionNo").html();

        data_Index.TotalAvailableQty = $scope.RRDetails.TotalAvailableQty;
        data_Index.StockinTradeTotalQty = $scope.RRDetails.StockinTradeTotalQty;
        data_Index.ShareInvestmentTotalQty = $scope.RRDetails.ShareInvestmentTotalQty;
        data_Index.ReteParShare = $scope.RRDetails.ReteParShare;
        data_Index.AdditionalQtyStockinTrade = $scope.RRDetails.AdditionalQtyStockinTrade;
        data_Index.AdditionalQtyShareInvestment = $scope.RRDetails.AdditionalQtyShareInvestment;
        data_Index.Qty1 = $scope.RRDetails.Qty1;
        data_Index.Qty2 = $scope.RRDetails.Qty2;
        data_Index.TotalQtyReceived = $scope.RRDetails.TotalQtyReceived;
        data_Index.StockinTradeRightsQtyReceived = $scope.RRDetails.StockinTradeRightsQtyReceived;
        data_Index.ShareInvestmentRightsQtyReceived = $scope.RRDetails.ShareInvestmentRightsQtyReceived;
        data_Index.ddlCashbanklist = $scope.ddlCashbanklist;
        data_Index.ddlPaymentMode = $scope.ddlPaymentMode;
        data_Index.txt_reference = $scope.txt_reference;
        data_Index.txtPDate = $scope.txtPDate;
        data_Index.txtAmount = $scope.txtAmount;
        data_Index.ddlBankAccount = $scope.ddlBankAccount;
        data_Index.txt_Account = $scope.txt_Account;





        var Pdata = new FormData();
        Pdata.append("JsonHead", '[' + JSON.stringify(data_Index) + ']');
        Pdata.append("Deatils", JSON.stringify($scope.Multimodel));
        $scope.FillTable();
        debugger;

        $.ajax({
            type: "POST",
            url: "/RightsReceivedEntry/SaveDb",
            type: 'POST',
            data: Pdata,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                //show content
                alert("Insert Data Successfully");
               // window.location.href = "/RightsReceivedEntry/Index";
                
            }
        });
    }
    $scope.ChngSprt = function () {

        //   alert($scope.ContractNoteId);
        for (var i = 0; i <= $scope.SplitEntryScpirt.length; i++) {

            if ($scope.ddlScript == $scope.SplitEntryScpirt[i].Script_ID) {
                // alert($scope.BILLS[i].NAME);
                $scope.RRDetails = $scope.SplitEntryScpirt[i];
              //  alert($scope.SplitEntryScpirt[i]);
                //$("#txt_RecordDate").val($scope.SplitEntryScpirt[i].RecordDate);
                //$("#txt_CurrentFaceValue").val($scope.SplitEntryScpirt[i].Qty1);
                //$("#txt_NewFaceValue").val($scope.SplitEntryScpirt[i].Qty2);
                $scope.FillTableDemat();
                break;
            }
        }
    }

    $scope.FillTableDemat = function () {
        $http({
            method: "get",
            url: "/RightsReceivedEntry/GetTableInfo"
        }).then(function (response) {
            // alert(JSON.parse(response.data));
            $scope.Multimodel = JSON.parse(response.data);
            //$scope.btntext = "Process";
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        });
    }

    //Chintan
    $scope.FillTable = function () {
        $http({
            method: "get",
            url: "/RightsReceivedEntry/GetTableData"
        }).then(function (response) {
            // alert(JSON.parse(response.data));
            $scope.aftersavedata = JSON.parse(response.data);
            //$scope.btntext = "Process";
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        });
    }
    //Chintan
    $scope.btn_click_sumit = function () {

        if ($("#ddlScript").val() == "") {
            alert("Please Select Script");
            $("#ddlScript").focus();
            return false;
        }
        if ($("#ddlHoldingType").val() == "") {
            alert("Please Select Holding Type");
            $("#ddlHoldingType").focus();
            return false;
        }
        if ($("#txt_Description").val() == "") {
            alert("Please Enter Description");
            $("#txt_Description").focus();
            return false;
        }

        $scope.btnposttext = "Please Wait.."
        $("#loader-1").show();
        debugger;
        var data_Index = {};
        data_Index.Script_id = $scope.ddlScript;
        data_Index.txt_TransactionNo = $("#txt_TransactionNo").val();
        data_Index.txt_RecordDate = $("#txt_RecordDate").val();
        data_Index.txt_Description = $("#txt_Description").val();
        data_Index.ddlHoldingType = $scope.ddlHoldingType;
        data_Index.txt_CurrentFaceValue = $("#txt_CurrentFaceValue").val();
        data_Index.txt_NewFaceValue = $("#txt_NewFaceValue").val();




        var Pdata = new FormData();
        Pdata.append("JsonData", '[' + JSON.stringify(data_Index) + ']');
        Pdata.append("Details", JSON.stringify($scope.TblData));

        debugger;

        $.ajax({
            type: "POST",
            url: "/SplitEntry/SaveData",
            type: 'POST',
            data: Pdata,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                //show content
                alert("Insert Data Successfully");
                window.location.href = "/SplitEntry/AddSplitEntry";

            }
        });

    }

});

