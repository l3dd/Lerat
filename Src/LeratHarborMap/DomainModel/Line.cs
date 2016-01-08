using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace LeratHarborMap.DomainModel
{
    public class Line
    {
        public List<Anchorage> Anchorages { get; set; }
         
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public float Rotation { get; set; }

        public Line()
        {
            Anchorages = new List<Anchorage>();
        }
    }

    public class Anchorage
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Point Coord { get; set; }
    }
}
