var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
	
       $scope.getinfo();
	     $scope.InfoAllYear();
    }

       $scope.getinfo = function () {
		   
		   
		   	fnGetDataUsingGetRequestWithModel("/Login/GetAllYear", "Info", $scope, $http);
	   }
	    $scope.InfoAllYear = function () {
		   
		   $scope.showLoader = true;
		   	//fnGetDataUsingGetRequestWithModel("/Login/GetYearByUser", "InfoAllYear", $scope, $http);
			
			 $http({
            method: "get",
            url: "/Login/GetYearByUser"
           
        }).then(function (response) {
			  
			  
            $scope.InfoAllYear = JSON.parse(response.data);
			$scope.showLoader = false;
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
			
	   }
	   $scope.ngopenmaster = function () {
		   
		   $("#master").show();
		   
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
    $scope.fnadd = function (data) {
       var idata={};
	   idata.FinincialYearId=$("#selectyear").val();
	      var posturl = "/Login/InsertFinancialYear";
        $http({
            method: "post",
            url: posturl,
            data: { 'JsonData':'['+ JSON.stringify(idata) +']'}
        }).then(function (response) {
			  
			  
              window.location.href="/Login/FinancialYear";
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
   }
   
});

