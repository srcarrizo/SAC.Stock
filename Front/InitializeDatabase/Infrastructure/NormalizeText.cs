namespace SAC.Stock.Front.InitializeDatabase.Infrastructure
{    
    using System.Linq; 
    public static class NormalizeText
    {
        public static string UpperCaseFirst(string nameOrDescription)
        {
            var words = nameOrDescription.Split(' ');

            return
              (from item in words let itemLower = item.ToLower() where item.Length > 0 && !item[0].Equals(' ') select itemLower).Aggregate(
                string.Empty, (current, itemLower) => current + " " + char.ToUpper(itemLower[0]) + itemLower.Substring(1)).Replace(" Y ", " y ");
        }

        public static string NameToCode(string name)
        {
            var upperCase = UpperCaseFirst(name).Replace(" y ", string.Empty).Replace(".", string.Empty);
            return CodeToCode(upperCase);
        }

        public static string CodeToCode(string code)
        {
            var result = string.Empty;
            foreach (var ch in code.Where(ch => ch != 32))
            {
                if ((ch == 46) || ((ch >= 48) && (ch <= 57)) || ((ch >= 65) && (ch <= 90)) || ((ch >= 97) && (ch <= 122)))
                {
                    result += ch;
                }
                else
                {
                    result += '_';
                }
            }

            return result;
        }
    }
}
