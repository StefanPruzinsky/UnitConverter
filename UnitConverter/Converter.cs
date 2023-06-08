using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitConverter
{
    /// <summary>
    /// Class represents universal unit convertor.
    /// </summary>
    class Converter
    {
        public UIHelper UIHelper { get; private set; }
        public QuantitiesReader QuantitiesReader { get; private set; }

        public Converter(UIHelper uiHelper)
        {
            UIHelper = uiHelper;
            QuantitiesReader = new QuantitiesReader("quantities.xml");
        }

        /// <summary>
        /// Converts required value to corresponding value.
        /// </summary>
        public void Convert()
        {
            List<decimal> offsetsOfConvertingUnits = QuantitiesReader.GetOffsetsOfConvertingUnits(UIHelper.QuantityCombo_SelectedIndex, new List<string> { UIHelper.FromCombo_SelectedItem, UIHelper.ToCombo_SelectedItem});
            decimal result = (UIHelper.FromTextBox_Text * offsetsOfConvertingUnits[0]) / offsetsOfConvertingUnits[1];

            UIHelper.ToTextBox_Text = result.ToString("G29");
        }
    }
}
