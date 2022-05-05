var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        
       $scope.getScript();
	   $scope.getAlldata();
	   $scope.details = [];
	 
    }
 $scope.getScript=function()
 {
  fnGetDataUsingGetRequestWithModel("/BonusEntry/getAllScript", "Info_Script", $scope, $http);
 }
 $scope.getAlldata=function()
 {
  fnGetDataUsingGetRequestWithModel("/BonusEntry/getAlldata", "Info_data", $scope, $http);
 }
 
 $scope.OnchngeScript=function(datavar)
 {
	 
	      $http({
            method: "post",
            url: "/BonusEntry/getISIN",
            data: { 'Id':''+datavar+''}
        }).then(function (response) {
			
$("#TXT_ExistingISIN").val(response.data);
	  $("#TXT_NewISIN").val(response.data);
       
			 
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
	

 // fnGetDataUsingGetRequestWithModel("/BonusEntry/getAllScript", "Info_Script", $scope, $http);
 }
 
  
 
	
  
    $scope.fnSubmit = function (data) {
	
		if($("#ddl_Script").val()=="")
		{
			alert("Please Select Script");
			$("#ddl_Script").focus();
			return false;
		}
		if($("#TXT_RecordDate").val()=="")
		{
			alert("Please Select RecordDate");
			$("#TXT_RecordDate").focus();
			return false;
		}
		if($("#TXT_ExistingISIN").val()=="")
		{
			alert("Please Enter Existing ISIN");
			$("#TXT_ExistingISIN").focus();
			return false;
		}
		if($("#TXT_BonusQtyPer").val()=="")
		{
			alert("Please Enter Bonus Qty Per");
			$("#TXT_BonusQtyPer").focus();
			return false;
		}
		if($("#TXT_Share").val()=="")
		{
			alert("Please Enter Share");
			$("#TXT_Share").focus();
			return false;
		}
		
		
		   var DataValues = {};
		    DataValues.ScriptId=$("#ddl_Script").val();
		    DataValues.RecordDate=$("#TXT_RecordDate").val();
		    DataValues.ExistingISIN=$("#TXT_ExistingISIN").val();
	        DataValues.NewISIN=$("#TXT_NewISIN").val();
		    DataValues.BonusQtyPer=$("#TXT_BonusQtyPer").val();
		    DataValues.Share=$("#TXT_Share").val();
         
	    var posturl = "/BonusEntry/SubmitData"; 
        $http({
            method: "post",
            url: posturl,
            data: { 'JsonData':'['+ JSON.stringify(DataValues) +']'}
        }).then(function (response) {
			//document.getElementById("signupform").submit();
			alert("Data Insert Successfully..")
            // window.location.href="/BonusEntry/Index";
        $scope.getAlldata();
			//  window.location.href="/Dashboard/Index";
            // $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
	  
	   
	   
   }
   
});


 jQuery('#ddl_Script').on('select2:select', function (e) {
    var data = e.params.data;
   
	
	angular.element('#myctrn').scope().OnchngeScript(data.id);
	
});