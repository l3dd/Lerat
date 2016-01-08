using LeratHarborMap.DomainModel;
using LeratHarborMap.TechnicalCore;

namespace LeratHarborMap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.Info("Starting program LeratHarborMap");
            var leratHarbor = new Harbor("Lerat", @"./Resources/HarborMap.jpg");
            leratHarbor.DrawNameOnMap();
        }
    }
}
