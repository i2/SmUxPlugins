using System;
using System.IO;
using System.Xml.Serialization;

namespace PluginSystem.Helpers
{
    public static class SerializationHelper
    {
        public static void Serialize<T>(Stream s, T t)
        {
            Serialize(s, t, typeof(T));
        }

        public static void Serialize<T>(Stream s, T t, Type type)
        {
            var serializer = new XmlSerializer(type);
            serializer.Serialize(s, t);
        }

        private static object Deserialize(Type type, Stream stream)
        {
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(stream);
        }

        public static T Deserialize<T>(Stream s)
        {
            return (T)Deserialize(typeof(T), s);
        }

        public static void SerializeToFile(string fileName, object value, Type type)
        {
            using (var stream = new MemoryStream())
            {
                Serialize(stream, value, type);
                stream.Seek(0, SeekOrigin.Begin);
                StreamHelper.WriteStreamToFile(stream, fileName);
            }
        }

        public static void SerializeToFile<T>(string fileName, T t)
        {
            SerializeToFile(fileName, t, typeof(T));
        }

        public static T DeserializeFromFile<T>(string fileName)
        {
            return (T)DeserializeFromFile(typeof(T), fileName);
        }

        public static object DeserializeFromFile(Type type, string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open))
            {
                return Deserialize(type, file);
            }
        }
    }
}
