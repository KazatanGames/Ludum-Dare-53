using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/**
 * Pew Pew Pew 2023
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.Framework
{
    public static class SerializingHelpers
    {
        public static byte[] ToBytes(object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new();
            MemoryStream ms = new();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static T ToObject<T>(byte[] arrBytes)
        {
            MemoryStream ms = new();
            BinaryFormatter bf = new();
            ms.Write(arrBytes, 0, arrBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return (T)bf.Deserialize(ms);
        }
    }
}