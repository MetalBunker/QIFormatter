﻿using QifApi;
using QifApi.Transactions;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QIFormatterGUI.AutoCompleter;

namespace QIFormatterGUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnGenerateQIF_Click(object sender, EventArgs e)
        {
            if (txtOutputFilePath.TextLength == 0)
            {
                MessageBox.Show("A dónde querés que lo guarde goma?", "Zapato!");
                return;
            }

            if (txtData.TextLength == 0)
            {
                MessageBox.Show("Qué datos querés que exporte goma?", "Zapato!");
                return;
            }

            try
            {
                QifDom targetFile = new QifDom();
                BasicTransaction trans = null;
                string[] recordValues = null;
                IAutoCompleter autoCompleter = new AutoCompleterBase();

                foreach (var record in txtData.Lines)
                {
                    recordValues = record.Split('\t');

                    trans = new BasicTransaction();

                    trans.Date      = DateTime.ParseExact(recordValues[0], "yyyy-MM-dd", null);
                    trans.Payee     = recordValues[1];
                    trans.Memo      = recordValues[2];
                    //Multiplico por -1 para que quede como un gasto desde la cuenta
                    trans.Amount    = decimal.Parse(recordValues[3], CultureInfo.InvariantCulture) * -1; 

                    autoCompleter.AttemptAutoComplete(trans);

                    targetFile.CashTransactions.Add(trans);
                }

                targetFile.Export(txtOutputFilePath.Text, Encoding.Default);

                var msgText = autoCompleter.GetAutoCompleteSummary();

                MessageBox.Show(msgText, "Éxito!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voló todo al carajo: " + ex.Message, "Click click Boom!");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtData.Clear();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                txtData.Text+= Clipboard.GetText();
            }
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            lblTransactionsCount.Text = txtData.Lines.Length + " transactions";
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (txtOutputFilePath.Text.Length > 0)
            {
                sfdQIF.FileName = txtOutputFilePath.Text;
            }

            if (sfdQIF.ShowDialog() == DialogResult.OK)
            {
                txtOutputFilePath.Text = sfdQIF.FileName;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtOutputFilePath.Text = Path.Combine(  Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                                                    "CC_Export_" + DateTime.Now.ToString("yyyy-MM-dd_HHmm") + ".qif");
        }
       
    }
}
