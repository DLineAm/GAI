using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace REG_MARK_LIB
{
    public static class RegMark
    {
        /// <summary>
        /// Проверяет переданный номерной знак в формате a999aa999  (латинскими буквами) и возвращает true или false в зависимости от правильности номерного знака. Метод учитывает также и существующие номера регионов. 
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static bool CheckMark(string mark)
        {
            if (mark.Length != 9) return false;
            var filter = @"^[A-Z0-9\-]*?$";
            if (!Regex.IsMatch(mark, filter)) return false;

            var allowedLetters = new[] {'A', 'B', 'E', 'K', 'M', 'H', 'O', 'P', 'C', 'T', 'Y', 'X'};

            for (var i = 0; i < mark.Length; i++)
            {
                var c = mark[i];
                if ((i >= 1 && i <= 3) || (i >= 6 && i <= 8))
                {
                    if (!char.IsDigit(c))
                        return false;
                }
                else
                {
                    if (char.IsDigit(c)) return false;
                    if (allowedLetters.ToList().IndexOf(c) == -1) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Выдает следующий номер в данной серии или создает следующую серию.
        /// </summary>
        /// <param name="mark">Номерной знак в формате a999aa999 (латинскими буквами)</param>
        /// <returns></returns>
        public static string GetMarkAfter(string mark)
        {
            if (!CheckMark(mark)) return "incorrent mark";

            //получаем регистрационный номер в номерном знаке
            var str = Convert.ToInt32(
                new string(mark.Where(p => mark.IndexOf(p) >= 1 && mark.IndexOf(p) <= 3).ToArray()));
            //преобразуем регистрационный номер в правильный формат
            var num = str < 1000 ? str : Convert.ToInt32(str.ToString().Remove(3));
            var result = mark.ToArray();
            var allowedLetters = new[] {'A', 'B', 'E', 'K', 'M', 'H', 'O', 'P', 'C', 'T', 'Y', 'X'};
            var checkedLetters = 0;

            for (var i = mark.Length - 1; i >= 0; i--)
            {
                if(i > 5) continue;

                var ch = mark[i];
                if (char.IsDigit(ch)) continue;
                if (ch == 'X')
                {
                    ++checkedLetters;
                    continue;
                }

                var index = allowedLetters.ToList().IndexOf(ch);
                result[i] = allowedLetters[++index];
                if (checkedLetters == 0 )
                    return new string(result);
                result[ checkedLetters > 1 
                    ? i + 2 + checkedLetters
                    : i + checkedLetters ] = 'A';
                result[checkedLetters - 1 <= 0
                    ? i + checkedLetters
                    : checkedLetters > 1 
                ? i + 3 + checkedLetters
                : i + (checkedLetters - 1)] = 'A';
                return new string(result);
            }

            if (checkedLetters == 3)
            {
                if (num == 999) return "out of stock";

                ++num;
                for (var i = 1; i < 4; i++)
                {
                    result[i] = num.ToString()[i - 1];
                }
            }

            return new string(result);
        }

        /// <summary>
        /// Выдает следующий номер в данной данном промежутке номеров rangeStart до rangeEnd (включая обе границы). 
        /// </summary>
        /// <param name="prevMark">номерной знак в формате a999aa999 (латинскими буквами)</param>
        /// <param name="rangeStart"></param>
        /// <param name="rangeEnd"></param>
        /// <returns>Если нет возможности выдать следующий номер, необходимо вернуть сообщение “out of stock”.</returns>
        public static string GetNextMarkAfterInRange(string prevMark, string rangeStart, string rangeEnd)
        {
            if (!CheckMark(prevMark)) return "incorrent prevMark";
            if (!CheckMark(rangeStart)) return "incorrent rangeStart";
            if (!CheckMark(rangeEnd)) return "incorrent rangeEnd";

            var compareResult = Compare(rangeStart, rangeEnd);
            if (compareResult == 1) return string.Empty;
            if (compareResult == 0) return rangeStart;

            return GetMarkAfter(rangeStart);
        }

        /// <summary>
        ///  Метод необходим, чтобы рассчитать оставшиеся свободные номера для региона.
        /// </summary>
        /// <param name="mark1">Номер в формате a999aa999 (латинскими буквами)</param>
        /// <param name="mark2">Номер в формате a999aa999 (латинскими буквами)</param>
        /// <returns>Количество возможных номеров между двумя указанными номерными знаками (включая обе границы).</returns>
        public static int GetCombinationsCountInRange(string mark1, string mark2)
        {
            if (!CheckMark(mark1)) return -1;
            if (!CheckMark(mark2)) return -1;

            var compareResult = Compare(mark1, mark2);
            if (compareResult == 1) return 0;
            if (compareResult == 0) return 1;

            var combinationsCounter = 0;
            var isSuccesed = false;
            var mark = "";

            while (!isSuccesed)
            {
                mark = GetMarkAfter(combinationsCounter == 0 ? mark1 : mark);
                if (mark == "out of stock") return combinationsCounter;
                ++combinationsCounter;
                if(Compare(mark, mark2) == 0)
                    isSuccesed = !isSuccesed;
            }

            return combinationsCounter;
        }

        private static int ToInt(string mark)
        {
            var convertedMark1 = "";

            for (var i = 0; i < mark.Length; i++)
            {
                if(i > 5) break;

                var ch = mark[i];

                if(i >=1 && i <= 3)
                {
                    convertedMark1 += Convert.ToInt32(ch.ToString()).ToString();
                    continue;
                }

                convertedMark1 += (int) ch;
            }

            var intConvMark1 = Convert.ToInt32(convertedMark1);

            return intConvMark1;
        }

        private static int Compare(string mark1, string mark2)
        {
            //var convertedMark1 = "";
            //var convertedMark2 = "";

            //for (var i = 0; i < mark1.Length; i++)
            //{
            //    if(i > 5) break;

            //    var ch1 = mark1[i];
            //    var ch2 = mark2[i];

            //    if(i >=1 && i <= 3)
            //    {
            //        convertedMark1 += Convert.ToInt32(ch1.ToString()).ToString();
            //        convertedMark2 += Convert.ToInt32(ch2.ToString()).ToString();
            //        continue;
            //    }

            //    convertedMark1 += (int) ch1;
            //    convertedMark2 += (int) ch2;
            //}

            var intConvMark1 = ToInt(mark1);
            var intConvMark2 = ToInt(mark2);
            //var intConvMark2 = Convert.ToInt32(convertedMark2);

            if (intConvMark1 > intConvMark2) return 1;
            if (intConvMark1 < intConvMark2) return -1;

            return 0;
        }
    }
}
