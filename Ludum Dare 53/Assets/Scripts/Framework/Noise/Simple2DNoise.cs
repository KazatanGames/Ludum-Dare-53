using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

/**
 * Pew Pew Pew 2023
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.PewPewPew2023
{
    public class Simple2DNoise : INoiseProvider
    {
        protected int width;
        protected int height;

        public Simple2DNoise(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public float GetPerlinValue(int x, int y, float scale, float offsetX, float offsetY)
        {
            float xCoord = ((float)x / width * scale) + offsetX;
            float yCoord = ((float)y / width * scale) + offsetY;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }

        public float GetVoronoiValue(int x, int y, float scale, float offsetX, float offsetY)
        {
            float xCoord = ((float)x / width * scale) + offsetX;
            float yCoord = ((float)y / width * scale) + offsetY;

            return noise.cnoise(new float2(xCoord, yCoord));
        }

        public float GetSimplexValue(int x, int y, float scale, float offsetX, float offsetY)
        {
            float xCoord = ((float)x / width * scale) + offsetX;
            float yCoord = ((float)y / width * scale) + offsetY;

            return noise.snoise(new float2(xCoord, yCoord));
        }
    }
}