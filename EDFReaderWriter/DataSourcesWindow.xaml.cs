
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using EDFLibrary;
using Microsoft.Win32;


using EDFLibrary.EDFData.Manager;
using EDFLibrary.Utility_Classes;
namespace EDFLibrary
{
    /// <summary>
    /// Interaction logic for DataSources.xaml
    /// </summary>
    public partial class DataSourcesWindow : Window
    {
        EDFDataManager manager;
        public DataSourcesWindow()
        {

            InitializeComponent();
            
            List<EDFSignal> signals = ObjectHolder.EDFHeaderHolder.edfSignals;
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
                manager = new EDFDataManager();
                manager.addFile(EDFDataManager.FileTypes.zeoEEGCNT, dlg.FileName, ObjectHolder.EDFHeaderHolder, cbSignals.SelectedIndex); //-1 for 0 based
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
            bool generate = true;
            for (int i = 0; i < (ObjectHolder.EDFHeaderHolder.edfSignals.Count - 1); i++)
            {
                EDFSignal signal = ObjectHolder.EDFHeaderHolder.edfSignals[i];
                if (signal.samples == null)
                {
                    System.Windows.MessageBox.Show("One or more signals have not had their data sets added!");
                    generate = false;
                    break;
                }
                    
            }
            if (generate)
                manager.generateEDFData();
        }
    }
}
