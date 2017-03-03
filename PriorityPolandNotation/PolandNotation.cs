using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication52
{
    class PolandNotation
    {
        string input = null;
        string output = null;
        Stack<string> operators = new Stack<string>();
        Stack<double> operands = new Stack<double>();
        int SearchCount = 0;
        Dictionary<string, int> PrioritySym = new Dictionary<string, int>();

        public PolandNotation() 
        {
            PrioritySym.Add("*", 3);
            PrioritySym.Add("/", 3);
            PrioritySym.Add("+", 2);
            PrioritySym.Add("-", 2);
            PrioritySym.Add("(", 1);
            PrioritySym.Add(")", 1);
            PrioritySym.Add(string.Empty, 0);
        }

        public string _input 
        {
            get { return input; }
            set { input = value; }
        }

        private void CalculComplete()
        {
            foreach (string s in operators)
            {
                output += s;
                operands.Push(BinaryCalcul(operands.Pop(), operands.Pop(), s));
            }
        }

        private bool Search() 
        {
            int cnt = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c == '(')
                {
                    cnt++;
                    if(i>1)
                        if(!PrioritySym.ContainsKey(input[i-1].ToString())) cnt--;
                    SearchCount++;
                }
                else 
                if (c == ')')
                {
                    cnt--;
                    if (i < input.Length-2)
                        if (!PrioritySym.ContainsKey(input[i + 1].ToString())) cnt++;
                    SearchCount++;
                }

            }
            if (cnt == 0) return true;
            else
            {
                SearchCount = 0;
                return false;
            }
        }

        private double BinaryCalcul(double first, double second, string sym)
        {
            switch (sym)
            {
                case "+":
                    return second + first;
                    break;
                case "-":
                    return second - first;
                    break;
                case "*":
                    return second * first;
                    break;
                case "/":
                    return second / first;
                    break;
            }
            return 0;
        }

        private void Push(string sym) 
        {
            if (sym != string.Empty) operators.Push(sym);
        }

        public string Calcul() 
        {
            string hash = null;
            if (!Search()) return "Упс,нам кажется,Вы где-то ошиблись в расстановке скобок,нет..?";
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
                    string sym = null;
                    if (input != string.Empty)
                    {
                        sym = input[0].ToString();
                        input = input.Remove(0, 1);
                    }
                    else
                    {
                        sym = string.Empty;
                        input = null;
                    }
                    if (sym != ")")
                    {
                        if (operators.Count != 0 && PrioritySym[sym] <= PrioritySym[operators.Peek()])
                        {
                            if (operators.Count + 1 - SearchCount > operands.Count) return "Упс,нам кажется,Вы где-то ошиблись в расстановке знаков,нет..?";
                            if (sym != "(")
                            {
                                output += operators.Peek();
                                operands.Push(BinaryCalcul(operands.Pop(), operands.Pop(), operators.Pop()));
                            }
                            Push(sym);
                        }
                        else
                        {
                            Push(sym);
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
            if (operands.Count == 1) return output + " = " + operands.Pop().ToString();
            if (operators.Count == operands.Count - 1)
            {
                CalculComplete();
                return output+" = "+operands.Pop().ToString();
            }
            if (operators.Count == 0 && operands.Count > 1) return output;
            return "Error";
        }
    }
}
