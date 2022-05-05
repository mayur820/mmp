var app = angular.module("myApp", ["ngStorage"]);
app.controller("myctrn", function ($scope, $http, $localStorage, $sessionStorage, $window) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {

        $("#btn_ApplyCoupon").hide();

    }

    $scope.GotoPayment = function () {
		
		window.location.href="/PaymantGetway/GoToPayment?Amount="+$("#totalPayAmout").text()+"&&QtyLicense="+$("#txt_noOfLicense").val()+"&&CouponCode="+$("#txt_ApplyCoupon").val()+"";
		
	}
    $scope.onclickApplyCouponCode = function () {

        var code = $("#txt_ApplyCoupon").val();
        var totalamout = $("#totalPayAmout").text();
        var a = $("#totalLicenseAmount").text();
        var b = $("#txt_noOfLicense").val();
        var AMT = a * b;
        //////////////

        var posturl = "/PaymantGetway/GetValidApplyCoupon?Coupon=" + code;
        $http({
            method: "post",
            url: posturl
        }).then(function (response) {
            if (response.data > 0) {

                var couponcodeamouttotal = (AMT - ((AMT) * response.data))
                $("#totalPayAmout").text(couponcodeamouttotal);

                alert("coupon code applied successfully");

            } else {
                alert("Your Have Enter Wrong coupon code !");
                return false;
            }

        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }

    $("#txt_ApplyCoupon").bind('keyup mouseup', function () {

        $("#btn_ApplyCoupon").show();

    });
    $("#txt_noOfLicense").bind('keyup mouseup', function () {
        var a = $("#totalLicenseAmount").text();
        var b = $("#txt_noOfLicense").val();
        $("#totalPayAmout").text(a * b);

    });

});
