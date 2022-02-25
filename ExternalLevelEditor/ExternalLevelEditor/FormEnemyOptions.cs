using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalLevelEditor
{
    public partial class FormEnemyOptions : Form
    {

        #region Fields

        private FormMain main;
        int x;
        int y;

        #endregion Fields

        public FormEnemyOptions(FormMain m, int x, int y)
        {
            InitializeComponent();
            main = m;
            this.x = x;
            this.y = y;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Enemy temp = new Enemy();
            temp.X = x;
            temp.Y = y;

            temp.Priority = (int)numericUpDownPriority.Value;

            // Check each radio button in enemy types.
            temp.EnemyType = (String)comboBoxEnemyType.SelectedItem;

            if (temp.EnemyType == "")
            {
                MessageBox.Show("Please enter an enemy type.", "No enemy type", MessageBoxButtons.OK);
                return;
            }

            // Check what options the player can use.
            foreach (CheckBox a in groupBoxPAttacks.Controls)
            {
                if (a.Checked)
                {
                    temp.PlayerOptions.Add(a.Text);
                }
            }
            // Check what options the enemy can use.
            foreach (CheckBox a in groupBoxEAttacks.Controls)
            {
                if (a.Checked)
                {
                    temp.EnemyOptions.Add(a.Text);
                }
            }
            // Send the enemy object to the cell's tag.
            main.Cells[x, y].Tag = temp;
            this.Close();
        }

        /// <summary>
        /// Closes this form and doesn't make an enemy object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            main.Cells[x, y].BackColor = Color.White;
            main.Cells[x, y].Tag = null;
            this.Close();
        }

        /// <summary>
        /// Certain actions this form takes upon loading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormEnemyOptions_Load(object sender, EventArgs e)
        {
            labelX.Text += " " + x;
            labelY.Text += " " + y;
        }
    }
}
