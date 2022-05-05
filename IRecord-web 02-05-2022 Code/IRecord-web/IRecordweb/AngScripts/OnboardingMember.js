var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        //  alert("hii");
        $scope.FinYear();
        $scope.getBroker();
        $scope.getDemate();
        $scope.getBank();
        $scope.getConsultant();
        $scope.demat_with_broker = [];
        $scope.Single_demats = [];
        $scope.Single_Bank = [];
        $scope.Single_Advisor = [];
        $scope.Add_Advisor = [];

        $scope.details = [];
    }
    $scope.FinYear = function () {

        // $("#ddlyear").empty();
        fnGetDataUsingGetRequestWithModel("/Login/GetAllYearWithMember?MemberID=string:0", "Info", $scope, $http);
        //  $scope.InfoAllMemberYear();
        //alert($("#ddlmember").val());
    }
    $scope.getBroker = function () {
        fnGetDataUsingGetRequestWithModel("/MemberMaster/GetBroker", "Info_Broker", $scope, $http);
    }
    $scope.getDemate = function () {
        fnGetDataUsingGetRequestWithModel("/MemberMaster/GetDemate", "Info_Demate", $scope, $http);
    }
    $scope.getBank = function () {
        fnGetDataUsingGetRequestWithModel("/MemberMaster/GetBank", "Info_Bank", $scope, $http);
    }
    $scope.getConsultant = function () {
        fnGetDataUsingGetRequestWithModel("/MemberMaster/GetConsultant", "Info_Consultant", $scope, $http);
    }
    $scope.FN_demat_with_broker = function () {

        if ($("#selectbrokerpopup").val() != "" && $("#selectDematpopup").val() != "") {

            //$.each($scope.Info_Broker, function (index, value) {
            //    //console.log("pizza: " + value['count']);
            //    //mvcTotal += value['count'];


            //});




            var VIEWMODEL = {};
            VIEWMODEL.DemateId = $("#selectDematpopup").val();
            VIEWMODEL.DemateName = fnGetddlText("selectDematpopup");
            VIEWMODEL.brokerId = $("#selectbrokerpopup").val();
            VIEWMODEL.BrokerName = fnGetddlText("selectbrokerpopup");
            VIEWMODEL.ClientCode = $("#txt_ClientCode").val();

            $scope.demat_with_broker.push(VIEWMODEL);

            var VIEWMODEL1 = {};
            VIEWMODEL1.DemateId = $("#selectDematpopup").val();
            VIEWMODEL1.DemateName = fnGetddlText("selectDematpopup");

            $scope.Single_demats.push(VIEWMODEL1);


            //$("#selectbrokerpopup").select2("destroy");
            //$("#selectDematpopup").select2("destroy");
            //$("#selectbrokerpopup").val("");
            //$("#selectDematpopup").val("");
            $("#txt_ClientCode").val("");

            //NOT IN CASE
            for (var i = 0; i <= $scope.Info_Broker.length; i++) {

                if ($("#selectbrokerpopup").val().split(':')[1] === $scope.Info_Broker[i].ID) {

                    // console.log($scope.Info_Broker[i].NAME + " FOUND forif" + true);
                    //alert("FOUND AND DELETE " + $scope.Info_Broker[i].NAME);
                    $("#selectbrokerpopup").select2("val", "");
                    $scope.Info_Broker.splice(i, 1)
                    break;
                }

            }
            //NOT IN CASE
            for (var i = 0; i <= $scope.Info_Demate.length; i++) {

                if ($("#selectDematpopup").val().split(':')[1] === $scope.Info_Demate[i].ID) {

                    // console.log($scope.Info_Broker[i].NAME + " FOUND forif" + true);
                    //alert("FOUND AND DELETE " + $scope.Info_Broker[i].NAME);
                    $("#selectDematpopup").select2("val", "");
                    $("#ddl_Single_demat").select2("val", "");

                    $scope.Info_Demate.splice(i, 1)
                    break;
                }

            }
            //$("#selectbrokerpopup").select2();
            //$("#selectDematpopup").select2();
        }
        else {
            alert("Plase Select Broker and Demate..");
            $("#selectbrokerpopup").focus();

        }

    }
    $scope.FN_demat = function () {

        if ($("#ddl_Single_demat").val() != "") {
            var VIEWMODEL = {};
            VIEWMODEL.DemateId = $("#ddl_Single_demat").val();
            VIEWMODEL.DemateName = fnGetddlText("ddl_Single_demat");

            $scope.Single_demats.push(VIEWMODEL);
            for (var i = 0; i <= $scope.Info_Demate.length; i++) {

                if ($("#ddl_Single_demat").val().split(':')[1] === $scope.Info_Demate[i].ID) {

                    // console.log($scope.Info_Broker[i].NAME + " FOUND forif" + true);
                    //alert("FOUND AND DELETE " + $scope.Info_Broker[i].NAME);
                    $("#selectDematpopup").select2("val", "");
                    $("#ddl_Single_demat").select2("val", "");

                    $scope.Info_Demate.splice(i, 1)
                    break;
                }

            }

        }
        else {
            alert("Plase Select Broker..");
            $("#ddl_Single_demat").focus();
            return false;

        }

    }
    $scope.FN_Bank = function () {


        if ($("#ddlbankPopup").val() != "") {
            var VIEWMODEL = {};
            VIEWMODEL.BankId = $("#ddlbankPopup").val();
            VIEWMODEL.BankName = fnGetddlText("ddlbankPopup");

            $scope.Single_Bank.push(VIEWMODEL);

            for (var i = 0; i <= $scope.Info_Bank.length; i++) {

                if ($("#ddlbankPopup").val().split(':')[1] === $scope.Info_Bank[i].ID) {

                    // console.log($scope.Info_Broker[i].NAME + " FOUND forif" + true);
                    //alert("FOUND AND DELETE " + $scope.Info_Broker[i].NAME);
                    $("#ddlbankPopup").select2("val", "");


                    $scope.Info_Bank.splice(i, 1)
                    break;
                }

            }



        }
        else {
            alert("Plase Select Bank..");
            $("#ddlbankPopup").focus();

        }

    }

    $scope.AddAdvisor = function () {

        var AddAdvisorsingle = {};
        AddAdvisorsingle.AdvisorName = $scope.txtAdvisorName;
        AddAdvisorsingle.Mobile = $scope.txtAdMobile;
        AddAdvisorsingle.EmailID = $scope.txtAdEmailID;

        $scope.Add_Advisor.push(AddAdvisorsingle);

    }

    $scope.Advisor = function () {

        if ($("#ddl_Advisor").val() != "") {
            var VIEWMODEL = {};
            VIEWMODEL.AdvisorId = $("#ddl_Advisor").val();
            VIEWMODEL.AdvisorName = fnGetddlText("ddl_Advisor");

            $scope.Single_Advisor.push(VIEWMODEL);

            for (var i = 0; i <= $scope.Info_Consultant.length; i++) {

                if ($("#ddl_Advisor").val().split(':')[1] === $scope.Info_Consultant[i].ID) {

                    // console.log($scope.Info_Broker[i].NAME + " FOUND forif" + true);
                    //alert("FOUND AND DELETE " + $scope.Info_Broker[i].NAME);
                    $("#ddl_Advisor").select2("val", "");


                    $scope.Info_Consultant.splice(i, 1)
                    break;
                }

            }


        }
        else {
            alert("Plase Select Advisor..");
            $("#ddl_Advisor").focus();

        }

    }


    $scope.fnSubmit = function (data) {


        var DefaultValues = {};
        DefaultValues.BrokerWithDemate = $("#ddl_DefaultBroker").val();
        DefaultValues.Demate = $("#ddl_DefaultDemat").val();
        DefaultValues.BankAc = $("#ddl_DefaultBank").val();
        DefaultValues.Advisor = $("#ddl_DefaultAdvisor").val();
        //DefaultValues.Advisor = $("#ddl_DefaultAdvisor").val();
        DefaultValues.FinancialYear = $("#ddlyear").val();
        var ListOfBrokerWithDemate = JSON.stringify($scope.demat_with_broker);
        var ListOfDemate = JSON.stringify($scope.Single_demats);
        var ListOfBankAc = JSON.stringify($scope.Single_Bank);
        var ListOfAdvisor = JSON.stringify($scope.Add_Advisor);
        var JsonDefaultValues = JSON.stringify(DefaultValues);


        var posturl = "/MemberMaster/SetMultiConfiguration";
        $http({
            method: "post",
            url: posturl,
            data: { 'JsonDefaultValues': '[' + JsonDefaultValues + ']', 'ListOfBrokerWithDemate': ListOfBrokerWithDemate, 'ListOfDemate': ListOfDemate, 'ListOfBankAc': ListOfBankAc, 'ListOfAdvisor': ListOfAdvisor }
        }).then(function (response) {
            document.getElementById("signupform").submit();


            //  window.location.href="/Dashboard/Index";
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });


    }

});

