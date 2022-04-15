using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class HistoryTransaction
    {
        public string Id { get; set; }
        public string CreateBy { get; set; }
        public string CreateTime { get; set; }
        public string CreateDate { get; set; }
        public string TranferId { get; set; }
        public string TranferName { get; set; }
        public string TranferAuth { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAuth { get; set; }
        public double Money { get; set; }
        public string Status { get; set; }
    }
}
