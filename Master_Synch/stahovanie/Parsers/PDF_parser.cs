using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

namespace stahovanie.Parsers
{
    /// <summary>
    /// Class PDF parser
    /// </summary>
    public class PDF_parser
    {
        #region PDF_Parser

        /// <summary>
        /// Metóda ktorá parsuje údaje PDF súboru pre zdravotnú poisťovňu UNION
        /// </summary>
        /// <param name="file">Cesta ku zložke so súborom</param>
        /// <returns>true, ak úspešne zbehla, inak false</returns>
        public bool PDF_Parser_UNION(string[] file)
        {
            StringBuilder text = new StringBuilder();
            using (PdfReader pdf = new PdfReader(file.FirstOrDefault()))
            {
                for (int page = 1; page <= pdf.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdf, page, strategy);

                    // enkódujeme do čiateteľného formátu a pokračujeme v parsovani údajov
                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    string[] data_After_Split = Regex.Split(currentText, "\n");

                    // preskočíme prvý riadok je pre nás nepodstatný(niesú tam relevantné dáta)
                    foreach (var item in data_After_Split.Skip(1))
                    {
                        // nahradíme všetky čudné symboly string.empty
                        var list = item.Replace("-", string.Empty).Replace(" .", string.Empty).Replace("+", string.Empty).Split(' ').Where(s => !string.IsNullOrWhiteSpace(s));
                        var meno_1 = string.Empty;
                        var meno_2 = string.Empty;
                        var meno = string.Empty;

                        // kontrola či je pred menom titul ak áno zoberieme prvé 3 fieldy ináč prvé dva... ďalej pokračujeme v parsovaní jednotlivých fieldov
                        if (list.Take(1).FirstOrDefault().ToLower() == "mgr." || list.Take(1).FirstOrDefault().ToLower() == "ing." || list.Take(1).FirstOrDefault().ToLower() == "bc")
                        {
                            meno_1 = list.Take(1).FirstOrDefault();
                            meno_2 = list.Skip(1).Take(2).FirstOrDefault() + " " + list.Skip(2).Take(3).FirstOrDefault();
                        }
                        else
                        {
                            meno_1 = list.Take(1).FirstOrDefault() + " " + list.Skip(1).Take(2).FirstOrDefault();
                        }

                        meno = meno_1 + " " + meno_2;

                        var mesto_1 = string.Empty;
                        if (list.Skip(2).Take(3).FirstOrDefault() != null && list.Skip(2).Take(3).FirstOrDefault().Length > 1 && !Char.IsNumber(list.Skip(2).Take(3).FirstOrDefault()[0]))
                        {
                            mesto_1 = Char.IsUpper(list.Skip(2).Take(3).FirstOrDefault()[1]) ? string.Empty : list.Skip(2).Take(3).FirstOrDefault();
                        }

                        var mesto_2 = string.Empty;
                        if (list.Skip(3).Take(4).FirstOrDefault() != null && list.Skip(3).Take(4).FirstOrDefault().Length >= 2 && !Char.IsNumber(list.Skip(3).Take(4).FirstOrDefault()[0]))
                        {
                            mesto_2 = Char.IsUpper(list.Skip(3).Take(4).FirstOrDefault()[1]) ? string.Empty : list.Skip(3).Take(4).FirstOrDefault();
                        }

                        var mesto_3 = string.Empty;
                        if (list.Skip(4).Take(5).FirstOrDefault() != null && !Char.IsNumber(list.Skip(4).Take(5).FirstOrDefault()[0]) && list.Skip(4).Take(5).FirstOrDefault().Length > 1)
                        {
                            mesto_3 = Char.IsUpper(list.Skip(4).Take(5).FirstOrDefault()[1]) ? string.Empty : list.Skip(4).Take(5).FirstOrDefault();
                        }

                        var mesto_4 = string.Empty;
                        if (list.Skip(5).Take(6).FirstOrDefault() != null)
                        {
                            if (!Char.IsNumber(list.Skip(5).Take(6).FirstOrDefault()[0]) && list.Skip(5).Take(6).FirstOrDefault().Length > 1)
                            {
                                mesto_4 = Char.IsUpper(list.Skip(5).Take(6).FirstOrDefault()[1]) ? string.Empty : list.Skip(5).Take(6).FirstOrDefault();
                            }
                        }

                        var mesto = mesto_1 + " " + mesto_2 + " " + mesto_3 + " " + mesto_4;

                        var ulica_1 = string.Empty;
                        var ulica_2 = string.Empty;
                        var ulica_3 = string.Empty;
                        var ulica_4 = string.Empty;

                        if (mesto_2 == string.Empty && list.Skip(3).Take(4).FirstOrDefault() != null && list.Skip(3).Take(4).FirstOrDefault().Length >= 3)
                        {
                            ulica_1 = list.Skip(3).Take(4).FirstOrDefault();
                            if (list.Skip(4).Take(5).FirstOrDefault() != null && list.Skip(4).Take(5).FirstOrDefault().Length > 1)
                            {
                                if (Char.IsNumber(list.Skip(4).Take(5).FirstOrDefault()[0]) || Char.IsUpper(list.Skip(4).Take(5).FirstOrDefault()[1]))
                                {
                                    ulica_2 = list.Skip(4).Take(5).FirstOrDefault();
                                    if (list.Skip(5).Take(6).FirstOrDefault() != null)
                                    {
                                        if (Char.IsNumber(list.Skip(5).Take(6).FirstOrDefault()[0]) && list.Skip(5).Take(6).FirstOrDefault().Length < 4)
                                        {
                                            ulica_3 = list.Skip(5).Take(6).FirstOrDefault();
                                        }
                                    }
                                }
                            }
                        }
                        else if (mesto_3 == string.Empty && mesto_2 != string.Empty && list.Skip(4).Take(5).FirstOrDefault() != null)
                        {
                            ulica_2 = list.Skip(4).Take(5).FirstOrDefault();
                            if (list.Skip(5).Take(6).FirstOrDefault() != null)
                            {
                                if (list.Skip(5).Take(6).FirstOrDefault().Length > 1)
                                {
                                    if (Char.IsUpper(list.Skip(5).Take(6).FirstOrDefault()[1]) && !Char.IsNumber(list.Skip(5).Take(6).FirstOrDefault()[1]))
                                    {
                                        ulica_3 = list.Skip(5).Take(6).FirstOrDefault();
                                        ulica_4 = list.Skip(6).Take(7).FirstOrDefault();
                                    }
                                    else
                                    {
                                        ulica_3 = string.Empty;
                                        ulica_4 = list.Skip(5).Take(6).FirstOrDefault();
                                    }
                                }
                                else
                                {
                                    ulica_3 = string.Empty;
                                    ulica_4 = list.Skip(5).Take(6).FirstOrDefault();
                                }
                            }
                        }
                        else if (mesto_2 == string.Empty && mesto_3 == string.Empty)
                        {
                            if (list.Skip(4).Take(5).FirstOrDefault() != null && list.Skip(4).Take(5).FirstOrDefault().Length > 1 && list.Skip(5).Take(6).FirstOrDefault().Length > 1)
                            {
                                if (Char.IsUpper(list.Skip(4).Take(5).FirstOrDefault()[1]) && Char.IsLower(list.Skip(5).Take(6).FirstOrDefault()[1]))
                                {
                                    ulica_1 = list.Skip(6).Take(7).FirstOrDefault();
                                    if (Char.IsUpper(list.Skip(7).Take(8).FirstOrDefault()[1]))
                                    {
                                        ulica_2 = list.Skip(7).Take(8).FirstOrDefault();
                                        ulica_3 = list.Skip(8).Take(9).FirstOrDefault();
                                    }
                                    else
                                    {
                                        ulica_2 = list.Skip(7).Take(8).FirstOrDefault();
                                    }
                                }
                                else
                                {
                                    ulica_1 = list.Skip(3).Take(4).FirstOrDefault();
                                    ulica_2 = list.Skip(4).Take(5).FirstOrDefault();
                                }
                            }
                            else
                            {
                                ulica_1 = list.Skip(3).Take(4).FirstOrDefault();
                                if (list.Skip(4).Take(5).FirstOrDefault() != null)
                                {
                                    ulica_2 = list.Skip(4).Take(5).FirstOrDefault();
                                }
                            }
                        }

                        var ulica = ulica_1 + " " + ulica_2 + " " + ulica_3 + " " + ulica_4;

                        var PSC = string.Empty;
                        if (list.Skip(4).Take(5).FirstOrDefault() != null && Char.IsNumber(list.Skip(4).Take(5).FirstOrDefault()[0]) && (list.Skip(4).Take(5).FirstOrDefault().Length == 4 || list.Skip(4).Take(5).FirstOrDefault().Length == 5) && !list.Skip(4).Take(5).FirstOrDefault().Contains('/'))
                        {
                            PSC = list.Skip(4).Take(5).FirstOrDefault();
                        }
                        else if (list.Skip(5).Take(6).FirstOrDefault() != null && (Char.IsNumber(list.Skip(5).Take(6).FirstOrDefault()[0]) && (list.Skip(5).Take(6).FirstOrDefault().Length == 4 || list.Skip(5).Take(6).FirstOrDefault().Length == 5) && !list.Skip(5).Take(6).FirstOrDefault().Contains('/')))
                        {
                            PSC = list.Skip(5).Take(6).FirstOrDefault();
                        }
                        else if (list.Skip(6).Take(7).FirstOrDefault() != null && (Char.IsNumber(list.Skip(6).Take(7).FirstOrDefault()[0]) && (list.Skip(6).Take(7).FirstOrDefault().Length == 4 || list.Skip(6).Take(7).FirstOrDefault().Length == 5) && !list.Skip(6).Take(7).FirstOrDefault().Contains('/')))
                        {
                            PSC = list.Skip(6).Take(7).FirstOrDefault();
                        }
                        else if (list.Skip(7).Take(8).FirstOrDefault() != null && Char.IsNumber(list.Skip(7).Take(8).FirstOrDefault()[0]) && (list.Skip(7).Take(8).FirstOrDefault().Length == 4 || list.Skip(7).Take(8).FirstOrDefault().Length == 5) && !list.Skip(7).Take(8).FirstOrDefault().Contains('/'))
                        {
                            PSC = list.Skip(7).Take(8).FirstOrDefault();
                        }
                        else if (list.Skip(8).Take(9).FirstOrDefault() != null && Char.IsNumber(list.Skip(8).Take(9).FirstOrDefault()[0]) && (list.Skip(8).Take(9).FirstOrDefault().Length == 4 || list.Skip(8).Take(9).FirstOrDefault().Length == 5) && !list.Skip(8).Take(9).FirstOrDefault().Contains('/'))
                        {
                            PSC = list.Skip(8).Take(9).FirstOrDefault();
                        }
                        else if (list.Skip(9).Take(10).FirstOrDefault() != null && Char.IsNumber(list.Skip(9).Take(10).FirstOrDefault()[0]) && (list.Skip(9).Take(10).FirstOrDefault().Length == 4 || list.Skip(9).Take(10).FirstOrDefault().Length == 5) && !list.Skip(9).Take(10).FirstOrDefault().Contains('/'))
                        {
                            PSC = list.Skip(9).Take(10).FirstOrDefault();
                        }
                        else if (list.Skip(10).Take(11).FirstOrDefault() != null && Char.IsNumber(list.Skip(10).Take(11).FirstOrDefault()[0]) && (list.Skip(10).Take(11).FirstOrDefault().Length == 4 || list.Skip(10).Take(11).FirstOrDefault().Length == 5) && !list.Skip(10).Take(11).FirstOrDefault().Contains('/'))
                        {
                            PSC = list.Skip(10).Take(11).FirstOrDefault();
                        }

                        var ICO = string.Empty;

                        if (list.Skip(4).Take(5).FirstOrDefault() != null && list.Skip(4).Take(5).FirstOrDefault().Length == 8 && list.Skip(4).Take(5).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(4).Take(5).FirstOrDefault().Length == 8 || list.Skip(4).Take(5).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(4).Take(5).FirstOrDefault();
                            }
                        }
                        else if (list.Skip(5).Take(6).FirstOrDefault() != null && list.Skip(5).Take(6).FirstOrDefault().Length == 8 && list.Skip(5).Take(6).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(5).Take(6).FirstOrDefault().Length == 8 || list.Skip(5).Take(6).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(5).Take(6).FirstOrDefault();
                            }
                        }
                        else if (list.Skip(6).Take(7).FirstOrDefault() != null && list.Skip(6).Take(7).FirstOrDefault().Length == 8 && list.Skip(6).Take(7).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(6).Take(7).FirstOrDefault().Length == 8 || list.Skip(6).Take(7).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(6).Take(7).FirstOrDefault();
                            }
                        }
                        else if (list.Skip(7).Take(8).FirstOrDefault() != null && list.Skip(7).Take(8).FirstOrDefault().Length == 8 && list.Skip(7).Take(8).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(7).Take(8).FirstOrDefault().Length == 8 || list.Skip(7).Take(8).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(7).Take(8).FirstOrDefault();
                            }
                        }
                        else if (list.Skip(8).Take(9).FirstOrDefault() != null && list.Skip(8).Take(9).FirstOrDefault().Length == 8 && list.Skip(8).Take(9).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(8).Take(9).FirstOrDefault().Length == 8 || list.Skip(8).Take(9).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(8).Take(9).FirstOrDefault();
                            }
                        }
                        else if (list.Skip(9).Take(10).FirstOrDefault() != null && list.Skip(9).Take(10).FirstOrDefault().Length == 8 && list.Skip(9).Take(10).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(9).Take(10).FirstOrDefault().Length == 8 || list.Skip(9).Take(10).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(9).Take(10).FirstOrDefault();
                            }
                        }
                        else if (list.Skip(10).Take(11).FirstOrDefault() != null && list.Skip(10).Take(11).FirstOrDefault().Length == 8 && list.Skip(10).Take(11).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(10).Take(11).FirstOrDefault().Length == 8 || list.Skip(10).Take(11).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(10).Take(11).FirstOrDefault();
                            }
                        }
                        else if (list.Skip(11).Take(12).FirstOrDefault() != null && list.Skip(11).Take(12).FirstOrDefault().Length == 8 && list.Skip(11).Take(12).FirstOrDefault().All(Char.IsNumber))
                        {
                            if (list.Skip(11).Take(12).FirstOrDefault().Length == 8 || list.Skip(11).Take(12).FirstOrDefault() == string.Empty)
                            {
                                ICO = list.Skip(11).Take(12).FirstOrDefault();
                            }
                        }
                        else
                        {
                            ICO = string.Empty;
                        }

                        var DlznaSuma = string.Empty;
                        if (list.Skip(4).Take(5).FirstOrDefault() != null && list.Skip(4).Take(5).FirstOrDefault().Length > 1 && list.Skip(4).Take(5).FirstOrDefault().Contains(",") && !list.Skip(4).Take(5).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(4).Take(5).FirstOrDefault()[1]))
                        {
                            DlznaSuma = list.Skip(4).Take(5).FirstOrDefault();
                        }
                        else if (list.Skip(5).Take(6).FirstOrDefault() != null && list.Skip(5).Take(6).FirstOrDefault().Length > 1 && (list.Skip(5).Take(6).FirstOrDefault().Contains(",") && !list.Skip(5).Take(6).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(5).Take(6).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(5).Take(6).FirstOrDefault();
                        }
                        else if (list.Skip(6).Take(7).FirstOrDefault() != null && list.Skip(6).Take(7).FirstOrDefault().Length > 1 && (list.Skip(6).Take(7).FirstOrDefault().Contains(",") && !list.Skip(6).Take(7).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(6).Take(7).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(6).Take(7).FirstOrDefault();
                        }
                        else if (list.Skip(7).Take(8).FirstOrDefault() != null && list.Skip(7).Take(8).FirstOrDefault().Length > 1 && (list.Skip(7).Take(8).FirstOrDefault().Contains(",") && !list.Skip(7).Take(8).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(7).Take(8).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(7).Take(8).FirstOrDefault();
                        }
                        else if (list.Skip(8).Take(9).FirstOrDefault() != null && list.Skip(8).Take(9).FirstOrDefault().Length > 1 && (list.Skip(8).Take(9).FirstOrDefault().Contains(",") && !list.Skip(8).Take(9).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(8).Take(9).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(8).Take(9).FirstOrDefault();
                        }
                        else if (list.Skip(9).Take(10).FirstOrDefault() != null && list.Skip(9).Take(10).FirstOrDefault().Length > 1 && (list.Skip(9).Take(10).FirstOrDefault().Contains(",") && !list.Skip(9).Take(10).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(9).Take(10).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(9).Take(10).FirstOrDefault();
                        }
                        else if (list.Skip(10).Take(11).FirstOrDefault() != null && list.Skip(10).Take(11).FirstOrDefault().Length > 1 && (list.Skip(10).Take(11).FirstOrDefault().Contains(",") && !list.Skip(10).Take(11).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(10).Take(11).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(10).Take(11).FirstOrDefault();
                        }
                        else if (list.Skip(11).Take(12).FirstOrDefault() != null && list.Skip(11).Take(12).FirstOrDefault().Length > 1 && (list.Skip(11).Take(12).FirstOrDefault().Contains(",") && list.Skip(11).Take(12).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(11).Take(12).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(11).Take(12).FirstOrDefault();
                        }
                        else if (list.Skip(12).Take(13).FirstOrDefault() != null && list.Skip(12).Take(13).FirstOrDefault().Length > 1 && (list.Skip(12).Take(13).FirstOrDefault().Contains(",") && list.Skip(12).Take(13).FirstOrDefault().Contains("/") && Char.IsNumber(list.Skip(12).Take(13).FirstOrDefault()[1])))
                        {
                            DlznaSuma = list.Skip(12).Take(13).FirstOrDefault();
                        }

                        var typ_zs = string.Empty;

                        if (list.Skip(4).Take(5).FirstOrDefault() != null && list.Skip(4).Take(5).FirstOrDefault().Contains("Á") && (list.Skip(4).Take(5).FirstOrDefault().Length == 4 || list.Skip(4).Take(5).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(4).Take(5).FirstOrDefault();
                        }
                        else if (list.Skip(5).Take(6).FirstOrDefault() != null && list.Skip(5).Take(6).FirstOrDefault().Contains("Á") && (list.Skip(5).Take(6).FirstOrDefault().Length == 4 || list.Skip(5).Take(6).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(5).Take(6).FirstOrDefault();
                        }
                        else if (list.Skip(6).Take(7).FirstOrDefault() != null && list.Skip(6).Take(7).FirstOrDefault().Contains("Á") && (list.Skip(6).Take(7).FirstOrDefault().Length == 4 || list.Skip(6).Take(7).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(6).Take(7).FirstOrDefault();
                        }
                        else if (list.Skip(7).Take(8).FirstOrDefault() != null && list.Skip(7).Take(8).FirstOrDefault().Contains("Á") && (list.Skip(7).Take(8).FirstOrDefault().Length == 4 || list.Skip(7).Take(8).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(7).Take(8).FirstOrDefault();
                        }
                        else if (list.Skip(8).Take(9).FirstOrDefault() != null && list.Skip(8).Take(9).FirstOrDefault().Contains("Á") && (list.Skip(8).Take(9).FirstOrDefault().Length == 4 || list.Skip(8).Take(9).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(8).Take(9).FirstOrDefault();
                        }
                        else if (list.Skip(9).Take(10).FirstOrDefault() != null && list.Skip(9).Take(10).FirstOrDefault().Contains("Á") && (list.Skip(9).Take(10).FirstOrDefault().Length == 4 || list.Skip(9).Take(10).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(9).Take(10).FirstOrDefault();
                        }
                        else if (list.Skip(10).Take(11).FirstOrDefault() != null && list.Skip(10).Take(11).FirstOrDefault().Contains("Á") && (list.Skip(10).Take(11).FirstOrDefault().Length == 4 || list.Skip(10).Take(11).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(10).Take(11).FirstOrDefault();
                        }
                        else if (list.Skip(11).Take(12).FirstOrDefault() != null && list.Skip(11).Take(12).FirstOrDefault().Contains("Á") && (list.Skip(11).Take(12).FirstOrDefault().Length == 4 || list.Skip(11).Take(12).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(11).Take(12).FirstOrDefault();
                        }
                        else if (list.Skip(12).Take(13).FirstOrDefault() != null && list.Skip(12).Take(13).FirstOrDefault().Contains("Á") && (list.Skip(12).Take(13).FirstOrDefault().Length == 4 || list.Skip(12).Take(13).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(12).Take(13).FirstOrDefault();
                        }
                        else if (list.Skip(13).Take(14).FirstOrDefault() != null && list.Skip(13).Take(14).FirstOrDefault().Contains("Á") && (list.Skip(13).Take(14).FirstOrDefault().Length == 4 || list.Skip(13).Take(14).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(13).Take(14).FirstOrDefault();
                        }
                        else if (list.Skip(14).Take(15).FirstOrDefault() != null && list.Skip(14).Take(15).FirstOrDefault().Contains("Á") && (list.Skip(14).Take(15).FirstOrDefault().Length == 4 || list.Skip(14).Take(15).FirstOrDefault().Length == 10))
                        {
                            typ_zs = list.Skip(14).Take(15).FirstOrDefault();
                        }

                        if (DlznaSuma == string.Empty || !Char.IsNumber(DlznaSuma[1]))
                        {
                            DlznaSuma = "0";
                        }

                        Save_Data_EF.Save_Data_Union(meno, mesto, ulica, PSC, ICO, DlznaSuma, typ_zs);
                    }
                }

                pdf.Close();
            }

            return true;
        }
        #endregion
    }
}
