using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using Complaints.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ComplaintsContext _context;
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
            return _context.Complaints;
        }

        public ComplaintEntity GetComplaintById(int id)
        {
            var complaint = _context.Complaints.Find(id);

            return complaint ?? throw new ComplaintException($"Complaint with id: {id} cannot be found");
        }
    }
}
