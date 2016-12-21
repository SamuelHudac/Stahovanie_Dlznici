using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stahovanie
{
    /// <summary>
    /// Dlznici form
    /// </summary>
    public partial class DlzniciForm : Form
    {
        #region konstruktor
        public DlzniciForm()
        {
            InitializeComponent();
            this.Stahovať.Click += this.Stahovať_Click;
            this.Zrušiť.Click += Zrušiť_Click;
        }
        #endregion

        #region event_handlers

        /// <summary>
        /// button zrušit aplikáciu
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void Zrušiť_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Naozaj si prajete ukončiť aplikáciu ?", "Zrušiť", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// button začať sťahovať ale najprv kontrola či niesú vybrané dve možnosťi
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Stahovať_Click(object sender, EventArgs e)
        {
            Console.WriteLine("START");
            HTTPcontroller skuska = new HTTPcontroller();

            if (this.Ddmoznost.SelectedItem != null && this.KurzListok.SelectedItem != null)
            {
                MessageBox.Show("Nieje možné sťahovať z viacerých zdrojov");
            }
            else if (this.Ddmoznost.SelectedItem != null && this.KurzListok.SelectedItem == null)
            {
                skuska.Response(this.Ddmoznost.SelectedItem.ToString());
            }
            else
            {
                skuska.Response(this.KurzListok.SelectedItem.ToString());
            }
        }
        #endregion
    }
}
