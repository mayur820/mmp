var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        // alert("hii");
        $("#ddlInvestmentType").focus();
        $scope.GetAllDemate();
        $scope.GetAllConsultant();
        $scope.GetAllBroker();
        $scope.ddlSegment = "0";
        //$scope.SaveBREntry = {};
        $scope.btntext = "Show Data";
        $scope.ddlHoldingType = "0";
       // $("#ddlHoldingType").val("0");
        $scope.senddata = [];


    }

    $scope.DownloadFile = function () {
        window.location = "/CN_SampleFile/Opening Stock.xlsx";
    }
  

    $scope.showhidefn = function () {
      

    }
  
    $scope.BrokerOnchange = function () {
        for (var i = 0; i <= $scope.Brokers.length; i++) {

            if ($scope.ddlBroker === $scope.Brokers[i].ID) {

                //alert($scope.Brokers[i].DefaultDemate);
                // alert($scope.Brokers[i].DefaultDemate);
                $("#Demat_Ac_Id").val($scope.Brokers[i].DefaultDemate);
                //  $scope.SaveBREntry.Demat_Ac_Id = ;
                // console.log($scope.Info_Broker[i].NAME + " FOUND forif" + true);
                //alert("FOUND AND DELETE " + $scope.Info_Broker[i].NAME);
                // $("#selectbrokerpopup").select2("val", "");
                //  $scope.Info_Broker.splice(i, 1)
                break;
            }

        }
        //   alert("hii");
    }
    $scope.btnupload = function () {

        let photo = document.getElementById("contractnotefile").files[0];
        if (typeof photo !== 'undefined') {


            let formData = new FormData();

            formData.append("photo", photo);
            fetch('/BrokerBillEntry/Upload', { method: "POST", body: formData }).then(function () {

                alert("File Upload successfully");
                $scope.btn_click_sumit();
            });
            $scope.btnupload = false;
        }
        else {
            // $("#ContractNoteId1").val("912");
            // $scope.ContractNoteId = "912";
            // alert($scope.SaveBREntry.ContractNoteId);
            alert("Please Select A files");
        }

    };

    $scope.bindbills = function () {


        //$http({
        //    method: "get",
        //    url: "/BrokerBillEntry/GetAllBILLS"
        //}).then(function (response) {
        //    $scope.BILLS = JSON.parse(response.data);

        //   // $scope.ContractNoteId = "912";
        //}, function (data) {
        //    //deferred.reject({ message: "Really bad" });
        //    alert("Error Occur During This Request" + geturl);
        //}).then(function() {
        //    //$("#ContractNoteId1").val("912");

        //})

        // fnGetDataUsingGetRequestWithModel("/BrokerBillEntry/GetAllBILLS?Invtype=" + $("#ddlInvestmentType").val() + "&&Date=" + $("#txt_Date").val() + "&&BrokerID=" + $("#ddlBroker").val(), "BILLS", $scope, $http);
        $scope.BrokerOnchange();
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

    $scope.btn_click_sumit = function () {

        if ($("#ddlSegment").val() == "") {
            alert("Please Select Segment!")
            $("#ddlSegment").focus();
            return false;
        }
        if ($("#ddlBroker").val() == "") {
            alert("Please Select Broker!")
            $("#ddlBroker").focus();
            return false;
        }
        if ($("#Demat_Ac_Id").val() == "") {
            alert("Please Select Demat Ac!")
            $("#Demat_Ac_Id").focus();
            return false;
        }
        if ($("#ddlHoldingType").val() == "") {
            alert("Please Select Holding Type!")
            $("#ddlHoldingType").focus();
            return false;
        }
        if ($("#ddlConsultant").val() == "") {
            alert("Please Select Consultant!")
            $("#ddlConsultant").focus();
            return false;
        }


        $scope.btntext = "Please Wait..";
        $("#loader-1").show();
        var data_Index = {};
        data_Index.ddlSegment = $scope.ddlSegment;
        data_Index.ddlSegmenttext = fnGetddlText("ddlSegment");
        data_Index.ddlBroker = $scope.ddlBroker;
        data_Index.ddlBrokertext = fnGetddlText("ddlBroker");
        data_Index.Demat_Ac_Id = $scope.Demat_Ac_Id;
        data_Index.Demat_Ac_Idtext = fnGetddlText("Demat_Ac_Id");
        data_Index.ddlHoldingType = $scope.ddlHoldingType;
        data_Index.ddlHoldingTypetext = fnGetddlText("ddlHoldingType");
        data_Index.ddlConsultant = $scope.ddlConsultant;
        data_Index.ddlConsultanttext = fnGetddlText("ddlConsultant");

       
        var Pdata = new FormData();
        Pdata.append("JsonData", '[' + JSON.stringify(data_Index) + ']');
        Pdata.append("ExcelFile", $("#Excelb64").val());
         
        debugger;

        $.ajax({
            type: "POST",
            url: "/OpeningStock/FnSaveManualEntry",
            type: 'POST',
            data: Pdata,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                //show content
              //  alert("Insert Data Successfully");
                window.location.href = "/OpeningStock/ViewOpeningStock";

            }
        });


       

    }

});

