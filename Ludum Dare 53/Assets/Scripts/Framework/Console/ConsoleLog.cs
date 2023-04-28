using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.Framework
{
    public struct ConsoleLog
    {
        public ConsoleLevel level;
        public string log;

        public ConsoleLog(ConsoleLevel level, string log)
        {
            this.level = level;
            this.log = log;
        }

        public override string ToString()
        {
            return $"{level.Prepend()}{log}";
        }
    }
}