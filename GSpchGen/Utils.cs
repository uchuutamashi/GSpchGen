using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSpchGen
{
    static class Utils
    {
        static public string GetLang(string JSON)
        {
            int level = 0;
            int index = 0;
            string lang="";

            foreach (char ch in JSON)
            {                
                if (ch == '[')
                {
                    level++;
                }
                else if (ch == ']')
                {
                    level--;
                }
                else if (ch == ',' && level==1)
                {
                    index++;
                }
                else if (level == 1 && index == 2 && ch != '"')
                {
                    lang += ch;
                }

                if (index > 2) { break; }
            }

            return lang;
        }
    }
}
