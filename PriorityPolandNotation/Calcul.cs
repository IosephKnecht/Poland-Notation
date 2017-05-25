using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolandNonatation
{
    class Calcul
    {
        string output_string;

        Stack<double> digits;
        Stack<char> operators;

        public string Output_String { get { return output_string; } }

        /// <summary>
        /// Матрица переходов...
        /// </summary>
        /// <param name="input_string_sym">Текущий символ в строке...</param>
        private void TransitionProcedure(char input_string_sym)
        {
            string stack_sym = null;
            try
            {
                stack_sym = operators.Peek().ToString();
            }
            catch
            {
                stack_sym = "_";
            }

            switch (Convert.ToChar(stack_sym))
                {
                    case '-':
                    case '+':
                        if (input_string_sym == '(' || input_string_sym == '*'
                            || input_string_sym == '/')
                        {
                            PutinStackandOutput(input_string_sym);
                        }
                        else
                        {
                            if (input_string_sym == '+' || input_string_sym == '-')
                            {
                                OnceCalculandPushinStack(input_string_sym);
                            }
                            else
                            {
                                AllCalculPushInStack(input_string_sym);
                            }
                        }
                        break;

                    case '*':
                    case '/':
                        if (input_string_sym == '(') PutinStackandOutput(input_string_sym);
                        else
                        {
                            if (input_string_sym == '*' || input_string_sym == '/')
                                OnceCalculandPushinStack(input_string_sym);
                            else
                            {
                                AllCalculPushInStack(input_string_sym);
                            }
                        }
                        break;

                    case '(':
                        if (input_string_sym == ')') WatchDigit();
                        else
                        {
                            if (input_string_sym != '_') PutinStackandOutput(input_string_sym);
                        }
                        break;

                    case '_':
                        if (input_string_sym != '_' && input_string_sym != ')') PutinStackandOutput(input_string_sym);
                        break;
                }
        }

        /// <summary>
        /// I-по спецификации...
        /// Команда добавляет оператор в стэк...
        /// </summary>
        /// <param name="input_string_sym">Текущий символ в строке...</param>
        private void PutinStackandOutput(char input_string_sym)
        {
            if (digits.Count == 0 && input_string_sym != '(') throw new NullReferenceException();
            operators.Push(input_string_sym);
        }

        /// <summary>
        /// II-по спецификации...
        ///Реализует выполнение ОДНОЙ операции...
        /// </summary>
        /// <param name="input_string_sym">Текущий символ в строке...</param>
        private void OnceCalculandPushinStack(char input_string_sym)
        {
            double new_digit = ReturnResult();
            digits.Push(new_digit);
            output_string += operators.Pop();
            PutinStackandOutput(input_string_sym);
        }


        private void WatchDigit()
        {
            output_string += digits.Pop();
            operators.Pop();
        }

        /// <summary>
        /// IV-команда по спецификации...
        /// Реализует досчет либо до конца строки,либо до первой встречающейся открыв.скобки...
        /// </summary>
        /// <param name="input_string_sym">Текущий символ в строке...</param>
        private void AllCalculPushInStack(char input_string_sym)
        {
            while(operators.Count!=0&&operators.Peek()!='(')
            {
                double new_digit = ReturnResult();
                digits.Push(new_digit);
                output_string += operators.Pop();
            }
            if (operators.Count!=0&&operators.Peek() == '(') operators.Pop();
            if(input_string_sym!=')') PutinStackandOutput(input_string_sym);
        }

        /// <summary>
        /// Метод реализующий математические вычисления...
        /// </summary>
        /// <returns>Возвращает результат математической операции...</returns>
        private double ReturnResult()
        {
            double digit_2 = digits.Pop();
            double digit_1 = digits.Pop();
            double result = 0;
            switch (operators.Peek())
            {
                case '+':
                    result=digit_1 + digit_2;
                    break;
                case '-':
                    result = digit_1 - digit_2;
                    break;
                case '*':
                    result = digit_1 * digit_2;
                    break;
                case '/':
                    result = digit_1 / digit_2;
                    break;
            }

            return result;
        }

        //private bool Trans(string input_string,char input_string_sym)
        //{
        //    char[] sym = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        //    if (input_string_sym == '(' || input_string_sym == ')')
        //    {
        //        if (input_string_sym == '('&&sym.Contains(input_string[1])) return true; return false;
        //        if (input_string_sym == ')' && !sym.Contains(input_string[1])) return true; return false;
        //    }
        //    else return false;
        //}

        /// <summary>
        /// Вспомогательный метод по пересчету реальных знаков в стеке операторов...
        /// </summary>
        /// <returns>Количество реальных знаков...</returns>
        private int CalculOperators()
        {
            int count = 0;
            foreach (char oper in operators)
            {
                if (oper != '(') count++;
            }
            return count;
        }

        /// <summary>
        /// Публичный метод реализующий всю логику по реализации однопроходного
        /// пересчета строки...
        /// </summary>
        /// <param name="input_string">Входная строка,заранее корректная(правильная расстановка
        /// скобок,удаление лишних пробелов и тд.)...</param>
        /// <returns>Результат вычислений...</returns>
        public string CalculString(string input_string)
        {
            digits = new Stack<double>();
            operators = new Stack<char>();
            output_string = null;
            string new_digit=null;

            while (input_string.Length > 0)
            {
                double result = 0;
                if (!double.TryParse(input_string[0].ToString(), out result))
                {
                    if (new_digit != null)
                    {
                        if (digits.Count == CalculOperators())
                        {
                            digits.Push(Convert.ToDouble(new_digit));
                            output_string += new_digit+" ";
                        }
                        else return "Error";
                    }
                    new_digit = null;
                    try
                    {
                        TransitionProcedure(input_string[0]);
                    }
                    catch
                    {
                        return "Error";
                    }
                }
                else
                {
                    new_digit += input_string[0].ToString();
                }
                input_string = input_string.Remove(0, 1);
            }
            if ((new_digit != null&&digits.Count+1>1))
            {
                digits.Push(Convert.ToDouble(new_digit));
                TransitionProcedure('_');
            }

            if (new_digit == null && (digits.Count - operators.Count) == 1) TransitionProcedure('_');

            return digits.Pop().ToString();
        }
    }
}
