using System;
using System.Windows.Forms;

namespace OctogeddonUnpack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string error = null;
            try
            {
                Class.AYGP.AYGP.UnPack(textBox1.Text, textBox2.Text);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            MessageBox.Show(error ?? "Succeed!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string error = null;
            try
            {
                Class.M5K.M5K.M5KPng(textBox3.Text);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            MessageBox.Show(error ?? "Succeed!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "Choose ayg file";
                dialog.Filter = "AYGP Package(*.ayg)|*.ayg|All(*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dialog.FileName;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Choose save folder";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dialog.SelectedPath;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Choose m5k folder";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
