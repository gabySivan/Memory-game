using System;
using Ex02.ConsoleUtils;

namespace B20_Ex02
{
    public class Program
    {
        public static void Main()
        {
            bool playAnother;
            do
            {
                playAnother = IOUtils.PlayGame();
                Screen.Clear();
            }
            while (playAnother == true);
        }
    }
}