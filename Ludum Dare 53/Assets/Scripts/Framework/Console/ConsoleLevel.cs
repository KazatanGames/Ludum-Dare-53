using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.Framework
{
    public enum ConsoleLevel
    {
        Verbose = 0,
        Debug = 1,
        Warning = 2,
        Error = 3
    }

    public static class ConsoleLevelHelpers
    {
        public static string Prepend (this ConsoleLevel level)
        {
            switch (level)
            {
                case ConsoleLevel.Verbose: return "";
                case ConsoleLevel.Debug: return "";
                case ConsoleLevel.Warning: return "WARN::";
                case ConsoleLevel.Error: return "ERROR::";
            }
            return "ERROR_UNKNOWN::";
        }
    }
}