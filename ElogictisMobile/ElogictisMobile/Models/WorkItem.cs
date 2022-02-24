using System;
using System.Collections.Generic;
using System.Text;
using ElogictisMobile.Services;

namespace ElogictisMobile.Models
{
    public class WorkItem : IIdentifiable
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeSpan Total
        {
            get => End - Start;
        }

        public string Id { get; set; }
    }
}
