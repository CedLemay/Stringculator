using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculatrice_Texte
{
    public class Calculator
    {
        //Tables
        static (char , bool ) [,,] SumTable;
        static (char , bool ) [,,] SubstractTable;
        static string[,]  MultiplicationTable;

        static Calculator()
        { InitiateTables(); }

        //
        static void Main(string[] args)
        {
            InitiateTables();


            bool isValid = false;
            string result = string.Empty;
            string sNum1 = string.Empty;
            string sNum2 = string.Empty;

            do
            {
                Console.Clear();

                Console.WriteLine(" 1- Addition \n 2- Soustraction \n 3- Multiplication \n 4- Division \n");

                Console.Write("Choix: ");

                result = Console.ReadLine();

                isValid = result[0] >= '1' && result[0] <= '4';

            } while (!isValid) ;

            GetBothNumbers(ref sNum1, ref sNum2);

            Console.Clear();

            switch (result[0])
            {
                case '1':

                    Console.WriteLine(sNum1 + " + " + sNum2 + " = " + AddNumbers(sNum1,sNum2));

                    break;
                case '2':

                    Console.WriteLine(sNum1 + " - " + sNum2 + " = " + SubstractNumbers(sNum1, sNum2));

                    break;
                case '3':

                    Console.WriteLine(sNum1 + " * " + sNum2 + " = " + MultiplyNumbers(sNum1, sNum2));

                    break;
                case '4':

                    Console.WriteLine(sNum1 + " / " + sNum2 + " = " + DivideNumbers(sNum1, sNum2));

                    break;
                default:
                    break;
            }

        }

        private static void InitiateTables()
        {
            SumTable = new (char , bool)[10, 10, 2];
            SubstractTable = new (char, bool)[10, 10, 2];
            MultiplicationTable = new string[10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    SumTable[i,j,0]             = ((char)((i + j) % 10 + 48), (i + j) > 9);
                    SumTable[i,j,1]             = ((char)((i + j + 1) % 10 + 48), (i + j + 1) > 9);
                                        
                    SubstractTable[i, j, 0]     = ((char)((i < j ? 10 + i - j : i - j) + '0'), i < j);
                    SubstractTable[i, j, 1]     = ((char)((i -1 < j ? 10 + i - 1 - j  : i - 1 - j) + '0'), i - 1 < j);
                    
                    MultiplicationTable[i, j]   = (i * j).ToString();
                }
            }
        }

        public static string AddNumbers(string n1, string n2)
        {
            int nLen = AdaptNumbers(ref n1,ref n2);

            Stack<char> resultStack = new Stack<char>();
                        
            bool hasCarry = false;
            bool leadingZero = n1.IndexOf(',') != -1 || n2.IndexOf(',') != -1;

            (char Result, bool CarriesOver) CharSum;


            int currentIndex;
            for (int i = 0; i < nLen; i++)
            {
                currentIndex = nLen - i - 1;

                if (n1[currentIndex] == ',')
                {

                    if (leadingZero)
                    {
                        resultStack.Clear();
                    }
                    else
                    {
                        resultStack.Push(',');
                    }

                    leadingZero = false;
                    continue;
                }

                CharSum = AddChars(n1[currentIndex], n2[currentIndex] ,hasCarry);

                hasCarry = CharSum.CarriesOver;

                if (leadingZero && CharSum.Result != '0')
                {
                    resultStack.Clear();
                    leadingZero = false;
                }

                resultStack.Push(CharSum.Result);
            }

            if (hasCarry)
                resultStack.Push('1');

            string sResult = string.Empty;


            while (resultStack.Count > 0)
            {
                sResult += resultStack.Pop();
            }            

            return sResult;
        }

        private static (char , bool ) AddChars(char c1, char c2, bool hasCarry)
        {
            return SumTable[c1 - '0', c2 - '0', hasCarry ? 1 : 0];
        }

        public static string SubstractNumbers(string n1, string n2)
        {
            bool willBeNegative = GetHighestAndLowest(out string highest, out string lowest, n1, n2);
             
            Stack<char> resultStack = new Stack<char>();

            int nLen = AdaptNumbers(ref highest, ref lowest);

            (char Result, bool MustBurrow) CharSubstract;

            bool mustBorrow = false;

            bool leadingZero = highest.IndexOf(',') != -1 || lowest.IndexOf(',') != -1;

            int currentIndex; 
            for (int i = 0; i < nLen; i++)
            {
                currentIndex = nLen - i - 1;

                if (n1[currentIndex] == ',')
                {
                    if (leadingZero)
                    {
                        resultStack.Clear();
                    }
                    else
                    {
                        resultStack.Push(',');
                    }

                    leadingZero = false;
                    continue;
                }

                CharSubstract = SubstractChars( highest[currentIndex], lowest[currentIndex] , mustBorrow);

                mustBorrow = CharSubstract.MustBurrow;

                if (leadingZero && CharSubstract.Result != '0')
                {
                    resultStack.Clear();
                    leadingZero = false;
                }               

                resultStack.Push(CharSubstract.Result);
            }

            //Leading 0
            while(resultStack.Peek() == '0' && resultStack.Count > 1)
            {
                resultStack.Pop();
            }

            if (willBeNegative)
                resultStack.Push('-');

            string sResult = string.Empty;


            while (resultStack.Count > 0)
            {
                sResult += resultStack.Pop();
            }



            return sResult;
        }

        private static (char , bool ) SubstractChars(char c1, char c2, bool mustBurrow)
        {
            return SubstractTable[c1 - '0', c2 - '0', mustBurrow ? 1 : 0];
        }

        public static string MultiplyNumbers(string n1, string n2)
        {
            string iterationResult;
            char c_n1, c_n2;

            int n1CommaPos = n1.IndexOf(',');
            int n2CommaPos = n2.IndexOf(',');

            bool isAnyDecimal = n1CommaPos != -1 || n2CommaPos != -1;

            int resultCommaPos = isAnyDecimal ? n1.Length + n2.Length - Math.Max(n1CommaPos,0) - Math.Max(n2CommaPos, 0)  - 2 : -1;

            string currentSum = "0";

            n1 = n1.Replace(",", "");
            n2 = n2.Replace(",", "");

            for (int i = 0; i < n1.Length; i++)
            {
                c_n1 = n1[n1.Length - 1 - i];
                for (int j = 0; j < n2.Length; j++)
                {
                    c_n2 = n2[n2.Length - 1 - j];
                    iterationResult = MultiplyChars(c_n1, c_n2) + new string('0', i + j);
                    currentSum = AddNumbers(currentSum, iterationResult);
                }
            }

            if (isAnyDecimal)
                currentSum = currentSum.Substring(0, currentSum.Length - resultCommaPos) + ',' + currentSum.Substring(currentSum.Length - resultCommaPos, resultCommaPos);

            CleanLeadingZeroes(ref currentSum);

            return currentSum;
        }

        private static void CleanLeadingZeroes(ref string n)
        {
            bool beforeDecimal = true;
            bool checkingForLeading = true;

            int preZeroEnd      = -1;

            int postZeroBegin   = -1;
            int postZeroEnd     = -1;

            foreach (var (c,i) in n.Select((c,i) => (c,i)))
            {
                if (c == ',')
                {
                    beforeDecimal = false;
                    checkingForLeading = true;
                }
                else if (c == '0' && checkingForLeading)
                {
                    if (checkingForLeading)
                    {
                        if(beforeDecimal)
                            preZeroEnd = i;

                    }
                    else
                    {

                    }

                    //if (beforeDecimal && preZeroEnd == i - 1)
                    //{
                    //    preZeroEnd = i;
                    //}
                    //else if (!beforeDecimal)
                    //{
                    //    if (postZeroBegin == -1)
                    //    {
                    //        postZeroBegin = i;
                    //    }
                    //    postZeroEnd = i;
                    //}
                }
                else
                {
                    if (beforeDecimal)
                        checkingForLeading = false;


                }


            }

            preZeroEnd = Math.Max(0, preZeroEnd);

            n = n.Substring(0, preZeroEnd) + n.Substring(preZeroEnd, postZeroBegin)

        }

        private static string MultiplyChars(char c1, char c2)
        {
            return MultiplicationTable[c1 - '0', c2 - '0'];
        }

        public static string DivideNumbers(string n1, string n2) 
        {
            string[] n2Multiplicator = new string[10];

            n2Multiplicator[0] = "0";
            n2Multiplicator[1] = n2;

            for (int i = 2; i < 10; i++)
            {
                n2Multiplicator[i] = MultiplyNumbers(n2, i.ToString());
            }

            string currentValue = string.Empty;
            string result = string.Empty;
            int curIndex = 0;
            int subIndex;


            while (currentValue != "0" && curIndex < n1.Length)
            {
                currentValue += n1[curIndex];

                subIndex = BinaryGet(currentValue, n2Multiplicator);

                result += subIndex;

                if (result == "0")
                    result = string.Empty;

                currentValue = SubstractNumbers(currentValue, n2Multiplicator[subIndex]);
                curIndex++;
            }


            return result;
        }

        public static int BinaryGet(string n, string[] values)
        {
            int l = 0;
            int r = values.Length - 1;
            int pos = 0;
            string posValue;

            while (l <= r)
            {
                pos = r - l / 2;
                posValue = values[pos];

                if (posValue == n)
                    break;
                else if (IsBigger(n, posValue))
                    l = pos + 1;
                else
                    r = pos - 1;
            }

            return pos;
        }

        private static bool IsBigger(string n1, string n2)
        {
            bool result = false;

            if (n1.Length > n2.Length)
            {
                result = true;
            }
            else if (n1.Length < n2.Length)
            {
                result = false;
            }
            else
            {
                int nLen = n1.Length;
                for (int i = 0; i < nLen; i++)
                {
                    if (n1[i] == n2[i])
                    {
                        continue;
                    }
                    else if (n1[i] > n2[i])
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        

        private static bool GetHighestAndLowest(out string h, out string l, string n1, string n2)
        {
            bool willBeNegative = false;

            h = n1;
            l = n2;

            if (n1 != n2 && !IsBigger(n1 , n2))
            { 
                h = n2;
                l = n1;
                willBeNegative = true;
            }            

            return willBeNegative;
        }

        private static void GetBothNumbers(ref string num1, ref string num2)
        {
            GetNumber(ref num1,true);
            GetNumber(ref num2,false);
        }

        private static void GetNumber(ref string num, bool isFirstNumber)
        {
            do
            {
                Console.Clear();
                Console.Write("Numero " +( isFirstNumber ? '1' : '2') + ": " );
                num = Console.ReadLine();

            } while (!Validate(num));
        }

        public static bool Validate(string s)
        {
            //dont want to use regex right now. i know it's better, but .... later alright?
            bool indexIsInDecimal = false;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == ',' && (indexIsInDecimal || i == s.Length - 1 || i == 0))
                    return false;
                else if (c == ',')
                    indexIsInDecimal = true;
                else if (!(c >= '0' && c <= '9'))
                    return false;
            }
            return true;
        }

        public static int AdaptNumbers(ref string n1, ref string n2)
        {
            int coma_pos_n1, coma_pos_n2;
            bool areBothInteger = false;

            coma_pos_n1 = n1.IndexOf(',');
            coma_pos_n2 = n2.IndexOf(',');

            if (coma_pos_n1 == -1 && coma_pos_n2 == -1)
                areBothInteger = true;

            int len_n1, len_n2;

            len_n1 = n1.Length;
            len_n2 = n2.Length;

            if (coma_pos_n1 == -1)
            { 
                coma_pos_n1 = len_n1++;                
                n1 += !areBothInteger ? "," : string.Empty;
            }
            
            if (coma_pos_n2 == -1 )
            {
                coma_pos_n2 = len_n2++;
                n2 += !areBothInteger ? "," : string.Empty;
            }

            int pref_zero_n1, pref_zero_n2;
            int suf_zero_n1, suf_zero_n2;

            int pref_zero_amount = Math.Abs(coma_pos_n1 - coma_pos_n2);

            pref_zero_n1 = coma_pos_n1 > coma_pos_n2 ? 0 : pref_zero_amount;
            pref_zero_n2 = coma_pos_n1 < coma_pos_n2 ? 0 : pref_zero_amount;

            int suf_zero_amount = Math.Abs((len_n1 - coma_pos_n1) - (len_n2 - coma_pos_n2));

            suf_zero_n1 = len_n1 - coma_pos_n1 > len_n2 - coma_pos_n2 ? 0 : suf_zero_amount;
            suf_zero_n2 = len_n1 - coma_pos_n1 < len_n2 - coma_pos_n2 ? 0 : suf_zero_amount;

            n1 = new string('0', pref_zero_n1) + n1 + new string('0', suf_zero_n1);
            n2 = new string('0', pref_zero_n2) + n2 + new string('0', suf_zero_n2);

            return n1.Length;
        }

    }
}
