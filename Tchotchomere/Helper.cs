using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Tchotchomere
{
    public static class Helper
    {
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string CareTitle(this string input)
        {
            string[] splitinput = input.Split(' ');
            string newstring = string.Empty;
            foreach (var word in splitinput)
            {
                if (!word.ToLower().Contains("temporada"))
                {
                    if (!((word.Contains("º")) || (word.Contains("ª"))))
                    {
                        newstring += word + " ";
                    }
                }
            }
            return newstring.TrimEnd();
        }
        public static int GetSeason(this string url)
        {
            int index = url.IndexOf("-temporada", 0);
            int x = 0;
            if (index > -1)
            {
                string str = url.Remove(index);
                var split = str.Split('-');
                string numberOnly = Regex.Replace(split[split.Length - 1], "[^0-9.]", "");
                x = Convert.ToInt32(numberOnly);
            }
            return x;
        }
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            ub.Query = httpValueCollection.ToString();

            return ub.Uri;
        }
        public static class LevenshteinDistance
        {
            public static int Compute(string s, string t)
            {
                if (string.IsNullOrEmpty(s))
                {
                    if (string.IsNullOrEmpty(t))
                        return 0;
                    return t.Length;
                }

                if (string.IsNullOrEmpty(t))
                {
                    return s.Length;
                }

                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // initialize the top and right of the table to 0, 1, 2, ...
                for (int i = 0; i <= n; d[i, 0] = i++) ;
                for (int j = 1; j <= m; d[0, j] = j++) ;

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= m; j++)
                    {
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                        int min1 = d[i - 1, j] + 1;
                        int min2 = d[i, j - 1] + 1;
                        int min3 = d[i - 1, j - 1] + cost;
                        d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                    }
                }
                return d[n, m];
            }
        }
    }
}
