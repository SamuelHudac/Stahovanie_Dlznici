using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.MappingAPI;
using System.Drawing;
using System.Linq;
using stahovanie;

namespace stahovanie
{
    /// <summary>
    /// Class pre CSV_Parser
    /// </summary>
    public class CSV_parser
    {
        #region Parser_Soc.Poistovna

        /// <summary>
        /// Metóda ktorá parsuje údaje z csv súboru pre sociálnu poistovňu
        /// </summary>
        /// <param name="file">Cesta ku zložke so súborom</param>
        /// <returns>true, ak úspešne zbehla, inak false</returns>
        public bool CSV_Parser_SP(string[] file)
        {
            using (StreamReader sr = new StreamReader(file.FirstOrDefault()))
            {
                string[] data_After_Split = Regex.Split(sr.ReadToEnd(), Environment.NewLine);
                var Id = 0;

                foreach (var item in data_After_Split.Skip(1))
                {
                    if (Id == 470)
                    {
                    }
                    var list = item.Replace("\"", string.Empty).Replace("&", string.Empty).Split(';').Where(s => !string.IsNullOrWhiteSpace(s));
                    Cursor.Current = Cursors.WaitCursor;
                    var meno_firmy = list.FirstOrDefault();
                    var adresa = list.Skip(1).Take(2).FirstOrDefault();
                    var mesto = list.Skip(2).Take(3).FirstOrDefault();
                    decimal dlzna_suma = 0;

                    if (list.Skip(3).Take(4).FirstOrDefault() != null && Char.IsNumber(list.Skip(3).Take(4).FirstOrDefault()[0]))
                    {
                        dlzna_suma = Convert.ToDecimal(list.Skip(3).Take(4).FirstOrDefault());
                    }
                    else if (list.Skip(4).Take(5).FirstOrDefault() != null && Char.IsNumber(list.Skip(4).Take(5).FirstOrDefault()[0]))
                    {
                        dlzna_suma = Convert.ToDecimal(list.Skip(4).Take(5).FirstOrDefault());
                    }

                    Id++;
                    Save_Data_EF.SaveData_SP(Id, meno_firmy, adresa, mesto, dlzna_suma);
                }

                Cursor.Current = Cursors.Default;
                sr.Close();
            }

            return true;
        }
        #endregion

        #region Parser_VsZP

        /// <summary>
        /// Metóda ktorá parsuje údaje z csv súboru pre Všeobecnú zdravotnú poisťovňu
        /// </summary>
        /// <param name="file">Cesta ku zložke so súboru</param>
        /// <returns>true, ak úspešne zbehla, inak false</returns>
        public bool CSV_Parser_VsZP(string[] file)
        {
            // číta údaje pre samoplatiteľov
            // pred samotným parsovaním musíme enkódovať súbor do čitateľného stavu
            using (StreamReader sr_Samoplatitelia = new StreamReader(file.FirstOrDefault(), Encoding.GetEncoding("windows-1250")))
            {
                string[] data_After_Split = Regex.Split(sr_Samoplatitelia.ReadToEnd(), Environment.NewLine);
                var Id = 0;

                foreach (var item in data_After_Split.Skip(2))
                {
                    var list = item.Replace("\"", string.Empty).Replace("=", string.Empty).Replace("&", string.Empty).Split(';');

                    Cursor.Current = Cursors.WaitCursor;
                    var meno = list.FirstOrDefault();
                    var PSC = list.Skip(1).Take(2).FirstOrDefault();
                    var ulica = list.Skip(2).Take(3).FirstOrDefault();
                    var mesto = list.Skip(3).Take(4).FirstOrDefault();
                    decimal dlzna_suma = Convert.ToDecimal(list.Skip(4).Take(5).FirstOrDefault());
                    var typ_platitela = list.Skip(5).Take(6).FirstOrDefault();
                    var narok_na_starostlivost = list.Skip(6).Take(7).FirstOrDefault();
                    Id++;

                    Save_Data_EF.Save_Data_VsZP_Samoplatitelia(meno, PSC, ulica, mesto, dlzna_suma, typ_platitela, narok_na_starostlivost, Id);
                }

                sr_Samoplatitelia.Close();
            }

            // číta udaje pre zamestnávateľov
            // pred samotným parsovaním musíme enkódovať súbor do čitateľného stavu
            using (StreamReader sr_Zamestnavatelia = new StreamReader(file[1], Encoding.GetEncoding("windows-1250")))
            {
                string[] data_After_Split = Regex.Split(sr_Zamestnavatelia.ReadToEnd(), Environment.NewLine);
                var Id = 0;

                foreach (var item in data_After_Split.Skip(2))
                {
                    var list = item.Replace("\"", string.Empty).Replace("=", string.Empty).Replace("&", string.Empty).Split(';');

                    Cursor.Current = Cursors.WaitCursor;
                    var meno = list.FirstOrDefault();
                    var PSC = list.Skip(1).Take(2).FirstOrDefault();
                    var ulica = list.Skip(2).Take(3).FirstOrDefault();
                    var mesto = list.Skip(3).Take(4).FirstOrDefault();
                    var Ico = list.Skip(4).Take(5).FirstOrDefault();
                    decimal dlzna_suma = Convert.ToDecimal(list.Skip(5).Take(6).FirstOrDefault());
                    var typ_platitela = list.Skip(6).Take(7).FirstOrDefault();
                    var narok_na_starostlivost = list.Skip(7).Take(8).FirstOrDefault() == "-" ? "Neuvedená" : list.Skip(7).Take(8).FirstOrDefault();
                    Id++;

                    Save_Data_EF.Save_Data_VsZP_Zamestnavatelia(meno, PSC, ulica, mesto, Ico, dlzna_suma, typ_platitela, narok_na_starostlivost, Id);
                }

                sr_Zamestnavatelia.Close();
            }

            Cursor.Current = Cursors.Default;
            return true;
        }
        #endregion
    }
}
