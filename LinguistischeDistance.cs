using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberBotPart3
{
    // german word thing 

    public static class LinguistischeDistance
    {
        public static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1,     // deletion
                                 d[i, j - 1] + 1),    // insertion
                                 d[i - 1, j - 1] + cost); // substitution
                }
            }

            return d[s.Length, t.Length];
        }

        public static string LinguistiDistance(string input, Dictionary<string, List<string>> intents)
        {
            input = input.ToLower().Trim();
            string bestMatch = null;
            int bestScore = int.MaxValue;
            int threshold = 5; // max allowed edits

            foreach (var intent in intents)
            {
                foreach (string phrase in intent.Value)
                {
                    int dist = LevenshteinDistance(input, phrase.ToLower());
                    if (dist < bestScore && dist <= threshold)
                    {
                        bestScore = dist;
                        bestMatch = intent.Key;
                    }
                }
            }

            return bestMatch; // returns the intent name like "reminder" or null if no close match
        }
    }
}

