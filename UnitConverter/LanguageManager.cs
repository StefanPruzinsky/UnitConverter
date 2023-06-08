using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;

namespace UnitConverter
{
    /// <summary>
    /// Class represents Language localization's manager of application.
    /// </summary>
    class LanguageManager
    {
        public UIHelper UIHelper { get; private set; }
        public Translator Translator { get; private set; }
        public QuantitiesReader QuantitiesReader { get; private set; }

        public List<string> KeywordsList { get; private set; }
        public string InputLanguage { get; private set; }
        public int QuantityCombo_SelectedIndex_LAST { get; private set; }
        public bool IsTranslating { get; private set; }

        public LanguageManager(UIHelper uiHelper)
        {
            UIHelper = uiHelper;
            Translator = new Translator();
            QuantitiesReader = new QuantitiesReader("quantities.xml");

            KeywordsList = new List<string> { "Settings", "Please select a language:", "Quantity:", "Convert from:", "Convert to:", "Created by", "Powered by", "Convert!", "Type a number in correct format." }; //List of application's keywords.
            InputLanguage = "en";
        }

        /// <summary>
        /// Translates UI of application.
        /// </summary>
        /// <param name="IsStart">Information about application's status - If is the application starting.</param>
        public void TranslateUI(bool IsStart)
        {
            if (!UIHelper.IsRenderingUI)
                return;

            IsTranslating = true;

            string outputLanguage;
            if (IsStart)
            {
                outputLanguage = GetActuallyRunTimeLanguage();
                UIHelper.LanguageCombo_SelectedItem = Translator.GetLanguageNameByKey(outputLanguage);
                UIHelper.QuantityCombo_SelectedIndex = 0;
            }
            else
                outputLanguage = Translator.GetKeyByLanguageName(UIHelper.LanguageCombo_SelectedItem);

            try
            {
                List<string> translatedKeywords = Translator.Translate(KeywordsList.Concat(QuantitiesReader.GetListOfQuantities()).ToList(), InputLanguage, outputLanguage); //Union
                UIHelper.InternetConnectionWarning_Text = translatedKeywords[8];
                UIHelper.PropertiesGroupBox_Header = translatedKeywords[0];
                UIHelper.LanguageLabel_Content = translatedKeywords[1];
                UIHelper.QuantityLabel_Content = translatedKeywords[2];

                List<string> QuantityCombo_Items_Translated = new List<string>();

                for (int i = 9; i < translatedKeywords.Count; i++)
                    QuantityCombo_Items_Translated.Add(translatedKeywords[i]);

                QuantityCombo_SelectedIndex_LAST = UIHelper.QuantityCombo_SelectedIndex;
                UIHelper.QuantityCombo_Items = QuantityCombo_Items_Translated;

                UIHelper.FromLabel_Content = translatedKeywords[3];
                UIHelper.ToLabel_Content = translatedKeywords[4];
                UIHelper.StatusBar_Items = new List<string>() { translatedKeywords[5], translatedKeywords[6] };
                UIHelper.ConvertButton_Content = translatedKeywords[7];
            }
            catch
            {
                MessageBox.Show("Translation isn't allowed, because internet connection isn't available.");
                QuantityCombo_SelectedIndex_LAST = UIHelper.QuantityCombo_SelectedIndex;
                UIHelper.QuantityCombo_Items = QuantitiesReader.GetListOfQuantities();
                UIHelper.LanguageCombo_SelectedItem = "English";
            }

            IsTranslating = false;
        }

        /// <summary>
        /// Gets actually runtime language.
        /// </summary>
        /// <returns>Returns two letters key of system language.</returns>
        private string GetActuallyRunTimeLanguage()
        {
            return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName; //CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }
    }
}
