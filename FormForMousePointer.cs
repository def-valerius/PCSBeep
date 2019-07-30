using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCSBeep
{
    public partial class FormForMousePointer : Form
    {
        private Class1 Beep = new Class1();
        public FormForMousePointer()
        {
            InitializeComponent();
        }

        private void FormForMousePointer_MouseDown(object sender, MouseEventArgs e)
        {
            Beep.blnBeep = true;
            //intGFreq = 440000;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(Beep.BeeperWithGlobalVariables));
            t.Start();
        }

        private void FormForMousePointer_Load(object sender, EventArgs e)
        {
            Beep.intGDur = 50;
        }

        private void FormForMousePointer_MouseMove(object sender, MouseEventArgs e)
        {
            var relativePoint = this.PointToClient(Cursor.Position);
            lblX.Text = "X: " + Convert.ToString(relativePoint.X);
            lblY.Text = "Y: " + Convert.ToString(relativePoint.Y);
            if (relativePoint.Y < 879)
            {
                Beep.intGFreq = Convert.ToUInt32(880000 - (1000 * relativePoint.Y));
            }          
            if (relativePoint.X < 1)
            {
                Beep.intGDur = 1;
            }
            else
            {
                Beep.intGDur = Convert.ToUInt32(relativePoint.X);
            }
            
        }

        private void FormForMousePointer_MouseUp(object sender, MouseEventArgs e)
        {
            Beep.blnBeep = false;
        }
    }
}
