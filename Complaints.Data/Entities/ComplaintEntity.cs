using Complaints.Data.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Complaints.Data.Entities
{
    public class ComplaintEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public static ComplaintEntity MapToEntity(ComplaintDataModel model)
        {
            return new ComplaintEntity
            {
                Title = model.Title,
                Description = model.Description
            };
        }
    }
}
