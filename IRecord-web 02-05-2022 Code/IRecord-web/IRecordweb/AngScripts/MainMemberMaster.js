var app = angular.module("myApp", ["ui.bootstrap"]);
app.controller("myctrn", function ($scope, $http) {
    // $scope.BILLS = null;
    $scope.PageLoad = function () {
        // alert("hdsfh");
        $scope.GetAllMember();
        $scope.fillallGetOperator();
        $scope.fillallGetFAMILY();
        // $scope.GetAllOperatorVIEWBYID();
        //viewbankstatmentdata();

        $scope.page = 1;
    }
    $scope.pageChanged = function () {
        var startPos = ($scope.page - 1) * 3;
        //$scope.displayItems = $scope.totalItems.slice(startPos, startPos + 3);
        console.log($scope.page);
    };

    $scope.GetAllMember = function () {
        //alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/MainMemberMaster/GetAllMember", "AllMember", $scope, $http);
    };

    $scope.DownloadFile = function () {
        window.location = "/CN_SampleFile/MainMemberImport.xlsx";
    }

    $scope.fillallGetOperator = function () {
        //   alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/MainMemberMaster/GetOperator", "FILLOperator", $scope, $http);
    };
    $scope.fillallGetFAMILY = function () {
        //   alert("hdsfhFF");
        fnGetDataUsingGetRequestWithModel("/MainMemberMaster/GetFAMILY", "FILLFAMILY", $scope, $http);
    };

    $scope.SubmitData = function () {
        // alert("hdsfh");ID, OperatorName, Mobile, Email, ManagerId, Sucriberid, Ismanager, Createddate, Createdby
        var posturl = "/MainMemberMaster/GetValidEmailId?Mailid=" + $scope.Email;
        $http({
            method: "post",
            url: posturl
        }).then(function (response) {
            if (response.data == 0) {
                debugger
                let photo = document.getElementById("memberfile").files[0];
                if (typeof photo !== 'undefined') {
                    let formData = new FormData();
                    formData.append("photo", photo);
                    fetch('/MainMemberMaster/Upload', {
                        method: "POST",
                        body: formData
                    }).then(function () {
                        if ($scope.MemberName == undefined || $scope.MemberName == "") {
                            alert("Please Enter MemberName");
                            return false;
                        }
                        if ($scope.FamilyID == undefined || $scope.FamilyID == "") {
                            alert("Please select the Family");
                            return false;
                        }
                        if ($scope.Gender == undefined || $scope.Gender == "") {
                            alert("Please Select the Gender.");
                            return false;
                        }


                        var BankStatement_Index = {};
                        //    BankStatement_Index.MemberID = $scope.MemberID;
                        BankStatement_Index.FamilyID = $scope.FamilyID;
                        BankStatement_Index.MemberName = $scope.MemberName;
                        BankStatement_Index.Code = $scope.Code;
                        BankStatement_Index.Address_1 = $scope.Address_1;
                        BankStatement_Index.Address_2 = $scope.Address_2;
                        BankStatement_Index.Address_3 = $scope.Address_3;
                        BankStatement_Index.Gender = $scope.Gender;
                        BankStatement_Index.ServTax_No = $scope.ServTax_No;
                        BankStatement_Index.AadharCardNo = $scope.AadharCardNo;
                        //  BankStatement_Index.ReportLogoPath = $scope.ReportLogoPath;
                        BankStatement_Index.UserId = $scope.UserId;
                        BankStatement_Index.PAN = $scope.PAN;
                        BankStatement_Index.Active = $scope.Active;
                        BankStatement_Index.CreatedBy = $scope.CreatedBy;
                        BankStatement_Index.CreatedDate = $scope.CreatedDate;
                        BankStatement_Index.ModifiedBy = $scope.ModifiedBy;
                        BankStatement_Index.ModifiedDate = $scope.ModifiedDate;
                        BankStatement_Index.SubscriberID = $scope.SubscriberID;
                        BankStatement_Index.OperatorID = $scope.OperatorID;
                        BankStatement_Index.Email = $scope.Email;
                        BankStatement_Index.Password = $scope.Password;
                        BankStatement_Index.Client_Code = $scope.Client_Code;

                        // BankStatement_Index.Createddate = $scope.BankId;

                        var posturl = "/MainMemberMaster/IndexSave";
                        $http({
                            method: "post",
                            url: posturl,
                            data: JSON.stringify(BankStatement_Index)
                        }).then(function (response) {
                            alert("Record Save Successfully !!");
                            window.location = "/MainMemberMaster/Index";
                            //$scope.GetAllMember();
                        }, function (data) {
                            alert("Error Occur During This Request" + posturl);
                        });

                    });
                    //  $scope.btnupload = false;
                } else {
                    if ($scope.MemberName == undefined || $scope.MemberName == "") {
                        alert("Please Enter MemberName");
                        return false;
                    }
                    if ($scope.FamilyID == undefined || $scope.FamilyID == "") {
                        alert("Please select the Family");
                        return false;
                    }


                    if ($scope.Gender == undefined || $scope.Gender == "") {
                        alert("Please Select the Gender.");
                        return false;
                    }

                    if ($scope.Email == undefined || $scope.Email == "") {
                        alert("Please Enter the Email.");
                        return false;
                    }
                    if ($scope.Password == undefined || $scope.Password == "") {
                        alert("Please Enter the Password.");
                        return false;
                    }
                    if ($scope.OperatorID == undefined || $scope.OperatorID == "") {
                        alert("Please Select the Operator.");
                        return false;
                    }
                    var BankStatement_Index = {};
                    //    BankStatement_Index.MemberID = $scope.MemberID;
                    BankStatement_Index.FamilyID = $scope.FamilyID;
                    BankStatement_Index.MemberName = $scope.MemberName;
                    BankStatement_Index.Code = $scope.Code;
                    BankStatement_Index.Address_1 = $scope.Address_1;
                    BankStatement_Index.Address_2 = $scope.Address_2;
                    BankStatement_Index.Address_3 = $scope.Address_3;
                    BankStatement_Index.Gender = $scope.Gender;
                    BankStatement_Index.ServTax_No = $scope.ServTax_No;
                    BankStatement_Index.AadharCardNo = $scope.AadharCardNo;
                    //  BankStatement_Index.ReportLogoPath = $scope.ReportLogoPath;
                    BankStatement_Index.UserId = $scope.UserId;
                    BankStatement_Index.PAN = $scope.PAN;
                    BankStatement_Index.Active = $scope.Active;
                    BankStatement_Index.CreatedBy = $scope.CreatedBy;
                    BankStatement_Index.CreatedDate = $scope.CreatedDate;
                    BankStatement_Index.ModifiedBy = $scope.ModifiedBy;
                    BankStatement_Index.ModifiedDate = $scope.ModifiedDate;
                    BankStatement_Index.SubscriberID = $scope.SubscriberID;
                    BankStatement_Index.OperatorID = $scope.OperatorID;
                    BankStatement_Index.Email = $scope.Email;
                    BankStatement_Index.Password = $scope.Password;
                    BankStatement_Index.Client_Code = $scope.Client_Code;
                    // BankStatement_Index.Createddate = $scope.BankId;

                    var posturl = "/MainMemberMaster/IndexSave";
                    $http({
                        method: "post",
                        url: posturl,
                        data: JSON.stringify(BankStatement_Index)
                    }).then(function (response) {
                        alert("Record Save Successfully !!");
                        $scope.GetAllMember();

                    }, function (data) {
                        alert("Error Occur During This Request" + posturl);
                    });

                }
            } else {
                alert("Email Id Already Exits in System!");
                return false;
            }

        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }

    $scope.EDITData = function (details) {
        //  alert(details.ID)
        $scope.EditDetails = details;
        $scope.UpdateDetails = details;

    }
    $scope.DeleteData = function (details) {
        //  alert(details.ID)
        var update_Index = {};
        update_Index.MemberID = details.MemberID;

        var posturl = "/MainMemberMaster/Delete";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(update_Index)
        }).then(function (response) {
            alert("Record Delete Successfully !!");
            $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }
    $scope.UpdateData = function (UpdateDetails) {


        if (UpdateDetails.MemberName == undefined || UpdateDetails.MemberName == "") {
            alert("Please Enter Member Name.");
            return false;
        }
        if (UpdateDetails.FamilyID == undefined || UpdateDetails.FamilyID == "0") {
            alert("Please select the Family.");
            return false;
        }

        if (UpdateDetails.Gender == undefined || UpdateDetails.Gender == "") {
            alert("Please Select the Gender.");
            return false;
        }
        if (UpdateDetails.Email == undefined || UpdateDetails.Email == "") {
            alert("Please Enter the Email.");
            return false;
        }
        if (UpdateDetails.OperatorID == undefined || UpdateDetails.OperatorID == "") {
            alert("Please Select the Operator.");
            return false;
        }


        var update_Index = {};
        update_Index.MemberID = UpdateDetails.MemberID;
        update_Index.FamilyID = UpdateDetails.FamilyID;
        update_Index.MemberName = UpdateDetails.MemberName;
        update_Index.Address_1 = UpdateDetails.Address_1;
        update_Index.Address_2 = UpdateDetails.Address_2;
        update_Index.Address_3 = UpdateDetails.Address_3;

        update_Index.Gender = UpdateDetails.Gender;
        update_Index.ServTax_No = UpdateDetails.ServTax_No;
        update_Index.AadharCardNo = UpdateDetails.AadharCardNo;
        update_Index.PAN = UpdateDetails.PAN;

        if (UpdateDetails.OperatorID == "") {
            update_Index.OperatorID = -1;
        } else {
            update_Index.OperatorID = UpdateDetails.OperatorID;
        }

        update_Index.Email = UpdateDetails.Email;
        update_Index.Password = UpdateDetails.Password;
        update_Index.Client_Code = UpdateDetails.Client_Code;

        // BankStatement_Index.SubscriberID = $scope.SubscriberID;
        //  BankStatement_Index.OperatorID = $scope.OperatorID;


        var posturl = "/MainMemberMaster/edit";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(update_Index)
        }).then(function (response) {
            alert("Record Update Successfully !!");
            $('#modal_addedit').modal('hide');

            $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
        // update


    }
    $scope.update = function (details) {

        var update_Index = {};
        update_Index.Code = details.MemberID;
        //   update_Index.SubscriberID = details.Session["UserID"];

        if (details.OperatorID == "") {
            update_Index.OperatorID = -1;
        } else {
            // update_Index.OperatorID = UpdateDetails.OperatorID;
            update_Index.OperatorID = details.OperatorID;
        }

        var posturl = "/MainMemberMaster/editselect";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(update_Index)
        }).then(function (response) {
            alert("Record Update Successfully !!");
            $scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });
    }
    $scope.SubmitDataaddopearator = function () {


        // alert("hdsfh");ID, OperatorName, Mobile, Email, ManagerId, Sucriberid, Ismanager, Createddate, Createdby
        var BankStatement_Index = {};
        BankStatement_Index.OperatorName = $scope.Operator_name;
        BankStatement_Index.Mobile = $scope.Mobile;
        BankStatement_Index.Email = $scope.Email;
        // BankStatement_Index.ManagerId = $scope.ManagerId;
        BankStatement_Index.Sucriberid = $scope.Sucriberid;
        // BankStatement_Index.Ismanager = $scope.Ismanager;
        BankStatement_Index.Createddate = $scope.Createddate;
        BankStatement_Index.Createdby = $scope.Createdby;

        // BankStatement_Index.Createddate = $scope.BankId;
        var posturl = "/MainMemberMaster/IndexSavenew";
        $http({
            method: "post",
            url: posturl,
            data: JSON.stringify(BankStatement_Index)
        }).then(function (response) {
            alert("Record Save Successfully !!");
            window.location.href = '/MainMemberMaster/Index/';
            //$scope.GetAllMember();
        }, function (data) {
            alert("Error Occur During This Request" + posturl);
        });

    }
});
