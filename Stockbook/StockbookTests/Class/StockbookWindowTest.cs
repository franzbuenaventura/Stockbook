// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockbookWindowTest.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
// <summary>
//   Unit Test (NUnit) for Product Class in StockBook project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StockbookTests.Class
{
    using System;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Stockbook.Class;
    using Stockbook.Model;

    /// <summary>
    /// The stock book window test.
    /// </summary>
    [TestFixture]
    public class StockbookWindowTest : StockbookWindows
    {
        /// <summary>
        /// The config path.
        /// </summary>
        private readonly string configPath = ConfigFullPath;

        /// <summary>
        /// Testing InitializeConfig() for creating a config with default values and asserting if it exist
        /// </summary>
        [Test]
        public void CreateProductTest()
        {
            if (File.Exists(this.configPath))
            {
                File.Delete(this.configPath);
            }

            var actualResult = OpenConfig();
            var expectedResult = new Config { IsAutoBackupOn = true};
            Assert.AreEqual(expectedResult.IsAutoBackupOn, actualResult.IsAutoBackupOn);

            if (File.Exists(this.configPath))
            {
                File.Delete(this.configPath);
            }
        }

        /// <summary>
        /// The save config test.
        /// </summary>
        [Test]
        public void SaveConfigTest()
        {
            if (File.Exists(this.configPath))
            {
                File.Delete(this.configPath);
            }


            var actualResult = OpenConfig();
            actualResult.IsAutoBackupOn = false;
            StockbookWindows.SaveConfig(actualResult);
            actualResult = OpenConfig();

            var expectedResult = new Config { IsAutoBackupOn = false };
            Assert.AreEqual(expectedResult.IsAutoBackupOn, actualResult.IsAutoBackupOn);

            if (File.Exists(this.configPath))
            {
                File.Delete(this.configPath);
            }
        }

        /// <summary>
        /// The database backup test.
        /// </summary>
        [Test]
        public void DatabaseBackupTest()
        {
            var testPath = Environment.CurrentDirectory + @"\SettingsTest\";
 
            StockbookWindows.DatabaseBackup(location: testPath);

            var actualResult = Directory.GetFiles(testPath, "*.stockbook").ToList().Count;
   
            var expectedResult = 1;
            Assert.AreEqual(expectedResult, actualResult);

            DirectoryInfo di = new DirectoryInfo(testPath); 
            di.Delete(true);
        }
    }
}
