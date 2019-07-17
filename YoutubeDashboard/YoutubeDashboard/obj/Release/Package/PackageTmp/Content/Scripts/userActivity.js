
$(function () {


    $.getJSON('/User/userLogActivity', function (data) {
        resultsDiv = "<table class='table table-striped table-bordered'>" +
                     "<tr><th> User Id </th> <th> Username </th> <th> Activity </th> <th> Login Count </th> <th> User Type </th></tr>";
        console.log("recent search");

        $.each(data, function (index, item) {


            resultsDiv += "<tr><td>" + item.user_id + "</td>" +
            "<td>" + item.username + "</td>" +
            "<td>" + item.user_action + "</td>" +
            "<td>" + item.Login_count + "</td>" +
            "<td>" + item.user_activity_type + "</td></tr>" ;
                             
        });
        resultsDiv += "</table>";

        console.log(resultsDiv);
        $("#Activityresults").html(resultsDiv);
    });


});    //function 


