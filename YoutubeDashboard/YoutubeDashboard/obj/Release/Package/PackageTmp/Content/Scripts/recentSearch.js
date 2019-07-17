
$(function () {


        $.getJSON('/User/recentSearchController', function (data) {
            resultsDiv = "";
            console.log("recent search");
            $.each(data, function (index, item) {


                resultsDiv += "<div id='" + item.video_id + "' class='col-md-4 video-result-container'>" + "<div class='video-result'>"
                                + "<div class='pull-left video-thumbnail'>"
                                  + "<a href=" + item.url + "><img src='" + item.thumbnail_url + "'/></a>"
                                + "</div>"
                                + "<div class='video-detail'>" + "<div class='video-title'>"
                                  + item.title
                                  + "</div>"
                                  + "<div class='video-author'> by " + item.channel_title
                                  + " on " + $.format.date(item.published_at, 'MM/dd/yyyy')
                                  + "</div>"
                                  + "<span class='statistics'>Views:"
                                  + item.view_count + "| Likes" + item.like_count
                                  + "</span><br>"
                                  + "<span class='statistics'>Dislikes:"
                                  + item.dislike_count + "| Comments" + item.comment_count
                                  + "</span><br>"

                                 + "</div>" + "</div>" + "</div>";
                //console.log(resultsDiv);
            });

            $("#results").html(resultsDiv);
        });

   
});    //function 
       

