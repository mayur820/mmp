var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        //alert("hii");
        console.log(angular.version);
        $scope.GetAllMember();

        //$scope.getinfo();
    }

    $scope.GetAllMember = function () {

        fnGetDataUsingGetRequestWithModel("/Login/GetAllMember", "InfoMember", $scope, $http);
    }
    $scope.onmemberchange = function () {
	
		$("#ddlyear").empty();
        fnGetDataUsingGetRequestWithModel("/Login/GetAllYearWithMember?MemberID=" + $("#ddlmember").val(), "Info", $scope, $http);
		 $scope.InfoAllMemberYear();
        //alert($("#ddlmember").val());
    }
	 $scope.fnsetFinancialYear = function (data) {
    
	      var posturl = "/Login/SetFinancialYearUser";
        $http({
            method: "post",
            url: posturl,
            data: { 'JsonData':'['+ JSON.stringify(data) +']'}
        }).then(function (response) {
			  
			  
              window.location.href="/Dashboard/Index";
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
	   
   }
    $scope.InfoAllMemberYear = function () {

        //$scope.showLoader = true;
        //fnGetDataUsingGetRequestWithModel("/Login/GetYearByUser", "InfoAllYear", $scope, $http);
       
        $http({
            method: "get",
            url: "/Login/GetYearByUser?MemberID="+ $("#ddlmember").val()

        }).then(function (response) {

            $scope.InfoAllYear = JSON.parse(response.data);
           // $scope.showLoader = false;
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }
    $scope.fnadd = function () {
        var idata = {};
        idata.FinincialYearId = $("#ddlyear").val();
        idata.MemberId = $("#ddlmember").val();
        var posturl = "/Login/InsertFinancialYear";
        $http({
            method: "post",
            url: posturl,
            data: {
                'JsonData': '[' + JSON.stringify(idata) + ']'
            }
        }).then(function (response) {

            // window.location.href="/Login/FinancialYear";
            // $scope.GetAllMember();
            $scope.onmemberchange();
			// $scope.InfoAllYear();
            alert("Financial Year Add successfully");
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }
    $scope.getinfo1 = function () {
        $http({
            method: "get",
            url: "/Login/GetUserInfo"
        }).then(function (response) {

            if (response.data == 'SingleUser') {
                window.location.href = "/Login/FinancialYear";
                // alert("hii");
            } else {
                fnGetDataUsingGetRequestWithModel("/Login/GetUserInfo", "Info", $scope, $http);
            }

            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
        //fnGetDataUsingGetRequestWithModel("/Login/GetUserInfo", "Info", $scope, $http);
    }
    $scope.fnchangelogin = function (data) {
        var posturl = "/Login/GetUserSingleInfo";
        $http({
            method: "post",
            url: posturl,
            data: {
                'JsonData': '[' + JSON.stringify(data) + ']'
            }
        }).then(function (response) {
            window.location.href = "/Login/FinancialYear";
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }

});
