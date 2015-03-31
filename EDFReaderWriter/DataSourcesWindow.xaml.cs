
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using EDFLibrary;
using Microsoft.Win32;


using EDFLibrary.EDFData.Manager;
using EDFReaderWriter.Utility_Classes;
namespace EDFReaderWriter
{
    /// <summary>
    /// Interaction logic for DataSources.xaml
    /// </summary>
    public partial class DataSourcesWindow : Window
    {
       
        public DataSourcesWindow()
        {

            InitializeComponent();
            List<EDFSignal> signals = ObjectHolder.signals;
            foreach (EDFSignal signal in signals)
            {
                cbSignals.Items.Add(signal.label);
            }
            cbSignals.SelectedIndex = 0;
                
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "EEG cnt file|*.cnt";
            if (dlg.ShowDialog() == true)
            {
                EDFDataManager manager = new EDFDataManager();
                manager.addFile(EDFDataManager.FileTypes.zeoEEGCNT, dlg.FileName,cbSignals.SelectedIndex); //-1 for 0 based
                manager.generateEDFData();
            }
        }


        /// <summary>
        /// saves data from zeo cnt file format into edf data objects
        /// </summary>
        /// <param name="filename"></param>
        private void zeoCnt(string filename) 
        {
            





        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
