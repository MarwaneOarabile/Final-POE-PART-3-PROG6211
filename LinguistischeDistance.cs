using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// code manpulated from https://www.geeksforgeeks.org/dsa/introduction-to-levenshtein-distance/
// code modified to fit the needs of the CyberBot project
namespace CyberBotPart3
{
    // german word thing 

    public static class LinguistischeDistance
    {
       
            public static int LevenshteinDistance(string s, string t)
            {
                if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
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
                            Math.Min(d[i - 1, j] + 1,     
                                     d[i, j - 1] + 1),    
                                     d[i - 1, j - 1] + cost); 
                    }
                }

                return d[s.Length, t.Length];
            }

            public static string LinguistiDistance(string input, Dictionary<string, List<string>> intents)
            {
                input = input.ToLower().Trim();
                string bestMatch = null;
                int bestScore = int.MaxValue;

                foreach (var intent in intents)
                {
                    foreach (string phrase in intent.Value)
                    {
                        string lowerPhrase = phrase.ToLower();
                        int dist = LevenshteinDistance(input, lowerPhrase);

                        // Calculate dynamic threshold based on phrase length
                        int threshold = CalculateThreshold(lowerPhrase);

                        // Apply minimum length requirement
                        bool lengthValid = input.Length >= lowerPhrase.Length / 2;

                        if (dist < bestScore && dist <= threshold && lengthValid)
                        {
                            bestScore = dist;
                            bestMatch = intent.Key;
                        }
                    }
                }

                return bestMatch;
            }

            private static int CalculateThreshold(string phrase)
            {
                int length = phrase.Length;

                // Stricter threshold rules
                return length switch
                {
                    <= 3 => 0,        // No typos allowed for very short phrases
                    <= 5 => 1,        // Max 1 typo for short phrases
                    <= 10 => 2,       // Max 2 typos for medium phrases
                    _ => 3            // Max 3 typos for long phrases
                };
            }
        }
    }


