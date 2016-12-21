using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using stahovanie;

namespace stahovanie
{
    /// <summary>
    /// Class XML parser
    /// </summary>
    public class XML_parser
    {
        #region DU_parsovanie

        /// <summary>
        /// Metóda ktorá parsuje údaje z xml súboru pre daňový úrad
        /// </summary>
        /// <param name="file">Cesta ku zložke so súborom</param>
        /// <returns>true, ak úspešne zbehla, inak false</returns>
        public bool XML_Parser_DU(string[] file)
        {
            XmlDocument xmlRead = new XmlDocument();
            if (file != null)
            {
                xmlRead.Load(file.FirstOrDefault());
                var poc = 0;
                foreach (XmlNode node in xmlRead.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode firstNode in node)
                    {
                        if (firstNode.FirstChild != null)
                        {
                            var Ic_Dph = firstNode["IC_DPH"] == null ? string.Empty : firstNode["IC_DPH"].InnerText;
                            var Meno = firstNode["NAZOV"] == null ? string.Empty : firstNode["NAZOV"].InnerText;
                            var Mesto = firstNode["OBEC"] == null ? string.Empty : firstNode["OBEC"].InnerText;
                            string PSC = firstNode["PSC"] == null ? string.Empty : firstNode["PSC"].InnerText;
                            var Adresa = firstNode["ADRESA"] == null ? string.Empty : firstNode["ADRESA"].InnerText;
                            int Rok_Porusenia = firstNode["ROK_PORUSENIA"] == null ? Convert.ToInt16(null) : Convert.ToInt16(firstNode["ROK_PORUSENIA"].InnerText);
                            DateTime Datum_Porusenia = firstNode["DAT_ZVEREJNENIA"] == null ? DateTime.UtcNow : Convert.ToDateTime(firstNode["DAT_ZVEREJNENIA"].InnerText);
                            int Ico = firstNode["ICO"] == null ? Convert.ToInt32(null) : Convert.ToInt32(firstNode["ICO"].InnerText);
                            poc++;
                            Save_Data_EF.SaveData_DU(Ic_Dph, Meno, Mesto, PSC, Adresa, Rok_Porusenia, Datum_Porusenia, Ico);
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region KL_parsovanie

        /// <summary>
        /// Metóda ktorá parsuje údaje z xml súboru pre Kurzový lístok
        /// </summary>
        /// <param name="file">dostaneme xml súbor priamo z web page</param>
        /// <returns>true, ak úspešne zbehla, inak false</returns>
        public bool XML_Parser_KL(string file)
        {
            XmlDocument xmlRead = new XmlDocument();
            xmlRead.LoadXml(file);
            if (xmlRead.DocumentElement != null)
            {
                foreach (XmlNode node in xmlRead.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode firstNode in node)
                    {
                        foreach (XmlNode secondNode in firstNode.ChildNodes)
                        {
                            // kontrola aby sme nečítali childnodes, ktoré nepotrebujeme
                            if (firstNode.ChildNodes.Count > 5)
                            {
                                var list = secondNode.OuterXml.Replace(".", ",").Split('\"').Where(s => !string.IsNullOrWhiteSpace(s));
                                var mena = list.Skip(1).Take(2).FirstOrDefault();
                                var kurz = list.Skip(3).Take(4).FirstOrDefault().ToString();

                                Save_Data_EF.Save_KL(mena, kurz);
                            }
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
