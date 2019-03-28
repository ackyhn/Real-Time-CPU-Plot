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
using System.Diagnostics;

namespace Real_Time_CPU_Plot
{
    public partial class Form1 : Form
    {
        private Thread cpuThread;
        private double[] cpuArrays = new double[30];

        public Form1()
        {
            InitializeComponent();
            btnStop.Enabled = false;
            btnResume.Enabled = false;
        }

        private void getPerformanceCounter()
        {
            var cpuPerCounter = new PerformanceCounter("Processor information", "% Processor Time", "_Total");

            while (true)
            {
                cpuArrays[cpuArrays.Length - 1] = Math.Round(cpuPerCounter.NextValue(), 0);
                Array.Copy(cpuArrays, 1, cpuArrays, 0, cpuArrays.Length - 1);

                if (CPU_chart1.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate { updateCpuChart(); });
                }
                else
                {
                    //...........
                }
                Thread.Sleep(100);
            }
            
        }
        private void updateCpuChart()
        {
            CPU_chart1.Series[0].Points.Clear();
            for (int i = 0; i < cpuArrays.Length - 1; ++i)
            {
                CPU_chart1.Series[0].Points.AddY(cpuArrays[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cpuThread = new Thread(new ThreadStart(this.getPerformanceCounter));
            cpuThread.IsBackground = true;
            cpuThread.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {           
            if (btnStart.Enabled == false)
            {
                cpuThread.Abort();
                
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            
        }
    }

    
}
