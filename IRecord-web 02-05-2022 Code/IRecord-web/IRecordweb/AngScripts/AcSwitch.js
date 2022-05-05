var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        $scope.getinfo();
        
    }
 
    $scope.getinfo = function () {
     $http({
            method: "get",
           url: "/Login/GetUserInfo"
        }).then(function (response) {
       
        if(response.data=='SingleUser')
        {
        window.location.href="/Login/FinancialYear";
       // alert("hii");
}
else
{
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
            data: { 'JsonData':'['+ JSON.stringify(data) +']'}
        }).then(function (response) {
               window.location.href="/Login/FinancialYear";
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
   }
   
});

