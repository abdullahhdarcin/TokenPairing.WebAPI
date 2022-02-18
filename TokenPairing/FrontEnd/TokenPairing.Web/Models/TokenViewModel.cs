using System;
using System.ComponentModel.DataAnnotations;

namespace TokenPairing.Web.Models
{
    public class TokenViewModel
    {
        public string EMPUSH_TOKEN { get; set; }

        public string EMPUSH_APP_ID { get; set; }

        public string CUSTOMER_ID { get; set; }

        [Key]
        public string MEMBER_ID { get; set; }

        public string STATUS { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime DS_LAST_CHANGE { get; set; }
    }
}
