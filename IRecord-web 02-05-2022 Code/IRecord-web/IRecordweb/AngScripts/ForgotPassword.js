var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
       $("#btn_submit").hide();


    }

  
    $scope.sendotpfn = function () {
		

        var code = $("#txt_userName").val();
        var wantto=$("#ddlotpwant").val();
        //////////////

        var posturl = "/Login/SendOtpFunction?username=" + code+"&&WantTo="+wantto;
        $http({
            method: "post",
            url: posturl
        }).then(function (response) {
            if (response.data != 0) {

                if(code=="1")
				{
					  alert("Otp successfully Your Email Id :"+response.data);
					  $("#lbldisplayid").text("OTP Send to "+response.data)
					  $("#btn_Otp_Send").hide();
					   $("#btn_submit").show();
					  $("#btn_submit").val("Otp Verifly / Submit");
					  
				}
				else
				{
					alert("Otp successfully Your Mobile Number :"+response.data);
					 $("#lbldisplayid").text("OTP Send to "+response.data)
					  $("#btn_Otp_Send").hide();
					   $("#btn_submit").show();
					   $("#btn_submit").val("Otp Verifly / Submit");
				}
              

            } else {
                alert("Your Have Enter Wrong User Name !");
                return false;
            }

        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }
	 $scope.otp_Verifly = function () {
		

        var code = $("#txt_Otp").val();
      

        var posturl = "/Login/OtpVerifly?Otp=" + code;
        $http({
            method: "post",
            url: posturl
        }).then(function (response) {
            if (response.data == 1) {
                  alert("Your Otp Is successfully Verifly ! ");
                 window.location.href="/Login/ResetPassword";
            } else {
                alert("Your Have Enter Wrong Otp !");
                return false;
            }

        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }
 $("#ddlotpwant").change(function (){
	 if($("#txt_userName").val()=="")
	 {
		 alert("Enter First user Name");
		 $("#ddlotpwant").val("-1");
		 return false;
	 }
	 $("#btn_Otp_Send").show();
	 $("#lbldisplayid").show();

 });
  
    $("#txt_noOfLicense").bind('keyup mouseup', function () {
        var a = $("#totalLicenseAmount").text();
        var b = $("#txt_noOfLicense").val();
        $("#totalPayAmout").text(a * b);

    });

});
