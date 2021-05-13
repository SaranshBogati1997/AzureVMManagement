using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureForms
{
    public partial class Prompt : Form
    {
        public Prompt(string message, bool isDeallocate)
        {
            InitializeComponent();
            messageText.Text = message;
            if (isDeallocate) okBtn.Text = "Deallocate";
            else okBtn.Text = "Start";
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
