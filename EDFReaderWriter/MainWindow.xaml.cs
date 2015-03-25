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
using EDFLibrary;
using EDFReaderWriter.Utility_Classes;
namespace EDFReaderWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //EDFLibrary.EDFHeader newHeader = new EDFLibrary.EDFHeader();
            //newHeader.setReserved(EDFLibrary.EDFReserved.EDF_CONTINUOUS);



        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string version = tbVersion.Text;

            string patient = tbPatientCode.Text.Replace(' ','_') + " " + tbPatientSex.Text.Replace(' ','_') + " " + 
                             tbPatientBirthday.SelectedDate.Value.ToString("dd-MMM-yyyy").ToUpper() + " " + tbPatientName.Text.Replace(' ','_');
            string recording = "Startdate " + tbStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy").ToUpper() + " "
                                + tbPSGNumber.Text.Replace(' ', '_') + tbTechnicianCode.Text.Replace(' ', '_')
                                + tbEquipCode.Text.Replace(' ', '_');

            string startDate = "";
            if (tbStartDate.SelectedDate.Value.Year <= 2084)
                startDate = tbStartDate.SelectedDate.Value.ToString("dd.MM.yy").ToUpper();
            else
                startDate = "yy";

            string startTime = tbStartHour.Text + "." + tbStartHour.Text + "." + tbStartSecond.Text;
            string reserved = cbReserved.Text;
            string numRecords = tbNumRecords.Text;
            string durationRecord = tbDurationRecord.Text;
            string ns = tbNs.Text;
            

            EDFHeader newHeader = new EDFHeader(version, patient,recording,startDate,startTime,reserved,numRecords,durationRecord,ns);
            ObjectHolder.EDFHeaderHolder = newHeader; //store it here for access from other windows

            AdvancedDetailsWindow window = new AdvancedDetailsWindow();
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
