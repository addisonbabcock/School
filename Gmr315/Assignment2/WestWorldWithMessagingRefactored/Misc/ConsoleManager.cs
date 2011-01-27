using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WestWorldWithMessagingRefactored.Misc
{
    public static class ConsoleManager
    {
        public static readonly ConsoleColor _originalForegroundColor = Console.ForegroundColor;
        public static readonly ConsoleColor _originalBackgroundColor = Console.BackgroundColor;

        public static void SetOutputColor(ConsoleColor foreground)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = _originalBackgroundColor;
        }

        public static void SetOutputColor(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        public static void RestoreOriginalColors()
        {
            Console.ForegroundColor = _originalForegroundColor;
            Console.BackgroundColor = _originalBackgroundColor;

        }

        public static void PressAnyKeyToContinue()
        {
            RestoreOriginalColors();
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
