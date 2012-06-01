using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StudyCourseEditor.Tools
{
    /// <summary>
    /// Wrap around XML
    /// </summary>
    public static class XmlManager
    {
        /// <summary>
        /// Serialize object into UTF-16 string
        /// </summary>
        /// <param name="objToConvert"></param>
        /// <returns></returns>
        public static string SerializeObjectUTF16<T>(T objToConvert)
        {
            var writer = new StringWriter();

            new XmlSerializer(typeof (T)).Serialize(writer, objToConvert);
            return writer.ToString();
        }

        /// <summary>
        /// Serialize object into UTF-8 string
        /// </summary>
        /// <param name="objToConvert"></param>
        /// <returns></returns>
        public static string SerializeObjectUTF8<T>(T objToConvert)
        {
            var serializer = new XmlSerializer(typeof (T));
            Stream stream = new MemoryStream();

            var xtWriter = new XmlTextWriter(stream, Encoding.UTF8);
            serializer.Serialize(xtWriter, objToConvert);
            xtWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Deserialize objects
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string xmlString)
        {
            return (T) new XmlSerializer(typeof (T)).Deserialize(new StringReader(xmlString));
        }
    }
}