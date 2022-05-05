var app = angular.module("myApp",["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        $("#Operator_name").focus();
       // $("#modal_add").show();
      
        // alert("hdsfh");
        $scope.fillAllOperator();
        $scope.ERRORMSG = "";
        $scope.page = 1;
    }

  
     $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        //$scope.displayItems = $scope.totalItems.slice(startPos, startPos + 3);
        console.log($scope.page);
    };



    $scope.fillAllOperator = function () {
        //alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/OperatorMaster/GetAllOperator", "AllOperator", $scope, $http);
    };

    $scope.SubmitData = function () {
    debugger
        if ($("#Operator_name").val() == "") {
           // $scope.ERRORMSG = "Please Enter Operator name !";
            alert("Please Enter Operator name !");
           // $("#addpopopen").click();
            $("#Operator_name").focus();
            $('#modal_add').modal('show');
             return false;
        }

        if ($("#Mobile").val() == "") {
            //$scope.ERRORMSG = "Please Enter Mobile !";
            alert("Please Enter Mobile !");
            $("#Mobile").focus();
            $('#modal_add').modal('show');
             return false;
        }

        var posturl = "/OperatorMaster/GetValidEmailId?Mailid=" + $scope.Email;
        $http({
            method: "post",
            url: posturl
        }).then(function (response) {
            if (response.data == 0) {
				  // alert("hdsfh");ID, OperatorName, Mobile, Email, ManagerId, Sucriberid, Ismanager, Createddate, Createdby
        var BankStatement_Index = {};
        BankStatement_Index.OperatorName = $scope.Operator_name;
        BankStatement_Index.Mobile = $scope.Mobile;
        BankStatement_Index.Email = $scope.Email;
        // BankStatement_Index.ManagerId = $scope.ManagerId;
        BankStatement_Index.Sucriberid = $scope.Sucriberid;
        // BankStatement_Index.Ismanager = $scope.Ismanager;
        BankStatement_Index.Createddate = $scope.Createddate;
        BankStatement_Index.Createdby = $scope.Createdby;
        BankStatement_Index.Password = $scope.Password;

        // BankStatement_Index.Createddate = $scope.BankId;

        var posturl = "/OperatorMaster/IndexSave";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(BankStatement_Index)
        }).then(function (response) {
            alert("Record Save Sucessfully !!");
            window.location = "/OperatorMaster/Index";
            $scope.fillAllOperator();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
				
			}
            else {
                alert("Email Id Already Exits in System!");
                return false;
            }

        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

      

    }

    $scope.EDITData = function (details) {
        //  alert(details.ID)
        $scope.EditDetails = details;
        $scope.UpdateDetails = details;
        if (details.Ismanager == "1") {
            $('#EditCheckbox').prop('checked', true);

        } else {
            $('#EditCheckbox').prop('checked', false);
        }

    }
    $scope.DeleteData = function (details) {
        //  alert(details.ID)
        var update_Index = {};
        update_Index.ID = details.ID;
        update_Index.OperatorName = details.OperatorName;
        update_Index.Mobile = details.Mobile;
        update_Index.Email = details.Email;
        //update_Index.ManagerId = details.ManagerId
        //update_Index.Ismanager = details.Ismanager;


        var posturl = "/OperatorMaster/Delete";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(update_Index)
        }).then(function (response) {
            alert("Record Deleted Successfully !!");
            $scope.fillAllOperator();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }
    $scope.UpdateData = function (UpdateDetails) {

        var update_Index = {};
        update_Index.ID = UpdateDetails.ID;
        update_Index.OperatorName = UpdateDetails.OperatorName;
        update_Index.Mobile = UpdateDetails.Mobile;
        update_Index.Email = UpdateDetails.Email;
        update_Index.Password = UpdateDetails.Password;
        //   update_Index.ManagerId = UpdateDetails.ManagerId;
        // update_Index.Ismanager = UpdateDetails.Ismanager;

        var posturl = "/OperatorMaster/edit";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(update_Index)
        }).then(function (response) {
            alert("Record Update Successfully !!");
            $scope.fillAllOperator();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }

});
