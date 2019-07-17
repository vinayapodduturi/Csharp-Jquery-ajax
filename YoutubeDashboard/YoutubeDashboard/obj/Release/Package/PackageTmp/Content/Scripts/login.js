$(function () {
    if (localStorage.chkbx == "Yes" && localStorage.usrname !== '' && localStorage.usr_token !== '') {
        uname = localStorage.usrname;
        utkn = localStorage.usr_token;
        $.ajax({
            url: '/Home/checkPreviousLogin?uname=' + uname + '&utoken=' + utkn,
            method: "GET",            
            success: function (r) {
                r = $.parseJSON(r);
                if (r.status == 'Yes') {
                    window.location = '/User/Index';
                } 
                else {
                    //destroy localstorage vars
                    localStorage.removeitem(usrname);
                    localStorage.removeitem(usr_token);
                    localStorage.removeitem(chkbx);
                } 
            },
            error: function () { }
        });
    }
});