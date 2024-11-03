using System.Xml;

namespace TelegramBot
{
    internal class SourceConnection
    {
        //get your directory
        public static string? appPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// make sure input string is the same as element name
        /// </summary>
        /// <param name="xmlTag"></param>
        /// <returns>stored properties</returns>
        public static string? LoadPropertiesFromXML(string xmlTag)
        {
            string fileName = appPath + @"/Source/Properties.xml";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
                string? properties = Convert.ToString(doc.GetElementsByTagName(xmlTag)[0].InnerText);
                return properties;
            }
            catch (Exception)
            {
                //add exception log here
                return null;
            }
        }
    }
}
