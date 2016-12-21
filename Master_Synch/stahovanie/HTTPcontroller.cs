using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using stahovanie.Parsers;
using System.Xml;
using System.Text.RegularExpressions;

namespace stahovanie
{
    /// <summary>
    /// Http controler, sťahovanie súborov do directory odkiaľ je spúštaná aplikácia
    /// </summary>
    public class HTTPcontroller
    {
        #region Poistovne

        /// <summary>
        /// metóda podľa ktorej sa začne sťahovať súbor zo zvolenej stránky
        /// </summary>
        /// <param name="poistovna">
        /// string poistovne/KL
        /// </param>
        public void Response(string poistovna)
        {
            WebClient webClient = new WebClient();
            CSV_parser CSV_parser = new CSV_parser();
            XML_parser XML_parser = new XML_parser();
            PDF_parser PDF_parser = new PDF_parser();

            if (poistovna == "Sociálna poisťovňa")
            {
                // vyparsujeme html stranku aby sme zistili najaktuálnejšie dáta(dátum), je to potrebné aby sme získali aktuálne dáta
                string html = webClient.DownloadString("http://www.socpoist.sk/zoznam-dlznikov-emw/487s");
                var match = Regex.Matches(html, "<p style=\"text-align: center;\">\\s*(.+?)\\s*</p>").Cast<Match>().Select(m => m.Value).ToArray();
                var parse_csv_zip = match.Skip(1).Take(2).FirstOrDefault();
                var url_string = parse_csv_zip.Split('>', '<').Skip(4).Take(5).FirstOrDefault();

                // vyparsovaný string doplníme do url pre sťiahnutie súboru
                var urlSoc = "http://www.socpoist.sk/index/open_file.php?file=dlznici/" + url_string;
                var safePathSoc = Directory.GetCurrentDirectory() + "\\SP.zip";
                var unzipPathSoc = Directory.GetCurrentDirectory() + "\\Socialna_Poistovna";

                try
                {
                    // download suboru 
                    webClient.DownloadFile(urlSoc, safePathSoc);

                    // extrahovanie zložky 
                    ZipFile.ExtractToDirectory(safePathSoc, unzipPathSoc);

                    // posielanie parametrov do metódy ktorá parsuje údaje z textu
                    // v prípade že metóda vráti true vymažeme stiahnute súbory a vypíše messagebox
                    if (CSV_parser.CSV_Parser_SP(Directory.GetFiles(unzipPathSoc)))
                    {
                        File.Delete(safePathSoc);
                        Directory.Delete(unzipPathSoc, true);
                        MessageBox.Show("Sťahovanie bolo dokončené");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nieje možné sťiahnuť súbor " + ex.Message + "!!");
                }
            }
            else if (poistovna == "Daňový úrad")
            {
                // pri daňovom úrade mame direct link na sťiahnutie súboru preto nieje potrebné parsovat html stránku
                var urlDan = "http://edane.drsr.sk/report/ds_dphz.zip";
                var safePathDan = Directory.GetCurrentDirectory() + "\\Dan.zip";
                var unzipPathDan = Directory.GetCurrentDirectory() + "\\Danovy_Urad";

                try
                {
                    // download suboru 
                    webClient.DownloadFile(urlDan, safePathDan);

                    // extrahovanie zložky 
                    ZipFile.ExtractToDirectory(safePathDan, unzipPathDan);

                    // posielanie parametrov do metódy ktorá parsuje údaje z XML súboru
                    // v pripade že metóda vráti true vymažeme stiahnute súbory 
                    if (XML_parser.XML_Parser_DU(Directory.GetFiles(unzipPathDan)))
                    {
                        File.Delete(safePathDan);
                        Directory.Delete(unzipPathDan, true);
                        MessageBox.Show("Sťahovanie bolo dokončené");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nieje možné sťiahnuť súbor " + ex.Message + "!!");
                }
            }
            else if (poistovna == "Zdravotná poisťovňa VŠZP")
            {
                // tu je opať potrebné parsovať html z dôvodu zistenia aktálnych dát
                string html = webClient.DownloadString("https://www.vszp.sk/platitelia/platenie-poistneho/zoznam-dlznikov.html");
                var match = Regex.Matches(html, @"<td>\s*(.+?)\s*</td>").Cast<Match>().Select(m => m.Value).ToArray();
                var parse_csv_zip = match.FirstOrDefault();
                var url_string = Regex.Split(parse_csv_zip, "\"").Skip(5).Take(6).FirstOrDefault();

                // dáata doplníme do url linku na sťahovanie súboru
                var urlVsZP = "https://www.vszp.sk" + url_string;
                var safePathVsZP = Directory.GetCurrentDirectory() + "\\VsZP.zip";
                var unzipPathVsZP = Directory.GetCurrentDirectory() + "\\VsZP";

                try
                {
                    // download suboru 
                    webClient.DownloadFile(urlVsZP, safePathVsZP);

                    // extrahovanie zložky 
                    ZipFile.ExtractToDirectory(safePathVsZP, unzipPathVsZP);

                    // posielanie parametrov do metódy ktorá parsuje údaje z textu
                    // v prípade že metóda vráti true vymažeme stiahnuté súbory 
                    if (CSV_parser.CSV_Parser_VsZP(Directory.GetFiles(unzipPathVsZP)))
                    {
                        File.Delete(safePathVsZP);
                        Directory.Delete(unzipPathVsZP, true);
                        MessageBox.Show("Sťahovanie bolo dokončené");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nieje možné sťiahnuť súbor " + ex.Message + "!!");
                }
            }
            else if (poistovna == "Zdravotná poisťovňa Dôvera")
            {
            }
            else if (poistovna == "Zdravotná poisťovňa Union")
            {
                // direct link na stiahnutuie pdf súboru s aktuálnymi dátami
                var urlUnion = "https://www.union.sk/documents/51150/Zoznam_dlznikov_FO";
                var new_Directory = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Union");
                var safePathUnion = new_Directory + "\\Union.pdf";

                try
                {
                    // download suboru 
                    webClient.DownloadFile(urlUnion, safePathUnion);

                    // posielanie parametrov do metódy ktorá parsuje údaje z pdf súboru
                    // v prípade že metóda vráti true vymažeme stiahnuté súbory 
                    if (PDF_parser.PDF_Parser_UNION(Directory.GetFiles(new_Directory.FullName)))
                    {
                        File.Delete(new_Directory.Name);
                        MessageBox.Show("Sťahovanie bolo dokončené");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nieje možné sťiahnuť súbor " + ex.Message + "!!");
                }
            }
            else if (poistovna == "Kurzový lístok NBS")
            {
                // xml pre denný kurzový lístok nesťahujeme pretože je priamo dostupný na webe
                // preto ho len prečítame a vyparsujeme priamo z webu
                var urlKL = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

                XmlTextReader reader = new XmlTextReader(urlKL);

                string xmlStr;
                using (var wc = new WebClient())
                {
                    // získame xml z webu ako string
                    xmlStr = wc.DownloadString(urlKL);
                }

                // ak nam metóda vráti true prebehlo to úspešne
                if (XML_parser.XML_Parser_KL(xmlStr))
                {
                    MessageBox.Show("Sťahovanie bolo dokončené");
                }
            }
        }
        #endregion
    }
}
