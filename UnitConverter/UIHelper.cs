using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Diagnostics;

namespace UnitConverter
{
    /// <summary>
    /// Class represents Helper for work with user interface.
    /// </summary>
    class UIHelper
    {
        public MainWindow MainWindow { get; private set; }
        public Translator Translator { get; private set; }
        public QuantitiesReader QuantitiesReader { get; private set; }
        public LanguageManager LanguageManager { get; private set; }

        //Properties for work with controls.
        public bool IsRenderingUI { get; private set; }
        private string internetConnectionWarning_Text = "Type a number in correct format.";
        public string InternetConnectionWarning_Text { get { return internetConnectionWarning_Text; } set { internetConnectionWarning_Text = value; } }

        public string PropertiesGroupBox_Header { get { return (string)MainWindow.propertiesGroupBox.Header; } set { MainWindow.propertiesGroupBox.Header = value; } }

        public string LanguageLabel_Content { get { return (string)MainWindow.languageLabel.Content; } set { MainWindow.languageLabel.Content = value; } }
        public List<string> LanguageCombo_Items { get { return (List<string>)MainWindow.languageCombo.ItemsSource; } set { MainWindow.languageCombo.ItemsSource = value; } }
        public string LanguageCombo_SelectedItem { get { return (string)MainWindow.languageCombo.SelectedItem; } set { MainWindow.languageCombo.SelectedItem = value; } }

        public string QuantityLabel_Content { get { return (string)MainWindow.quantityLabel.Content; } set { MainWindow.quantityLabel.Content = value; } }
        public List<string> QuantityCombo_Items { get { return (List<string>)MainWindow.quantityCombo.ItemsSource; } set { MainWindow.quantityCombo.ItemsSource = value; } }
        public int QuantityCombo_SelectedIndex { get { return MainWindow.quantityCombo.SelectedIndex; } set { MainWindow.quantityCombo.SelectedIndex = value; } }

        public string FromLabel_Content { get { return (string)MainWindow.fromLabel.Content; } set { MainWindow.fromLabel.Content = value; } }
        public decimal FromTextBox_Text 
        { 
            get 
            {
                decimal number;
                if (!decimal.TryParse(MainWindow.fromTextBox.Text, out number))
                {
                    MessageBox.Show(InternetConnectionWarning_Text);
                }
                return number;
            } 
            private set { } 
        }
        public List<string> FromCombo_Items { get { return (List<string>)MainWindow.fromCombo.ItemsSource; } set { MainWindow.fromCombo.ItemsSource = value; } }
        public string FromCombo_SelectedItem { get { return (string)MainWindow.fromCombo.SelectedItem; } set { MainWindow.fromCombo.SelectedItem = value; } }

        public string ToLabel_Content { get { return (string)MainWindow.toLabel.Content; } set { MainWindow.toLabel.Content = value; } }
        public string ToTextBox_Text { get { return MainWindow.toTextBox.Text; } set { MainWindow.toTextBox.Text = value; } }
        public List<string> ToCombo_Items { get { return (List<string>)MainWindow.toCombo.ItemsSource; } set { MainWindow.toCombo.ItemsSource = value; } }
        public string ToCombo_SelectedItem { get { return (string)MainWindow.toCombo.SelectedItem; } set { MainWindow.toCombo.SelectedItem = value; } }

        public List<string> StatusBar_Items 
        { 
            get 
            { 
                return null; 
            } 
            set 
            {
                TextBlock textBlockConstant = new TextBlock();
                textBlockConstant.Text = "Unit Converter";

                Separator separator1 = new Separator();
                TextBlock textBlockCustom = new TextBlock();
                textBlockCustom.Text = value[0] + " Štefan Pružinský";

                Separator separator2 = new Separator();
                TextBlock textBlockYandex = new TextBlock();
                textBlockYandex.Text = value[1];

                Hyperlink hyperlink = new Hyperlink(new Run("Yandex.Translate"));
                hyperlink.NavigateUri = new Uri("http://translate.yandex.com/");
                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

                MainWindow.statusBar.Items.Clear();
                MainWindow.statusBar.Items.Add(textBlockConstant);
                MainWindow.statusBar.Items.Add(separator1);
                MainWindow.statusBar.Items.Add(textBlockCustom);
                MainWindow.statusBar.Items.Add(separator2);
                MainWindow.statusBar.Items.Add(textBlockYandex);
                MainWindow.statusBar.Items.Add(hyperlink);
            } 
        }

        public string ConvertButton_Content { get { return (string)MainWindow.convertButton.Content; } set { MainWindow.convertButton.Content = value; } }

        public UIHelper(MainWindow mainWindow)
        {
            MainWindow = mainWindow;

            Translator = new Translator();
            QuantitiesReader = new QuantitiesReader("quantities.xml");
            LanguageManager = new LanguageManager(this);
            
            IsRenderingUI = true;
        }

        //Render user interface at start of application.
        public void RenderUI()
        {
            QuantityCombo_Items = new List<string> { "" };
            LanguageManager.TranslateUI(true);

            ToCombo_SelectedItem = QuantitiesReader.GetValueOfMainUnit(QuantityCombo_SelectedIndex);
            FromCombo_SelectedItem = QuantitiesReader.GetValueOfMainUnit(QuantityCombo_SelectedIndex);

            IsRenderingUI = false;
            LanguageCombo_Items = Translator.GetListOfLanguages();
            IsRenderingUI = true;

            FromCombo_Items = QuantitiesReader.GetListOfQuantityValues(QuantityCombo_SelectedIndex);
            ToCombo_Items = QuantitiesReader.GetListOfQuantityValues(QuantityCombo_SelectedIndex);
        }

        /// <summary>
        /// Changes units at "QuantityCombo_SelectionChanged" event.
        /// </summary>
        public void QuantityCombo_SelectionChanged_ChangeUnits()
        {
            if (LanguageManager.IsTranslating)
            {
                QuantityCombo_SelectedIndex = LanguageManager.QuantityCombo_SelectedIndex_LAST;
            }

            ToCombo_SelectedItem = QuantitiesReader.GetValueOfMainUnit(QuantityCombo_SelectedIndex);
            FromCombo_SelectedItem = QuantitiesReader.GetValueOfMainUnit(QuantityCombo_SelectedIndex);

            FromCombo_Items = QuantitiesReader.GetListOfQuantityValues(QuantityCombo_SelectedIndex);
            ToCombo_Items = QuantitiesReader.GetListOfQuantityValues(QuantityCombo_SelectedIndex);
        }

        //Event method for Hyperlink Request Navigation
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
