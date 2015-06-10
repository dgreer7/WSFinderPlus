namespace WSFinderPlus
{
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

    //TODO 97: Wrap methods below in try/catch statements and add logging.
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const byte MAX_SESSIONS = 50;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search.OpenAstroConfigs();
        }

        private void txtCityCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCityCode.Text.Length > 0)
                txtCityCode.Text = ValidateLastCharIsLetter(txtCityCode.Text).ToUpper();

            if (txtCityCode.Text.Length == 3)
            {
                chkBoxTR.Focus();
                btnFindMatchingNames.IsEnabled = true;
            }
            else
                btnFindMatchingNames.IsEnabled = false;

            txtCityCode.Select(txtCityCode.Text.Length, 0);
        }

        private void txtMaxNumberSessions_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMaxNumberSessions.Text.Length > 0)
            {
                byte numSessions = ValidateLastCharIsInt(txtMaxNumberSessions.Text);
                if (numSessions > MAX_SESSIONS)
                    txtMaxNumberSessions.Text = MAX_SESSIONS.ToString();
                else
                    txtMaxNumberSessions.Text = numSessions.ToString();
            }

            if (txtMaxNumberSessions.Text.Length == 2 && btnLaunchSearchWindows.IsEnabled)
            {
                btnLaunchSearchWindows.Focus();
            }

            txtMaxNumberSessions.Select(txtMaxNumberSessions.Text.Length, 0);
        }

        public static string ValidateLastCharIsLetter(string stringToEvaluate)
        {
            if (!char.IsLetter(stringToEvaluate, stringToEvaluate.Length - 1))
                if (stringToEvaluate.Length > 1)
                    stringToEvaluate = stringToEvaluate.Remove(stringToEvaluate.Length - 1);
                else
                    stringToEvaluate = String.Empty;

            return stringToEvaluate;
        }

        public static byte ValidateLastCharIsInt(string stringToEvaluate)
        {
            byte evaluated = 0;
            int stringLenght = stringToEvaluate.Length;
            for (int i = 0; i < stringLenght; i++)
            {
                if (!byte.TryParse(stringToEvaluate, out evaluated))
                    stringToEvaluate = stringToEvaluate.Remove(stringToEvaluate.Length - 1);
            }
            return evaluated;
        }

        //TODO 01: Working on adding working list to display.
        private void btnFindMatchingNames_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WorkstationList listToDisplay = new WorkstationList(txtCityCode.Text);
                if (listToDisplay.WorkingList.Count == 0)
                {
                    lstBoxReadInWorkstationNames.Items.Add("No matches were found");
                    log.Warn(String.Format("No results were returned could be displayed with search terms {0}", txtCityCode.Text));
                }
                else
                {
                    foreach (KeyValuePair<string, List<string>> workstation in listToDisplay.WorkingList)
                    {
                        lstBoxReadInWorkstationNames.Items.Add(String.Format("{0}\t\t{1}", workstation.Key, workstation.Value.ToString()));
                        log.Info(String.Format("Results returned with search terms {0} have been displayed.", txtCityCode.Text));
                    }
                }
            }
            catch (Exception displayException)
            {
                log.Error(String.Format("Error occured when attempting to place matching workstations in display. Search critera - city: {0}", txtCityCode.Text), displayException);
            }
        }
    }
}
//TODO 25: Handle an update to a workstation name without refilling the entire list
//TODO 26: If an update to lstBoxReadInWorkstationsNames, then also update lstBoxUserListToSearch
