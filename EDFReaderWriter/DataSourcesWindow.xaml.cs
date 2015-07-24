
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using EDFLibrary;
using Microsoft.Win32;


using EDFLibrary.EDFData.Manager;
using EDFLibrary.Utility_Classes;
using System;
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

        /// <summary>
        /// Saves the cnt file into edf - either first channel read from CNT or all channels read and put into a range of signals in edf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "EEG cnt file|*.cnt";
             manager = new EDFDataManager();
            switch(rbSingleSignalMode.IsChecked)
            {
               
                case true:
                    if (dlg.ShowDialog() == true)
                    {
                        
                        ObjectHolder.EDFHeaderHolder = manager.addFile(EDFDataManager.FileTypes.zeoEEGCNT, dlg.FileName, ObjectHolder.EDFHeaderHolder, cbSignals.SelectedIndex, cbSignals.SelectedIndex); //selected index repeated twice to get only 1 signal filled
                    }
                    break;
                case false:
                    if (int.Parse(tbStartSignalNumber.Text) >= 0 && int.Parse(tbStartSignalNumber.Text) < (ObjectHolder.EDFHeaderHolder.edfSignals.Count - 1) && int.Parse(tbEndSignalNumber.Text) >= 0 && int.Parse(tbEndSignalNumber.Text) < (ObjectHolder.EDFHeaderHolder.edfSignals.Count - 1))
                    {
                        if (dlg.ShowDialog() == true)
                        {

                            ObjectHolder.EDFHeaderHolder = manager.addFile(EDFDataManager.FileTypes.zeoEEGCNT, dlg.FileName, ObjectHolder.EDFHeaderHolder, int.Parse(tbStartSignalNumber.Text), int.Parse(tbEndSignalNumber.Text)); //selected index repeated twice to get only 1 signal filled
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Indices for signals out of range! Expecting range 0 to " + (ObjectHolder.EDFHeaderHolder.edfSignals.Count - 1) + ".");
                    }
                   
                    break;
                default:
                    throw new ApplicationException("The world has ended - boolean does not return true or false");

            }
            
        }


        /// <summary>
        /// Checks if all signals are assigned
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            {

                manager.generateEDFData(ObjectHolder.EDFHeaderHolder, ObjectHolder.EDFHeaderHolder.FilePath);
            }
                
        }

        private void rbMultipleSignalMode_Checked(object sender, RoutedEventArgs e)
        {
            cbSignals.IsEnabled = false;
            tbStartSignalNumber.IsEnabled = true;
            tbEndSignalNumber.IsEnabled = true;
        }

        private void rbSingleSignalMode_Checked(object sender, RoutedEventArgs e)
        {
            cbSignals.IsEnabled = true;
            tbStartSignalNumber.IsEnabled = false;
            tbEndSignalNumber.IsEnabled = false;
        }
    }
}
