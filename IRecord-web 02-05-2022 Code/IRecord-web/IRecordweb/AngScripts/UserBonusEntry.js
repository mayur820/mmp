var app = angular.module("myApp", []);
app.controller("myctrn", function ($scope, $http) {

    $scope.PageLoad = function () {
        
    //   $scope.getScript();
	   // $scope.getAlldata();
       // fnGetDataUsingGetRequestWithModel("/UserBonusEntry/getAllScript", "Info_Script", $scope, $http);
	   $scope.Info_data = [];
	   $scope.details = [];

        if ($("#hf_CActionView").val() != "0") {
            //in case of View only
            $http({
                method: "get",
                url: "/UserBonusEntry/getAllScript"
            }).then(function (response) {
                // alert(JSON.parse(response.data));
                $scope.Info_Script = JSON.parse(response.data);
                $scope.details.ddl_Script = $("#hf_CActionView").val();
                $scope.OnchngeScript($("#hf_CActionView").val());

                setTimeout(function () {
                    $scope.getAlldata();
                }, 2000);
            }, function (data) {
                //deferred.reject({ message: "Really bad" });
                alert("Error Occur During This Request" + geturl);
            });
        }
        else {
            $scope.getScript();
        }

    }
    $scope.ChngSprt = function () {

        //   alert($scope.ContractNoteId);
        for (var i = 0; i <= $scope.SplitEntryScpirt.length; i++) {

            if (details.ddl_Script == $scope.SplitEntryScpirt[i].Script_ID) {
                // alert($scope.BILLS[i].NAME);
                alert("asdf");
                //$("#txt_RecordDate").val($scope.SplitEntryScpirt[i].RecordDate);
                //$("#txt_CurrentFaceValue").val($scope.SplitEntryScpirt[i].Qty1);
                //$("#txt_NewFaceValue").val($scope.SplitEntryScpirt[i].Qty2);

                break;
            }
        }
    }
 $scope.getScript=function()
 {
  fnGetDataUsingGetRequestWithModel("/UserBonusEntry/getAllScript", "Info_Script", $scope, $http);
    }

 $scope.getAlldata=function()
 {
	// $("#divStatus").show();
     $http({
         method: "get",
         url: "/UserBonusEntry/getAlldata"
     }).then(function (response) {
         console.log(response.data);
         $scope.Info_data = response.data;
         console.log($scope.Info_data);
     }, function (data) {
         //deferred.reject({ message: "Really bad" });
         alert("Error Occur During This Request" + geturl);
     })
     //fnGetDataUsingGetRequestWithModel("/UserBonusEntry/getAlldata?ScpritId=" + $("#ddl_Script").val() + "&&Date=" + $("#TXT_RecordDate").val(), "Info_data", $scope, $http);
    
 }
    $scope.AllCalculate = function () {
        for (var i = 0; i < $scope.Info_data.length; i++) {
            debugger;
            var totalsum = 0;
            for (var U = 0; U < $scope.Info_data[i].ListofDetils.length; U++) {

                totalsum += $scope.Info_data[i].ListofDetils[U].AllocateQty;
            }
            $scope.Info_data[i].NewScriptCAQty = totalsum; 
              
        }
    }
 
 $scope.OnchngeScript=function(datavar)
 {
     //   alert($scope.ContractNoteId);
     for (var i = 0; i <= $scope.Info_Script.length; i++) {

         if (datavar.replace("string:","") == $scope.Info_Script[i].Script_ID) {
             // alert($scope.BILLS[i].NAME);
             //alert("asdf");
             $("#TXT_RecordDate").val($scope.Info_Script[i].RecordDate);
             $("#TXT_ISIN").val($scope.Info_Script[i].ISIN);
             $("#TXT_BonusQtyPer").val($scope.Info_Script[i].Qty2);
             $("#TXT_Quantity").val($scope.Info_Script[i].Qty1);

             break;
         }
     }
	  //    $http({
   //         method: "post",
   //         url: "/UserBonusEntry/getFullDeatils",
   //         data: { 'Id':''+datavar+''}
   //     }).then(function (response) {

   //    $("#TXT_ExistingISIN").val(JSON.parse(response.data)[0].ExistingISIN);
	  // $("#TXT_NewISIN").val(JSON.parse(response.data)[0].NewISIN);
	  // $("#TXT_RecordDate").val(JSON.parse(response.data)[0].RecordDate);
	  //$("#TXT_BonusQtyPer").val(JSON.parse(response.data)[0].PerQty);
		 //   $("#TXT_Share").val(JSON.parse(response.data)[0].BonusQty);
   //    	   $("#lblTotalShare").text(JSON.parse(response.data)[0].BonusQty);
		 //   $("#lblRemaingShare").text(JSON.parse(response.data)[0].BonusQty);
			 
   //         // $scope.GetAllMember();
   //     }, function (data) {
   //         alert("Error Occur During This Request" + posturl);
   //     });
	

 // fnGetDataUsingGetRequestWithModel("/BonusEntry/getAllScript", "Info_Script", $scope, $http);
 }
 
    $scope.CalculationGrid= function (data,status) {
		

		// alert("Data Save");
		// alert($scope.Info_data)
		var Total_IN_Bonus_In_Share_Inv_Ac=0;
		var Total_IN_Bonus_In_Trading_Ac=0;
		 for (var i = 0; i < $scope.Info_data.length; i++) {
        // console.log($scope.Info_data.length);
			var a=0;
			var b=0;
			if($scope.Info_data[i].Bonus_In_Share_Inv_Ac!="")
			{
				a=$scope.Info_data[i].Bonus_In_Share_Inv_Ac;
			}
			if($scope.Info_data[i].Bonus_In_Trading_Ac!="")
			{
				b=$scope.Info_data[i].Bonus_In_Trading_Ac;
			}
		Total_IN_Bonus_In_Share_Inv_Ac+=parseInt(a);
        Total_IN_Bonus_In_Trading_Ac+=parseInt(b);
          }
		  if((parseInt($("#lblTotalShare").text())-parseInt(Total_IN_Bonus_In_Share_Inv_Ac+Total_IN_Bonus_In_Trading_Ac)<0))
		  { 
	
	                if(status=="a")
					{
						   alert("Not Allow vlaue"+data.Bonus_In_Share_Inv_Ac);
						data.Bonus_In_Share_Inv_Ac=0;
					}
					 if(status=="b")
					{
						     alert("Not Allow vlaue"+data.Bonus_In_Trading_Ac);
						data.Bonus_In_Trading_Ac=0;
					}
			 
		  }
		  else
		  {
			  		  $("#lblAllotShare").text(Total_IN_Bonus_In_Share_Inv_Ac+Total_IN_Bonus_In_Trading_Ac);
		  $("#lblTotalShare_Inv").text(Total_IN_Bonus_In_Share_Inv_Ac);
		  $("#lblTotalShare_Trading").text(Total_IN_Bonus_In_Trading_Ac);
		  $("#lblRemaingShare").text(parseInt($("#lblTotalShare").text())-parseInt($("#lblAllotShare").text()));
		  }

				
				//data.Bonus_In_Share_Inv_Ac=0;
		 	//	 alert(Total_IN_Bonus_In_Trading_Ac);
		 
	 }
 
	
     $scope.getSubmitdata1 = function (data) {
		//if($("#ddl_Script").val()=="")
		//{
		//	alert("Please Select Script");
		//	$("#ddl_Script").focus();
		//	return false;
		//}
		//if($("#TXT_RecordDate").val()=="")
		//{
		//	alert("Please Select RecordDate");
		//	$("#TXT_RecordDate").focus();
		//	return false;
		//}
		//if($("#TXT_ExistingISIN").val()=="")
		//{
		//	alert("Please Enter Existing ISIN");
		//	$("#TXT_ExistingISIN").focus();
		//	return false;
		//}
		//if($("#TXT_BonusQtyPer").val()=="")
		//{
		//	alert("Please Enter Bonus Qty Per");
		//	$("#TXT_BonusQtyPer").focus();
		//	return false;
		//}
		//if($("#TXT_Share").val()=="")
		//{
		//	alert("Please Enter Share");
		//	$("#TXT_Share").focus();
		//	return false;
		//}
		if($("#lblRemaingShare").text()=="0")
		{
		
		   var DataValues = {};
		    DataValues.ScriptId=$("#ddl_Script").val();
		    DataValues.RecordDate=$("#TXT_RecordDate").val();
		    DataValues.ExistingISIN=$("#TXT_ExistingISIN").val();
	        DataValues.NewISIN=$("#TXT_NewISIN").val();
		    DataValues.BonusQtyPer=$("#TXT_BonusQtyPer").val();
		    DataValues.Share=$("#TXT_Share").val();
         
	    var posturl = "/UserBonusEntry/SubmitData"; 
        $http({
            method: "post",
            url: posturl,
            data: { 'JsonScriptHeader':'['+ JSON.stringify(DataValues) +']','JsonData':JSON.stringify($scope.Info_data)}
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
		else
		{
			alert("You Are Not Allot Fully bonus!..")
		}
		
	  
	 }
    $scope.fnSubmit = function (data) {
	
		
	   
	   
   }
   
});


 jQuery('#ddl_Script').on('select2:select', function (e) {
    var data = e.params.data;
   
	
	angular.element('#myctrn').scope().OnchngeScript(data.id);
	
});