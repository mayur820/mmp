var app = angular.module("myApp", ["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {

        $scope.GetAllBROKERFORMATE();
        $scope.GetAllSAccount();
        $scope.GetAllBANK();
        $scope.btntext = "Submit";
        $scope.ddl_File_Type = "EXCEL";
        $("#txt_Sr_No").focus();
        $scope.page = 1;

    }
    $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        console.log($scope.page);
    };

    $scope.Setvalue = function () {
        $scope.txt_Description = fnGetddlText("ddlBroker") + " " + fnGetddlText("ddlInvestmentType") + " " + fnGetddlText("ddlFileType"); //+ fnGetddlText("ddl_File_Extension_Type");
    };

    $scope.GetAllBROKERFORMATE = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillFormatMaster/ViewBrokerFormatedata", "Brokerformats", $scope, $http);

    }
    $scope.GetAllSAccount = function () {
        fnGetDataUsingGetRequestWithModel("/BrokerBillFormatMaster/GetAllAccountList", "Accounts", $scope, $http);

    }
    $scope.GetAllBANK = function () {
        fnGetDataUsingGetRequestWithModel("/BankStConfig/GetBank", "bank", $scope, $http);

    }
    $scope.fnchangeformattype = function () {

        if ($scope.ddlFileType == "PDF") {

            $scope.ddl_File_Extension_Type = ".pdf";
            $scope.Setvalue();
        }
        if ($scope.ddlFileType == "EXCEL") {
            $scope.ddl_File_Extension_Type = ".xls";
            $scope.Setvalue();
        }
        if ($scope.ddlFileType == "html") {
            $scope.ddl_File_Extension_Type = ".html";
            $scope.Setvalue();
        }
    }
    $scope.FnEditItems = function (details) {

        $scope.Multimodel = [];
        $scope.txt_Sr_No = details.Sr_No;
        $scope.txt_Description = details.Description;
        $scope.ddlFileType = details.FileType;
        $scope.ddl_File_Extension_Type = details.FileExtension;

        $scope.ddlInvestmentType = "" + details.InvestmentType;
        $scope.ddlBroker = details.BrokerID;
        $scope.chk_DisplayInList = details.Display_in_List;
        $scope.chk_IsTradeFile = details.isTradefile;
        $scope.chk_CombinedFile = details.CombinedFormat;
        $scope.btntext = "Update";
        $scope.Multimodel.push(details);


    }
    $scope.FndeleteItems = function (dataS) {
        console.log(dataS);
        var r = confirm("Are you sure delete this item?");
        if (r == true) {
            $http({
                method: "get",
                url: "/BrokerBillFormatMaster/BrokerFormatDeleteConfig?ID=" + dataS.Sr_No,

            }).then(function (response) {
                $scope.GetAllBROKERFORMATE();
            }, function (data) {
                alert("Error Occur During This Request Of Post Data");
            })



        } else {

        }



    }
    $scope.FnFinalSubmit = function () {

        $scope.Multimodel = [];
        $scope.singlemodel = {};
        $scope.singlemodel.txt_Sr_No = $scope.txt_Sr_No;
        $scope.singlemodel.txt_Description = $scope.txt_Description;
        $scope.singlemodel.ddlFileType = $scope.ddlFileType;
        $scope.singlemodel.ddl_File_Extension_Type = $scope.ddl_File_Extension_Type;
        $scope.singlemodel.ddlInvestmentType = $scope.ddlInvestmentType;
        $scope.singlemodel.txt_Segment = fnGetddlText("ddlInvestmentType");
        $scope.singlemodel.ddlBroker = $scope.ddlBroker;
        $scope.singlemodel.uploadfileurl = $("#FileUpload").val();;
        $scope.singlemodel.chk_DisplayInList = $scope.chk_DisplayInList;
        $scope.singlemodel.chk_IsTradeFile = $scope.chk_IsTradeFile;
        $scope.singlemodel.chk_CombinedFile = $scope.chk_CombinedFile;


        $scope.Multimodel.push($scope.singlemodel);
        if ($scope.btntext == "Submit") {

            $http({
                method: "get",
                url: "/BrokerBillFormatMaster/Get_Validate_SrNo?SrNo=" + $("#txt_Sr_No").val()

            }).then(function (response) {
                if (response.data == 1) {
                    alert("Sr No Already Exist!");
                    $("#txt_Sr_No").focus();
                    return false;

                }
                else {
                    var Pdata = new FormData();
                    Pdata.append("JsonData", JSON.stringify($scope.Multimodel));
                    Pdata.append("files", $("#filedata").val());
                    $.ajax({
                        type: "POST",
                        url: "/BrokerBillFormatMaster/FnBrokerFormatMasterSaveEntry",
                        type: 'POST',
                        data: Pdata,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            alert("Insert Data Successfully");
                            window.location.href = "/BrokerBillFormatMaster/Index";
                            $('#txt_Sr_No').val(data.Sr_No);
                        }

                    });
                }

            }, function (data) {
                alert("Error Occur During This Request" + posturl);
            });


        }
        else {
            $("#txt_Sr_No").focus();
            $("#loader-1").show();
            $scope.btntext = "Please Wait..";
            //$http({
            //    method: "post",
            //    url: "/BrokerBillFormatMaster/FnBrokerFormatMasterUpdateEntry",
            //    data: { 'JsonData': JSON.stringify($scope.Multimodel) }
            //}).then(function (response) {
            //    alert("Record Update Successfully !!");
            //    window.location.href = "/BrokerBillFormatMaster/Index";
            //}, function (data) {
            //    alert("Error Occur During This Request Of Post Data");
            //})
            var Pdata = new FormData();
            Pdata.append("JsonData", JSON.stringify($scope.Multimodel));
            Pdata.append("files", $("#filedata").val());
            $.ajax({
                type: "POST",
                url: "/BrokerBillFormatMaster/FnBrokerFormatMasterUpdateEntry",
                type: 'POST',
                data: Pdata,
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                      alert("Record Update Successfully !!");
                    window.location.href = "/BrokerBillFormatMaster/Index";
                   // $('#txt_Sr_No').val(data.Sr_No);
                }

            });
        }
    }
    $scope.FnReset = function () {
        var r = confirm("Are you sure Reset This Page?");
        if (r == true) {
            window.location.href = "/BrokerBillFormatMaster/Index";
        } else {

        }
    }
});

