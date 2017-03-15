using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Newtonsoft.Json;
using Stockbook.Model;
using OfficeOpenXml;
using Stockbook.Class;
using Excel = Microsoft.Office.Interop.Excel;

namespace Stockbook.Class
{
    public class DbClass
    {
        public static ProductHelper ProductHelper = new ProductHelper();

        public static TransactionHelper TransactionHelper = new TransactionHelper();

        public static EtcHelper EtcHelper = new EtcHelper();


    }
}
