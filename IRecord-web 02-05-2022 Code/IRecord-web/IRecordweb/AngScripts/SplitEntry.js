var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {

      


        if ($("#hfchecker").val() != "0") {
            //in case of View only
            $http({
                method: "get",
                url: "/SplitEntry/GetDataWithView?Trans_No=" + $("#hfchecker").val()
            }).then(function (response) {
                // alert(JSON.parse(response.data));
                $scope.TblData = JSON.parse(response.data);
                $scope.btntext = "Process";
            }, function (data) {
                //deferred.reject({ message: "Really bad" });
                alert("Error Occur During This Request" + geturl);
            });
        }


        $http({
            method: "get",
            url: "/SplitEntry/GetScpirt"
        }).then(function (response) {
            $scope.SplitEntryScpirt = JSON.parse(response.data);
            //reverse calling function
            if ($("#hf_CActionView").val() != "0") {
                //alert("sadf");
                $("#ddlScript").val('string:'+$("#hf_CActionView").val());
                $scope.ddlScript = $("#hf_CActionView").val();
                $scope.ChngSprt();
                $("#txt_Description").val("My Description");
                $scope.txt_Description = "My Description";
                $("#ddlHoldingType").val("0");
                $scope.ddlHoldingType = "0";
                setTimeout(function () {
                    $scope.FillTable();
                }, 3000);
               
            
            }
            //reverse calling function
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        })
        $http({
            method: "get",
            url: "/SplitEntry/GetTranstionNO"
        }).then(function (response) {
            // alert(JSON.parse(response.data));
            $("#txt_TransactionNo").val(JSON.parse(response.data));
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        })

        document.getElementById("txt_RecordDate").readOnly = true;
        document.getElementById("txt_NewFaceValue").readOnly = true;
        $scope.btntext = "Process";
        $scope.btnposttext = "Post In Respective Demat";



    }


    $scope.FillTable = function () {
        if ($("#ddlScript").val() == "") {
            alert("Please Select Script");
            $("#ddlScript").focus();
            return false;
        }
        if($("#ddlHoldingType").val() == "") {
            alert("Please Select Holding Type");
            $("#ddlHoldingType").focus();
            return false;
        }
        if ($("#txt_Description").val() == "") {
            alert("Please Enter Description");
            $("#txt_Description").focus();
            return false;
        }

        $scope.btntext = "Please Wait..";
        $http({
            method: "get",
            url: "/SplitEntry/GetData?Script_Id=" + $("#ddlScript").val() + "&&Trans_Dt=" + $("#txt_RecordDate").val()
        }).then(function (response) {
            // alert(JSON.parse(response.data));
            $scope.TblData = JSON.parse(response.data);
            $scope.btntext = "Process";
        }, function (data) {
            //deferred.reject({ message: "Really bad" });
            alert("Error Occur During This Request" + geturl);
        });
    }
    $scope.ChngSprt = function () {

        //   alert($scope.ContractNoteId);
        for (var i = 0; i <= $scope.SplitEntryScpirt.length; i++) {

            if ($scope.ddlScript == $scope.SplitEntryScpirt[i].Script_ID) {
                // alert($scope.BILLS[i].NAME);
                $("#txt_RecordDate").val($scope.SplitEntryScpirt[i].RecordDate);
                $("#txt_CurrentFaceValue").val($scope.SplitEntryScpirt[i].Qty1);
                $("#txt_NewFaceValue").val($scope.SplitEntryScpirt[i].Qty2);

                break;
            }
        }
    }
    //Chintan
    $scope.FillTableNew = function () {
        $http({
            method: "get",
            url: "/SplitEntry/GetTableData"
        }).then(function (response) {
            // alert(JSON.parse(response.data));
            $scope.aftersavedatanew = JSON.parse(response.data);
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
                //window.location.href = "/SplitEntry/AddSplitEntry";

            }
        });
        $scope.FillTableNew();
    }

});

