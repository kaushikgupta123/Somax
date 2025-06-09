using System;

namespace Client.Models.Project
{
    public class ProjectTaskTimelineModel
    {
        public long id { get; set; }
        public string text { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        //public long parent { get; set; }
        public decimal progress { get; set; }
        public bool open { get; set; } = true;
        public long ProjectId { get; set; }
        public bool unscheduled { get; set; } = false;
        public string end_date_grid { get; set; }
    }
}