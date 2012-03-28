using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace StudyCourseEditor.Tools
{
    /// <summary>
    /// Класс-обертка для работы с XML
    /// </summary>
    public static class XmlManager
    {
        /// <summary>
        /// Преобразует объекты в XML-строку (UTF-16)
        /// </summary>
        /// <param name="objToConvert">Объект, который необходимо преобразовать</param>
        /// <returns>XML-строка</returns>
        public static string SerializeObjectUTF16<T>(T objToConvert)
        {
            var writer = new StringWriter();
            
            new XmlSerializer(typeof(T)).Serialize(writer, objToConvert);
            return writer.ToString();
        }

        /// <summary>
        /// Преобразует объекты в XML-строку (UTF-8)
        /// </summary>
        /// <param name="objToConvert">Объект, который необходимо преобразовать</param>
        /// <returns>XML-строка</returns>
        public static string SerializeObjectUTF8<T>(T objToConvert)
        {
            var serializer = new XmlSerializer(typeof(T));
            Stream stream = new MemoryStream();

            var xtWriter = new System.Xml.XmlTextWriter(stream, Encoding.UTF8);
            serializer.Serialize(xtWriter, objToConvert);
            xtWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Преобразует объекты с XML-строку
        /// </summary>
        /// <param name="xmlString">XML-строка, из которой будет восстановлен объект</param>
        /// <returns>Распакованный экземпляр заданного класса</returns>
        public static T DeserializeObject<T>(string xmlString)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xmlString));
        }
    }
}