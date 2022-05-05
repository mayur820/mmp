function setmenu(tp) {
    var menu_list = $(".metismenu > li").length;
    for (i = 1; i <= menu_list; i++) {
        if (eval(i) == tp) 
    {
      if(document.getElementsByClassName("topmenu_" + i))
            $(".topmenu_" + i).addClass("mm-active");
            $(".topmenu_" + i + " ul").addClass("mm-show");
        }
    else 
    {
      if(document.getElementsByClassName("topmenu_" + i))
            $(".topmenu_" + i).removeClass("mm-active");
            $(".topmenu_" + i + " ul").removeClass("mm-show");
        }
    }
}


function setsubmenu(tp_1) {
    var menu_list_1 = $(".sub-menu li").length;
    for (i = 1; i <= menu_list_1; i++) {
        if (eval(i) == tp_1) 
    {
      if(document.getElementsByClassName("submenu_" + i))
            $(".submenu_" + i).addClass("mm-active");
        }
    else 
    {
      if(document.getElementsByClassName("submenu_" + i))
            $(".submenu_" + i).removeClass("mm-active");
        }
    }
}

function setreportsmenu(reports_list) {
    var reports = $(".sub-menu li").length;
    for (i = 1; i <= reports; i++) {
        if (eval(i) == reports_list) 
    {
      if(document.getElementsByClassName("reports_" + i))
            $(".reports_" + i).addClass("mm-active");
        }
    else 
    {
      if(document.getElementsByClassName("reports_" + i))
            $(".reports_" + i).removeClass("mm-active");
        }
    }
}