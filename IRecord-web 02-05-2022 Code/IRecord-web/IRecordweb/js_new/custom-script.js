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

function setcorporatesmenu(coraction_list) {
    var coraction = $(".sub-menu li").length;
    for (i = 1; i <= coraction; i++) {
        if (eval(i) == coraction_list) 
    {
      if(document.getElementsByClassName("coraction_" + i))
            $(".coraction_" + i).addClass("mm-active");
        }
    else 
    {
      if(document.getElementsByClassName("coraction_" + i))
            $(".coraction_" + i).removeClass("mm-active");
        }
    }
}

function setentrymenu(entry_list) {
    var entry = $(".sub-menu li").length;
    for (i = 1; i <= entry; i++) {
        if (eval(i) == entry_list) 
    {
      if(document.getElementsByClassName("entry_" + i))
            $(".entry_" + i).addClass("mm-active");
        }
    else 
    {
      if(document.getElementsByClassName("entry_" + i))
            $(".entry_" + i).removeClass("mm-active");
        }
    }
}