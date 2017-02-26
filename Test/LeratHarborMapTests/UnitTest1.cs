using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Lerat.Core.DomainModel;
using NUnit.Framework;
using System.Data.OleDb;

namespace LeratHarborMapTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            Harbor leratHarbor = new Harbor("Lerat");            
            Assert.IsFalse(string.IsNullOrEmpty(leratHarbor.Name));
        }
        
        [Test]
        public void DeserializeTest()
        {
            Harbor harbor;
            var harborMapXmlFilePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Resources\HarborMap.xml";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (Harbor));

            Assert.IsTrue(File.Exists(harborMapXmlFilePath));

            using (StreamReader reader = new StreamReader(harborMapXmlFilePath))
            {
                harbor = (Harbor)xmlSerializer.Deserialize(reader);
            }

            Assert.IsNotNull(harbor);
            Assert.IsTrue(harbor.Lines.Any());
            Assert.IsTrue(harbor.Lines.First().Anchorages.Any());
            Assert.IsTrue(string.IsNullOrEmpty(harbor.Lines.First().Anchorages.First().FirstName));
            Assert.IsTrue(string.IsNullOrEmpty(harbor.Lines.First().Anchorages.First().LastName));
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
            DrawerName d = new DrawerName(harbor);
            d.DrawLines();
        }

        private TemporaryPersonn splitLineInObject(string line)
        {
            var c = line.Split(';');
            // for line / anchore
            var lineName = c[7].Split('-').First();
            TemporaryPersonn ts = new TemporaryPersonn(c[0], c[1], lineName, c[7].Substring(c[7].Length-1, 1));

            return ts;
        }

        private void CleanAllImageFromExecutionDirectory()
        {
            var di = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory);
            di.GetFiles("*.jpg").ToList().ForEach(image => image.Delete());
            di = null;
        }

        /// <summary>
        /// Load a Harbor Map and draw name from the harmap.xml file.
        /// The generated file should be on test execution directory with name "PlanLeratAvecNoms.jpg"
        /// </summary>
        [Test]
        public void DrawingTest()
        {
            CleanAllImageFromExecutionDirectory();

            // populate dummy datas
            // read datas from xml : name and coordinates
            Harbor harbor;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Harbor));
            using (StreamReader reader = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"Resources\HarborMap.xml"))
            {
                harbor = (Harbor)xmlSerializer.Deserialize(reader);
            }

            // provide some dummy names because the xml harbormap is empty
            foreach (var line in harbor.Lines)
            {
                foreach (var anchore in line.Anchorages)
                {
                    anchore.FirstName = line.Name + " names";
                    anchore.LastName = line.Name + " Lastnames " + anchore.Name;
                }
            }

            // 
            DrawerName d = new DrawerName(harbor);
            d.DrawLines();

            Assert.AreEqual(3, Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory, "*.jpg").Length, "The generated file was not found");
            // CleanAllImageFromExecutionDirectory();
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
