/*This File Created By Ankit Tailor*/
//ng-app="myApp"
//ng-controller="myctrn" ng-init="PageLoad()"
//<select class="form-control" id="ddl_City" ng-change="Getcitytext()" ng-model="ddl_City">
//   <option selected="selected" value="-1">---Select---</option>
//   <option ng-repeat="City in  listCitys" value="{{City.ID}}">{{City.NAME}}</option>
//</select>
//Angular Foramte
//var app = angular.module("myApp", []);
//app.controller("myctrn", function ($scope, $http) {
//    $scope.liststates;
//    $scope.PageLoad = function () {
//        $scope.bindstate();
//        //call your fucntion on page load()
//    }
//});
//$scope.FnInsertData = function () {
//    fnPostDataUsingPostRequestWithModel("/Home/Insertdata", "EmpModel", $scope, $http);
//    $scope.EmpModel = null;
//}
//fnGetDataUsingGetRequestWithModel("/Home/GetCityByid?id=" + id, "listCitys", $scope, $http);

function fnGetDataUsingGetRequestWithModel(geturl, DynamicFiled, $scope, $http) {
    
    $http({
        method: "get",
        url: geturl
    }).then(function (response) {
        $scope[DynamicFiled] = JSON.parse(response.data);
       
    }, function (data) {
        //deferred.reject({ message: "Really bad" });
        alert("Error Occur During This Request" + geturl);
    })
  
}
function fnGetSingleDataUsingGetRequestWithModel(geturl, DynamicFiled, $scope, $http) {

    $http({
        method: "get",
        url: geturl
    }).then(function (response) {
      
       $scope[DynamicFiled] = JSON.parse(response.data)[0];

    }, function (data) {
        //deferred.reject({ message: "Really bad" });
        alert("Error Occur During This Request" + geturl);
    })

}
function fnPostDataUsingPostRequestWithModel(geturl, Dynamicdata, $scope, $http) {
  
    $http({
        method: "post",
        url: geturl,
        data: $scope[Dynamicdata]
    }).then(function (response) {
       
       
       //$scope[DynamicFiled] = JSON.parse(response.data);
    }, function (data) {
        
        alert("Error Occur During This Request" + geturl);
    })
}
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}


function fnPostDataUsingPostRequestWithJson(geturl, Dynamicdata, $scope, $http) {

    $http({
        method: "post",
        url: geturl,
        data: JSON.stringify(Dynamicdata)
    }).then(function (response) {


        //$scope[DynamicFiled] = JSON.parse(response.data);
    }, function (data) {

        alert("Error Occur During This Request" + geturl);
    })
}

function fnPostData_Msg_UsingPostRequestWithModel(geturl, Dynamicdata, $scope, $http,Msg,RedirectUrl) {

    $http({
        method: "post",
        url: geturl,
        data: $scope[Dynamicdata]
    }).then(function (response) {
        alert(Msg);
        if(RedirectUrl!="")
        {
            window.location.href = RedirectUrl;
        }
        //$scope[DynamicFiled] = JSON.parse(response.data);
    }, function (data) {

        alert("Error Occur During This Request" + geturl);
    })
}
function fnGetddlText(DynamicFiled) {
    var e = document.getElementById(DynamicFiled);
    var value = e.options[e.selectedIndex].value;// get selected option value
    var text = e.options[e.selectedIndex].text;
    return text;
}

//<input type="file" id="image" accept="image/*" onchange="encodeImgtoBase64(this,'b64')">
//<input type="hidden" id="b64" class="form-control">
function encodeImgtoBase64(element, HFControl) {
    var img = element.files[0];
    var UploadFilesName = img.name;
    var reader = new FileReader();
    reader.onloadend = function () {
        $("#" + HFControl + "").val((reader.result));
        //$("#" + HFControl + "").val(encodeURIComponent(reader.result));

    }
    reader.readAsDataURL(img);
}
function encodeImgtoBase64New(element, HFControl) {
    debugger;
    var img = element.files[0];
    var UploadFilesName = img.name;
    var reader = new FileReader();
    reader.onloadend = function () {
        $("#" + HFControl + "").val((UploadFilesName + '@' + reader.result));
        //$("#" + HFControl + "").val(encodeURIComponent(reader.result));

    }
    reader.readAsDataURL(img);
}
