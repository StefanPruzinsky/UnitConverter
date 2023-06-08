using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;

namespace UnitConverter
{
    /// <summary>
    /// Class represents word translator builded on Yandex API.
    /// </summary>
    class Translator
    {
        public WebClient WebClient { get; private set; }
        public XDocument XDocument { get; private set; }
        public string APIKey { get; private set; }

        public Translator()
        {
            WebClient = new WebClient();
            APIKey = "trnsl.1.1.20150701T215834Z.e6e72be79301c859.e59ea0b37232fcf83c322e61c8671ce45502c6a4";
        }

        /// <summary>
        /// Translates required string.
        /// </summary>
        /// <param name="text">Text to translate.</param>
        /// <param name="inputLanguage">Input language.</param>
        /// <param name="outputLanguage">Output language</param>
        /// <returns>Translated text.</returns>
        public List<string> Translate(List<string> text, string inputLanguage, string outputLanguage)
        {
            WebClient.Encoding = Encoding.UTF8;
            string request = "https://translate.yandex.net/api/v1.5/tr/translate?key=" + APIKey + "&lang=" + inputLanguage + "-" + outputLanguage;
            foreach (string s in text)
                request += "&text=" + s;

            byte[] decodedArray = Encoding.Default.GetBytes(WebClient.DownloadString(request));
            XDocument = XDocument.Parse(WebClient.DownloadString(request));//Encoding.UTF8.GetString(decodedArray));

            var query = from x in XDocument.Element("Translation").Elements("text")
                        select x.Value;

            return query.ToList();
        }

        /// <summary>
        /// Returns List of languages which are situated in "langs.xml".
        /// </summary>
        /// <returns>List of languages from "langs.xml" file.</returns>
        public List<string> GetListOfLanguages()
        {
            XDocument xDocumentLangs = XDocument.Load("langs.xml");

            var query = from x in xDocumentLangs.Element("Langs").Elements("Item")
                        select x.Attribute("value").Value;

            return query.ToList();
        }

        /// <summary>
        /// Returns language key based on "key" attribute in "langs.xml".
        /// </summary>
        /// <param name="languageName">Name of language.</param>
        /// <returns>Returns language key based on "key" attribute in "langs.xml".</returns>
        public string GetKeyByLanguageName(string languageName)
        {
            XDocument xDocumentLangs = XDocument.Load("langs.xml");

            var query = from x in xDocumentLangs.Element("Langs").Elements("Item")
                        where x.Attribute("value").Value.Contains(languageName)
                        select x.Attribute("key").Value;

            return query.ElementAt(0);
        }

        /// <summary>
        /// Returns language name based on "value" attribute in "langs.xml".
        /// </summary>
        /// <param name="key">Language key.</param>
        /// <returns>Returns language name based on "value" attribute in "langs.xml".</returns>
        public string GetLanguageNameByKey(string key)
        {
            XDocument xDocumentLangs = XDocument.Load("langs.xml");

            var query = from x in xDocumentLangs.Element("Langs").Elements("Item")
                        where x.Attribute("key").Value.Contains(key)
                        select x.Attribute("value").Value;

            return query.ElementAt(0);
        }

        /// <summary>
        /// Gets document with translated languages.
        /// </summary>
        private void GetDocumentWithTranslatedLanguages()
        {
            XDocument xDocumentLangs;
            string request = "https://translate.yandex.net/api/v1.5/tr/getLangs?key=" + APIKey + "&ui=en";
            byte[] decodedArray = Encoding.Default.GetBytes(WebClient.DownloadString(request));
            xDocumentLangs = XDocument.Parse(Encoding.UTF8.GetString(decodedArray));

            var query = from x in xDocumentLangs.Element("Langs").Elements("Item")
                        select x;
            Translator translator;
            foreach (XElement xE in query)
            {
                translator = new Translator();
                xE.Attribute("value").Value = translator.Translate(new List<string>() { xE.Attribute("value").Value }, "en", xE.Attribute("key").Value)[0];
            }

            xDocumentLangs.Save("langs.xml");
        }
    }
}
