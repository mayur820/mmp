var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        // alert("hdsfh");
        $scope.GetLedger();
        $scope.GetJVUNIQUENO();
        $scope.viewdata = [];
    }

    $scope.GetJVUNIQUENO = function () {


        $http({
            method: "get",
            url: "/JVEntry/GetJVUNIQUENO"
        }).then(function (response) {
            $scope.ALLJVUNIQUENO = JSON.parse(response.data);

            $("#txt_jvno").val($scope.ALLJVUNIQUENO[0].ID);
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        })


        //  fnGetDataUsingGetRequestWithModel(, "", $scope, $http);

        //   $("#txt_jvno").val(ALLJVUNIQUENO.ID);

    };
    $scope.GetLedger = function () {
        //alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/JVEntry/Getledger", "Allledger", $scope, $http);
    };
    $scope.GetLedger2 = function () {
        //alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/JVEntry/Getledger2?Legerid=" + $("#ddl_Primary_Ledger").val(), "Allledger2", $scope, $http);
    };
    $scope.fillallGetOperator = function () {
        //   alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/MainMemberMaster/GetOperator", "FILLOperator", $scope, $http);
    };
    $scope.fillallGetFAMILY = function () {
        //   alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/MainMemberMaster/GetFAMILY", "FILLFAMILY", $scope, $http);
    };

    $scope.FN_SAVE = function () {
    
        if ($("#txt_Counter_Ledger_Amount").val() != "" && $("#txt_Counter_Ledger_Amount").val() != "0" && $("#txt_Primary_Ledger_amount").val() != "" && $("#txt_Primary_Ledger_amount").val() != "0") {
            var VIEWMODEL = {};
            VIEWMODEL.Ledger = $scope.details.Counter_Ledger_ID;
            VIEWMODEL.LedgerName = fnGetddlText("ddl_Counter_Ledger"); //$scope.MemberName;
            VIEWMODEL.DRCR = $("#ddl_Counter_Ledger_cr_dr").val();
            VIEWMODEL.DRCR_NAME = fnGetddlText("ddl_Counter_Ledger_cr_dr");
            VIEWMODEL.AMOUNT = $scope.detail.Counter_Ledger_Amount;
            VIEWMODEL.Narration = $scope.detail.Counter_Ledger_Narration;

            $scope.viewdata.push(VIEWMODEL);
            $("#txt_Counter_Ledger_Amount").val("0");
            $("#txt_Counter_Ledger_Narration").val("");
        }
        else {
            alert("Plase Enter Amount..");
            $("#txt_Counter_Ledger_Amount").focus();

}
     
    }
    $scope.deleterow = function (data) {

        var person_to_delete = $scope.viewdata[data];
        $scope.viewdata.splice(data, 1);

    }
    $scope.editrow = function (data) {
        $scope.details.Counter_Ledger_ID = data.Ledger;
        $scope.details.Counter_Ledger_cr_dr = data.DRCR;
        $scope.detail.Counter_Ledger_Amount = data.AMOUNT;
        $scope.detail.Counter_Ledger_Narration = data.Narration;


    }
    $scope.checkamountvalid = function () {
        var tableamount = 0;
        angular.forEach($scope.viewdata, function (value) {
            tableamount += parseInt(value.AMOUNT);
        });
        var orgnialamount = parseInt($("#txt_Counter_Ledger_Amount").val()) + tableamount;
        var x = parseInt($("#txt_Primary_Ledger_amount").val());
        if (x < orgnialamount) {
            alert("Plase Enter Valid amount");
            $("#txt_Counter_Ledger_Amount").val(0);
            $("#btn_add").hide();


        }
        else {
            $("#btn_add").show();
        }
    }

    $scope.drcrchange = function () {

        var i = $("#ddl_PrimaryLedger_cr_dr").val();
        if (i == "1") {
            $("#ddl_Counter_Ledger_cr_dr").val("2");

        }
        else {
            $("#ddl_Counter_Ledger_cr_dr").val("1");
        }

    }

    $scope.SubmitData = function () {
        // alert("hdsfh");ID, OperatorName, Mobile, Email, ManagerId, Sucriberid, Ismanager, Createddate, Createdby



        var BankStatement_Index = {};
        //    BankStatement_Index.MemberID = $scope.MemberID;
        //  BankStatement_Index.Trans_No = "0";
        //   BankStatement_Index.Sr_No = $scope.detail.Sr_No;
        BankStatement_Index.Trans_Dt = $("#txt_date").val();

        //BankStatement_Index.Member_Code = $scope.detail.Member_Code;
        //BankStatement_Index.Year_Code = $scope.detail.Year_Code;

        BankStatement_Index.Vouch_No = $("#txt_jvno").val();
        //  BankStatement_Index.Dr_Code = $scope.Dr_Code;
        BankStatement_Index.Dr_CR_Code = $("#ddl_Counter_Ledger_cr_dr").val();
        //  BankStatement_Index.Cr_Code = $scope.Cr_Code;
        // BankStatement_Index.Cr_Code = $("#ddl_Counter_Ledger_cr_dr").val();
        //BankStatement_Index.Amount = $scope.detail.Amount;


        BankStatement_Index.Parent_Narr = $("#txt_Primary_Ledger_Narration").val();

        BankStatement_Index.Counter_Narr = $("#txt_Counter_Ledger_Narration").val();

        BankStatement_Index.Entry_Narr = $("#txt_entery_Level_Narration").val();

        BankStatement_Index.Parent_Ac_Code = $scope.detail.Primary_Ledger_Id; //$("#ddl_Primary_Ledger").val();
        // BankStatement_Index.Parent_Ac_Type = $scope.Parent_Ac_Type;
        BankStatement_Index.Parent_Ac_Type = $("#ddl_PrimaryLedger_cr_dr").val();

        BankStatement_Index.Parent_Amt = $("#txt_Primary_Ledger_amount").val();
        var i = JSON.stringify($scope.viewdata);



        var posturl = "/JVEntry/IndexSave";
        $http({
            method: "post",
            url: posturl,
            data: { 'JsonData': '[' + JSON.stringify(BankStatement_Index) + ']', 'JsonData2': i }
        }).then(function (response) {
            alert("Save");
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });


    }


});

