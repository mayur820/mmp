var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {


    $scope.PageLoad = function () {
        $scope.btnsubmit = "Approve Now";
        $("#tbl_Equity").hide();
        $("#tbl_fno").hide();
        $("#SettlementNo").focus();
        //console.log($sessionStorage.SessionSaveBREntry);
        //if ($sessionStorage.TypeOfInvst == "1" || $sessionStorage.TypeOfInvst == "3" || $sessionStorage.TypeOfInvst == "4") {
        //    $("#tbl_fno").show();
        //    $("#Demat_Ac_Id").prop("disabled", true);
        //    $("#ddlHoldingType").prop("disabled", true);
        //    $("#ddlInvestmentType").prop("disabled", true);
        //    $("#ddlConsultant").prop("disabled", true);
        //    $('#Eqvar1').addClass('displayNO');
        //    $('#Eqvar2').addClass('displayNO');
        //}
        //else {
        //    $("#tbl_Equity").show();

        //    $("#Demat_Ac_Id").prop("disabled", true);
        //    $("#ddlHoldingType").prop("disabled", true);
        //    $("#ddlInvestmentType").prop("disabled", true);
        //    $("#ddlConsultant").prop("disabled", true);

        //}
        $scope.GetAllConsultant();
        $scope.GetAllDemate();
        $scope.getinfo();
        $scope.getDetails();

        $scope.getDbscript();
        $scope.getExpanse();
        $scope.CalcaluteAlltotal();
        $scope.fn_Cal_GrossAmt();
        // $scope.fn_LotNo();
        $scope.AllScriptMapModel = [];
        $scope.AllScriptMapModel = [];
      
        $scope.TotalOfNetAmount = 0;


    }
    $scope.GetAllDemate = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllDemate", "Demates", $scope, $http);
        console.log($scope.Demates)
    }
    $scope.getExpanse = function () {


        // fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/Get_Expenses", "Exps", $scope, $http);
        var posturl = "/BrokerBillEntry/Get_Expenses"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {
            $scope.Exps = JSON.parse(response.data);
            $scope.Calculate_total_exp();

        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }
    $scope.FndeleteItems = function (detail) {

        var r = confirm("Are you sure delete this item");
        if (r == true) {
            if ($("#BTN_ACTION").val() == "Update") {
                $http({
                    method: "get",
                    url: "/BrokerBillEntry/DeleteDetails?ID=" + detail.ID
                }).then(function (response) {
                    var i = $scope.details.indexOf(detail);
                    if (i >= 0) $scope.details.splice(i, 1);

                }, function (data) {
                    //deferred.reject({ message: "Really bad" });
                    alert("Error Occur During This Request" + geturl);
                })
            }
            else {
                var i = $scope.details.indexOf(detail);
                if (i >= 0) $scope.details.splice(i, 1);
            }


        } else {

        }



    }
    $scope.getinfo = function () {

        //$http({
        //    method: "get",
        //    url: "/BrokerBillEntry/Get_Header"
        //}).then(function (response) {
        //    $scope.Info = JSON.parse(response.data);
        //    console.log($scope.Info);
        //    ddlInvestmentType = $scope.Info.invstyp;

        //    Info.Date = $scope.Info.Date; 
        //    alert("hii");
        //}, function (data) {
        //    //deferred.reject({ message: "Really bad" });
        //    alert("Error Occur During This Request" + geturl);
        //})
        var posturl = "/BrokerBillEntry/Get_Header"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {
            $scope.Info = JSON.parse(response.data);
            //  alert($scope.Info.invstyp)
            //if ($scope.Info.invstyp) {

            //}
            if ($scope.Info.invstyp == "1" || $scope.Info.invstyp == "3" || $scope.Info.invstyp == "4") {
                $("#tbl_fno").show();
                $("#Demat_Ac_Id").prop("disabled", true);
                $("#ddlHoldingType").prop("disabled", true);
                $("#ddlInvestmentType").prop("disabled", true);
                $("#ddlConsultant").prop("disabled", true);
                $('#Eqvar1').addClass('displayNO');
                $('#Eqvar2').addClass('displayNO');
            }
            else {
                $("#tbl_Equity").show();
                //$("#Demat_Ac_Id").prop("disabled", false);
                //$("#ddlHoldingType").prop("disabled", false);
                $("#Demat_Ac_Id").prop("disabled", true);
                $("#ddlHoldingType").prop("disabled", true);
                $("#ddlInvestmentType").prop("disabled", true);
                $("#ddlConsultant").prop("disabled", true);
                $scope.txt_invstyp = "Equity";
            }


        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

        // fnGetSingleDataUsingGetRequestWithModel("/BrokerBillEntry/Get_Header", "Info", $scope, $http);
        //console.log($scope.Info);
        //ddlInvestmentType = $scope.Info.invstyp;

    }

    $scope.getDetails = function () {
        // fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/Get_Details", "details", $scope, $http);
        var posturl = "/BrokerBillEntry/Get_Details"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {
            $scope.details = JSON.parse(response.data);
            $scope.Calculate_Net_Amount();
            $scope.Calculate_BrokerageAmt();
            $scope.Calculate_GrossAmt();
            $scope.Calculate_HoldingType();
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }
    $scope.CalcaluteAlltotal = function () {
        if ($scope.txt_invstyp == "Equity") {
            // $scope.fn_LotNo();
            $scope.fn_Cal_GrossAmt();
            $scope.fn_Cal_BrokAmt();
            $scope.fn_Cal_NetRate();
            $scope.fn_Cal_NetAmount();
        }
        else {
            $scope.fn_LotNo();
            $scope.fn_Cal_GrossAmt();
            $scope.fn_Cal_BrokAmt();
            $scope.fn_Cal_NetRate();
            $scope.fn_Cal_NetAmount();
        }
        $scope.Calculate_Net_Amount();
        $scope.Calculate_BrokerageAmt();
        $scope.Calculate_GrossAmt();
        $scope.Calculate_HoldingType();
    }
    $scope.fn_LotNo = function () {

        var Lott, LotQty = 0;
        for (i = 0; i < $scope.details.length; i++) {
            Lott = Number($scope.details[i].Lott);
            LotQty = Number($scope.details[i].LotQty);
            $scope.details[i].Qty = Lott * LotQty;
        }
    }
    $scope.fn_Cal_GrossAmt = function () {
        try {

            var GRate, Qty = 0;
            for (i = 0; i < $scope.details.length; i++) {

                Qty = Number($scope.details[i].Qty);
                GRate = Number($scope.details[i].GrossRate);
                $scope.details[i].GrossAmt = parseFloat(Qty * GRate).toFixed(2);
            };

        } catch{ }
    }
    $scope.fn_Cal_BrokAmt = function () {
        try {

            var GRate, Qty = 0;
            for (i = 0; i < $scope.details.length; i++) {

                Qty = Number($scope.details[i].Qty);
                BUnit = Number($scope.details[i].brkpunit);
                $scope.details[i].BrokAmt = parseFloat(Qty * BUnit).toFixed(2);
            };

        } catch{ }
    }
    $scope.fn_Cal_NetRate = function () {
        try {
            for (i = 0; i < $scope.details.length; i++) {

                GRate = Number($scope.details[i].GrossRate);
                BUnit = Number($scope.details[i].brkpunit);
                // $scope.details[i].Amount = Qty * NRate;
                // alert(GRate + BUnit);
                $scope.details[i].Rate = parseFloat(GRate + BUnit).toFixed(2);
            }
        } catch{ }
    }
    $scope.fn_Cal_NetAmount = function () {
        try {

            var NRate, Qty = 0;
            for (i = 0; i < $scope.details.length; i++) {

                Qty = Number($scope.details[i].Qty);
                NRate = Number($scope.details[i].Rate);
                $scope.details[i].Amount = parseFloat(Qty * NRate).toFixed(2);
            };

        } catch{ }
    }
    $scope.Calculate_total_exp = function () {

        var total = 0;
        for (i = 0; i < $scope.Exps.length; i++) {
            total = total + Number($scope.Exps[i].ExpAmount);
        };
        $scope.TotalExpenses = parseFloat(total).toFixed(2);
        var netcal = parseFloat($scope.TotalOfNetAmount) + parseFloat($scope.TotalExpenses);
        $scope.TotalOfNetAmount = parseFloat(netcal).toFixed(2);
    }
    $scope.Calculate_Net_Amount = function () {

        var total = 0;
        for (i = 0; i < $scope.details.length; i++) {
            total = total + Number($scope.details[i].Amount);
        };
        $scope.TotalOfNetAmount = parseFloat(total).toFixed(2);
    }
    $scope.Calculate_BrokerageAmt = function () {

        var total = 0;
        for (i = 0; i < $scope.details.length; i++) {
            total = total + parseFloat($scope.details[i].BrokAmt);
        };
        $scope.TotalOfBrokAmt = parseFloat(total).toFixed(2);
    }
    $scope.Calculate_GrossAmt = function () {

        var total = 0;
        for (i = 0; i < $scope.details.length; i++) {
            total = total + parseFloat($scope.details[i].GrossAmt);
        };
        $scope.TotalGrossAmount = parseFloat(total).toFixed(2);
    }
    $scope.Calculate_GrossAmt = function () {

        var total = 0;
        var Bought = 0;
        var Sold = 0;
        for (i = 0; i < $scope.details.length; i++) {
            if ($scope.details[i].Type == "Bought") {
                Bought = Bought + parseFloat($scope.details[i].GrossAmt);
            }
            else {
                Sold = Sold + parseFloat($scope.details[i].GrossAmt);
            }

        };

        if (Bought > Sold) {
            $scope.TotalGrossAmount = parseFloat(Bought - Sold).toFixed(2);
        }
        else {
            $scope.TotalGrossAmount = parseFloat(Sold - Bought).toFixed(2);
        }
    }
    $scope.Calculate_HoldingType = function () {

        var total_TotalInTrade = 0;
        var total_TotalInInvestment = 0;
        for (i = 0; i < $scope.details.length; i++) {
            if ($scope.details[i].HoldingType == "0") {

                total_TotalInInvestment = total_TotalInInvestment + $scope.details[i].Amount;
            }
            else {
                total_TotalInTrade = total_TotalInTrade + $scope.details[i].Amount;
            }

        };
        $scope.TotalInInvestment = parseFloat(total_TotalInInvestment).toFixed(2);;
        $scope.TotalInTrade = parseFloat(total_TotalInTrade).toFixed(2);;
    }

    $scope.getDbscript = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/Get_All_Scprit", "scriptmaster", $scope, $http);
    }

    $scope.ngscriptmapping = function (ddata) {
        // alert("sdsdf");

        $scope.ScpritMappingedit = ddata;

        document.getElementById("openModalButton").click();



        // $("#").show();

    }
    $scope.updateScriptmaster = function (ddata) {
        // alert("sdsdf");
        //alert($scope.ScpritMappingedit.ScriptCode);
        //$scope.ScpritMappingedit.ScriptCode = $scope.ScpritMappingedit.ScriptCode;
        //$scope.ScpritMappingedit.ScriptName = fnGetddlText("ddlScriptCode");
        //$scope.ScpritMappingedit.Script_color = "";
        var TEXTDATA = $scope.ScpritMappingedit.ScriptName;
        var ColorScript = ($scope.ScpritMappingedit.Script_color == null ? "" : $scope.ScpritMappingedit.Script_color).toUpperCase();
        for (var i = 0; i < $scope.details.length; i++) {

            if (TEXTDATA === $scope.details[i].ScriptName) {
                //   var item = $scope.scriptmaster.find(item => item.ID === $scope.AllScriptMapModel[j].MapScriptId);

                $scope.details[i].ScriptName = fnGetddlText("ddlScriptCode");
                $scope.details[i].ScriptCode = $scope.ScpritMappingedit.ScriptCode;
                $scope.details[i].Script_color = "";
                if (ColorScript == "RED") {
                    fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/InsertScriptMapping?MapScript_ID=" + $scope.ScpritMappingedit.ScriptCode + "&&Script_Name=" + encodeURIComponent(TEXTDATA), "Mdata", $scope, $http);

                }


            }

        }

        // $("#").show();

    }
    $scope.SubmitAllScriptMap = function () {
        for (var j = 0; j < $scope.AllScriptMapModel.length; j++) {
            for (var i = 0; i < $scope.details.length; i++) {
                if ($scope.AllScriptMapModel[j].MapScriptId != "-1") {
                    if ($scope.AllScriptMapModel[j].ScriptName === $scope.details[i].ScriptName) {
                        var item = $scope.scriptmaster.find(item => item.ID === $scope.AllScriptMapModel[j].MapScriptId);

                        $scope.details[i].ScriptName = item.NAME;
                        $scope.details[i].ScriptCode = $scope.AllScriptMapModel[j].MapScriptId;
                        $scope.details[i].Script_color = "";
                        fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/InsertScriptMapping?MapScript_ID=" + $scope.AllScriptMapModel[j].MapScriptId + "&&Script_Name=" + encodeURIComponent($scope.AllScriptMapModel[j].ScriptName), "Mdata", $scope, $http);

                    }
                }
            }
        }


    }

    $scope.GetAllConsultant = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllConsultant", "Consultants", $scope, $http);

    }
    $scope.GetAllNotMapScript = function () {
        $scope.AllScriptMapModel = [];

        for (var i = 0; i < $scope.details.length; i++) {
            if ($scope.details[i].Script_color != null) {


                if ("RED" === $scope.details[i].Script_color.toUpperCase()) {
                    ScriptMapModel = {};
                    ScriptMapModel.ScriptName = $scope.details[i].ScriptName;
                    ScriptMapModel.MapScriptId = "-1";
                    $scope.AllScriptMapModel.push(ScriptMapModel);
                }
            }

        }
        console.log($scope.AllScriptMapModel);
        $scope.AllScriptMapModel = UniqueArraybyId($scope.AllScriptMapModel, "ScriptName");
        // console.log(uniqueStandards);

    }

    function UniqueArraybyId(collection, keyname) {
        var output = [],
            keys = [];

        angular.forEach(collection, function (item) {
            var key = item[keyname];
            if (keys.indexOf(key) === -1) {
                keys.push(key);
                output.push(item);
            }
        });
        return output;
    };


    $scope.PostDeatils = function (e) {

        // alert($("#BillNo").val())
        if ($("#BTN_ACTION").val() == "Update") {
            $scope.Postdata();
        }
        else {


            $http({
                method: "get",
                url: "/BrokerBillEntry/Get_Validate_billNo?BillNo=" + $("#BillNo").val()
                //data: { 'BillNo':  }
            }).then(function (response) {
                if (response.data == 1) {
                    alert("Bill No Already Exist!");
                    $("#BillNo").focus();
                    return false;
                }
                else {
                    $scope.Postdata();
                }

            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });
        }
    }

    $scope.Postdata = function () {

        for (var i = 0; i < $scope.details.length; i++) {
            if ($scope.details[i].Script_color != null) {
                if ("RED" === $scope.details[i].Script_color.toUpperCase()) {

                    alert("Please Map Script!");

                    document.getElementById("openModalButtonAll").click();
                    $scope.GetAllNotMapScript();
                    // e.preventDefault()
                    // event.preventDefault()
                    return false;
                    break;
                }
            }

        }


        if ($("#BTN_ACTION").val() == "Update") {


            var senddata = {}
            senddata.invstyp = $scope.Info.invstyp;
            senddata.Date = $scope.Info.Date;
            senddata.BrokerName = $scope.Info.BrokerName;
            senddata.Demat_Ac_Id = $scope.Info.Demat_Ac_Id;
            senddata.SettlementNo = $scope.Info.SettlementNo;
            senddata.BillNo = $scope.Info.BillNo;
            senddata.HoldingTypeCode = $scope.Info.HoldingTypeCode;
            senddata.ConsultantCode = $scope.Info.ConsultantCode;


            $("#loader-1").show();
            var posturl = "/BrokerBillEntry/BRDetailsUpdate";
            $http({
                method: "post",
                url: posturl,
                data: { 'JsonData': JSON.stringify($scope.details), 'JsonHead': JSON.stringify(senddata), 'JsonExpense': JSON.stringify($scope.Exps) }
            }).then(function (response) {
                // alert("Save");
                // $scope.GetAllMember();
                alert("Update Data Successfully")
                window.location.href = "/BrokerBillEntry/SaveBREntry";
                //  fnPostData_Msg_UsingPostRequestWithModel("/BrokerBillEntry/SaveAllDeailts", "msg", $scope, $http, "Insert Data Successfully", "/BrokerBillEntry/SaveBREntry");
            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });
        }
        else {



            $("#loader-1").show();
            $scope.btnsubmit = "Please Wait..";
            //post a data
            var posturl = "/BrokerBillEntry/BRDetailsSave";
            $http({
                method: "post",
                url: posturl,
                data: { 'JsonData': JSON.stringify($scope.details), 'BillNo': $("#BillNo").val() }
            }).then(function (response) {


                //////

                $http({
                    method: "post",
                    url: "/BrokerBillEntry/SaveAllDeailts",

                }).then(function (response) {



                    alert("Insert Data Successfully")
                    window.location.href = "/BrokerBillEntry/SaveBREntry";
                    //fnPostData_Msg_UsingPostRequestWithModel("", "msg", $scope, $http, "Insert Data Successfully", "/BrokerBillEntry/SaveBREntry");
                }, function (data) {
                    alert("Error Occur During This Request" + posturl);
                });
                //////





                //fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/SaveAllDeailts", "xyz", $scope, $http);
                //alert("Update Data Successfully")
                //fnPostData_Msg_UsingPostRequestWithModel("/BrokerBillEntry/SaveAllDeailts", "msg", $scope, $http, "Insert Data Successfully", "/BrokerBillEntry/SaveBREntry");
            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });
            //window.location.href = "/BrokerBillEntry/BRExpense";
        }
    }


});

