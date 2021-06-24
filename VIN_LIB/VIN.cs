using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VIN_LIB
{
    public static class VIN
    {
        /// <summary>
        /// Проверяет переданный VIN номер и возвращает true или false в зависимости от правильности VIN номера. 
        /// VIN номер должен удовлетворять всем требованиям, а также у него должна сходиться контрольная сумма.
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public static bool CheckVIN(string vin)
        {
            if (vin.Length != 17)
                return false;
            if (vin.Contains("I") ||
                vin.Contains("O") ||
                vin.Contains("Q"))
                return false;

            if (!char.IsDigit(vin[16]) ||
                !char.IsDigit(vin[15]) ||
                !char.IsDigit(vin[14]))
                return false;

            if (!char.IsDigit(vin[8]) &&
                char.ToUpper(vin[8]) != 'X')
                return false;

            var numAnalog = new Dictionary<char, int>
            {
                {'A', 1}, {'B', 2}, {'C', 3},
                {'D', 4}, {'E', 5}, {'F', 6},
                {'G', 7}, {'H', 8}, {'J', 1},
                {'K', 2}, {'L', 3}, {'M', 4},
                {'N', 5}, {'P', 7}, {'R', 9},
                {'S', 2}, {'T', 3}, {'U', 4},
                {'W', 6}, {'X', 7}, {'Y', 8},
                {'Z', 9}
            };
            var height = new int[17] { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };
            var nums = 0;

            for (var i = 0; i < vin.Length; i++)
            {
                if (i == 8) continue;

                var ch = vin[i];
                if (char.IsDigit(ch))
                {
                    var n = Convert.ToInt32(ch.ToString());
                    nums += n * height[i];
                    continue;
                }

                int num;
                if (!numAnalog.TryGetValue(ch, out num))
                    return false;
                nums += num * height[i];
            }

            var chk = vin[8] != 'X' ? Convert.ToInt32(vin[8].ToString()) : 10;

            var div = (nums / 11) * 11;

            var result = nums - div;

            if (result == 10) return vin[8] == 'X';
            return chk == result;
        }

        /// <summary>
        /// Возвращает страну, в которой было изготовлено транспортное средство. 
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public static string GetVINCountry(string vin)
        {
            if (!CheckVIN(vin)) return "incorrent vin";

            var code = vin[0];
            var ccode = vin[1];

            if (code >= 'A' && code <= 'H')
            {
                switch (code)
                {
                    case 'A' when ccode >= 'A' && ccode <= 'H':
                        return "ЮАР";
                    case 'A' when ccode >= 'J' && ccode <= 'N':
                        return "Котд’Ивуар";
                    case 'A' when ccode >= 'P' && ccode <= '0':
                        return "not using";
                    case 'B' when ccode >= 'A' && ccode <= 'E':
                        return "Ангола";
                    case 'B' when ccode >= 'F' && ccode <= 'K':
                        return "Кения";
                    case 'B' when ccode >= 'L' && ccode <= 'R':
                        return "Танзания";
                    case 'B' when ccode >= 'S' && ccode <= '0':
                        return "not using";
                    case 'C' when ccode >= 'A' && ccode <= 'E':
                        return "Бенин";
                    case 'C' when ccode >= 'F' && ccode <= 'K':
                        return "Мадагаскар";
                    case 'C' when ccode >= 'L' && ccode <= 'R':
                        return "Тунис";
                    case 'C' when ccode >= 'S' && ccode <= '0':
                        return "not using";
                    case 'D' when ccode >= 'A' && ccode <= 'E':
                        return "Египет";
                    case 'D' when ccode >= 'F' && ccode <= 'K':
                        return "Марокко";
                    case 'D' when ccode >= 'L' && ccode <= 'R':
                        return "Замбия";
                    case 'D' when ccode >= 'S' && ccode <= '0':
                        return "not using";
                    case 'E' when ccode >= 'A' && ccode <= 'E':
                        return "Эфиопия";
                    case 'E' when ccode >= 'F' && ccode <= 'K':
                        return "Мозамбик";
                    case 'E' when ccode >= 'L' && ccode <= '0':
                        return "not using";
                    case 'F' when ccode >= 'A' && ccode <= 'E':
                        return "Гана";
                    case 'F' when ccode >= 'F' && ccode <= 'K':
                        return "Нигерия";
                    case 'F' when ccode >= 'L' && ccode <= '0':
                    case 'G':
                    case 'H':
                        return "not using";
                    default: return "not using";
                }
            }
            if (code >= 'J' && code <= 'R')
            {
                switch (code)
                {
                    case 'J' when ccode >= 'A' && ccode <= 'T':
                        return "Япония";
                    case 'K' when ccode >= 'A' && ccode <= 'E':
                        return "Шри Ланка";
                    case 'K' when ccode >= 'F' && ccode <= 'K':
                        return "Израиль";
                    case 'K' when ccode >= 'L' && ccode <= 'R':
                        return "Южная Корея";
                    case 'K' when ccode >= 'S' && ccode <= '0':
                        return "Казахстан";
                    case 'L':
                        return "Китай";
                    case 'M' when ccode >= 'A' && ccode <= 'E':
                        return "Индия";
                    case 'M' when ccode >= 'F' && ccode <= 'K':
                        return "Индонезия";
                    case 'M' when ccode >= 'L' && ccode <= 'R':
                        return "Таиланд";
                    case 'M' when ccode >= 'S' && ccode <= '0':
                        return "not using";
                    case 'N' when ccode >= 'F' && ccode <= 'K':
                        return "Пакистан";
                    case 'N' when ccode >= 'L' && ccode <= 'R':
                        return "Турция";
                    case 'N' when ccode >= 'T' && ccode <= '0':
                        return "not using";
                    case 'P' when ccode >= 'A' && ccode <= 'E':
                        return "Филиппины";
                    case 'P' when ccode >= 'F' && ccode <= 'K':
                        return "Сингапур";
                    case 'P' when ccode >= 'L' && ccode <= 'R':
                        return "Малайзия";
                    case 'P' when ccode >= 'S' && ccode <= '0':
                        return "not using";
                    case 'R' when ccode >= 'A' && ccode <= 'E':
                        return "ОАЭ";
                    case 'R' when ccode >= 'F' && ccode <= 'K':
                        return "Тайвань";
                    case 'R' when ccode >= 'L' && ccode <= 'R':
                        return "Вьетнам";
                    case 'R' when ccode >= 'S' && ccode <= '0':
                        return "Саудовская Аравия";
                    default: return "not using";
                }
            }
            if (code >= 'S' && code <= 'Z')
            {
                switch (code)
                {
                    case 'S' when ccode >= 'A' && ccode <= 'M':
                        return "Великобритания";
                    case 'S' when ccode >= 'N' && ccode <= 'T':
                        return "Германия";
                    case 'S' when ccode >= 'U' && ccode <= 'Z':
                        return "Польша";
                    case 'S' when ccode >= '1' && ccode <= '4':
                        return "Латвия";
                    case 'T' when ccode >= 'A' && ccode <= 'H':
                        return "Швейцария";
                    case 'T' when ccode >= 'J' && ccode <= 'P':
                        return "Чехия";
                    case 'T' when ccode >= 'R' && ccode <= 'V':
                        return "Венгрия";
                    case 'T' when ccode >= 'W' && ccode <= '1':
                        return "Португалия";
                    case 'T' when ccode >= '2' && ccode <= '0':
                        return "not using";
                    case 'U' when ccode >= 'A' && ccode <= 'G':
                        return "not using";
                    case 'U' when ccode >= 'H' && ccode <= 'M':
                        return "Дания";
                    case 'U' when ccode >= 'N' && ccode <= 'T':
                        return "Ирландия";
                    case 'U' when ccode >= 'U' && ccode <= 'Z':
                        return "Румыния";
                    case 'U' when ccode >= '1' && ccode <= '4':
                        return "not using";
                    case 'U' when ccode >= '5' && ccode <= '7':
                        return "Словакия";
                    case 'U' when ccode >= '8' && ccode <= '0':
                        return "not using";
                    case 'V' when ccode >= 'A' && ccode <= 'E':
                        return "Австрия";
                    case 'V' when ccode >= 'F' && ccode <= 'R':
                        return "Франция";
                    case 'V' when ccode >= 'S' && ccode <= 'W':
                        return "Испания";
                    case 'V' when ccode >= 'X' && ccode <= '2':
                        return "Сербия";
                    case 'V' when ccode >= '3' && ccode <= '5':
                        return "Хорватия";
                    case 'V' when ccode >= '6' && ccode <= '0':
                        return "Эстония";
                    case 'W' when ccode >= 'A' && ccode <= '0':
                        return "Германия";
                    case 'X' when ccode >= 'A' && ccode <= 'E':
                        return "Болгария";
                    case 'X' when ccode >= 'F' && ccode <= 'K':
                        return "Греция";
                    case 'X' when ccode >= 'L' && ccode <= 'R':
                        return "Нидерланды";
                    case 'X' when ccode >= 'S' && ccode <= 'W':
                        return "СССР/СНГ";
                    case 'X' when ccode >= 'X' && ccode <= '2':
                        return "Люксембург";
                    case 'X' when ccode >= '3' && ccode <= '0':
                        return "Россия";
                    case 'Y' when ccode >= 'A' && ccode <= 'E':
                        return "Бельгия";
                    case 'Y' when ccode >= 'F' && ccode <= 'K':
                        return "Финляндия";
                    case 'Y' when ccode >= 'L' && ccode <= 'R':
                        return "Мальта";
                    case 'Y' when ccode >= 'S' && ccode <= 'W':
                        return "Швеция";
                    case 'Y' when ccode >= 'X' && ccode <= '2':
                        return "Норвегия";
                    case 'Y' when ccode >= '3' && ccode <= '5':
                        return "Беларусь";
                    case 'Y' when ccode >= '6' && ccode <= '0':
                        return "Украина";
                    case 'Z' when ccode >= 'A' && ccode <= 'R':
                        return "Италия";
                    case 'Z' when ccode >= 'S' && ccode <= 'W':
                        return "not using";
                    case 'Z' when ccode >= 'X' && ccode <= '2':
                        return "Словения";
                    case 'Z' when ccode >= '3' && ccode <= '5':
                        return "Литва";
                    case 'Z' when ccode >= '6' && ccode <= '0':
                        return "Россия";
                    default: return "not using";
                }
            }
            if (code >= '1' && code <= '5')
            {
                switch (code)
                {
                    case '1' when ccode >= 'A' && ccode <= '0':
                        return "США";
                    case '2' when ccode >= 'A' && ccode <= '0':
                        return "Канада";
                    case '3' when ccode >= 'A' && ccode <= 'W':
                        return "Мексика";
                    case '3' when ccode >= 'X' && ccode <= '3':
                        return "Коста Рика";
                    case '3' when ccode >= '8' && ccode <= '0':
                        return "Каймановы острова";
                    case '4' when ccode >= 'A' && ccode <= '0':
                        return "США";
                    case '5' when ccode >= 'A' && ccode <= '0':
                        return "США";
                    default: return "not using";

                }
            }
            if (code >= '6' && code <= '7')
            {
                switch (code)
                {
                    case '6' when ccode >= 'A' && ccode <= 'W':
                        return "Австралия";
                    case '6' when ccode >= 'X' && ccode <= '0':
                        return "not using";
                    case '7' when ccode >= 'A' && ccode <= 'E':
                        return "Новая Зеландия";
                    case '7' when ccode >= 'F' && ccode <= '0':
                        return "not using";
                    default: return "not using";
                }
            }
            if (code >= '8' && code <= '9')
            {
                switch (code)
                {
                    case '8' when ccode >= 'A' && ccode <= 'E':
                        return "Аргентина";
                    case '8' when ccode >= 'F' && ccode <= 'K':
                        return "Чили";
                    case '8' when ccode >= 'L' && ccode <= 'R':
                        return "Эквадор";
                    case '8' when ccode >= 'S' && ccode <= 'W':
                        return "Перу";
                    case '8' when ccode >= 'X' && ccode <= '2':
                        return "Венесуэла";
                    case '8' when ccode >= '3' && ccode <= '0':
                        return "not using";
                    case '9' when ccode >= 'A' && ccode <= 'E':
                        return "Бразилия";
                    case '9' when ccode >= 'F' && ccode <= 'K':
                        return "Колумбия";
                    case '9' when ccode >= 'L' && ccode <= 'R':
                        return "Парагвай";
                    case '9' when ccode >= 'S' && ccode <= 'W':
                        return "Уругвай";
                    case '9' when ccode >= 'X' && ccode <= '2':
                        return "Тринидад и Тобаго";
                    case '9' when ccode >= '3' && ccode <= '9':
                        return "Бразилия";
                    default: return "not using";
                }
            }
            return "not using";
        }

        /// <summary>
        /// Возвращает год, в который было выпущено транспортное средство.
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public static int GetTransportYear(string vin)
        {
            if (!CheckVIN(vin)) return -1;

            var ch = vin[9];

            var years = new Dictionary<char, int>()
            {
                {'L', 1990}, {'M', 1991}, {'N', 1992},
                {'P', 1993}, {'R', 1994}, {'S', 1995},
                {'T', 1996}, {'V', 1997}, {'W', 1998},
                {'X', 1999}, {'Y', 2000}, {'A', 2010},
                {'B', 2011}, {'C', 2012}, {'D', 2013},
                {'E', 2014}, {'F', 2015}, {'G', 2016},
                {'H', 2017}, {'J', 2018}, {'K', 2019}
            };

            if (char.IsDigit(ch))
            {
                var num = Convert.ToInt32(ch.ToString());
                return Convert.ToInt32($"200{num}");
            }

            if (!years.TryGetValue(ch, out var result)) return -1;

            return result;

        }
    }
}
