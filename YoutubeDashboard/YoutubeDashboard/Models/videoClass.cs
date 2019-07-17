using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeDashboard.Models
{
    public class videoClass
    {
        public string user_id { get; set; }
        public string video_id { get; set; }
        public string Search_keyword { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string channel_title { get; set; }
        public string published_at { get; set; }
        public string view_count { get; set; }
        public string like_count { get; set; }
        public string favourite_count { get; set; }
        public string dislike_count { get; set; }
        public string comment_count { get; set; }
        public string thumbnail_url { get; set; }
        public string date_created { get; set; }

    }
}