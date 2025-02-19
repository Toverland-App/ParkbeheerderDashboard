using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkbeheerderDashboard;
using ParkbeheerderDashboard.Models; // Added this line

namespace ParkbeheerderDashboard.Tests
{
    [TestClass]
    public class AttractionTests
    {
        [TestMethod]
        public void TestAttractionProperties()
        {
            // Arrange
            var attraction = new Attraction
            {
                Id = 1,
                Name = "Roller Coaster",
                MinHeight = 1.2,
                AreaId = 5,
                Description = "A thrilling roller coaster ride.",
                OpeningTime = "09:00 AM",
                ClosingTime = "10:00 PM",
                Capacity = 24,
                QueueSpeed = 10,
                QueueLength = 100
            };

            // Act & Assert
            Assert.AreEqual(1, attraction.Id);
            Assert.AreEqual("Roller Coaster", attraction.Name);
            Assert.AreEqual(1.2, attraction.MinHeight);
            Assert.AreEqual(5, attraction.AreaId);
            Assert.AreEqual("A thrilling roller coaster ride.", attraction.Description);
            Assert.AreEqual("09:00 AM", attraction.OpeningTime);
            Assert.AreEqual("10:00 PM", attraction.ClosingTime);
            Assert.AreEqual(24, attraction.Capacity);
            Assert.AreEqual(10, attraction.QueueSpeed);
            Assert.AreEqual(100, attraction.QueueLength);
        }
    }
}
