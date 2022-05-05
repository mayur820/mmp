var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        $scope.Details = [];
        $scope.GetAllConsultant();
        $scope.GetLoadDeatils();
        $scope.GetLoadHead();
        $scope.btntextAdd = "Add";
        $scope.btntext = "Final Submit"
        $scope.getDbscript();

    }
    $scope.GetLoadHead = function () {
        $http({
            method: "get",
            url: "/OpeningStock/GetOp_HeadSection"
        }).then(function (response) {
            debugger
            $scope.dataHead = JSON.parse(response.data);

        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            //alert("Error Occur During This Request" + geturl);
        })

    }
    $scope.fnReset = function (data) {
        $scope.txtQuantity = "";
        $scope.txtRateofPurchase = "";
        $scope.txtInvestmentAmount = "";
        //$scope.ddlConsultant = "";
        $scope.ddlscript= "";
        $("#ddlscript").val('').trigger('change');
        /*$scope.getDbscript();*/
    }


    $scope.EditItems = function (data) {
        $("#txt_Date").val(data.DateofPurchase)
        $scope.ddlInvenstmentType = data.InvestmentType;

        $scope.txtQuantity = data.Quantity;
        $scope.txtRateofPurchase = data.RateofPurchase;
        $scope.txtInvestmentAmount = data.InvestmentAmount;
        $scope.ddlConsultant = parseInt(data.ConsultantId);
        $scope.ddlscript = parseInt(data.ScriptID);



        $scope.btntextAdd = "Update";
        $scope.Edititemsdata = data;
        // console.log(Edititemsdata);
    }
    $scope.FndeleteItems = function (data) {

        var r = confirm("Are you sure delete this item?");
        if (r == true) {
            var i = $scope.Details.indexOf(data);
            if (i >= 0) $scope.Details.splice(i, 1);
        } else {

        }
    }
    $scope.getDbscript = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/Get_All_Scprit", "scriptmaster", $scope, $http);
    }
    $scope.fnScriptMapping = function (ddata) {
        document.getElementById("openModalButton").click();
        $scope.ScpritMappingedit = ddata;
        //alert("Not");
    }
    $scope.updateScriptmaster = function (ddata) {
        // alert("sdsdf");
        //alert($scope.ScpritMappingedit.ScriptCode);
        //$scope.ScpritMappingedit.ScriptCode = $scope.ScpritMappingedit.ScriptCode;
        //$scope.ScpritMappingedit.ScriptName = fnGetddlText("ddlScriptCode");
        //$scope.ScpritMappingedit.Script_color = "";
        debugger;
        var TEXTDATA = $scope.ScpritMappingedit.ScriptName;
        var ColorScript = $scope.ScpritMappingedit.ScriptColor.toUpperCase();
        for (var i = 0; i < $scope.Details.length; i++) {

            if (TEXTDATA === $scope.Details[i].ScriptName) {
                //   var item = $scope.scriptmaster.find(item => item.ID === $scope.AllScriptMapModel[j].MapScriptId);

                $scope.Details[i].ScriptName = fnGetddlText("ddlScriptCode");
                $scope.Details[i].ScriptID = $scope.ScpritMappingedit.ScriptID;
                $scope.Details[i].ScriptColor = "";
                if (ColorScript == "RED") {
                    //alert("asdf");
                    fnPostDataUsingPostRequestWithModel("/OpeningStock/InsertScriptMapping?MapScript_ID=" + $scope.ScpritMappingedit.ScriptID + "&&Script_Name=" + encodeURIComponent(TEXTDATA), "Mdata", $scope, $http);

                }


            }

        }

        // $("#").show();

    }
    $scope.GetLoadDeatils = function () {
        $http({
            method: "get",
            url: "/OpeningStock/GetOpDetailsdt"
        }).then(function (response) {
            $scope.Details = JSON.parse(response.data);

        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            //alert("Error Occur During This Request" + geturl);
        })


        // fnGetDataUsingGetRequestWithModel("/OpeningStock/GetOpDetailsdt", "Details", $scope, $http);

    }
    $scope.GetAllConsultant = function () {
        fnGetDataUsingGetRequestWithModel("/OpeningStock/GetAllConsultant", "Consultants", $scope, $http);

    }
    $scope.GetAllDemate = function () {
        fnGetDataUsingGetRequestWithModel("/OpeningStock/GetAllDemate", "Demates", $scope, $http);
        console.log($scope.Demates)
    }
    $scope.GetAllBroker = function () {
        fnGetDataUsingGetRequestWithModel("/OpeningStock/GetAllBroker", "Brokers", $scope, $http);

    }
    $scope.fnAdd = function () {
        if ($("#txt_Date").val() == "") {
            alert("Please Enter Date!")
            $("#ddlSegment").focus();
            return false;
        }
        if ($("#ddlInvenstmentType").val() == "") {
            alert("Please Select Invenstment Type!")
            $("#ddlInvenstmentType").focus();
            return false;
        }

        if ($("#ddlscript").val() == "") {
            alert("Please Select script!")
            $("#ddlscript").focus();
            return false;
        }

        if ($("#txtQuantity").val() == "") {
            alert("Please Enter Quantity!")
            $("#txtQuantity").focus();
            return false;
        }

        if ($("#txtRateofPurchase").val() == "") {
            alert("Please Enter Rate of Purchase!")
            $("#txtRateofPurchase").focus();
            return false;
        }

        if ($("#txtInvestmentAmount").val() == "") {
            alert("Please Enter Investment Amount!")
            $("#txtInvestmentAmount").focus();
            return false;
        }

        if ($("#ddlConsultant").val() == "") {
            alert("Please Select Consultant!")
            $("#ddlConsultant").focus();
            return false;
        }




        if ($scope.btntextAdd == "Update") {
            $scope.Edititemsdata.DateofPurchase = $("#txt_Date").val();
            $scope.Edititemsdata.InvestmentType = $scope.ddlInvenstmentType;
            $scope.Edititemsdata.InvestmentTypeName = fnGetddlText("ddlInvenstmentType");
            $scope.Edititemsdata.ScriptName = fnGetddlText("ddlscript");
            $scope.Edititemsdata.ScriptID = $scope.ddlscript;
            $scope.Edititemsdata.ScriptColor = "";
            $scope.Edititemsdata.Quantity = $scope.txtQuantity;
            $scope.Edititemsdata.RateofPurchase = $scope.txtRateofPurchase;
            $scope.Edititemsdata.InvestmentAmount = $scope.txtInvestmentAmount;
            $scope.Edititemsdata.ConsultantId = $scope.ddlConsultant;
            $scope.Edititemsdata.ConsultantName = fnGetddlText("ddlConsultant");
            $scope.btntextAdd = "Add";
            $scope.fnReset();
        }
        else {
            if ($("#btn_submit").val() == "Update") {
                alert("During to Updatetion Add items Not Allowed")
            }
            else {
                var m_Data = {};
                m_Data.DateofPurchase = $("#txt_Date").val();
                m_Data.InvestmentType = $scope.ddlInvenstmentType;
                m_Data.InvestmentTypeName = fnGetddlText("ddlInvenstmentType");
                m_Data.ScriptName = fnGetddlText("ddlscript");
                m_Data.ScriptID = $scope.ddlscript;
                m_Data.ScriptColor = "";
                m_Data.Quantity = $scope.txtQuantity;
                m_Data.RateofPurchase = $scope.txtRateofPurchase;
                m_Data.InvestmentAmount = $scope.txtInvestmentAmount;
                m_Data.ConsultantId = $scope.ddlConsultant;
                m_Data.ConsultantName = fnGetddlText("ddlConsultant");
                $scope.Details.push(m_Data);
                $scope.fnReset();
            }
        }

    }
    $scope.GetAllNotMapScript = function () {
        $scope.AllScriptMapModel = [];

        for (var i = 0; i < $scope.Details.length; i++) {
            if ($scope.Details[i].ScriptColor != null) {


                if ("RED" === $scope.Details[i].ScriptColor.toUpperCase()) {
                    ScriptMapModel = {};
                    ScriptMapModel.ScriptName = $scope.Details[i].ScriptName;
                    ScriptMapModel.MapScriptId = "-1";
                    $scope.AllScriptMapModel.push(ScriptMapModel);
                }
            }

        }
        console.log($scope.AllScriptMapModel);
        $scope.AllScriptMapModel = UniqueArraybyId($scope.AllScriptMapModel, "ScriptName");
        // console.log(uniqueStandards);

    }



    $scope.FnCalculateInvestmentAmount = function () {
        var Quantity = 1;
        var RateofPurchase = 1;
        if ($("#txtQuantity").val() != "" && $("#txtQuantity").val() != "0") {
            Quantity = $scope.txtQuantity;
        }
        if ($("#txtRateofPurchase").val() != null && $("#txtRateofPurchase").val() != "0") {
            RateofPurchase = $scope.txtRateofPurchase;
        }
        $scope.txtInvestmentAmount = (Quantity * RateofPurchase);
    }
    $scope.SubmitAllScriptMap = function () {
        for (var j = 0; j < $scope.AllScriptMapModel.length; j++) {
            for (var i = 0; i < $scope.Details.length; i++) {
                if ($scope.AllScriptMapModel[j].MapScriptId != "-1") {
                    if ($scope.AllScriptMapModel[j].ScriptName === $scope.Details[i].ScriptName) {
                        var item = $scope.scriptmaster.find(item => item.ID === $scope.AllScriptMapModel[j].MapScriptId);

                        $scope.Details[i].ScriptName = item.NAME;
                        $scope.Details[i].ScriptID = $scope.AllScriptMapModel[j].MapScriptId;
                        $scope.Details[i].ScriptColor = "";
                        fnPostDataUsingPostRequestWithModel("/OpeningStock/InsertScriptMapping?MapScript_ID=" + $scope.AllScriptMapModel[j].MapScriptId + "&&Script_Name=" + encodeURIComponent($scope.AllScriptMapModel[j].ScriptName), "Mdata", $scope, $http);

                    }
                }
            }
        }

        for (var i = 0; i < $scope.Details.length; i++) {
            if ($scope.Details[i].ScriptColor != null) {
                if ("RED" === $scope.Details[i].ScriptColor.toUpperCase()) {


                    return false;
                    break;
                }
            }

        }

        var r = confirm("All Scripts  Are Map Successfully Click Ok Then Save Your Opening  Stock?");
        if (r == true) {

            $scope.FnFinalSubmit();

        } else {

        }

    }

    $scope.FnUpdateSubmit = function () {
        $("#loader-1").show();

        var Pdata = new FormData();
        Pdata.append("JsonData", JSON.stringify($scope.Details));

        debugger;

        $.ajax({
            type: "POST",
            url: "/OpeningStock/FnFinalUpdate",
            type: 'POST',
            data: Pdata,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                //show content
                alert("Insert Data Successfully");
                window.location.href = "/OpeningStock/OpeningStocklIST";

            }
        });
    }
    $scope.FnFinalSubmit = function () {

        debugger;


        for (var i = 0; i < $scope.Details.length; i++) {
            if ($scope.Details[i].ScriptColor != null) {
                if ("RED" === $scope.Details[i].ScriptColor.toUpperCase()) {

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


        $("#loader-1").show();

        var Pdata = new FormData();
        Pdata.append("JsonData", JSON.stringify($scope.Details));

        debugger;

        $.ajax({
            type: "POST",
            url: "/OpeningStock/FnFinalSubmit",
            type: 'POST',
            data: Pdata,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                //show content
                alert("Insert Data Successfully");
                window.location.href = "/OpeningStock/OpeningStocklIST";

            }
        });




    }

});

