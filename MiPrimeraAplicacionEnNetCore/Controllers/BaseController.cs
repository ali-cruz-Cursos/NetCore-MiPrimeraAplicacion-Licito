using Microsoft.AspNetCore.Mvc;
using MiPrimeraAplicacionEnNetCore.Clases;
using MiPrimeraAplicacionEnNetCore.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using cm = System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout.Element;

using iText.Layout;
using Syncfusion.DocIO.DLS;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class BaseController : Controller
    {
        // Creando metodo que genera array de bytes
        public byte[] exportarExcelDatos<T>(string[] nombrePropiedades, List<T> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage ep = new ExcelPackage())
                {
                    ep.Workbook.Worksheets.Add("Hoja1");

                    ExcelWorksheet ew = ep.Workbook.Worksheets[0];

                    Dictionary<string, string> diccionario = cm.TypeDescriptor.GetProperties(typeof(T))
                        .Cast<cm.PropertyDescriptor>()
                        .ToDictionary(p => p.Name, p => p.DisplayName);

                    if (nombrePropiedades != null && nombrePropiedades.Length > 0 && lista != null )
                    {
                        for (int i = 0; i < nombrePropiedades.Length; i++)
                        {
                            ew.Cells[1, i + 1].Value = diccionario[nombrePropiedades[i]];
                            ew.Column(i + 1).Width = 50;
                        }

                        int fila = 2;
                        int columna = 1;

                        foreach (object item in lista)
                        {
                            columna = 1;
                            foreach (string propiedad in nombrePropiedades)
                            {
                                ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad)
                                                .GetValue(item).ToString();
                                columna++;
                            }
                            fila++;
                        }
                    }

                    ep.SaveAs(ms);

                    byte[] buffer = ms.ToArray();
                    return buffer;
                }
            }
        }

        public byte[] exportarPDFDatos<T>(string[] nombrePropiedades, List<T> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Sacar los display name
                Dictionary<string, string> diccionario = cm.TypeDescriptor
                        .GetProperties(typeof(T))
                        .Cast<cm.PropertyDescriptor>()
                        .ToDictionary(p => p.Name, p => p.DisplayName);

                PdfWriter writer = new PdfWriter(ms);
                using (var pdfDoc = new PdfDocument(writer))
                {
                    try
                    {
                        Document doc = new Document(pdfDoc);
                        Paragraph c1 = new Paragraph("Reporte en PDF");
                        
                        c1.SetFontSize(20);
                        c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);                        
                        doc.Add(c1);

                        if (nombrePropiedades != null && nombrePropiedades.Length > 0)
                        {
                            Table table = new Table(nombrePropiedades.Length);
                            Cell cell;

                            for (int i = 0; i < nombrePropiedades.Length; i++)
                            {
                                cell = new Cell();
                                cell.Add(new Paragraph(diccionario[nombrePropiedades[i]]));
                                table.AddHeaderCell(cell);
                            }

                            foreach (object item in lista)
                            {
                                foreach (string propiedad in nombrePropiedades)
                                {
                                    cell = new Cell();
                                    cell.Add(new Paragraph(item
                                        .GetType()
                                        .GetProperty(propiedad)
                                        .GetValue(item)
                                        .ToString()));
                                    table.AddCell(cell);
                                }
                            }

                            doc.Add(table);
                        }

                        doc.Close();
                        writer.Close();
                    } catch (Exception ex)
                    {

                    }

                }

                return ms.ToArray();
            }
        }

        public byte[] exportarDatosWord<T>(string[] nombrePropiedades, List<T> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WordDocument documentW = new WordDocument();
                WSection sectionW = documentW.AddSection() as WSection;
                sectionW.PageSetup.Margins.All = 72;
                sectionW.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);
                IWParagraph paragraph = sectionW.AddParagraph();
                paragraph.ApplyStyle("Normal");
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                WTextRange textRange = paragraph.AppendText("Reporte en Word") as WTextRange;
                textRange.CharacterFormat.FontSize = 20f;
                textRange.CharacterFormat.FontName = "Calibri";
                textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Blue;

                if (nombrePropiedades != null && nombrePropiedades.Length > 0)
                {                    
                    IWTable table = sectionW.AddTable();
                    int numeroColumnas = nombrePropiedades.Length;
                    int nFilas = lista.Count();
                    table.ResetCells(nFilas + 1, numeroColumnas);
                    Dictionary<string, string> diccionario = cm.TypeDescriptor.GetProperties(typeof(T))
                        .Cast<cm.PropertyDescriptor>()
                        .ToDictionary(p => p.Name, p => p.DisplayName);


                    for (int i = 0; i < numeroColumnas; i++)
                    {
                        table[0, i].AddParagraph().AppendText(diccionario[nombrePropiedades[i]]);
                    }

                    int fila = 1;
                    int col = 0;

                    foreach (object item in lista)
                    {
                        col = 0;
                        foreach (string propiedad in nombrePropiedades)
                        {
                            table[fila, col].AddParagraph().AppendText(
                                item.GetType().GetProperty(propiedad).GetValue(item).ToString()
                                );
                            col++;
                        }

                        fila++;
                    }

                }
                documentW.Save(ms, Syncfusion.DocIO.FormatType.Docx);
                return ms.ToArray();
            }
        }


    }
}
