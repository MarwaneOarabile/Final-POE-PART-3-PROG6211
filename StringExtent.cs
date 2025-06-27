using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberBotPart3
{
    public static class StringExtensions
    {
        public static bool ContainsAny(this string input, params string[] keywords)
        {
            return keywords.Any(k => input.Contains(k));
        }
    }
}
