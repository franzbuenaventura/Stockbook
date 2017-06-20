// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelExport.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   The ExcelExport Class that contains the logic fo excel export in StockBook project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Class
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;

    using Model;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    using Excel = Microsoft.Office.Interop.Excel;

    /// <summary>
    /// The excel helper.
    /// </summary>
    public static class ExcelExport
    {
        /// <summary>
        /// The create excel doc.
        /// </summary>
        public class CreateExcelDoc
        {
            private Excel.Application app = null;

            private Excel.Workbook workbook = null;

            private Excel.Worksheet worksheet = null;

            private Excel.Range workSheet_range = null;

            public CreateExcelDoc()
            {
                createDoc();
            }

            public void createDoc()
            {
                try
                {
                    app = new Excel.Application();
                    app.Visible = true;
                    workbook = app.Workbooks.Add(1);
                    worksheet = (Excel.Worksheet)workbook.Sheets[1];
                }
                catch (Exception e)
                {
                    Console.Write("Error");
                }
            }

            public void createHeaders(
                int row,
                int col,
                string htext,
                string cell1,
                string cell2,
                int mergeColumns,
                string b,
                bool font,
                int size,
                string fcolor,
                bool border = false)
            {
                worksheet.Cells[row, col] = htext;
                workSheet_range = worksheet.get_Range(cell1, cell2);
                workSheet_range.Merge(mergeColumns);
                switch (b)
                {
                    case "YELLOW":
                        workSheet_range.Interior.Color = Excel.XlRgbColor.rgbYellow;
                        break;
                    case "GRAY":
                        workSheet_range.Interior.Color = Excel.XlRgbColor.rgbGray;
                        break;
                    case "GAINSBORO":
                        workSheet_range.Interior.Color = Excel.XlRgbColor.rgbGainsboro;
                        break;
                    case "Turquoise":
                        workSheet_range.Interior.Color = Excel.XlRgbColor.rgbTurquoise;
                        break;
                    case "PeachPuff":
                        workSheet_range.Interior.Color = Excel.XlRgbColor.rgbPeachPuff;
                        break;
                    default:
                        //  workSheet_range.Interior.Color = System.Drawing.Color..ToArgb();
                        break;
                }
                if (border)
                {
                    workSheet_range.Borders.Color = Excel.XlRgbColor.rgbBlack;
                }
                workSheet_range.Font.Bold = font;
                workSheet_range.Font.Size = size;
                workSheet_range.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                workSheet_range.ColumnWidth = size;
                if (fcolor.Equals(string.Empty))
                {
                    workSheet_range.Font.Color = Excel.XlRgbColor.rgbWhite;
                }
                else
                {
                    workSheet_range.Font.Color = Excel.XlRgbColor.rgbBlack;
                }
            }

            public void addData(int row, int col, string data, string cell1, string cell2, string format)
            {
                worksheet.Cells[row, col] = data;
                workSheet_range = worksheet.get_Range(cell1, cell2);
                workSheet_range.Borders.Color = Excel.XlRgbColor.rgbBlack;
                workSheet_range.NumberFormat = format;
            }
        }

        /// <summary>
        /// The excel invoice.
        /// </summary>
        public class ExcelInvoice
        {
            /// <summary>
            /// The export transaction invoice.
            /// </summary>
            /// <param name="transOrder">
            /// The trans order.
            /// </param>
            public static void ExportTransactionInvoice(TransactionOrder transOrder)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = transOrder.DateTransaction.ToString("MMMMM dd yyyy") + " - " + transOrder.RefNo;
                    // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx"; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    string filename = dlg.FileName;
                    FileInfo templateFile = new FileInfo(@"Invoice Template.xlsx");
                    FileInfo newFile = new FileInfo(filename);
                    if (newFile.Exists)
                    {
                        try
                        {
                            newFile.Delete();
                            newFile = new FileInfo(filename);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("The file is currently being used, close the file or choose a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    var config = StockbookWindows.OpenConfig();
                    using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
                    {
                        var currency = string.Empty;
                        switch (config.Currency)
                        {
                            case "PHP - ₱":
                                currency = "₱#,###,##0.00";
                                break;
                            case "USD - $":
                                currency = "$#,###,##0.00";
                                break;
                            case "YEN - ¥":
                                currency = "¥#,###,##0.00";
                                break;
                        }

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        worksheet.Cells["A1"].Value = config.CompanyName;
                        worksheet.Cells["A2"].Value = "SALES INVOICE - REFERENCE NUMBER: " + transOrder.RefNo;
                        worksheet.Cells["A3"].Value = "DATE: " + transOrder.DateTransaction.ToString("MMMM dd, yyyy");
                        worksheet.Cells["A4"].Value = "SOLD TO: " + transOrder.Particular;
                        worksheet.Cells["A5"].Value = "ADDRESS: " + transOrder.ParticularAddress;
                        worksheet.Cells["A6"].Value = "SALESMAN: " + transOrder.SalesmanName;
                        worksheet.Cells["F4"].Value = "TERMS: " + transOrder.Terms;
                        worksheet.Cells["F5"].Value = "DISCOUNT: " + transOrder.DiscountPercentage + "%";
                        int i = 9;
                        decimal unitTotal = 0;
                        decimal billedTotal = 0;
                        string tempCategory = string.Empty;
                        foreach (var trans in transOrder.Transactions)
                        {
                            decimal tempTotal = 0;
                            if (trans.CaseTransact != 0)
                            {
                                tempTotal = trans.Product.CaseValue * trans.CaseTransact;
                                worksheet.Cells["A" + i].Value = trans.Product.ProdCode;
                                worksheet.Cells["B" + i].Value = trans.Product.Name;
                                worksheet.Cells["C" + i].Value = "Case";
                                worksheet.Cells["D" + i].Value = trans.CaseTransact;
                                worksheet.Cells["E" + i].Value = trans.Product.CaseValue;
                                unitTotal += tempTotal;
                                if (transOrder.DiscountPercentage > 0)
                                {
                                    tempTotal = ((transOrder.DiscountPercentage / 100) + 1) * tempTotal;
                                }
                                billedTotal += tempTotal;
                                worksheet.Cells["F" + i].Value = tempTotal;
                                i++;
                            }
                            if (trans.PackTransact != 0)
                            {
                                tempTotal = trans.Product.PackValue * trans.PackTransact;
                                worksheet.Cells["A" + i].Value = trans.Product.ProdCode;
                                worksheet.Cells["B" + i].Value = trans.Product.Name;
                                worksheet.Cells["C" + i].Value = "Pack";
                                worksheet.Cells["D" + i].Value = trans.PackTransact;
                                worksheet.Cells["E" + i].Value = trans.Product.PackValue;
                                unitTotal += tempTotal;
                                if (transOrder.DiscountPercentage > 0)
                                {
                                    tempTotal = ((transOrder.DiscountPercentage / 100) + 1) * tempTotal;
                                }
                                billedTotal += tempTotal;
                                worksheet.Cells["F" + i].Value = tempTotal;
                                i++;
                            }
                            if (trans.PieceTransact != 0)
                            {
                                tempTotal = trans.Product.PieceValue * trans.PieceTransact;
                                worksheet.Cells["A" + i].Value = trans.Product.ProdCode;
                                worksheet.Cells["B" + i].Value = trans.Product.Name;
                                worksheet.Cells["C" + i].Value = "Piece";
                                worksheet.Cells["D" + i].Value = trans.PieceTransact;
                                worksheet.Cells["E" + i].Value = trans.Product.PieceValue;
                                unitTotal += tempTotal;
                                if (transOrder.DiscountPercentage > 0)
                                {
                                    tempTotal = ((transOrder.DiscountPercentage / 100) + 1) * tempTotal;
                                }
                                billedTotal += tempTotal;
                                worksheet.Cells["F" + i].Value = tempTotal;
                                i++;
                            }
                        }

                        worksheet.Cells["A" + 9 + ":A" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["b" + 9 + ":B" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C" + 9 + ":C" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["D" + 9 + ":D" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["E" + 9 + ":E" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["F" + 9 + ":F" + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        worksheet.Cells["A" + 9 + ":A" + i].Style.WrapText = true;
                        worksheet.Cells["b" + 9 + ":B" + i].Style.WrapText = true;
                        worksheet.Cells["C" + 9 + ":C" + i].Style.WrapText = true;
                        worksheet.Cells["D" + 9 + ":D" + i].Style.WrapText = true;
                        worksheet.Cells["E" + 9 + ":E" + i].Style.WrapText = true;
                        worksheet.Cells["F" + 9 + ":F" + i].Style.WrapText = true;

                        i++;
                        worksheet.Cells["C" + i + ":C" + (i + 2)].Style.Numberformat.Format = currency;
                        worksheet.Cells["F" + i + ":F" + (i + 2)].Style.Numberformat.Format = currency;

                        worksheet.Cells["C" + i + ":C" + (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells["F" + i + ":F" + (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells["E" + i + ":E" + (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells["B" + i + ":B" + (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        worksheet.Cells["B" + i].Value = "Vatable Sales: ";
                        worksheet.Cells["C" + i].Value = billedTotal * (decimal)0.88;
                        worksheet.Cells["B" + (i + 1)].Value = "12% VAT: ";
                        worksheet.Cells["C" + (i + 1)].Value = billedTotal * (decimal)0.12;
                        worksheet.Cells["B" + (i + 2)].Value = "Total w/o Discount: ";
                        worksheet.Cells["C" + (i + 2)].Value = billedTotal;

                        worksheet.Cells["E" + i].Value = "Invoice Amt.: ";
                        worksheet.Cells["F" + i].Value = unitTotal;
                        worksheet.Cells["E" + (i + 1)].Value = "Discount: " + transOrder.DiscountPercentage + "%";
                        worksheet.Cells["F" + (i + 1)].Value = unitTotal * (transOrder.DiscountPercentage / 100);
                        worksheet.Cells["E" + (i + 2)].Value = "Total w/ Discount: ";
                        worksheet.Cells["F" + (i + 2)].Value = billedTotal;

                        i += 6;
                        worksheet.Cells["A" + i].Value = "PREPARED BY/DATE";
                        worksheet.Cells["A" + (i + 3)].Value = "PRINT NAME & SIGNATURE";
                        worksheet.Cells["D" + i].Value = "DELIVERED BY/DATE";
                        worksheet.Cells["D" + (i + 3)].Value = "PRINT NAME & SIGNATURE";
                        i += 6;
                        worksheet.Cells["A" + i].Value = "CHECKED BY/DATE";
                        worksheet.Cells["A" + (i + 3)].Value = "PRINT NAME & SIGNATURE";
                        worksheet.Cells["D" + i].Value = "APPROVED BY";
                        worksheet.Cells["D" + (i + 3)].Value = "PRINT NAME & SIGNATURE";
                        worksheet.View.PageLayoutView = false;

                        package.Save();
                    }
                }
            }

            /// <summary>
            /// The order list to stock card.
            /// </summary>
            /// <param name="transactionOrders">
            /// The transaction orders.
            /// </param>
            /// <param name="dateFrom">
            /// The date from.
            /// </param>
            /// <param name="dateTo">
            /// The date to.
            /// </param>
            /// <returns>
            /// The <see cref="List"/>.
            /// </returns>
            private static List<StockCard> OrderListToStockCard(List<TransactionOrder> transactionOrders, DateTime dateFrom, DateTime dateTo)
            {
                var stockCards = new List<StockCard>();
                List<Transaction> listProductsInTrans =
                    transactionOrders.SelectMany(q => q.Transactions).ToList();
                var listProd = listProductsInTrans.GroupBy(q => q.Product.Name).Select(s => s.First()).Select(q => q.Product).ToList();
                var tempProd = new Product();
                foreach (var prod in listProd)
                {
                    var temp = new StockCard();
                    var tempListTrans = new List<StockCardTransaction>();
                    temp.Description = prod.Name;
                    temp.Category = prod.Category;
                    temp.ProdCode = prod.ProdCode;
                    temp.CaseBalance = prod.CaseBalance;
                    temp.PackBalance = prod.PackBalance;
                    temp.PieceBalance = prod.PieceBalance;
                    tempProd = prod;
                    temp.DateFrom = dateFrom;
                    temp.DateTo = dateTo;
                    foreach (var transOrder in transactionOrders.Where(q => q.Transactions.Exists(s => s.Product.Name == prod.Name)).ToList().OrderBy(k => k.DateTransaction))
                    {
                        var stockCardTrans = new StockCardTransaction();
                        stockCardTrans.Transaction = transOrder.TransactionType;
                        stockCardTrans.Particular = transOrder.Particular;
                        stockCardTrans.RefNo = transOrder.RefNo;
                        stockCardTrans.Date = transOrder.DateTransaction;
                        var prodTemp = transOrder.Transactions.FirstOrDefault(q => q.Product.Name == prod.Name);
                        stockCardTrans.Case = prodTemp.CaseTransact;
                        stockCardTrans.Pack = prodTemp.PackTransact;
                        stockCardTrans.Piece = prodTemp.PieceTransact;
                        tempProd = Product.BalanceCasePackPiece(prodTemp, tempProd, transOrder.TransactionType);
                        stockCardTrans.CaseBalance = tempProd.CaseBalance;
                        stockCardTrans.PackBalance = tempProd.PackBalance;
                        stockCardTrans.PieceBalance = tempProd.PieceBalance;

                        tempListTrans.Add(stockCardTrans);
                    }
                    temp.ListTransactions = tempListTrans;
                    stockCards.Add(temp);
                }

                return stockCards;
            }

            /// <summary>
            /// The export transactions.
            /// </summary>
            /// <param name="orderList">
            /// The order list.
            /// </param>
            /// <param name="dateFrom">
            /// The date from.
            /// </param>
            /// <param name="dateTo">
            /// The date to.
            /// </param>
            public static void ExportTransactions(List<TransactionOrder> orderList, DateTime dateFrom, DateTime dateTo)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = DateTime.Now.ToString("MMMMM dd yyyy") + " - Stock Cards"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx"; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    string filename = dlg.FileName;
                    FileInfo templateFile = new FileInfo(@"Transaction Template.xlsx");
                    FileInfo newFile = new FileInfo(filename);
                    if (newFile.Exists)
                    {
                        try
                        {
                            newFile.Delete();
                            newFile = new FileInfo(filename);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(
                                "The file is currently being used, close the file or choose a different name.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }
                    }
                    using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
                    {
                        var listStock = OrderListToStockCard(orderList, dateFrom, dateTo);
                        int worksheetNum = 1;
                        ExcelWorksheet worksheet = null;
                        foreach (var list in listStock)
                        {
                            worksheet = package.Workbook.Worksheets.Copy("Sheet1", list.Description);
                            worksheet.Cells["A3"].Value = "LOCATION: " + list.Location;
                            worksheet.Cells["A4"].Value = "DATE: " + list.DateFrom.ToString("MMMM dd, yyyy") + " - " + list.DateTo.ToString("MMMM dd, yyyy");
                            worksheet.Cells["B5"].Value = list.Description;
                            worksheet.Cells["B6"].Value = list.Category;
                            worksheet.Cells["B7"].Value = list.ProdCode;
                            int cellNumber = 9;
                            foreach (var trans in list.ListTransactions)
                            {
                                worksheet.Cells["A" + cellNumber].Value = trans.Date.ToString("MMMM dd, yyyy");
                                worksheet.Cells["B" + cellNumber].Value = trans.RefNo;
                                worksheet.Cells["C" + cellNumber].Value = trans.Transaction;
                                worksheet.Cells["D" + cellNumber].Value = trans.Case;
                                worksheet.Cells["E" + cellNumber].Value = trans.Pack;
                                worksheet.Cells["F" + cellNumber].Value = trans.Piece;
                                worksheet.Cells["G" + cellNumber].Value = trans.CaseBalance;
                                worksheet.Cells["H" + cellNumber].Value = trans.PackBalance;
                                worksheet.Cells["I" + cellNumber].Value = trans.PieceBalance;
                                worksheet.Cells["J" + cellNumber].Value = trans.Particular;
                                cellNumber++;
                            }

                            worksheetNum++;
                        }
                        package.Workbook.Worksheets.Delete("Sheet1");
                        worksheet.View.PageLayoutView = false;

                        package.Save();
                    }
                }

            }

            /// <summary>
            /// The export products.
            /// </summary>
            /// <param name="prodLists">
            /// The prod lists.
            /// </param>
            /// <param name="location">
            /// The location.
            /// </param>
            /// <param name="principal">
            /// The principal.
            /// </param>
            /// <param name="category">
            /// The category.
            /// </param>
            public static void ExportProducts(List<Product> prodLists, string location, string principal, string category)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = DateTime.Now.ToString("MMMMM dd yyyy") + " - Product Inventory Report"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx"; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    string filename = dlg.FileName;
                    FileInfo templateFile = new FileInfo(@"Product Template.xlsx");
                    FileInfo newFile = new FileInfo(filename);
                    if (newFile.Exists)
                    {
                        try
                        {
                            newFile.Delete();
                            newFile = new FileInfo(filename);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(
                                "The file is currently being used, close the file or choose a different name.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }
                    }

                    var config = StockbookWindows.OpenConfig();
                    var currencyStyle = string.Empty;
                    var currency = string.Empty;
                    switch (config.Currency)
                    {
                        case "PHP - ₱":
                            currencyStyle = "₱#,###,##0.00";
                            currency = "Peso";
                            break;
                        case "USD - $":
                            currencyStyle = "$#,###,##0.00";
                            currency = "Dollar";
                            break;
                        case "YEN - ¥":
                            currencyStyle = "¥#,###,##0.00";
                            currency = "Yen";
                            break;
                    }


                        using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        worksheet.Cells["A1"].Value = config.CompanyName;
                        worksheet.Cells["A3"].Value = "As of " + DateTime.Now.ToString("MMMM dd, yyyy");
                        worksheet.Cells["A4"].Value = "Location: " + location;
                        worksheet.Cells["A5"].Value = "Principal: " + principal;
                        worksheet.Cells["A6"].Value = "Category: " + category;
                        worksheet.Cells["G8"].Value = currency + " Value";
                        int i = 10;
                        decimal grandTotal = 0;
                        decimal subTotal = 0;
                        string tempCategory = string.Empty;
                        var categories = prodLists.Select(q => q.Category).Distinct().OrderBy(q => q);
                        foreach (var cat in categories)
                        {
                            foreach (var prod in prodLists.Where(q => q.Category == cat))
                            {
                                var caseVal = prod.CaseValue * prod.CaseBalance;
                                var packVal = prod.PackValue * prod.PackBalance;
                                var pieceVal = prod.PieceValue * prod.PieceBalance;
                                if (tempCategory == string.Empty || tempCategory != prod.Category)
                                {
                                    worksheet.Cells["A" + i].Value = prod.Category;
                                    tempCategory = prod.Category;
                                }
                                worksheet.Cells["B" + i].Value = prod.Name;
                                worksheet.Cells["C" + i].Value = prod.ProdCode;
                                worksheet.Cells["D" + i].Value = prod.CaseBalance;
                                worksheet.Cells["E" + i].Value = prod.PackBalance;
                                worksheet.Cells["F" + i].Value = prod.PieceBalance;
                                worksheet.Cells["G" + i].Value = caseVal;
                                worksheet.Cells["H" + i].Value = packVal;
                                worksheet.Cells["I" + i].Value = pieceVal;
                                worksheet.Cells["J" + i].Value = caseVal + packVal + pieceVal;
                                subTotal += caseVal + packVal + pieceVal;
                                i++;
                            }
                            worksheet.Cells["I" + i].Value = "Subtotal:";
                            worksheet.Cells["J" + i].Value = subTotal;

                            i++;
                            grandTotal += subTotal;
                            subTotal = 0;
                            tempCategory = cat;
                        }
                        i++;
                        worksheet.Cells["I" + i].Value = "Grandtotal:";
                        worksheet.Cells["J" + i].Value = grandTotal;
                        worksheet.Cells["J" + i].Style.Font.Bold = true;
                        worksheet.Cells["J" + i].Style.Font.Size = 16;

                        worksheet.Cells["J" + 10 + ":J" + i].Style.Numberformat.Format = currencyStyle;

                        worksheet.View.PageLayoutView = false;
                        package.Save();
                    }
                }
            }

        }
    }
}