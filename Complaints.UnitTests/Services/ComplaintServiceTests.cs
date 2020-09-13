using Complaints.Core.Complaint;
using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using Complaints.Data.ViewModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Complaints.UnitTests.Services
{
    public class ComplaintServiceTests
    {
        [Theory]
        [InlineData("Poor food quality", "The food quality was poor")]
        public void ShouldFetchAllComplaintsFromDb(string title, string description)
        {
            // Arrange
            using var context = new ComplaintsContext(DbFixtureProvider.CreateNewContextOptions());
            var complaintService = new ComplaintService(context);
            var complaintEntityEntries = new List<EntityEntry<ComplaintEntity>>();
            var complaintIds = new List<int>();

            for (var i = 0; i < 5; i++)
            {
                complaintEntityEntries.Add(
                    context.Complaints.Add(new ComplaintEntity
                    {
                        Title = title,
                        Description = description
                    }));
            }

            context.SaveChanges();

            // Act
            var complaintsInDb = complaintService.GetAll().ToList();

            // Assert 
            Assert.Equal(complaintsInDb.Count, complaintEntityEntries.Count);
            foreach(var complaint in complaintEntityEntries)
            {
                Assert.NotNull(complaintsInDb.Find(x => x.Id == complaint.Entity.Id));
            }
        }

        [Theory]
        [InlineData("Poor service", "The service is poor")]
        public void ShouldFetchCorrectComplaintFromDb(string title, string description)
        {
            // Arrange
            using var context = new ComplaintsContext(DbFixtureProvider.CreateNewContextOptions());
            var complaintService = new ComplaintService(context);
            var complaintEntity = new ComplaintEntity
            {
                Title = title,
                Description = description
            };

            var createdComplaint = context.Complaints.Add(complaintEntity);
            context.SaveChanges();

            // Act
            var complaintInDb = complaintService.GetComplaintById(createdComplaint.Entity.Id);

            // Assert 
            Assert.Equal(complaintInDb, createdComplaint.Entity);
        }

        [Theory]
        [InlineData(21)]
        public void ShouldThrowAnExceptionGivenIdIfItemNotFoundInDatabase(int complaintId)
        {
            // Arrange
            using var context = new ComplaintsContext(DbFixtureProvider.CreateNewContextOptions());
            var complaintService = new ComplaintService(context);

            // Act + Assert 
            Assert.Throws<ComplaintException>(() => complaintService.GetComplaintById(complaintId));
        }

        [Theory]
        [InlineData("Poor service", "The service is poor")]
        public void ShouldAddComplaintToDatabase(string title, string description)
        {
            // Arrange
            using var context = new ComplaintsContext(DbFixtureProvider.CreateNewContextOptions());
            var complaintService = new ComplaintService(context);
            var complaintEntity = new ComplaintEntity
            {
                Title = title,
                Description = description
            };

            // Act
            var createdComplaint = complaintService.AddComplaint(complaintEntity);

            // Assert
            var complaintInDb = context.Complaints.Find(createdComplaint.Id);

            Assert.NotNull(complaintInDb);
            Assert.Equal(createdComplaint, complaintInDb);
        }
    }
}
