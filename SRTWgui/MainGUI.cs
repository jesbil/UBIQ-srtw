using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SRTWgui
{
    public partial class MainGUI : Form
    {
        delegate void SetTextCallback(string text);
        public MainGUI()
        {
            InitializeComponent();
            Controller controller = new Controller(this);
            panel1.BackColor = System.Drawing.Color.Snow;
            textBox1.Text = "Welcome wizards";
        }


        private void SetText(string text)
        {
            this.textBox1.Text = text;
        }


        public void changeState(int state) 
        {
            SetTextCallback stc = new SetTextCallback(SetText);
            switch (state)
            {
                case State.g_eating:
                    panel1.BackColor = System.Drawing.Color.Salmon;
                     this.Invoke(stc, new object[] { "Eating" });
                    
                     
                    break;
                case State.g_finished_eating:
                    panel1.BackColor = System.Drawing.Color.Purple;
                    this.Invoke(stc, new object[] { "Wants to pay" });
                    break;
                case State.g_more_water:
                    panel1.BackColor = System.Drawing.Color.Aqua;
                    this.Invoke(stc, new object[] { "Needs drink refill" });
                    break;
                case State.g_ready_to_order:
                    panel1.BackColor = System.Drawing.Color.Goldenrod;
                    this.Invoke(stc, new object[] { "Ready to order" });
                    break;
                case State.g_seated:
                    panel1.BackColor = System.Drawing.Color.RoyalBlue;
                    this.Invoke(stc, new object[] { "Wants menu" });
                    break;
                case State.g_table_is_set:
                    panel1.BackColor = System.Drawing.Color.SeaGreen;
                    this.Invoke(stc, new object[] { "Table is set" });
                    break;
                case State.g_table_is_unset:
                    panel1.BackColor = System.Drawing.Color.DarkRed;
                    this.Invoke(stc, new object[] { "Table needs cleaning" });
                    break;
                case State.g_waiting_food:
                    panel1.BackColor = System.Drawing.Color.DimGray;
                    this.Invoke(stc, new object[] { "Waiting for food" });
                    break;
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainGUI_Load(object sender, EventArgs e)
        {

        }

    }
}
