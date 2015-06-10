using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSFinderPlus
{
    public static class FormHelper
    {
        public static string ValidateLastCharIsLetter(string stringToEvaluate)
        {
            if (!char.IsLetter(stringToEvaluate, stringToEvaluate.Length - 1))
                if (stringToEvaluate.Length > 1)
                    stringToEvaluate = stringToEvaluate.Remove(stringToEvaluate.Length - 1);
                else
                    stringToEvaluate = String.Empty;

            return stringToEvaluate;
        }

        public static byte ValidateLastCharIsInt(string stringToEvaluate)
        {
            byte evaluated = 0;
            int stringLenght = stringToEvaluate.Length;
            for (int i = 0; i < stringLenght; i++)
            {
                if (!byte.TryParse(stringToEvaluate, out evaluated))
                    stringToEvaluate = stringToEvaluate.Remove(stringToEvaluate.Length - 1);
            }
            return evaluated;
        }
    }
}
