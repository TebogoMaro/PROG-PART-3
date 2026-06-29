using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityBotGUI1 
    {
    //Class for detecting user sentiment/emotion
    internal class SentimentAnalyzer
    {
        //Static method for analyzing user input
        public static string Detect(string input)
        {
            //convert input to lowercase to make comparisons case-insensitive
            input = input.ToLower();
            //check if the user seems worried
            if(input.Contains("worried"))
                return "worried";
            //check if the user seems confused
            if (input.Contains("confused"))
                return "confused";
            //default sentiment if no keywords are found
            return "neutral";
        }
    }
}

