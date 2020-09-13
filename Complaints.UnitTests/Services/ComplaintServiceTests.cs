using Complaints.Core.Complaint;
using Complaints.Data.Contexts;
using Complaints.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
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

        [Fact]
        public void ShouldAddComplaintToDatabase()
        {
            // Arrange
            using var context = _serviceProvider.GetService<ComplaintsContext>();
            var complaintService = new ComplaintService(context);
            var complaintEntity = new ComplaintEntity
            {
                Title = "Poor service",
                Description = "The service is poor"
            };

            // Act
            var createdComplaint = complaintService.AddComplaint(complaintEntity);

            // Assert
            var complaintInDb = complaintService.GetComplaintById(createdComplaint.Id);

            Assert.NotNull(complaintInDb);
            Assert.Equal(createdComplaint, complaintInDb);
        }
    }
}
