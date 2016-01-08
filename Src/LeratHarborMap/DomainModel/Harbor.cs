using LeratHarborMap.TechnicalCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LeratHarborMap.DomainModel
{
    public class Harbor
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public Map Map { get; set; }

        public List<Line> Lines { get; set; }

        /// <summary>
        /// Default constructor used for deserialization
        /// </summary>
        public Harbor()
        {
            Lines = new List<Line>();
        }

        public Harbor(string name, string mapFilePath) : this()
        {
            Name = name;
            Map = new Map(mapFilePath);
        }

        public void Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Logger.Info("Trying to deserialize file {0}, but it dosen't exist", filePath);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof (Harbor));
            using (StreamReader reader = new StreamReader(@".\Resources\HarborMap.xml"))
            {
                xmlSerializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Draw all the names on the orginal map and save it to a file
        /// </summary>
        /// <returns>File path of the generated image containing the name</returns>
        public string DrawNameOnMap()
        {
            string sourceHarborMap = @"./Resources/HarborMap.jpg";
      
            DrawerName d = new DrawerName(sourceHarborMap, this);
            return d.Draw();
        }
    }

    public class Map
    {
        [XmlIgnore]
        public bool Exists 
        {
            get { return MapFile != null && MapFile.Size != Size.Empty; }
        }

        [XmlIgnore]
        public Bitmap MapFile {get; private set;}

        public Map() 
        { }

        /// <summary>
        /// Default constructor for a map of a harbor
        /// </summary>
        /// <param name="filePath">Image file path of the map of the harbor</param>
        public Map(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                MapFile = new Bitmap(filePath);
            }
        }
    }
}
