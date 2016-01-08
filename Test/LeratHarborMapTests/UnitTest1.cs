using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LeratHarborMap.DomainModel;
using NUnit.Framework;

namespace LeratHarborMapTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            Harbor leratHarbor = new Harbor("Lerat", @"./Resources/HarborMap.jpg");
            Assert.IsTrue(leratHarbor.Map.Exists);
            Assert.IsFalse(string.IsNullOrEmpty(leratHarbor.Name));
        }
        
        [Test]
        public void DeserializeTest()
        {
            Harbor harbor;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (Harbor));
            using (StreamReader reader = new StreamReader(@"./Resources/HarborMap.xml"))
            {
                harbor = (Harbor)xmlSerializer.Deserialize(reader);
            }

            Assert.IsNotNull(harbor);
            Assert.IsTrue(harbor.Lines.Any());
            Assert.IsTrue(harbor.Lines.First().Anchorages.Any());
            Assert.IsFalse(string.IsNullOrEmpty(harbor.Lines.First().Anchorages.First().FirstName));
            Assert.IsFalse(string.IsNullOrEmpty(harbor.Lines.First().Anchorages.First().LastName));
            Assert.IsFalse(string.IsNullOrEmpty(harbor.Lines.First().Anchorages.First().Name));
            Assert.IsNotNull(harbor.Lines.First().Anchorages.First().Coord);
        }

        [Test]
        public void DrawingTest()
        {
            Harbor harbor;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Harbor));
            using (StreamReader reader = new StreamReader(@"./Resources/HarborMap.xml"))
            {
                harbor = (Harbor)xmlSerializer.Deserialize(reader);
            }
            
            DrawerName d = new DrawerName(@"./Resources/HarborMap.jpg", harbor);
            d.Draw();
        }
    }
}
