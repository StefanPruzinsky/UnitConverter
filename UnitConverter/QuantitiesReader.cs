using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UnitConverter
{
    /// <summary>
    /// Class represents a processor for processing informations from file, in which is situated an optional number of quantities.
    /// </summary>
    class QuantitiesReader
    {
        public XDocument XDocument { get; private set; }

        public QuantitiesReader(string path)
        {
            XDocument = XDocument.Load(path, LoadOptions.None);
        }

        /// <summary>
        /// Gets list of quantities.
        /// </summary>
        /// <returns>Returns list of quantities.</returns>
        public List<string> GetListOfQuantities()
        {
            var query = from x in XDocument.Element("Quantities").Elements("Quantity")
                             select x.Attribute("name").Value;

            List<string> listOfQuantities = new List<string>();

            foreach (string s in query)
                listOfQuantities.Add(s);

            return listOfQuantities;
        }

        /// <summary>
        /// Gets list of quantity values.
        /// </summary>
        /// <param name="quantityIndex">Index of quantity.</param>
        /// <returns>Returns list of quantity values.</returns>
        public List<string> GetListOfQuantityValues(int quantityIndex)
        {
            var query = from x in XDocument.Element("Quantities").Elements("Quantity").ElementAt(quantityIndex).Elements()
                        select x;

            List<string> listOfQuantityValues = new List<string>();

            foreach (XElement xE in query)
                listOfQuantityValues.Add(xE.Value);

            return listOfQuantityValues;
        }

        /// <summary>
        /// Gets value of main unit.
        /// </summary>
        /// <param name="quantityIndex"></param>
        /// <returns>Returns value of main unit.</returns>
        public string GetValueOfMainUnit(int quantityIndex)
        {
            var query = from x in XDocument.Element("Quantities").Elements("Quantity").ElementAt(quantityIndex).Elements("Unit")
                        where x.Attribute("offset").Value == "1"
                        select x.Value; //.ElementsBeforeSelf().Count();

            return query.ElementAt(0);
        }

        /// <summary>
        /// Gets offsets of converting units.
        /// </summary>
        /// <param name="quantityIndex">Index of quantity.</param>
        /// <param name="InputAndOutputValues">List of two items - input unit and output unit.</param>
        /// <returns>Returns offsets of converting units.</returns>
        public List<decimal> GetOffsetsOfConvertingUnits(int quantityIndex, List<string> InputAndOutputValues)
        {
            List<decimal> offsetsAccordingToIO = new List<decimal>();

            var queryInput = from x in XDocument.Element("Quantities").Elements("Quantity").ElementAt(quantityIndex).Elements("Unit")
                        where x.Value == InputAndOutputValues[0]
                        select decimal.Parse(x.Attribute("offset").Value);
            offsetsAccordingToIO.Add(queryInput.ElementAt(0));

            var queryOutput = from x in XDocument.Element("Quantities").Elements("Quantity").ElementAt(quantityIndex).Elements("Unit")
                             where x.Value == InputAndOutputValues[1]
                             select decimal.Parse(x.Attribute("offset").Value);
            offsetsAccordingToIO.Add(queryOutput.ElementAt(0));

            return offsetsAccordingToIO;
        }
    }
}
