using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriorityPolandNotation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //3+4*2/(1-5)=1
        //17-5*6/3-2+4/2=7
        //5+(7-2*3)*(6*4)/2=17
        //(64000/128-3280/164*15)*700=140000
        //(15+2*3*(8-3)-8))=Error
        //18-2+-328+8=Error
        //((2-7)*6)*(15-7))=Error
        //2+5-8**9+5/5+1=Error

        /// <summary>
        /// Функция выполнения бинарного алгебралического действия...
        /// </summary>
        /// <param name="first">Аргумент стека операндов...</param>
        /// <param name="second">Аргумент стека операндов..</param>
        /// <param name="sym"></param>
        /// <returns>Значение выражения...</returns>
        private double BinaryCalcul(double first,double second,string sym) 
        {
            switch (sym) 
            {
                case "+" :
                    return second+first;
                    break;
                case "-":
                    return second - first;
                    break;
                case "*" :
                    return second * first;
                    break;
                case "/":
                    return second / first;
                    break;
            }
            return 0;
        }
        /// <summary>
        /// Функция проверки правильности расстановки скобок...
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string Skobki(string s) 
        {
            int cnt = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '(')
                    cnt++;
                else if (c == ')')
                    cnt--;
                       
            }
            if (cnt == 0) return s; else return "Error";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> SymMas = new Dictionary<string, int>();
            SymMas.Add("^", 4);
            SymMas.Add("*", 3);
            SymMas.Add("/", 3);
            SymMas.Add("+", 2);
            SymMas.Add("-", 2);
            SymMas.Add("(", 1);
            SymMas.Add(")", 1);
            string input=null;
            string output = null;
            if (Skobki(textBox1.Text) != "Error") input = textBox1.Text; else output = "Error";
            string hash = null;
            Stack<string> operators = new Stack<string>();
            Stack<double> operands = new Stack<double>();
            while (input != null) 
            {
                try
                {
                    hash += Convert.ToInt32(input[0].ToString()).ToString();
                    input = input.Remove(0, 1);
                }
                catch 
                {
                    output += hash;
                    if (hash != null) operands.Push(Convert.ToDouble(hash));
                    hash = null;
                    if (input == "") 
                    {
                        foreach (string s in operators)
                        {
                            try
                            {
                                output += s;
                                operands.Push(BinaryCalcul(operands.Pop(), operands.Pop(), s));
                            }
                            catch
                            {
                                output = "Error";
                                break;
                            }
                        }
                        //стек опреаторов не дочищается...
                        break;
                    }
                    string sym = input[0].ToString();
                    input = input.Remove(0, 1);
                    if (sym != ")")
                    {
                        if (operators.Count!=0&&SymMas[sym] <= SymMas[operators.Peek()])
                        { 
                            if (sym != "(")
                            {
                                try
                                {
                                    output += operators.Peek();
                                    operands.Push(BinaryCalcul(operands.Pop(), operands.Pop(), operators.Pop()));
                                    operators.Push(sym);
                                }
                                catch
                                {
                                    output = "Error";
                                    break;
                                } 
                            }
                            else 
                            {
                                operators.Push(sym);
                            }
                        }
                        else
                        {
                            operators.Push(sym);
                        }
                    }
                    else 
                    {
                        while (operators.Peek() != "(") 
                        {
                            output += operators.Peek();
                            operands.Push(BinaryCalcul(operands.Pop(), operands.Pop(), operators.Pop()));
                        }
                        operators.Pop();
                    }

                }
            }
            if (output == "Error") MessageBox.Show(output);else MessageBox.Show(output + "==" + operands.Peek());
        }
    }
}
