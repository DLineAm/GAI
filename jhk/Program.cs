using System;
using System.Linq;
using System.Text.RegularExpressions;
using REG_MARK_LIB;
using VIN_LIB;

namespace jhk
{
    class Program
    {
        //Для тестирования библиотек REG_MARK_LIB и VIN_LIB;
        static void Main(string[] args)
        {
            var s = "JHMCM56557C404453";
            var z = "T168TT067";
            Console.WriteLine(RegMark.CheckMark(z));
            Console.WriteLine(RegMark.GetMarkAfter(z));
            Console.WriteLine(RegMark.GetNextMarkAfterInRange(z, "T169TT067", "T171TT067"));
            Console.WriteLine(RegMark.GetCombinationsCountInRange("T168TT067", "X168XX067"));
            Console.WriteLine(VIN.CheckVIN(s));
            Console.WriteLine(VIN.GetVINCountry(s));
            Console.ReadKey();
        }

    }
}
