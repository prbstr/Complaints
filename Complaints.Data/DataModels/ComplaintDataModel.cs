using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Complaints.Data.DataModels
{
    public class ComplaintDataModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
