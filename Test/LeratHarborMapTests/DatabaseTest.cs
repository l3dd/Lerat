using Lerat.Core.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lerat.Test
{
    [TestFixture]
    public class DatabaseTest
    {
        /// <summary>
        /// test the connection to a societaire db
        /// </summary>
        [Test]
        public void TestDatabaseAccess()
        {
            // Share Mode=16 - use if multiple users must have simultaneous access to the db
            string constr = @"Provider=Microsoft.ACE.OLEDB.12.0;Mode=12;Data Source=C:\Work\Git\Lerat\Test\LeratHarborMapTests\societaire.accdb;user id=;password=;";

            using (var connection = new OleDbConnection(constr))
            {
                connection.Open();
                Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);

                using (var command = new OleDbCommand("SELECT * FROM SOCIETAIRE2015", connection))
                {
                    var reader = command.ExecuteReader();
                    Assert.IsTrue(reader.Read());
                    Assert.IsNotNull(reader[0]);
                    reader.Close();
                }

                connection.Close();
            }
        }

        [Test]
        public void TestFullMapWithDbGeneration()
        {
            SocietaireDataAccess c = new SocietaireDataAccess();
            var harbor = c.GetDatasFromSql(System.AppDomain.CurrentDomain.BaseDirectory + @"Resources\HarborMap.xml");

            Assert.IsNotNull(harbor);
            Assert.IsNotNull(harbor.Lines);
            Assert.Greater(harbor.Lines.Count, 0);

            harbor.DrawNameOnMaps();
        }
    }
}
