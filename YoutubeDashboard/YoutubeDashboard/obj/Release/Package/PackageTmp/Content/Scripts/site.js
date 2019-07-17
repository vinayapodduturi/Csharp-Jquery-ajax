$(function () {    
    $("#login-btn").click(function (e) {
        uname = $("#username").val();
        pwd = $("#password").val();

        $.ajax({
            url: '/Home/AuthenticateUser?username=' + uname + '&password=' + pwd,
            method: "GET",
            beforeSend:function(){$("#login-btn").button('loading');},
            success: function (r) {
                r = $.parseJSON(r);
                if (r.status == "Yes") {
                    
                    if ($("#remember_me").is(':checked')) {
                        localStorage.usrname = $("#username").val();
                        localStorage.usr_token = r.tken;
                        localStorage.chkbx = "Yes";
                    } else {
                        if (!localStorage.getItem("usrname") === null) {
                            localStorage.removeitem(usrname);
                        }

                        if (!localStorage.getItem("usr_token") === null) {
                            localStorage.removeitem(usr_token);
                        }

                        if (!localStorage.getItem("chkbx") === null) {
                            localStorage.removeitem(chkbx);
                        }                      
                    }
                    //  alert(localstorage.usrname + " " + localstorage.usr_token);
                    window.location = 'User/Index';
                } else {
                    $("#login-btn").button('reset');
                    $("#login-error").html(r.output_message + ': ' + r.error_message).fadeIn("slow");
                    //$("#ajaxout").html(r.output_message);
                    //$("#ajaxerr").html(r.error_message);
                }
            },
            error: function () { }


        });        
    });
    
});