using System;
using System.Threading;
using System.Windows.Forms;
using CircuitBreakerGUI.CircuitBreakerBackEnd;
using Polly;
using Polly.CircuitBreaker;

namespace CircuitBreakerGUI
{
    public partial class Form1 : Form
    {
        private readonly CircuitBreakerPolicy _breaker;
        private readonly Service1Client _client = new Service1Client();

        public Form1()
        {
            _breaker = Policy.Handle<Exception>()
                .CircuitBreaker(2, TimeSpan.FromSeconds(10), OnBreak, OnReset, OnHalfOpen);

            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WriteTextSafe("Start DoWork");

            var i = 1;
            new Thread(() =>
            {
                while (true)
                {
                    WriteTextSafe($"Tentative #{i}");
                    try
                    {
                        var resp = _breaker.Execute(_client.DoWork);
                        WriteTextSafe($"Reponse du service recue : {resp}");
                    }
                    catch (BrokenCircuitException)
                    {
                        WriteTextSafe("Le Circuit est ouvert !");
                    }
                    catch (Exception)
                    {
                        WriteTextSafe($"Je n'ai pas trouvé d'endpoint !");
                    }

                    Thread.Sleep(4000);
                    i++;
                }
            }).Start();
        }

        private void OnBreak(Exception r, TimeSpan ts)
        {
            WriteTextSafe("On vient d'ouvrir le circuit !");
        }

        private void OnReset()
        {
            WriteTextSafe("Le circuit vient d'être reset");
        }

        private void OnHalfOpen()
        {
            WriteTextSafe("Mhhh.. Je suis half open, une seule requete est autorisée avant que j'ouvre le circuit");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _client.SetIsAlive(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _client.SetIsAlive(true);
        }

        private void WriteTextSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                richTextBox1.Invoke(d, text);
            }
            else
            {
                richTextBox1.AppendText(text);
                richTextBox1.AppendText("\n");
                richTextBox1.ScrollToCaret();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private delegate void SafeCallDelegate(string text);
    }
}