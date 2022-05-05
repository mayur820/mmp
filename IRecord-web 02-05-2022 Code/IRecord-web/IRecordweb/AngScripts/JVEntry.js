var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        // alert("hdsfh");
        $("#ddl_Primary_Ledger").focus();

       // $scope.GetLedger();
        $scope.details = {};
        $scope.viewdata = [];
        $scope.DataEdit = null;
        $scope.btnadd = "Add";


        var posturl = "/JVEntry/Getledger"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {
            $scope.Allledger = JSON.parse(response.data);
            if ($("#btn_submit").val() == "Update") {
                $scope.Getdetails();
                $scope.GetHeadLevel();
                $("#ddl_PrimaryLedger_cr_dr").attr("disabled", "disabled");
            }
            else {
                $scope.GetJVUNIQUENO();
            }



        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });




       
    }

    $scope.GetHeadLevel = function () {
        var posturl = "/JVEntry/Get_HeadLevel_Info"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {
            $scope.editdataeee = JSON.parse(response.data);
            console.log($scope.editdataeee);
            $("#txt_date").val($scope.editdataeee[0].txt_date);
            $("#txt_Primary_Ledger_Narration").val($scope.editdataeee[0].txt_Primary_Ledger_Narration);
            $("#txt_entery_Level_Narration").val($scope.editdataeee[0].txt_entery_Level_Narration);
            $("#ddl_Primary_Ledger").val('string:' + $scope.editdataeee[0].ddl_Primary_Ledger);
            $("#ddl_PrimaryLedger_cr_dr").val($scope.editdataeee[0].ddl_PrimaryLedger_cr_dr);
            $("#txt_Primary_Ledger_amount").val($scope.editdataeee[0].txt_Primary_Ledger_amount);
            
            $("#txt_jvno").val($scope.editdataeee[0].txt_jvno);
           // fnGetDataUsingGetRequestWithModel("/JVEntry/Getledger2?Legerid=" + 'string:' + $scope.editdataeee[0].ddl_Primary_Ledger, "Allledger2", $scope, $http);
            //$scope.GetLedger2();
            $scope.GetLedgerWithPara('string:' + $scope.editdataeee[0].ddl_Primary_Ledger);
            $("#ddl_Primary_Ledger").focus();
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }


    $scope.Getdetails = function () {
        var posturl = "/JVEntry/Get_DeatilsLevel_Info"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {
            $scope.viewdata = JSON.parse(response.data);
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
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
    $scope.GetLedgerWithPara = function (Par) {
        //alert("hdsfhFF");
      
        fnGetDataUsingGetRequestWithModel("/JVEntry/Getledger2?Legerid=" + Par, "Allledger2", $scope, $http);
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
        if ($scope.btnadd == "Update") {


            if ($("#txt_Counter_Ledger_Amount").val() != "" && $("#txt_Counter_Ledger_Amount").val() != "0" && $("#txt_Primary_Ledger_amount").val() != "" && $("#txt_Primary_Ledger_amount").val() != "0") {
                var VIEWMODEL = {};
                $scope.DataEdit.Ledger = $scope.details.Counter_Ledger_ID;
                $scope.DataEdit.LedgerName = fnGetddlText("ddl_Counter_Ledger"); //$scope.MemberName;
                $scope.DataEdit.DRCR = $("#ddl_Counter_Ledger_cr_dr").val();
                $scope.DataEdit.DRCR_NAME = fnGetddlText("ddl_Counter_Ledger_cr_dr");
                $scope.DataEdit.AMOUNT = $("#txt_Counter_Ledger_Amount").val();
                $scope.DataEdit.Narration = $("#txt_Counter_Ledger_Narration").val();
                $scope.DataEdit.EditRowStatus = 0;


                $("#ddl_PrimaryLedger_cr_dr").attr("disabled", "disabled");
                $("#txt_Counter_Ledger_Amount").val("0");
                $("#txt_Counter_Ledger_Narration").val("");
                $("#ddl_Counter_Ledger").val("");
                $scope.btnadd = "Add";
            }
            else {
                alert("Plase Enter Amount..");
                $("#txt_Counter_Ledger_Amount").focus();

            }


        }
        else {
            if ($("#btn_submit").val() != "Update") {
                if ($("#txt_Counter_Ledger_Amount").val() != "" && $("#txt_Counter_Ledger_Amount").val() != "0" && $("#txt_Primary_Ledger_amount").val() != "" && $("#txt_Primary_Ledger_amount").val() != "0") {
                    var VIEWMODEL = {};
                    VIEWMODEL.Ledger = $scope.details.Counter_Ledger_ID;
                    VIEWMODEL.LedgerName = fnGetddlText("ddl_Counter_Ledger"); //$scope.MemberName;
                    VIEWMODEL.DRCR = $("#ddl_Counter_Ledger_cr_dr").val();
                    VIEWMODEL.DRCR_NAME = fnGetddlText("ddl_Counter_Ledger_cr_dr");
                    VIEWMODEL.AMOUNT = $scope.detail.Counter_Ledger_Amount;
                    VIEWMODEL.Narration = $scope.detail.Counter_Ledger_Narration;
                    VIEWMODEL.EditRowStatus = 0;

                    $scope.viewdata.push(VIEWMODEL);
                    $("#ddl_PrimaryLedger_cr_dr").attr("disabled", "disabled");
                    $("#txt_Counter_Ledger_Amount").val("0");
                    $("#txt_Counter_Ledger_Narration").val("");
                    $("#ddl_Counter_Ledger").val("");
                }
                else {
                    alert("Plase Enter Amount..");
                    $("#txt_Counter_Ledger_Amount").focus();

                }
            }
            else {
                alert("In Update Case Not Alloted To Add Row!!");
            }
        }




    }
    $scope.deleterow = function (data) {

        var person_to_delete = $scope.viewdata[data];
        $scope.viewdata.splice(data, 1);

    }
    $scope.editrow = function (data) {
       
        $scope.DataEdit = null;

        $scope.details.Counter_Ledger_ID = data.Ledger;
        $("#ddl_Counter_Ledger_cr_dr").val(data.DRCR)
        $("#txt_Counter_Ledger_Amount").val(data.AMOUNT)
        $("#txt_Counter_Ledger_Narration").val(data.Narration)
        data.EditRowStatus = 1;
        $scope.DataEdit = data;
        $scope.btnadd = "Update";

    

    }
    $scope.checkamountvalid = function () {
        var tableamount = 0;
        angular.forEach($scope.viewdata, function (value) {
            if (value.EditRowStatus == 0) {
                tableamount += parseInt(value.AMOUNT);
            }
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



        var tableAmount = 0;
        angular.forEach($scope.viewdata, function (value) {
            tableAmount += parseInt(value.AMOUNT);
        });
        var x = parseInt($("#txt_Primary_Ledger_amount").val());

        if (x == tableAmount) {


            var BankStatement_Index = {};
            BankStatement_Index.Trans_Dt = $("#txt_date").val();

            BankStatement_Index.Vouch_No = $("#txt_jvno").val();
            BankStatement_Index.Dr_CR_Code = $("#ddl_Counter_Ledger_cr_dr").val();


            BankStatement_Index.Parent_Narr = $("#txt_Primary_Ledger_Narration").val();

            BankStatement_Index.Counter_Narr = $("#txt_Counter_Ledger_Narration").val();

            BankStatement_Index.Entry_Narr = $("#txt_entery_Level_Narration").val();
            
            BankStatement_Index.Parent_Ac_Code = $("#ddl_Primary_Ledger").val().replace("string:",""); //$("#ddl_Primary_Ledger").val();
            // BankStatement_Index.Parent_Ac_Type = $scope.Parent_Ac_Type;
            BankStatement_Index.Parent_Ac_Type = $("#ddl_PrimaryLedger_cr_dr").val();

            BankStatement_Index.Parent_Amt = $("#txt_Primary_Ledger_amount").val();
            var i = JSON.stringify($scope.viewdata);
            var posturl = "";
            if ($("#btn_submit").val() == "Update") {
                posturl = "/JVEntry/UpdateJV";
            }
            else {
                posturl = "/JVEntry/IndexSave";
            }

            //posturl = "/JVEntry/IndexSave";
            $http({
                method: "post",
                url: posturl,
                data: { 'JsonData': '[' + JSON.stringify(BankStatement_Index) + ']', 'JsonData2': i }
            }).then(function (response) {
                
                if ($("#btn_submit").val() == "Update") {
                    alert("Updated");
                }
                else {
                    alert("Save");
                }

                window.location = "/JVEntry/ViewJV";
                // $scope.GetAllMember();
            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });
        }
        else {
            alert("Please Enter Valid Amount");
        }


    }


});

