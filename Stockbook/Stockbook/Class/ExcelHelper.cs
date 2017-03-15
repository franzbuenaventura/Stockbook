using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stockbook.Model;
using OfficeOpenXml;
using Excel = Microsoft.Office.Interop.Excel;

namespace Stockbook.Class
{
    public static class ExcelHelper
    {
        #region Excel 
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
                finally
                {
                }
            }

            public void createHeaders(int row, int col, string htext, string cell1,
            string cell2, int mergeColumns, string b, bool font, int size, string
            fcolor, bool border = false)
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
                if (fcolor.Equals(""))
                {
                    workSheet_range.Font.Color = Excel.XlRgbColor.rgbWhite;
                }
                else
                {
                    workSheet_range.Font.Color = Excel.XlRgbColor.rgbBlack;
                }
            }

            public void addData(int row, int col, string data,
                string cell1, string cell2, string format)
            {
                worksheet.Cells[row, col] = data;
                workSheet_range = worksheet.get_Range(cell1, cell2);
                workSheet_range.Borders.Color = Excel.XlRgbColor.rgbBlack;
                workSheet_range.NumberFormat = format;
            }
        }
        //public void ExportAllStudentsToExcel()
        //{
        //    var studentList = GetAllStudentInformationList();
        //    CreateExcelDoc excell_app = new CreateExcelDoc();
        //    creates the main header
        //    excell_app.createHeaders(1, 1, "Children of Lourdes Academy", "A1", "A1", 2, "GRAY", true, 20, "n");
        //    creates subheaders
        //    excell_app.createHeaders(2, 1, "Name", "A2", "A2", 0, "GRAY", true, 14, "");
        //    excell_app.createHeaders(2, 2, "Year Level", "B2", "B2", 0, "GRAY", true, 14, "");
        //    excell_app.createHeaders(2, 3, "Contact Number", "C2", "C2", 0, "GRAY", true, 14, "");
        //    excell_app.createHeaders(2, 4, "Tuition Fee Balance", "D2", "D2", 0, "GRAY", true, 14, "");
        //    add Data to cells
        //    int rowData = 3;
        //    foreach (var student in studentList)
        //    {
        //        var tempCol = 1;
        //        excell_app.addData(rowData, tempCol, student.Name, "A" + tempCol, "A" + tempCol, "");
        //        tempCol++;
        //        excell_app.addData(rowData, tempCol, student.YearLevel, "B" + tempCol, "B" + tempCol, "");
        //        tempCol++;
        //        excell_app.addData(rowData, tempCol, student.ContactNumber, "C" + tempCol, "C" + tempCol, "");
        //        tempCol++;
        //        excell_app.addData(rowData, tempCol, student.TuitionFeeBalance.ToString(), "D" + tempCol, "D" + tempCol, "#,##0");
        //        rowData++;
        //    }
        //}

        public class ExcelInvoice
        {
            public static void ExportTransactionInvoice(TransactionOrder transOrder)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = transOrder.DateTransaction.ToString("MMMMM dd yyyy") + " - " + transOrder.RefNo; // Default file name
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
                        newFile.Delete();
                        newFile = new FileInfo(filename);
                    }
                    using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        worksheet.Cells["A2"].Value = "SALES INVOICE - REFERENCE NUMBER:" + transOrder.RefNo;
                        worksheet.Cells["A3"].Value = "DATE: " + transOrder.DateTransaction.ToString("MMMM dd, YYYY");
                        worksheet.Cells["A4"].Value = "SOLD TO: " + transOrder.Particular;
                        worksheet.Cells["A5"].Value = "ADDRESS: " + transOrder.ParticularAddress;
                        worksheet.Cells["A6"].Value = "Salesman: " + transOrder.SalesmanName;
                        worksheet.Cells["F5"].Value = "TERMS: " + transOrder.Terms;
                        worksheet.Cells["F6"].Value = "DISCOUNT: " + transOrder.DiscountPercentage + "%";

                        int i = 9;
                        decimal unitTotal = 0;
                        decimal billedTotal = 0;
                        string tempCategory = "";
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
                                worksheet.Cells["E" + i].Value = tempTotal;
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
                                worksheet.Cells["E" + i].Value = tempTotal;
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
                                worksheet.Cells["E" + i].Value = tempTotal;
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
                        i++;
                        worksheet.Cells["B" + i].Value = "Vatable Sales";
                        worksheet.Cells["C" + i].Value = billedTotal * (decimal)0.88;
                        worksheet.Cells["B" + (i + 1)].Value = "12% VAT";
                        worksheet.Cells["C" + (i + 1)].Value = billedTotal * (decimal)0.12;
                        worksheet.Cells["B" + (i + 2)].Value = "Total";
                        worksheet.Cells["C" + (i + 2)].Value = billedTotal;

                        worksheet.Cells["E" + i].Value = "Invoice Amt.:";
                        worksheet.Cells["F" + i].Value = unitTotal;
                        worksheet.Cells["E" + (i + 1)].Value = transOrder.DiscountPercentage + "% - Discount";
                        worksheet.Cells["F" + (i + 1)].Value = unitTotal * (transOrder.DiscountPercentage / 100);
                        worksheet.Cells["F" + (i + 2)].Value = billedTotal;

                        i += 6;
                        worksheet.Cells["A" + i].Value = "PREPARED BY/DATE";
                        worksheet.Cells["A" + (i + 2)].Value = "PRINT NAME & SIGNATURE";
                        worksheet.Cells["D" + i].Value = "DELIVERED BY/DATE";
                        worksheet.Cells["D" + (i + 2)].Value = "PRINT NAME & SIGNATURE";
                        i += 3;
                        worksheet.Cells["A" + i].Value = "CHECKED BY/DATE";
                        worksheet.Cells["A" + (i + 2)].Value = "PRINT NAME & SIGNATURE";
                        worksheet.Cells["D" + i].Value = "APPROVED BY";
                        worksheet.Cells["D" + (i + 2)].Value = "PRINT NAME & SIGNATURE";
                        worksheet.View.PageLayoutView = false;
                        package.Save();
                    }
                }
            }
            private static List<StockCard> OrderListToStockCard(List<TransactionOrder> transactionOrders, DateTime dateFrom, DateTime dateTo)
            {
                var stockCards = new List<StockCard>();
                List<Transaction> listProductsInTrans =
                    transactionOrders.SelectMany(q => q.Transactions).ToList();
                var listProd = listProductsInTrans.GroupBy(q => q.Product.Name).Select(s => s.First()).Select(q => q.Product).ToList();
                var tempProd = new Product();
                foreach (var prod in listProd)
                {
                    var db = new DbClass();

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
                        tempProd = DbClass.EtcHelper.BalanceCasePackPiece(prodTemp, tempProd, transOrder.TransactionType);
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
                        newFile.Delete();
                        newFile = new FileInfo(filename);
                    }
                    using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
                    {
                        var listStock = OrderListToStockCard(orderList, dateFrom, dateTo);
                        int worksheetNum = 1;
                        ExcelWorksheet worksheet;
                        package.Workbook.Worksheets.FirstOrDefault(q => q.Index == 1).Hidden = eWorkSheetHidden.VeryHidden;
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

                            worksheet.View.PageLayoutView = false;
                            worksheetNum++;
                        }
                        package.Save();
                    }
                }

            }


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
                        newFile.Delete();
                        newFile = new FileInfo(filename);
                    }
                    using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        worksheet.Cells["A3"].Value = "As of " + DateTime.Now.ToString("MMMM dd, yyyy");
                        worksheet.Cells["A4"].Value = "Location: " + location;
                        worksheet.Cells["A5"].Value = "Principal: " + principal;
                        worksheet.Cells["A6"].Value = "Category: " + category;
                        int i = 9;
                        decimal grandTotal = 0;
                        decimal subTotal = 0;
                        string tempCategory = "";
                        var categories = prodLists.Select(q => q.Category).Distinct().OrderBy(q => q);
                        foreach (var cat in categories)
                        {
                            foreach (var prod in prodLists.Where(q => q.Category == cat))
                            {
                                var caseVal = (prod.CaseValue * prod.CaseBalance);
                                var packVal = (prod.PackValue * prod.PackBalance);
                                var pieceVal = (prod.PieceValue * prod.PieceBalance);
                                if (tempCategory == "" || tempCategory != prod.Category)
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
                        worksheet.View.PageLayoutView = false;
                        package.Save();
                    }
                }
            }

        }

        #endregion

    }
}