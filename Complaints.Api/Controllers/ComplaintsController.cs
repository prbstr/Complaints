using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Complaints.Core.Complaint;
using Complaints.Data.DataModels;
using Complaints.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Complaints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintService _complaintService;

        public ComplaintsController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        [HttpGet]
        public IActionResult GetComplaints()
        {
            var complaints = _complaintService.GetAll();
            return Ok(complaints);
        }

        [Route("add")]
        [HttpPost]
        public IActionResult AddComplaint([FromBody]ComplaintDataModel complaintModel)
        {
            var complaintEntity = ComplaintEntity.MapToEntity(complaintModel);
            var complaint = _complaintService.AddComplaint(complaintEntity);
            return Ok(complaint);
        }
    }
}