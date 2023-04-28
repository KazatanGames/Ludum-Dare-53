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
    public static class Convert
    {
        public static float ByteToFloat(byte ro)
        {
            return ((float)ro) / 0x100;
        }

        public static byte FloatToByte(float ro)
        {
            return (byte)(ro * 0x100);
        }

    }
}