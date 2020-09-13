using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Complaints.Core.Complaint
{
    public interface IComplaintService
    {
        ComplaintEntity AddComplaint(ComplaintEntity complaint);
        IEnumerable<ComplaintEntity> GetAll();
        ComplaintEntity GetComplaintById(int id);
    }

    public class ComplaintService : IComplaintService
    {
        private ComplaintsContext _context;
        public ComplaintService(ComplaintsContext context)
        {
            _context = context;
        }

        public ComplaintEntity AddComplaint(ComplaintEntity complaintEntity)
        {
            var complaint = _context.Add(complaintEntity);
            _context.SaveChanges();
            return complaint.Entity;
        }

        public IEnumerable<ComplaintEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public ComplaintEntity GetComplaintById(int id)
        {
            return _context.Complaints.Find(id);
        }
    }
}
