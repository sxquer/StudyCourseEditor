using System.IO;
using System.Xml.Serialization;

namespace StudyCourseEditor.Tools
{
    /// <summary>
    /// Класс-обертка для работы с XML
    /// </summary>
    public static class XmlManager
    {
        /// <summary>
        /// Преобразует объекты с XML-строку
        /// </summary>
        /// <param name="objToConvert">Объект, который необходимо преобразовать</param>
        /// <returns>XML-строка</returns>
        public static string SerializeObject<T>(T objToConvert)
        {
            var writer = new StringWriter();
            new XmlSerializer(typeof(T)).Serialize(writer, objToConvert);
            return writer.ToString();
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