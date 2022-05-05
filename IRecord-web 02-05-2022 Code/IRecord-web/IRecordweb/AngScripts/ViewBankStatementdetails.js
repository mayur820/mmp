var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.getDetails();
        $scope.getAcc();
        $scope.btnsubmit = "Final Submit";
    }

    $scope.getDetails = function () {
        fnGetDataUsingGetRequestWithModel("/BankStatementImport/ReadAllBankData", "Info", $scope, $http);

    }
    $scope.getAcc = function () {
        fnGetDataUsingGetRequestWithModel("/BankStatementImport/ViewACCOUNT", "Acc", $scope, $http);

    }
    $scope.FndeleteItems = function (detail) {

        var r = confirm("Are you sure delete this item");
        if (r == true) {
            //if ($("#BTN_ACTION").val() == "Update") {
            //    $http({
            //        method: "get",
            //        url: "/BrokerBillEntry/DeleteDetails?ID=" + detail.ID
            //    }).then(function (response) {
            //        var i = $scope.details.indexOf(detail);
            //        if (i >= 0) $scope.details.splice(i, 1);

            //    }, function (data) {
            //        //deferred.reject({ message: "Really bad" });
            //        alert("Error Occur During This Request" + geturl);
            //    })
            //}
            //else {
            var i = $scope.Info.indexOf(detail);
            if (i >= 0) $scope.Info.splice(i, 1);
            // }


        } else {

        }



    }
    $scope.Ac_click = function (data) {

        $scope.ACMappingedit = data;
        document.getElementById("openModalButton").click();


    }
    $scope.AddAccClick = function () {

      
        document.getElementById("openModalAccountAdd").click();


    }
    
    $scope.fnsubmit = function () {
        //  fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/SaveBREntry", "SaveBREntry", $scope, $http);
        window.location.href = "/BankStatementImport/SaveInfo";
    }
    $scope.updateScriptmaster = function (ddata) {
        // alert("sdsdf");
        //alert($scope.ScpritMappingedit.ScriptCode);
        //$scope.ScpritMappingedit.ScriptCode = $scope.ScpritMappingedit.ScriptCode;
        //$scope.ScpritMappingedit.ScriptName = fnGetddlText("ddlScriptCode");
        //$scope.ScpritMappingedit.Script_color = "";
        var TEXTDATA = $scope.ACMappingedit.Accounts;
        var ColorScript = $scope.ACMappingedit.Accounts_color.toUpperCase();
        for (var i = 0; i < $scope.Info.length; i++) {

            if (TEXTDATA === $scope.Info[i].Accounts) {
                //   var item = $scope.scriptmaster.find(item => item.ID === $scope.AllScriptMapModel[j].MapScriptId);

                $scope.Info[i].Accounts = fnGetddlText("ddlACCode");
                $scope.Info[i].ACcode = $scope.ACMappingedit.ACcode;
                $scope.Info[i].Accounts_color = "";
                if (ColorScript == "RED") {
                    //fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/InsertScriptMapping?MapScript_ID=" + $scope.ScpritMappingedit.ScriptCode + "&&Script_Name=" + encodeURIComponent(TEXTDATA), "Mdata", $scope, $http);

                }


            }

        }

        // $("#").show();

    }
    $scope.GetAllNotMapScript = function () {
        $scope.AllScriptMapModel = [];

        for (var i = 0; i < $scope.Info.length; i++) {
            if ($scope.Info[i].Accounts_color != null) {


                if ("RED" === $scope.Info[i].Accounts_color.toUpperCase()) {
                    ScriptMapModel = {};
                    ScriptMapModel.Accounts = $scope.Info[i].Accounts;
                    ScriptMapModel.ACcode = "-1";
                    $scope.AllScriptMapModel.push(ScriptMapModel);
                }
            }

        }
        console.log($scope.AllScriptMapModel);
        $scope.AllScriptMapModel = UniqueArraybyId($scope.AllScriptMapModel, "ScriptName");
        // console.log(uniqueStandards);

    }
    $scope.SubmitAllScriptMap = function () {
        for (var j = 0; j < $scope.AllScriptMapModel.length; j++) {
            for (var i = 0; i < $scope.Info.length; i++) {
                if ($scope.AllScriptMapModel[j].ACcode != "-1") {
                    if ($scope.AllScriptMapModel[j].Accounts === $scope.Info[i].Accounts) {
                        var item = $scope.Acc.find(item => item.ID === $scope.AllScriptMapModel[j].ACcode);

                        $scope.Info[i].Accounts = item.NAME;
                        $scope.Info[i].ACcode = $scope.AllScriptMapModel[j].ACcode;
                        $scope.Info[i].Accounts_color = "";
                        // fnPostDataUsingPostRequestWithModel("/BrokerBillEntry/InsertScriptMapping?MapScript_ID=" + $scope.AllScriptMapModel[j].MapScriptId + "&&Script_Name=" + encodeURIComponent($scope.AllScriptMapModel[j].ScriptName), "Mdata", $scope, $http);

                    }
                }
            }
        }
        for (var i = 0; i < $scope.Info.length; i++) {
            if ($scope.Info[i].Accounts_color != null) {
                if ("RED" === $scope.Info[i].Accounts_color.toUpperCase()) {

                    //alert("Please Mapped Account!");

                    //document.getElementById("openModalButtonAll").click();
                    //$scope.GetAllNotMapScript();

                    return false;
                    break;
                }
            }

        }
        var r = confirm("All Account Are Map Successfully Click Ok Then Save Your Bank Statement?");
        if (r == true) {
         
            $scope.PostDeatils();

        } else {

        }

    }

    $scope.PostDeatils = function (e) {




        for (var i = 0; i < $scope.Info.length; i++) {
            if ($scope.Info[i].Accounts_color != null) {
                if ("RED" === $scope.Info[i].Accounts_color.toUpperCase()) {

                    alert("Please Mapped Account!");

                    document.getElementById("openModalButtonAll").click();
                    $scope.GetAllNotMapScript();

                    return false;
                    break;
                }
            }

        }


        //if ($("#BTN_ACTION").val() == "Update") {


        //    var senddata = {}
        //    senddata.invstyp = $scope.Info.invstyp;
        //    senddata.Date = $scope.Info.Date;
        //    senddata.BrokerName = $scope.Info.BrokerName;
        //    senddata.Demat_Ac_Id = $scope.Info.Demat_Ac_Id;
        //    senddata.SettlementNo = $scope.Info.SettlementNo;
        //    senddata.BillNo = $scope.Info.BillNo;
        //    senddata.HoldingTypeCode = $scope.Info.HoldingTypeCode;
        //    senddata.ConsultantCode = $scope.Info.ConsultantCode;


        //    $("#loader-1").show();
        //    var posturl = "/BrokerBillEntry/BRDetailsUpdate";
        //    $http({
        //        method: "post",
        //        url: posturl,
        //        data: { 'JsonData': JSON.stringify($scope.details), 'JsonHead': JSON.stringify(senddata), 'JsonExpense': JSON.stringify($scope.Exps) }
        //    }).then(function (response) {
        //        // alert("Save");
        //        // $scope.GetAllMember();
        //        alert("Update Data Successfully")
        //        window.location.href = "/BrokerBillEntry/SaveBREntry";
        //        //  fnPostData_Msg_UsingPostRequestWithModel("/BrokerBillEntry/SaveAllDeailts", "msg", $scope, $http, "Insert Data Successfully", "/BrokerBillEntry/SaveBREntry");
        //    }, function (data) {
        //        alert("Error Occur During This Request" + posturl);
        //    });
        //}
        //else {



        $("#loader-1").show();
        $scope.btnsubmit = "Please Wait..";


        $http({
            method: "post",
            url: "/BankStatementImport/Savedata",
            data: { 'JsonData': JSON.stringify($scope.Info) }
        }).then(function (response) {



            alert("Insert Data Successfully")
            window.location.href = "/BankStatementImport/Index";
            //fnPostData_Msg_UsingPostRequestWithModel("", "msg", $scope, $http, "Insert Data Successfully", "/BrokerBillEntry/SaveBREntry");
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

        //window.location.href = "/BrokerBillEntry/BRExpense";
        //  }
    }
});

