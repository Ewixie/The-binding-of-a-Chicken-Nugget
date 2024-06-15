using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DataBase
    {
        private static string RecordsPath(string fileType)
        {
            return Application.persistentDataPath + "/" + fileType + ".bruh";
        }  
        
        public static object Read<T>()
        {
            if (System.IO.File.Exists(RecordsPath(typeof(T).ToString())))
            {
                var formatter = new BinaryFormatter();
                var file = System.IO.File.Open(RecordsPath(typeof(T).ToString()), System.IO.FileMode.Open);
                var data = (T) formatter.Deserialize(file);
                file.Close();
                return data;
            }

            return null;
        }

        public static void Save<T>(T data)
        {
            var formatter = new BinaryFormatter();
            var file = System.IO.File.Create(RecordsPath(typeof(T).ToString()));
            formatter.Serialize(file, data);
            file.Close();
        }
    }
}