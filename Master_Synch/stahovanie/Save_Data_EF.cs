using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stahovanie
{
    /// <summary>
    /// Ukladanie a synchronizovanie dát pomocov EF
    /// </summary>
    public class Save_Data_EF
    {
        #region DU_SaveData

        /// <summary>
        /// uklada a aktualizuje údaje pre daňový úrad
        /// </summary>
        /// <param name="Ic_Dph">ic_dph firmy</param>
        /// <param name="Meno">meno firmy</param>
        /// <param name="Mesto">mesto kde firma sídli</param>
        /// <param name="Psc">psc firmy</param>
        /// <param name="Adresa">adresa firmy</param>
        /// <param name="Rok">rok porusenia</param>
        /// <param name="Datum_Porusenia">datum porusenia</param>
        /// <param name="Ico">ico firmy</param>
        public static void SaveData_DU(string Ic_Dph, string Meno, string Mesto, string Psc, string Adresa, int Rok, DateTime Datum_Porusenia, int Ico)
        {
            using (Dlznici_Entities DU = new Dlznici_Entities())
            {
                // SaveData_DU používame aj na aktualizáciu údajov
                // selectneme si zhodu podla mena a ic_dph ak takýto údaj v Dbo nájdeme updatneme ak je selectnuty objekt null insertneme
                Danovy_Urad DU_update = DU.Danovy_Urad.FirstOrDefault(w => w.Ic_Dph == Ic_Dph && w.Meno == Meno);

                if (DU_update != null)
                {
                    // synchronizácia
                    DU_update.Id = DU_update.Id;
                    DU_update.Ic_Dph = Ic_Dph;
                    DU_update.Meno = Meno;
                    DU_update.Mesto = Mesto;
                    DU_update.PSC = Psc;
                    DU_update.Adresa = Adresa;
                    DU_update.Rok_Porusenia = Rok;
                    DU_update.Datum_Porusenia = Datum_Porusenia;
                    DU_update.Ico = Ico;
                    DU_update.Datum_Aktualizacie = DateTime.UtcNow;

                    DU.SaveChanges();
                }
                else
                {
                    // pridávanie nového údaja
                    var obj = new Danovy_Urad
                    {
                        Id = Guid.NewGuid().ToString(),
                        Ic_Dph = Ic_Dph,
                        Meno = Meno,
                        Mesto = Mesto,
                        PSC = Psc,
                        Adresa = Adresa,
                        Rok_Porusenia = Rok,
                        Datum_Porusenia = Datum_Porusenia,
                        Ico = Ico,
                        Datum_Aktualizacie = DateTime.UtcNow
                    };

                    // vložíme objekt 
                    DU.Danovy_Urad.Add(obj);
                    DU.SaveChanges();
                }
            }
        }
        #endregion

        #region VsZP_SaveData

        /// <summary>
        /// ukladá a aktualizuje údaje pre VsZP-samoplatiteľov
        /// </summary>
        /// <param name="Meno">meno firmy</param>
        /// <param name="PSC">psc firmy</param>
        /// <param name="Ulica">ulica kde firma sídli</param>
        /// <param name="Mesto">mesto kde firma sídli</param>
        /// <param name="Dlh">dlh firmy</param>
        /// <param name="Typ_Platitela">typ platitela(v tomto prípade samoplatitel)</param>
        /// <param name="Narok_Na_Starostlivost">náarok na zdravotnú starostlivosť</param>
        /// <param name="ID">id obycajný count na počet firiem</param>
        public static void Save_Data_VsZP_Samoplatitelia(string Meno, string PSC, string Ulica, string Mesto, decimal Dlh, string Typ_Platitela, string Narok_Na_Starostlivost, int ID)
        {
            using (Dlznici_Entities VsZP_SP = new Dlznici_Entities())
            {
                // Save_Data_VsZP_Samoplatitelia používame aj na aktualizáciu údajov
                // selectneme si zhodu podla mena a psc ak takýto údaj v Dbo nájdeme updatneme ak je selectnuty objekt null insertneme
                VsZP_Samoplatitelia VsZP_UpdateSP = VsZP_SP.VsZP_Samoplatitelia.FirstOrDefault(w => w.Meno == Meno && w.PSC == PSC);

                if (VsZP_UpdateSP != null)
                {
                    // update údajov
                    VsZP_UpdateSP.GuidID = VsZP_UpdateSP.GuidID;
                    VsZP_UpdateSP.Meno = Meno;
                    VsZP_UpdateSP.PSC = PSC;
                    VsZP_UpdateSP.Ulica = Ulica;
                    VsZP_UpdateSP.Mesto = Mesto;
                    VsZP_UpdateSP.Dlzna_Suma = Dlh;
                    VsZP_UpdateSP.Typ_Platitela = Typ_Platitela;
                    VsZP_UpdateSP.Narok_Na_Starostlivost = Narok_Na_Starostlivost;
                    VsZP_UpdateSP.Datum_Aktualizacie = DateTime.UtcNow;

                    VsZP_SP.SaveChanges();
                }
                else
                {
                    // insert údajov
                    var obj = new VsZP_Samoplatitelia
                    {
                        GuidID = Guid.NewGuid().ToString(),
                        ID = ID,
                        Meno = Meno,
                        PSC = PSC,
                        Ulica = Ulica,
                        Mesto = Mesto,
                        Dlzna_Suma = Dlh,
                        Typ_Platitela = Typ_Platitela,
                        Narok_Na_Starostlivost = Narok_Na_Starostlivost,
                        Datum_Aktualizacie = DateTime.UtcNow
                    };

                    // nakoniec vložíme do objektu
                    VsZP_SP.VsZP_Samoplatitelia.Add(obj);
                    VsZP_SP.SaveChanges();
                }
            }
        }

        /// <summary>
        /// ukladá a aktualizuje údaje pre VsZP-Zamestnavateľov(patria tu aj živnostníci)
        /// </summary>
        /// <param name="Meno">meno firmy</param>
        /// <param name="PSC">psc firmy</param>
        /// <param name="Ulica">ulica kde firma sídli</param>
        /// <param name="Mesto">mesto kde firma sídli</param>
        /// <param name="Ico">ico firmy</param>
        /// <param name="Dlh">dlh firmy</param>
        /// <param name="Typ_Platitela">typ platitela(v tomto prípade zamestnávateľ)</param>
        /// <param name="Narok_Na_Starostlivost">náarok na zdravotnú starostlivosť</param>
        /// <param name="ID">id obycajný count na počet firiem</param>
        public static void Save_Data_VsZP_Zamestnavatelia(string Meno, string PSC, string Ulica, string Mesto, string Ico, decimal Dlh, string Typ_Platitela, string Narok_Na_Starostlivost, int ID)
        {
            using (Dlznici_Entities VsZP_SP = new Dlznici_Entities())
            {
                // Save_Data_VsZP_Zamestnavatelia používame aj na aktualizáciu údajov
                // selectneme si zhodu podla mena a ico ak takýto údaj v Dbo nájdeme updatneme ak je selectnuty objekt null insertneme
                VsZP_Zamestnavatelia VsZP_UpdateZ = VsZP_SP.VsZP_Zamestnavatelia.FirstOrDefault(w => w.ICO == Ico && w.Meno == Meno);

                if (VsZP_UpdateZ != null)
                {
                    // update údajov
                    VsZP_UpdateZ.GuidID = VsZP_UpdateZ.GuidID;
                    VsZP_UpdateZ.Meno = Meno;
                    VsZP_UpdateZ.PSC = PSC;
                    VsZP_UpdateZ.Ulica = Ulica;
                    VsZP_UpdateZ.Mesto = Mesto;
                    VsZP_UpdateZ.ICO = Ico;
                    VsZP_UpdateZ.Dlzna_Suma = Dlh;
                    VsZP_UpdateZ.Typ_Platitela = Typ_Platitela;
                    VsZP_UpdateZ.Narok_Na_Starostlivost = Narok_Na_Starostlivost;
                    VsZP_UpdateZ.Datum_Aktualizacie = DateTime.UtcNow;

                    VsZP_SP.SaveChanges();
                }
                else
                {
                    // insert údajov
                    var obj = new VsZP_Zamestnavatelia
                    {
                        GuidID = Guid.NewGuid().ToString(),
                        ID = ID,
                        Meno = Meno,
                        PSC = PSC,
                        Ulica = Ulica,
                        Mesto = Mesto,
                        ICO = Ico,
                        Dlzna_Suma = Dlh,
                        Typ_Platitela = Typ_Platitela,
                        Narok_Na_Starostlivost = Narok_Na_Starostlivost,
                        Datum_Aktualizacie = DateTime.UtcNow
                    };

                    // nakoniec vložíme objekt
                    VsZP_SP.VsZP_Zamestnavatelia.Add(obj);
                    VsZP_SP.SaveChanges();
                }
            }
        }
        #endregion

        #region Soc.Poistovna_SaveData

        /// <summary>
        /// ukladá a aktualizuje údaje pre sociálnu poisťovňu
        /// </summary>
        /// <param name="Id">id firmy (obyčajný identifikátor count)</param>
        /// <param name="meno_firmy">meno firmy</param>
        /// <param name="adresa">adresa kde firma sídli</param>
        /// <param name="mesto">mesto kde firma sídli</param>
        /// <param name="dlzna_suma">dlh firmy</param>
        public static void SaveData_SP(int Id, string meno_firmy, string adresa, string mesto, decimal dlzna_suma)
        {
            using (Dlznici_Entities SP = new Dlznici_Entities())
            {
                // SaveData_SP používame aj na aktualizáciu údajov
                // selectneme si zhodu podla mena a adresy ak takýto údaj v Dbo nájdeme updatneme ak je selectnuty objekt null insertneme
                Socialna_Poistovna SP_Update = SP.Socialna_Poistovna.FirstOrDefault(w => w.MenoFirmy == meno_firmy && w.Adresa == adresa);

                if (SP_Update != null)
                {
                    // update údajov
                    SP_Update.MenoFirmy = meno_firmy;
                    SP_Update.Adresa = adresa;
                    SP_Update.Mesto = mesto;
                    SP_Update.DlznaSuma = dlzna_suma;
                    SP_Update.GuidID = SP_Update.GuidID;
                    SP_Update.DatumAktualizacie = DateTime.UtcNow;

                    SP.SaveChanges();
                }
                else
                {
                    // insert údajov
                    var obj = new Socialna_Poistovna
                    {
                        Id = Id,
                        MenoFirmy = meno_firmy,
                        Adresa = adresa,
                        Mesto = mesto,
                        DlznaSuma = dlzna_suma,
                        GuidID = Guid.NewGuid().ToString(),
                        DatumAktualizacie = DateTime.UtcNow
                    };

                    // nakoniec vložíme ojekt
                    SP.Socialna_Poistovna.Add(obj);
                    SP.SaveChanges();
                }
            }
        }
        #endregion

        #region Union_SaveData

        /// <summary>
        /// ukladá a aktualizuje údaje pre zdravotnú poisťovňu UNION
        /// </summary>
        /// <param name="meno">meno firmy</param>
        /// <param name="mesto">mesto kde firma sídli</param>
        /// <param name="ulica">ulica kde firma sídli</param>
        /// <param name="psc">psc firmy</param>
        /// <param name="ico">ico firmy</param>
        /// <param name="dlzna_suma">dlh firmy</param>
        /// <param name="typ_zs">typ zdravotnej starostlivosti</param>
        public static void Save_Data_Union(string meno, string mesto, string ulica, string psc, string ico, string dlzna_suma, string typ_zs)
        {
            using (Dlznici_Entities Union = new Dlznici_Entities())
            {
                // Save_Data_Union používame aj na aktualizáciu údajov
                // selectneme si zhodu podla mena a psc ak takýto údaj v Dbo nájdeme updatneme ak je selectnuty objekt null insertneme
                Zp_Union Union_Update = Union.Zp_Union.FirstOrDefault(w => w.Meno == meno && w.PSC == psc);

                if (Union_Update != null)
                {
                    // update údajov
                    Union_Update.GuidID = Union_Update.GuidID;
                    Union_Update.Meno = meno;
                    Union_Update.Mesto = mesto;
                    Union_Update.Ulica = ulica;
                    Union_Update.PSC = psc;
                    Union_Update.ICO = ico;
                    Union_Update.Dlzna_Suma = Convert.ToDecimal(dlzna_suma);
                    Union_Update.Typ_Starostlivosti = typ_zs;
                    Union_Update.Datum_Aktualizacie = DateTime.UtcNow;

                    Union.SaveChanges();
                }
                else
                {
                    // insert údajov
                    var obj = new Zp_Union
                    {
                        GuidID = Guid.NewGuid().ToString(),
                        Meno = meno == string.Empty ? null : meno,
                        Mesto = mesto == string.Empty ? null : mesto,
                        Ulica = ulica == string.Empty ? null : ulica,
                        PSC = psc == string.Empty ? null : psc,
                        ICO = ico == string.Empty ? null : ico,
                        Dlzna_Suma = Convert.ToDecimal(dlzna_suma),
                        Typ_Starostlivosti = typ_zs == string.Empty ? null : typ_zs,
                        Datum_Aktualizacie = DateTime.UtcNow
                    };

                    // nakoniec vložíme objekt
                    Union.Zp_Union.Add(obj);
                    Union.SaveChanges();
                }
            }
        }
        #endregion

        #region Kurzovy_Listok

        /// <summary>
        /// ukladá kurzový lístok
        /// </summary>
        /// <param name="Mena">skratka krajiny pre ktorú je daná mena</param>
        /// <param name="Kurz">kurz pre danú krajinu</param>
        public static void Save_KL(string Mena, string Kurz)
        {
            // tu je zbytočné robiť update KL pretože jaždý deň je KL nový a chceme držať aj staršie údaje
            using (Dlznici_Entities KL = new Dlznici_Entities())
            {
                // insert kurzového lístku
                var obj = new KurzovyListok
                {
                    GuidID = Guid.NewGuid().ToString(),
                    Mena = Mena,
                    Kurz = Convert.ToDecimal(Kurz),
                    DatumAktualizacie = DateTime.UtcNow
                };

                // nakoniec vložíme objekt
                KL.KurzovyListok.Add(obj);
                KL.SaveChanges();
            }
        }
        #endregion
    }
}
