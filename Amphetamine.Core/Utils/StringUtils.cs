using System;
using System.Collections.Generic;
using System.Linq;

namespace Amphetamine.Core.Utils
{
    public static class StringUtils
    {
        public static string FirstCharToUpper(string input) => string.IsNullOrEmpty(input) ? input : input.First<char>().ToString().ToUpper() + input.Substring(1);

        public static string[] SplitWords(string inputString) => ((IEnumerable<string>) inputString.Split('"')).Select<string, string[]>((Func<string, int, string[]>) ((element, index) => index % 2 != 0 ? new string[1]
        {
            element
        } : element.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries))).SelectMany<string[], string>((Func<string[], IEnumerable<string>>) (element => (IEnumerable<string>) element)).ToArray<string>();
    }
}