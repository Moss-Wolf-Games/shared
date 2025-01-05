using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MossWolfGames.Shared.Runtime.Extensions
{
    public static class StringExtensions
    {
        private const char Space = ' ';
        private static readonly StringBuilder stringBuilder= new StringBuilder();

        public static string AddSpacesToPascalCase(this string originalString)
        {
            stringBuilder.Clear();
            for(int i = 0; i < originalString.Length; i++)
            {
                char c = originalString[i];
                if(char.IsUpper(c))
                {
                    stringBuilder.Append(Space);
                }
                
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Trim();
        }

        public static string SetColor(this string originalString, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{originalString}</color>";
        }
    }
}