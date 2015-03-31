using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EDFReaderWriter.Utility_Classes;
using System.IO;
using EDFLibrary.EDFHeader;

namespace EDFReaderWriter
{
    
    /// <summary>
    /// Interaction logic for AdvancedDetailsWindow.xaml
    /// </summary>
    public partial class AdvancedDetailsWindow : Window
    {
        
        List<string> labels;
        List<string> transducerTypes;
        List<string> physicalDimensions;
        List<string> physicalMinimums;
        List<string> physicalMaximums;
        List<string> digitalMinimums;
        List<string> digitalMaximums;
        List<string> prefilterings;
        List<string> numSamplesPerRecords;

        int numSignals;
        bool[] saved;
        EDFHeader header;
        public AdvancedDetailsWindow()
        {
            InitializeComponent();


           

            header = ObjectHolder.EDFHeaderHolder;
            numSignals = Convert.ToInt32(header.ns) - 1; //-1 to keep annotations separate from real signals

            saved = new bool[numSignals];

            lblSaved.Visibility =  System.Windows.Visibility.Hidden;

            //clear/create the relevant lists
            labels = new List<string>();
            transducerTypes = new List<string>();
            physicalDimensions = new List<string>();
            physicalMinimums = new List<string>();
            physicalMaximums = new List<string>();
            digitalMinimums = new List<string>();
            digitalMaximums = new List<string>();
            prefilterings = new List<string>();
            numSamplesPerRecords = new List<string>();

            //initialise list with blank strings to we can assign to them by index later, +1 to allow for annotations to slip in
            for (int i = 0; i < numSignals + 1; i++)
            {
                labels.Add("");
                transducerTypes.Add("");
                physicalDimensions.Add("");
                physicalMinimums.Add("");
                physicalMaximums.Add("");
                digitalMinimums.Add("");
                digitalMaximums.Add("");
                prefilterings.Add("");
                numSamplesPerRecords.Add("");
            }
           
            for (int i = 0; i < numSignals; i++)
            {
                cbSignals.Items.Add("Signal " + i);
            }

            cbSignals.SelectedIndex = 0;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            //convert the number gotten from the cb - parse "signal 1" -> 1 -> int
            int signalIndex = Convert.ToInt32(cbSignals.SelectedValue.ToString().Split(' ')[1]); 
            Console.WriteLine(signalIndex);

            

            labels[signalIndex] = tbLabel.Text;
            transducerTypes[signalIndex] = tbTransducerType.Text;
            physicalDimensions[signalIndex] = tbPhysicalDimension.Text;
            physicalMinimums[signalIndex] = tbPhysicalMinimum.Text;
            physicalMaximums[signalIndex] = tbPhysicalMaximum.Text;
            digitalMinimums[signalIndex] = tbDigitalMinimum.Text;
            digitalMaximums[signalIndex] = tbDigitalMaximum.Text;
            prefilterings[signalIndex] = tbPrefiltering.Text;
            numSamplesPerRecords[signalIndex] = tbNumSamples.Text;



            saved[signalIndex] = true;
            lblNotSaved.Visibility = System.Windows.Visibility.Hidden;
            lblSaved.Visibility = System.Windows.Visibility.Visible;
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            bool allSaved = true;
            foreach (bool save in saved)
            {
                if (save == false)
                {
                    allSaved = false;
                    break;
                }
                   
            }

            if (allSaved)
            {
                //add annotations
                int ns = Convert.ToInt32(header.ns);
                labels[ns-1] = tbLabel_Copy.Text;
                transducerTypes[ns-1] = tbTransducerType_Copy.Text;
                physicalDimensions[ns-1] = tbPhysicalDimension_Copy.Text;
                physicalMinimums[ns-1] = tbPhysicalMinimum_Copy.Text;
                physicalMaximums[ns-1] = tbPhysicalMaximum_Copy.Text;
                digitalMinimums[ns-1] = tbDigitalMinimum_Copy.Text;
                digitalMaximums[ns-1] = tbDigitalMaximum_Copy.Text;
                prefilterings[ns-1] = tbPrefiltering_Copy.Text;
                numSamplesPerRecords[ns-1] = tbNumSamples_Copy.Text;
               

                header.setNSDependantData(labels, transducerTypes, physicalDimensions, physicalMinimums, physicalMaximums, digitalMinimums, digitalMaximums, prefilterings, numSamplesPerRecords);



                Console.WriteLine(header.generateEDFHeader());
                StreamWriter writer = new StreamWriter(@"D:\out.edf");
                writer.Write(header.generateEDFHeader());
                writer.Close();

                //open next window
                
                DataSourcesWindow window = new DataSourcesWindow();
                App.Current.MainWindow = window;
                this.Close();
                window.Show();
                
            }
            else
            {
                MessageBox.Show("Not all signal details have been filled!");
            }
            
        }

        private void cbSignals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int signalIndex = Convert.ToInt32(cbSignals.SelectedValue.ToString().Split(' ')[1]); 
            if (saved[signalIndex])
            {
                lblSaved.Visibility = System.Windows.Visibility.Visible;
                lblNotSaved.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                lblSaved.Visibility = System.Windows.Visibility.Hidden;
                lblNotSaved.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void tbPhysicalMinimum_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbNumSamples_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbLabel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

      
        private void cbSaveProgram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
