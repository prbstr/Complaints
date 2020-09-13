using Complaints.Core.Complaint;
using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using Complaints.Data.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xunit;

namespace Complaints.UnitTests.Services
{
    public class ComplaintServiceTests : IClassFixture<DbFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        public ComplaintServiceTests(DbFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Theory]
        [InlineData("Poor service", "The service is poor")]
        public void ShouldFetchCorrectComplaintFromDb(string title, string description)
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
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
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var complaintService = new ComplaintService(context);

            // Act + Assert 
            Assert.Throws<ComplaintException>(() => complaintService.GetComplaintById(complaintId));
        }

        [Theory]
        [InlineData("Poor service", "The service is poor")]
        public void ShouldAddComplaintToDatabase(string title, string description)
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
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
