using System;
using System.Collections.Generic;
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
        public void ImportAndDrawingTest()
        {
            Harbor harbor;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Harbor));
            using (StreamReader reader = new StreamReader(@"./Resources/HarborMap.xml"))
            {
                harbor = (Harbor)xmlSerializer.Deserialize(reader);
            }

            IList<TemporaryPersonn> ts = new List<TemporaryPersonn>();
            using (StreamReader reader = File.OpenText("./lerat - societaire.csv"))
            {
                reader.ReadLine(); // skip the title
                while (reader.Peek() >= 0)
                {
                    ts.Add(splitLineInObject(reader.ReadLine()));
                }
            }

            // create the link between harbor and temp class TemporaryPersonn
            foreach (var line in harbor.Lines)
            {
                foreach (var anchor in line.Anchorages)
                {
                    if (ts.Any(personn => personn.AnchorName.ToLowerInvariant().Equals(anchor.Name.ToLowerInvariant()) &&
                                                           personn.LineName.ToLowerInvariant().Equals(line.Name.ToLowerInvariant())))
                    {
                        anchor.LastName = ts.First(personn => personn.AnchorName.ToLowerInvariant().Equals(anchor.Name.ToLowerInvariant()) &&
                                                               personn.LineName.ToLowerInvariant().Equals(line.Name.ToLowerInvariant())).LastName;
                    }
                }
            }

            // draw the map

            DrawerName d = new DrawerName(@"./Resources/HarborMap.jpg", harbor);
            d.Draw();
        }

        private TemporaryPersonn splitLineInObject(string line)
        {
            var c = line.Split(';');
            // for line / anchore
            var lineName = c[7].Split('-').First();
            TemporaryPersonn ts = new TemporaryPersonn(c[0], c[1], lineName, c[7].Substring(c[7].Length-1, 1));

            return ts;
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

    internal class TemporaryPersonn
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LineName { get; set; }
        public string AnchorName { get; set; }

        public TemporaryPersonn(string lastName, string firstName, string lineName, string anchorName)
        {
            LastName = lastName;
            FirstName = firstName;
            LineName = lineName;
            AnchorName = anchorName;
        }
    }
}
