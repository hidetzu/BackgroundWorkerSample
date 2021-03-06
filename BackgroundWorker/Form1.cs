﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Configuration;
using System.Runtime.InteropServices;

namespace BackgroundWorkerSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private System.ComponentModel.BackgroundWorker CreateBackgroundWorker()
        {
            System.ComponentModel.BackgroundWorker  bgWorker = new System.ComponentModel.BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);

            return bgWorker;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bgWorker = CreateBackgroundWorker();
            var form2 = new Form2()
            {
                Owner = this,
                BgWorker = bgWorker,
            };
            bgWorker.RunWorkerAsync(form2);
            this.Enabled = false;
            form2.Show();
        }

        class Result
        {
            public int errorCode { set; get; }
            public Form2 form2 { set; get; }
            public bool canncel { set; get; }
        }

        /// <summary>
        /// DoWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoWork(object sender,
            DoWorkEventArgs e)
        {
            var form = (Form2)e.Argument;

            // Get the BackgroundWorker that raised this event.
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            for (int i = 0; i < 1000000000; i++) ;

            var result = new Result()
            {
                form2 = form,
                errorCode = 0,
                canncel = worker.CancellationPending,
            };
            e.Result = result;
        }

        /// <summary>
        /// RunWorkerCompleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunWorkerCompleted(
            object sender, RunWorkerCompletedEventArgs e)
        {
            var result = (Result)e.Result;

            if (result.form2 != null)
            {
                result.form2.Close();
                result.form2 = null;
            }

            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled || result.canncel)
            {
                MessageBox.Show("処理が中断されました。");
            }
            else
            {
                MessageBox.Show("処理が完了しました。");
            }
            this.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var startInfo = new ProcessStartInfo(".\\70c07400.jpg");
            try
            {
                using (Process pProcess = Process.Start(startInfo.Exe, startInfo.Parameters))
                {
                    pProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void dllCallback(int ret);

        [DllImport("DLLSample.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "add")] // .NetFramework4.0
        public static extern double Add(double a, double b);

        [StructLayout(LayoutKind.Sequential)]
        private struct AccessContext
        {
            public int idx;
            public dllCallback callback;
        }

        [DllImport("DLLSample.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "start_thread")] // .NetFramework4.0
        private static extern void start_thread(ref AccessContext accessContext);

        private void resCallback(System.Int32 ret)
        {
            MessageBox.Show(string.Format("{0}です。", ret));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AccessContext context = new AccessContext() {
                idx = 10,
                callback = resCallback
            };

            start_thread(ref context);
            Console.WriteLine("idx={0}", context.idx);
            //MessageBox.Show("処理中");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var imagePath = ".\\70c07400.jpg";
            var exe = ConfigurationManager.AppSettings["paint"];
            try
            {
                using (Process pProcess = Process.Start(exe, imagePath))
                {
                    pProcess.WaitForExit();
                }
                var a = 10;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
