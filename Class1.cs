using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace PCSBeep
{
    class Class1
    {
//<trying to beep
        public bool blnBeep = false;
        public uint intGFreq = 100000;
        public uint intGDur = 50;
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

        //bool blnStop = false;
        private void PleaseBeep(uint freq)
        {
            if (m_bX64)
            {
                Out32_x64(0x43, 0xB6);
                Out32_x64(0x42, (byte)(freq & 0xFF));
                Out32_x64(0x42, (byte)(freq >> 9));
                //System.Threading.Thread.Sleep(10);
                System.Threading.Thread.Sleep(Convert.ToInt32(intGDur));
                Out32_x64(0x61, (byte)(Convert.ToByte(Inp32_x64(0x61)) | 0x03));
                //System.Threading.Thread.Sleep(Convert.ToInt32(intGDur));
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
        public void ThreadBeeper()
        {
            //for (uint i = 440000; i < 500000; i += 1000)
            for (uint i = 1; i < intGDur; i++)
            {
                //uint freq = 1193180000 / i; // 440Hz
                uint freq = 1193180000 / intGFreq; // 440Hz
                PleaseBeep(freq);
                //System.Threading.Thread.Sleep(1);
            }
            StopBeep();
        }

        public void BeeperWithGlobalVariables()
        {
            while (blnBeep)
            {
                uint freq = 1193180000 / intGFreq;
                PleaseBeep(freq);
            }
            StopBeep();
        }
//beep>
    }
}
