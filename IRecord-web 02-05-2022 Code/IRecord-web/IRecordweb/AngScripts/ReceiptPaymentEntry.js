var app = angular.module("myApp", ['angucomplete-alt']);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $("#Mode").focus();
        $scope.GetAllNarration();
        $scope.GetAllCashBank();
        //$scope.GetPaymode();
        $scope.Multimodel = [];
      //  $scope.GetAllAccount();
        $scope.btntext = "Final Submit";
        $("#CONTRentryflg").hide();

        $scope.btntextAdd = "Add";
        $scope.Edititemsdata = null;
        $scope.Book_Type = "BK";
        ///////////

     
        var posturl1 = "/ReceiptPaymentEntry/TypeList1"
        $http({
            method: "get",
            url: posturl1

        }).then(function (response) {
            $scope.Cashbanklist = JSON.parse(response.data);
           /////////////
            var posturl = "/ReceiptPaymentEntry/GetAllAccountList1"
            $http({
                method: "get",
                url: posturl

            }).then(function (response) {
                $scope.Accounts = JSON.parse(response.data);
                if ($("#btn_submit").val() == "Update") {
                    $scope.GetHeadLevel();
                    $scope.Getdetails();
                }
                else {
                    // alert("Not");
                }



            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });


            //////////////



        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });


       

       
    }
    $scope.GetCheckForContraEntry = function () {

        var TEXTCashBAccount = "";
        for (var i = 0; i < $scope.CashBAccount.length; i++) {

            if ($scope.ddlCashBankAccount === $scope.CashBAccount[i].ID) {
                //   var item = $scope.scriptmaster.find(item => item.ID === $scope.AllScriptMapModel[j].MapScriptId);
                TEXTCashBAccount = $scope.CashBAccount[i].GroupName
             

            }

        }
        var TEXTddlAccount = "";
        for (var i = 0; i < $scope.Accounts.length; i++) {

            if ($scope.ddlAccount === $scope.Accounts[i].ID) {
                //   var item = $scope.scriptmaster.find(item => item.ID === $scope.AllScriptMapModel[j].MapScriptId);
                TEXTddlAccount = $scope.Accounts[i].GroupName


            }

        }

        var flgchecker = 0;
        if (TEXTCashBAccount == "BANK" && TEXTddlAccount == "BANK") {
            flgchecker = 1;
        }
        if (TEXTCashBAccount == "BANK" && TEXTddlAccount == "CASH") {
            flgchecker = 1;
        }
        if (TEXTCashBAccount == "CASH" && TEXTddlAccount == "BANK") {
            flgchecker = 1;
        }
        if (flgchecker == 1) {
            $("#CONTRentryflg").show();
            $scope.Book_Type="CB"
        }
        else {
            $("#CONTRentryflg").hide();
            $scope.Book_Type = "BK"
        }


    }
    $scope.GetHeadLevel = function () {
        var posturl = "/ReceiptPaymentEntry/Get_HeadLevel_Info"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {

            //ID									Amount	Narration
           
             $scope.editdataeee = JSON.parse(response.data);
            console.log($scope.editdataeee);
            $("#txt_Date").val($scope.editdataeee[0].Date);
            $scope.Mode = $scope.editdataeee[0].Mode
            $("#ddlCashbanklist").val("number:" + $scope.editdataeee[0].CashBank);
            $("#Cheque").val($scope.editdataeee[0].ChequeNo);
            $("#Mode").focus();
            $("#txt_reference").val($scope.editdataeee[0].RefernceNo);
            if ($scope.editdataeee[0].CashBank == 20) {
               // alert("1");
                $('#showincash').addClass('displayNO');
                $('#showinbank').removeClass('displayNO');
                $('#divdemo').removeClass('displayNO');
              
              
            } else {
              //  alert("2");
                $('#showinbank').addClass('displayNO');
                $('#showincash').removeClass('displayNO');
                $('#divdemo').removeClass('displayNO');
                
            }
            $scope.GetAccount();
          //  $scope.GetPaymode();

            var posturle = "/ReceiptPaymentEntry/GetPaymode?Name=" + $scope.editdataeee[0].CashBank
            $http({
                method: "get",
                url: posturle

            }).then(function (response) {
                $scope.paymod = JSON.parse(response.data);
                //$("#ddlPaymentMode").val("number:" + $scope.editdataeee[0].PaymentMode);
                $scope.ddlPaymentMode = $scope.editdataeee[0].PaymentMode;
                // $scope.GetAllMember();
            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });
          
         
          
            //$("#ddlCashBankAccount").val("number:" + $scope.editdataeee[0].CashBankAccount);
            //$("#ddlAccount").val("number:" + $scope.editdataeee[0].Account);
            
            



        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }

    $scope.Getdetails = function () {
        var posturl = "/ReceiptPaymentEntry/Get_DeatilsLevel_Info"
        $http({
            method: "get",
            url: posturl

        }).then(function (response) {
            $scope.Multimodel = JSON.parse(response.data);
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }
    // Get State record

    //$scope.state = function () {

    //    $http.get('/ReceiptPaymentEntry/GetPaymode?Name=' + $scope.Cashbank).then(function (d) {
    //    $scope.Cashbanklist = d.data;

    //    });

    //};
    $scope.changecashbanklist = function () {
        
        $('#divdemo').removeClass('displayNO');
        if ($scope.ddlCashbanklist == "20") {
            $('#showincash').addClass('displayNO');
            $('#showinbank').removeClass('displayNO');
         
           
            $scope.GetPaymode();
            $scope.GetAccount();
        } else {
            $('#showinbank').addClass('displayNO');
            $('#showincash').removeClass('displayNO');
            //$('.actionButton.single-pet').addClass('single-pet-shown');
            //$('#showincash').attr("style", "display: inline !important");

            //$('#showinbank').attr("style", "display: none !important");
            //$("#showinbank").hide();
            //$("#showincash").show();
            $scope.GetPaymode();
            $scope.GetAccount();
        }
    }

    $scope.GetAllCashBankNotInAccount = function () {

       
        var posturl1 = "/ReceiptPaymentEntry/GetAllAccountList1"
        $http({
            method: "get",
            url: posturl1

        }).then(function (response) {
            $scope.Accounts = JSON.parse(response.data);
        
            for (var i = 0; i <= $scope.Accounts.length; i++) {

                if ($scope.ddlCashBankAccount === $scope.Accounts[i].ID) {

                    $scope.ddlAccount = "";

                    //$("#ddlAccount").val("");
                    $scope.Accounts.splice(i, 1)
                    break;
                }

            }


        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
        $scope.GetCheckForContraEntry();

        //$scope.GetAccount();
        //for (var i = 0; i <= $scope.Accounts.length; i++) {

        //    if ($scope.ddlCashBankAccount === $scope.Accounts[i].ID) {

        //        $scope.ddlAccount = "";
                
        //        //$("#ddlAccount").val("");
        //        $scope.Accounts.splice(i, 1)
        //        break;
        //    }

        //}
    }

    $scope.GetAllCashBank = function () {
        fnGetDataUsingGetRequestWithModel("/ReceiptPaymentEntry/TypeList1", "Cashbanklist", $scope, $http);

    }
    $scope.GetPaymode = function () {
        fnGetDataUsingGetRequestWithModel("/ReceiptPaymentEntry/GetPaymode?Name=" + $scope.ddlCashbanklist, "paymod", $scope, $http);
    }
    $scope.GetAccount = function () {
        fnGetDataUsingGetRequestWithModel("/ReceiptPaymentEntry/GetAccountList?Name=" + $scope.ddlCashbanklist, "CashBAccount", $scope, $http);
    }
    $scope.GetAllAccount = function () {
        fnGetDataUsingGetRequestWithModel("/ReceiptPaymentEntry/GetAllAccountList1", "Accounts", $scope, $http);

    }
    $scope.GetAllNarration = function () {
        fnGetDataUsingGetRequestWithModel("/ReceiptPaymentEntry/GetNarrationList", "Narration", $scope, $http);

    }

    $scope.FnAdd = function () {

        if ($("#txt_Narration_value").val() == "") {
            alert("Please Enter Narration!");
            $("#txt_Narration_value").focus();
            return false;
        }

        if ($scope.btntextAdd == "Update") {
          
            //$scope.Edititemsdata.txtDate = $("#txt_Date").val();
            //$scope.Edititemsdata.Mode = $scope.Mode;
            //$scope.Edititemsdata.ddlCashbanklist = $("#ddlCashbanklist").val();
            //$scope.Edititemsdata.txt_CashBank = fnGetddlText("ddlCashbanklist");
            //$scope.Edititemsdata.ddlPaymentMode = $("#ddlPaymentMode").val();
            //$scope.Edititemsdata.txt_PaymentMode = fnGetddlText("ddlPaymentMode");
            //$scope.Edititemsdata.txt_Cheque = $("#Cheque").val(); 
            //$scope.Edititemsdata.txt_reference = $("#txt_reference").val();  
            //$scope.Edititemsdata.ddlCashBankAccount = $("#ddlCashBankAccount").val();  
            //$scope.Edititemsdata.CashBankAccounttext = fnGetddlText("ddlCashBankAccount");
            //$scope.Edititemsdata.ddlAccount = $("#ddlAccount").val();  
            //$scope.Edititemsdata.Accounttext = fnGetddlText("ddlAccount");
            //$scope.Edititemsdata.txt_amount = $scope.txt_amount;
            //$scope.Edititemsdata.txt_Narration = $scope.txt_Narration;

            $scope.Edititemsdata.txtDate = $("#txt_Date").val();
            $scope.Edititemsdata.Mode = $scope.Mode;
            $scope.Edititemsdata.ddlCashbanklist = $scope.ddlCashbanklist;
            $scope.Edititemsdata.txt_CashBank = fnGetddlText("ddlCashbanklist");
            $scope.Edititemsdata.ddlPaymentMode = $scope.ddlPaymentMode;
            $scope.Edititemsdata.txt_PaymentMode = fnGetddlText("ddlPaymentMode");
            $scope.Edititemsdata.txt_Cheque = $scope.txt_Cheque;
            $scope.Edititemsdata.txt_reference = $scope.txt_reference;
            $scope.Edititemsdata.ddlCashBankAccount = $scope.ddlCashBankAccount;
            $scope.Edititemsdata.CashBankAccounttext = fnGetddlText("ddlCashBankAccount");
            $scope.Edititemsdata.ddlAccount = $scope.ddlAccount;
            $scope.Edititemsdata.Accounttext = fnGetddlText("ddlAccount");
            $scope.Edititemsdata.txt_amount = $scope.txt_amount;
            $scope.Edititemsdata.Book_Type = $scope.Book_Type;
            $scope.Edititemsdata.txt_Narration = $("#txt_Narration_value").val();

            $scope.btntextAdd = "Add";

        }
        else {

            if ($("#btn_submit").val() == "Update") {
                alert("During to Updatetion Add items Not Allowed")
            }
            else {
                // alert("Not");
        

            $scope.singlemodel = {};
            $scope.singlemodel.txtDate = $("#txt_Date").val();
            $scope.singlemodel.Mode = $scope.Mode;
            $scope.singlemodel.ddlCashbanklist = $scope.ddlCashbanklist;
            $scope.singlemodel.txt_CashBank = fnGetddlText("ddlCashbanklist");
            $scope.singlemodel.ddlPaymentMode = $scope.ddlPaymentMode;
            $scope.singlemodel.txt_PaymentMode = fnGetddlText("ddlPaymentMode");
            $scope.singlemodel.txt_Cheque = $scope.txt_Cheque;
            $scope.singlemodel.txt_reference = $scope.txt_reference;
            $scope.singlemodel.ddlCashBankAccount = $scope.ddlCashBankAccount;
            $scope.singlemodel.CashBankAccounttext = fnGetddlText("ddlCashBankAccount");
            $scope.singlemodel.ddlAccount = $scope.ddlAccount;
            $scope.singlemodel.Accounttext = fnGetddlText("ddlAccount");
                $scope.singlemodel.txt_amount = $scope.txt_amount;
                $scope.singlemodel.Book_Type = $scope.Book_Type;
                $scope.singlemodel.txt_Narration = $("#txt_Narration_value").val();// $scope.txt_Narration;

                $scope.Multimodel.push($scope.singlemodel);
            }
        }
    }
    $scope.EditItems = function (data) {
        $scope.Mode = data.Mode;
        $("#txt_Date").val(data.txtDate);
        $scope.ddlCashbanklist = data.ddlCashbanklist;
        $scope.ddlPaymentMode = data.ddlPaymentMode;
        $scope.txt_Cheque = data.txt_Cheque;
        $scope.txt_reference = data.txt_reference;
    
        $scope.ddlCashBankAccount = data.ddlCashBankAccount;
        $scope.ddlAccount = data.ddlAccount;
        //$("#ddlCashBankAccount").val("number:" + data.ddlCashBankAccount);
        //$("#ddlAccount").val("number:" + data.ddlAccount);
        
       // $scope.ddlAccount = data.ddlAccount;
        $scope.txt_amount = data.txt_amount;
        $("#txt_Narration_value").val(data.txt_Narration);
      //  $scope.txt_Narration = data.txt_Narration;
        $scope.btntextAdd = "Update";
        $scope.Edititemsdata = data;
       // console.log(Edititemsdata);
    }

    $scope.FndeleteItems = function (data) {

        var r = confirm("Are you sure delete this item?");
        if (r == true) {
            var i = $scope.Multimodel.indexOf(data);
            if (i >= 0) $scope.Multimodel.splice(i, 1);
        } else {

        }
    }
    $scope.EDITData = function (details) {
        //  alert(details.ID)
        //  $scope.EditDetails = details;
        $scope.UpdateDetails = details;

    }
    

    $scope.FnUpdateSubmit = function (UpdateDetails) {

        $("#loader-1").show();
        $scope.btntext = "Please Wait..";
        $http({
            method: "post",
            url: "/ReceiptPaymentEntry/FnUpdateReceiptEntry",
            data: { 'JsonData': JSON.stringify($scope.Multimodel) }
        }).then(function (response) {
            alert("Update Data Successfully");
            window.location.href = "/ReceiptPaymentEntry/SaveReceiptPaymentEntry";
        }, function (data) {
            alert("Error Occur During This Request Of Post Data");
        })


    }

    $scope.FnFinalSubmit = function () {
        $("#loader-1").show();
        $scope.btntext = "Please Wait..";
        $http({
            method: "post",
            url: "/ReceiptPaymentEntry/FnSaveReceiptEntry",
            data: { 'JsonData': JSON.stringify($scope.Multimodel) }
        }).then(function (response) {
            alert("Insert Data Successfully");
            window.location.href = "/ReceiptPaymentEntry/SaveReceiptPaymentEntry";
        }, function (data) {
            alert("Error Occur During This Request Of Post Data");
        })
    }
    $scope.FnReset = function () {
        var r = confirm("Are you sure Reset This Page?");
        if (r == true) {
            window.location.href = "/ReceiptPaymentEntry/SaveReceiptPaymentEntry";
        } else {

        }
    }
});

