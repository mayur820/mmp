var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.GetAllDemate();
        $scope.GetAllConsultant();
        $scope.GetAllBroker();
        $scope.GetAllExpense();
        $scope.getDbscript();
        $scope.SetDefaultValues();
        $scope.Multimodel = [];
        $scope.btntext = "Final Submit";
        $("#TBL_Other").hide();
    }
    $scope.StrikeShow = function () {
        if ($scope.ddlOptionType == " ") {
            $("#txt_StrikePrice").val(0);
            $("#txt_StrikePrice").prop("disabled", true);
            //$("#Demat_Ac_Id").prop("disabled", true);
        }
        else {

            $("#txt_StrikePrice").prop("disabled", false);
        }
    }

    $scope.Calculate_BrokerageAmt = function () {

        var total = 0;
        for (i = 0; i < $scope.details.length; i++) {
            total = total + parseFloat($scope.details[i].BrokAmt);
        };
        $scope.TotalOfBrokAmt = parseFloat(total).toFixed(2);
    }
    $scope.changeSegment = function () {

        if ($scope.ddlSegment == "0") {
            $('#divdemo').addClass('displayNO');
            $("#divIntraDays").removeClass("displayNO");
            $("#div_demate").removeClass("displayNO");
            $("#div_HoldingType").removeClass("displayNO");
            //
            $("#div_ExpiryDate").addClass("displayNO");
            $("#div_StrikePrice").addClass("displayNO");
            $("#div_Lot").addClass("displayNO");
            $("#div_LotNo").addClass("displayNO");
            $("#div_StockType").addClass("displayNO");
            $("#div_OptionType").addClass("displayNO");

            $("#TBL_Equity").show();
            $("#TBL_Other").hide();

        }
        else {
            $('#divdemo').removeClass('displayNO');
            $("#div_HoldingType").addClass("displayNO");
            $("#div_demate").addClass("displayNO");
            $("#divIntraDays").addClass("displayNO");
            //
            $("#div_ExpiryDate").removeClass("displayNO");
            $("#div_StrikePrice").removeClass("displayNO");
            $("#div_Lot").removeClass("displayNO");
            $("#div_LotNo").removeClass("displayNO");
            $("#div_StockType").removeClass("displayNO");
            $("#div_OptionType").removeClass("displayNO");
            $("#TBL_Equity").hide();
            $("#TBL_Other").show();
        }

    };
    $scope.SetDefaultValues = function () {
        $scope.txt_Quantity = 0;
        $scope.txt_GrossRate = 0;
        $scope.txt_GrossAmount = 0;
        $scope.txt_BrokerageperUnit = 0;
        $scope.txt_BrokerageAmount = 0;
        $scope.txt_StrikePrice = 0;
        $scope.txt_NetRate = 0;
        $scope.txt_NetAmount = 0;

    };
    $scope.GetAllDemate = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllDemate", "Demates", $scope, $http);
        console.log($scope.Demates)
    }
    $scope.GetAllBroker = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllBroker", "Brokers", $scope, $http);

    }
    $scope.GetAllConsultant = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllConsultant", "Consultants", $scope, $http);

    }
    $scope.GetAllExpense = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllExpense", "Expense", $scope, $http);

    }

    $scope.bindbills = function () {
        $scope.BrokerOnchange();
    }
    $scope.BrokerOnchange = function () {
        for (var i = 0; i <= $scope.Brokers.length; i++) {
            if ($scope.ddlBroker === $scope.Brokers[i].ID) {
                $("#Demat_Ac_Id").val($scope.Brokers[i].DefaultDemate);
                break;
            }
        }
    }
    $scope.getDbscript = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/Get_All_Scprit", "scriptmaster", $scope, $http);
    }
    $scope.getinfo = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetListofCn", "Info", $scope, $http);
    }
    $scope.fn_AllCalculation = function () {
        try {

            if ($scope.ddlSegment == "0") {

                //  $scope.fn_LotNo();
                $scope.fn_Cal_NetRate();
                $scope.fn_Cal_NetAmount();
                $scope.fn_Cal_GrossAmount();
                $scope.fn_Cal_BrokerageAmount();
            }
            else {
                $scope.fn_LotNo();
                $scope.fn_Cal_NetRate();
                $scope.fn_Cal_NetAmount();
                $scope.fn_Cal_GrossAmount();
                $scope.fn_Cal_BrokerageAmount();
            }
        }
        catch { }
    }

    $scope.Calculate_BrokerageAmt = function () {

        var total = 0;
        for (i = 0; i < $scope.details.length; i++) {
            total = total + parseFloat($scope.details[i].BrokAmt);
        };
        $scope.TotalOfBrokAmt = parseFloat(total).toFixed(2);
    }

    $scope.fn_LotNo = function () {
        try {
            $scope.txt_Quantity = $scope.txt_Lot * $scope.txt_LotNo;
        } catch { }
    }
    $scope.fn_NetRate = function () {
        try {
            $scope.txt_NetAmount = $scope.txt_Quantity * $scope.txt_NetRate;
        } catch { }
    }

    $scope.fn_Cal_GrossAmount = function () {
        try {
            $scope.txt_GrossAmount = $scope.txt_Quantity * $scope.txt_GrossRate;
        } catch { }
    }
    $scope.fn_Cal_BrokerageAmount = function () {
        try {
            $scope.txt_BrokerageAmount = $scope.txt_Quantity * $scope.txt_BrokerageperUnit;
        } catch { }
    }
    $scope.fn_Cal_NetRate = function () {
        try {
            $scope.txt_NetRate = parseInt($scope.txt_GrossRate) + parseFloat($scope.txt_BrokerageperUnit);
        } catch { }
    }
    $scope.fn_Cal_NetAmount = function () {
        try {
            //$scope.txt_NetAmount = $scope.txt_GrossAmount + $scope.txt_BrokerageAmount;
            $scope.txt_NetAmount = $scope.txt_NetRate * $scope.txt_Quantity;
        } catch { }
    }
    $scope.Calculate_total_exp = function () {
        var total = 0;
        for (i = 0; i < $scope.Expense.length; i++) {
            if ($scope.Expense[i].AMOUNT != 0) {
                if ($scope.Expense[i].ADDLESS == 'A') {
                    total = total + Number($scope.Expense[i].AMOUNT);
                }
                else {
                    total = total - Number($scope.Expense[i].AMOUNT);
                }
            }
        };
        $scope.TotalExpenses = parseFloat(total).toFixed(2);
        var netcal = parseFloat($scope.TotalOfNetAmount) + parseFloat($scope.TotalExpenses);
        $scope.TotalFinal = parseFloat(netcal).toFixed(2);
    }

    $scope.fn_Quantity = function () {
        $scope.fn_Cal_GrossAmount();
        $scope.fn_Cal_BrokerageAmount();
    }
    $scope.fn_GrossRate = function () {
        $scope.fn_Cal_GrossAmount();
        $scope.fn_Cal_NetRate();
        $scope.fn_Cal_NetAmount();
    }
    $scope.fn_GrossAmount = function () {
        $scope.fn_Cal_NetAmount();
    }
    $scope.fn_BrokerageperUnit = function () {
        $scope.fn_Cal_BrokerageAmount();
        $scope.fn_Cal_NetRate();
        $scope.fn_Cal_NetAmount();
    }
    $scope.fn_BrokerageAmount = function () {
        $scope.fn_Cal_NetAmount();
    }

    $scope.fn_NetAmount = function () {

    }
    $scope.FnAdd = function () {


        $scope.singlemodel = {};
        $scope.singlemodel.ddlSegment = $scope.ddlSegment;
        $scope.singlemodel.Segmenttext = fnGetddlText("ddlSegment");
        //add Type in  Sold,Bought
        $scope.singlemodel.Type = $scope.Type;
        $scope.singlemodel.txtDate = $("#txt_Date").val();

        $scope.singlemodel.ddlBroker = $scope.ddlBroker;
        $scope.singlemodel.Brokertext = fnGetddlText("ddlBroker");
        try {
            $scope.singlemodel.Demat_Ac_Id = $("#Demat_Ac_Id").val();
            $scope.singlemodel.Demattext = fnGetddlText("Demat_Ac_Id");
        }
        catch {
            //$scope.singlemodel.Demat_Ac_Id = $("#Demat_Ac_Id").val();
            //$scope.singlemodel.Demattext = fnGetddlText("Demat_Ac_Id");
        }
        $scope.singlemodel.txt_SettlementNo = $scope.txt_SettlementNo;
        $scope.singlemodel.txt_BillNo = $scope.txt_BillNo;
        $scope.singlemodel.ddlConsultant = $scope.ddlConsultant;
        $scope.singlemodel.Consultanttext = fnGetddlText("ddlConsultant");
        $scope.singlemodel.ddlHoldingType = $scope.ddlHoldingType;
        $scope.singlemodel.HoldingTypetext = fnGetddlText("ddlHoldingType");
        $scope.singlemodel.ddlScript_Id = $scope.ddlScript_Id;
        $scope.singlemodel.Scripttext = fnGetddlText("ddlScript_Id");
        $scope.singlemodel.chk_intraday = $scope.chk_intraday;
        $scope.singlemodel.txt_Quantity = $scope.txt_Quantity;
        $scope.singlemodel.txt_GrossRate = $scope.txt_GrossRate;
        $scope.singlemodel.txt_GrossAmount = $scope.txt_GrossAmount;
        $scope.singlemodel.txt_BrokerageperUnit = $scope.txt_BrokerageperUnit;
        $scope.singlemodel.txt_BrokerageAmount = $scope.txt_BrokerageAmount;
        $scope.singlemodel.txt_NetRate = $scope.txt_NetRate;
        $scope.singlemodel.txt_NetAmount = $scope.txt_NetAmount;
        ////new para
        try {
            $scope.singlemodel.ExpiryDate = $("#txt_ExpiryDate").val();
            $scope.singlemodel.OptionType = $scope.ddlOptionType;
            $scope.singlemodel.OptionTypetext = fnGetddlText("ddlOptionType");
            $scope.singlemodel.StockType = $scope.ddlStockType;
            $scope.singlemodel.StockTypetext = fnGetddlText("ddlStockType");
            $scope.singlemodel.Lott = $scope.txt_Lot;
            $scope.singlemodel.LotQty = $scope.txt_LotNo;////
            $scope.singlemodel.StrikePrice = $scope.txt_StrikePrice;


        } catch { }



        $scope.Multimodel.push($scope.singlemodel);
        var BrAmttotal = 0;
        var GrossAmttotal = 0;
        var NetAmttotal = 0;
        var quantity = 0;
        var totalstock = 0;
        for (i = 0; i < $scope.Multimodel.length; i++) {
            BrAmttotal = BrAmttotal + parseFloat($scope.Multimodel[i].txt_BrokerageAmount);
            GrossAmttotal = GrossAmttotal + parseFloat($scope.Multimodel[i].txt_GrossAmount);
            NetAmttotal = NetAmttotal + parseFloat($scope.Multimodel[i].txt_NetAmount);
            quantity = quantity + parseInt($scope.Multimodel[i].txt_Quantity);
            totalstock = totalstock + 1;
        };
        $scope.TotalInInvestment = 0;
        $scope.TotalInTrade = 0;
        $scope.TotalOfBrokAmt = parseFloat(BrAmttotal).toFixed(2);
        $scope.TotalExpenses = 0;
        $scope.TotalGrossAmount = parseFloat(GrossAmttotal).toFixed(2);
        $scope.TotalOfNetAmount = parseFloat(NetAmttotal).toFixed(2);
        $scope.TotalFinal = parseFloat(NetAmttotal).toFixed(2);
        $scope.TotalInInvestment = quantity;
        $scope.TotalInTrade = totalstock;
        //$("#ddlSegment").prop("disabled", true);
        //$("#txt_Date").prop("disabled", true);
        //$("#ddlBroker").prop("disabled", true);
        //$("#txt_BillNo").prop("disabled", true);
        //$("#txt_SettlementNo").prop("disabled", true);
        $("#Demat_Ac_Id").val("");
        $("#ddlConsultant").val("");
        $("#ddlScript_Id").val("");
        $("#Type").val("");
        // $("#txt_ExpiryDate").val("");
        $("#ddlStockType").val("");
        $("#ddlOptionType").val("");
        $("#txt_StrikePrice").val("0");
        $("#txt_Lot").val("0");
        $("#txt_LotNo").val("0");
        $("#txt_Quantity").val("0");
        $("#txt_GrossRate").val("0");
        $("#txt_GrossAmount").val("0");
        $("#txt_BrokerageperUnit").val("0");
        $("#txt_BrokerageAmount").val("0");
        $("#txt_NetRate").val("0");
        $("#txt_NetAmount").val("0");

        //console.log($scope.Multimodel);
    }
    $scope.FndeleteItems = function (data) {

        var r = confirm("Are you sure delete this item?");
        if (r == true) {
            var i = $scope.Multimodel.indexOf(data);
            if (i >= 0) $scope.Multimodel.splice(i, 1);
        } else {

        }



    }
    $scope.FnFinalSubmit = function () {
        var posturl = "";

        if ($scope.ddlSegment == "0") {
            posturl = "/BrokerBillEntry/FnSaveManualEntryForEquity";
        }
        if ($scope.ddlSegment == "1") {
            posturl = "/BrokerBillEntry/FnSaveManualEntryForFNO";
        }
        if ($scope.ddlSegment == "3") {
            posturl = "/BrokerBillEntry/FnSaveManualEntryForMCX";
        }
        if ($scope.ddlSegment == "4") {
            posturl = "/BrokerBillEntry/FnSaveManualEntryForCurrency";
        }



        //$("#loader-1").show();
        //$scope.btntext = "Please Wait..";
        $http({
            method: "get",
            url: "/BrokerBillEntry/Get_Validate_billNo?BillNo=" + $("#txt_BillNo").val()
            //data: { 'BillNo':  }
        }).then(function (response) {
            if (response.data == 1) {
                alert("Bill No Already Exist!");
                $("#txt_BillNo").focus();
                //   return false;
            }
            else {
                $("#loader-1").show();
                $scope.btntext = "Please Wait..";
                $http({
                    method: "post",
                    url: posturl,
                    data: { 'JsonData': JSON.stringify($scope.Multimodel), 'ExpenseData': JSON.stringify($scope.Expense) }
                }).then(function (response) {
                    alert("Insert Data Successfully");
                    window.location.href = "/BrokerBillEntry/ManualEntry";
                }, function (data) {
                    alert("Error Occur During This Request Of Post Data");
                })
            }
        });

    }
    $scope.FnReset = function () {
        var r = confirm("Are you sure Reset This Page?");
        if (r == true) {
            window.location.href = "/BrokerBillEntry/ManualEntry";
        } else {

        }
    }
});