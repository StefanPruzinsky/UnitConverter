using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Diagnostics;

namespace UnitConverter
{
    /**
     * For competition "Machr na Alogoritmy" created Štefan Pružinský (stevkopr).
     * */

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UIHelper uiHelper;
        Converter converter;

        public MainWindow()
        {
            InitializeComponent();

            uiHelper = new UIHelper(this);
            converter = new Converter(uiHelper);

            uiHelper.RenderUI();
        }

        //Control's events
        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            converter.Convert();
        }

        private void languageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            uiHelper.LanguageManager.TranslateUI(false);
        }

        private void quantityCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            uiHelper.QuantityCombo_SelectionChanged_ChangeUnits();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
