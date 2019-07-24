using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace PCSBeep
{
    public partial class FormForMousePointer : Form
    {
//<trying to beep
        //32bit?
        [DllImport("inpout32.dll")]
        private static extern void Out32(short PortAddress, short Data);
        [DllImport("inpout32.dll")]
        private static extern char Inp32(short PortAddress);
        //64bit?
        [DllImport("inpoutx64.dll", EntryPoint = "Out32")]
        private static extern void Out32_x64(short PortAddress, short Data);

        [DllImport("inpoutx64.dll", EntryPoint = "Inp32")]
        private static extern char Inp32_x64(short PortAddress);

        bool m_bX64 = true;
        uint intGFreq = 100000;
        //bool blnStop = false;
        private void PleaseBeep(uint freq)
        {
            if (m_bX64)
            {
                Out32_x64(0x43, 0xB6);
                Out32_x64(0x42, (byte)(freq & 0xFF));
                Out32_x64(0x42, (byte)(freq >> 9));
                System.Threading.Thread.Sleep(50);
                Out32_x64(0x61, (byte)(Convert.ToByte(Inp32_x64(0x61)) | 0x03));
            }
            else
            {
                Out32(0x43, 0xB6);
                Out32(0x42, (byte)(freq & 0xFF));
                Out32(0x42, (byte)(freq >> 9));
                System.Threading.Thread.Sleep(10);
                Out32(0x61, (byte)(Convert.ToByte(Inp32(0x61)) | 0x03));
            }
        }
        private void StopBeep()
        {
            if (m_bX64)
                Out32_x64(0x61, (byte)(Convert.ToByte(Inp32_x64(0x61)) & 0xFC));
            else
                Out32(0x61, (byte)(Convert.ToByte(Inp32(0x61)) & 0xFC));
        }

        private void BeeperWithGlobalVariables()
        {
            while (blnBeep)
            {
                uint freq = 1193180000 / intGFreq;
                PleaseBeep(freq);
            }
            StopBeep();
        }
        //beep>
        bool blnBeep = false;
        public FormForMousePointer()
        {
            InitializeComponent();
        }

        private void FormForMousePointer_MouseDown(object sender, MouseEventArgs e)
        {
            blnBeep = true;
            //intGFreq = 440000;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(BeeperWithGlobalVariables));
            t.Start();
        }

        private void FormForMousePointer_Load(object sender, EventArgs e)
        {

        }

        private void FormForMousePointer_MouseMove(object sender, MouseEventArgs e)
        {
            var relativePoint = this.PointToClient(Cursor.Position);
            lblX.Text = "X: " + Convert.ToString(relativePoint.X);
            lblY.Text = "Y: " + Convert.ToString(relativePoint.Y);
            intGFreq = Convert.ToUInt32(880000 - (1000 * relativePoint.Y));
        }

        private void FormForMousePointer_MouseUp(object sender, MouseEventArgs e)
        {
            blnBeep = false;
        }
    }
}
