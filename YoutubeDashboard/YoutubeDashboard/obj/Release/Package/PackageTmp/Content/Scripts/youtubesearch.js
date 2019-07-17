
$(function () {

    var sort_list = '';
    var arr_list = '';
    var array_list = '';
    var Search_keyword = '';

$("#SearchVideo").click(function (e) {
    $.ajax({
        url: 'https://www.googleapis.com/youtube/v3/search?part=snippet&q=' + $("#userInput").val() + '&key=AIzaSyA1hSn5SXpPweusL-xQJgqxk978pvTF7LQ',
        type: 'GET',
         success: function (r) {
            
            resultsDiv = "";
            //resultvideoid = "";

            $.each(r.items, function (index, item) {
                vid = 'https://www.youtube.com/watch?v=' + item.id.videoId;
                resultsDiv += "<div id='" + item.id.videoId + "' class='col-md-4 video-result-container'>"
                               +"<div class='video-result'>" 
                               + "<div class='pull-left video-thumbnail'>"
                                  + "<a href=" + vid + "><img src='" + item.snippet.thumbnails.default.url + "'/></a>"
                                + "</div>"
                                + "<div class='video-detail'>"
                                  + "<div class='video-title'>"
                                  + item.snippet.title
                                  +"</div>"                                  
                                  + "<div class='video-author'> by "+ item.snippet.channelTitle
                                  +" on "+ $.format.date(item.snippet.publishedAt, 'MM/dd/yyyy')
                                  + "</div>"
                                  + "<span class='statistics' id='index'></span>"
                                + "</div>"
                                + "</div>"
                              + "</div>";
                $.ajax({
                    url: 'https://www.googleapis.com/youtube/v3/videos?part=statistics&id=' + item.id.videoId + '&key=AIzaSyA1hSn5SXpPweusL-xQJgqxk978pvTF7LQ',
                    data: 'url',
                    success: function (j) {
                        //var video_url = 'https://www.youtube.com/v/' + $('.videoid').attr('id');
                        console.log(j);
                        sort_list = j;
                        $("#" + item.id.videoId).find(".statistics").html("<br><p>View count:" + j.items[0].statistics.viewCount + "</p><br>");
                        $("#" + item.id.videoId).find(".statistics").append("<p>Comment count:" + j.items[0].statistics.commentCount + "</p><br>");
                        $("#" + item.id.videoId).find(".statistics").append("<p>Like count:" + j.items[0].statistics.likeCount + "</p><br>");
                        $("#" + item.id.videoId).find(".statistics").append("<p>Favourite count:" + j.items[0].statistics.favoriteCount + "</p><br>");
                        $("#" + item.id.videoId).find(".statistics").append("<p>Dislike count:" + j.items[0].statistics.dislikeCount + "</p><br>");

                        // var video_id = item.id.videoId;
                        var Search_keyword = $("#userInput").val();
                        // var title = item.snippet.title;
                        var v_url = 'https://www.youtube.com/watch?v=' + item.id.videoId;
                        

                        $.ajax({
                            url: '/User/loadVideoSearchLog?&p_video_id= '+item.id.videoId+ '&p_Search_keyword= '+ 
                                Search_keyword+'&p_title= '+ 
                                item.snippet.title+'&p_url= '+
                                v_url+'&p_channel_title= '+
                                item.snippet.channelTitle+'&p_published_at= '+ 
                                item.snippet.publishedAt+'&p_view_count= '+
                                j.items[0].statistics.viewCount+'&p_like_count= '+
                                j.items[0].statistics.likeCount+'&p_favourite_count= '+
                                j.items[0].statistics.favoriteCount+'&p_dislike_count= '+
                                j.items[0].statistics.dislikeCount+'&p_comment_count= '+
                                j.items[0].statistics.commentCount+'&p_thumbnail_url= '+
                                item.snippet.thumbnails.default.url,
                            method: "GET"                             
                        });        
            
                        $("#results").html(resultsDiv);
                        
                        /*
                         $("#" + item.id.videoId).find(".statistics").html("<br><p>View count:"+j.items[0].statistics.viewCount+"</p><br>");
                        $("#" + item.id.videoId).find(".statistics").append("<p>Comment count:"+j.items[0].statistics.commentCount+"</p><br>");
                         $("#" + item.id.videoId).find(".statistics").append("<p>Like count:"+j.items[0].statistics.likeCount+"</p><br>");
                        $("#" + item.id.videoId).find(".statistics").append("<p>Favourite count:"+j.items[0].statistics.favoriteCount+"</p><br>");
                        $("#" + item.id.videoId).find(".statistics").append("<p>Dislike count:"+j.items[0].statistics.dislikeCount+"</p><br>");
                        */
                        //$("#results").html(resultsDiv);
                    }
                });
            });                                   
        }
    });
});


    $(".sort-link").click(function (e) {

        sortCriteria = $(this).data("sort");
        console.log(sortCriteria);
    
        var Search_Keyword = $("#userInput").val();
        $.getJSON('/User/RankVideos?p_search_keyword=' + Search_Keyword + '&p_rank_keyword=' + sortCriteria, function (data) {
            resultsDiv = "";
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
                                  + item.view_count +"| Likes"+ item.like_count
                                  + "</span><br>"
                                  + "<span class='statistics'>Dislikes:"
                                  + item.dislike_count + "| Comments" + item.comment_count
                                  + "</span><br>"
                                 
                                 + "</div>" + "</div>" + "</div>";

                console.log(resultsDiv);
            });

            $("#results").html(resultsDiv);
        });
    });    //function 
       
});

