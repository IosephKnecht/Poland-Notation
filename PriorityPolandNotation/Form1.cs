using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolandNonatation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Доп.метод на проверку скобок...
        /// </summary>
        /// <param name="source">Входная строка с мат.выражением...</param>
        /// <returns>Возвращает true/false в зависимости от правильности расстановки скобок...</returns>
        private bool ParseBrackets(string source)
        {
            int n = 0;
            foreach (var c in source)
            {
                if (c == '(')
                    n++;
                if (c == ')')
                    n--;
                if (n < 0)
                    return false;
            }
            return n == 0 ? true : false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input_string = textBox1.Text;
            input_string = input_string.Replace(" ", string.Empty);
            if (ParseBrackets(input_string))
            {
                Calcul calc = new Calcul();
                calculLabel.Text = calc.CalculString(input_string);
                if (calculLabel.Text != "Error") polandStringLabel.Text = calc.Output_String; else polandStringLabel.Text = "Error";
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
