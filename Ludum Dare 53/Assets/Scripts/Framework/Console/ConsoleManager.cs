using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Pew Pew Pew 2023
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.PewPewPew2023
{
    public class ConsoleManager : Singleton<ConsoleManager>
    {
        public bool logToUnityConsole = true;

        public Queue<ConsoleLog> logs = new();
        public int maxLogs = 250;

        public void WriteLog(ConsoleLevel level, string log)
        {
            while (logs.Count > maxLogs) logs.Dequeue();
            logs.Enqueue(new ConsoleLog(level, log));

            if (!logToUnityConsole) return;

            switch (level)
            {
                case ConsoleLevel.Error:
                    Debug.LogError(log);
                    break;
                case ConsoleLevel.Warning:
                    Debug.LogWarning(log);
                    break;
                default:
                    Debug.Log(log);
                    break;
            }
        }
    }
}