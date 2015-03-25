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
using System.Windows.Shapes;
using EDFLibrary;
using EDFReaderWriter.Utility_Classes;

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
        public AdvancedDetailsWindow()
        {
            InitializeComponent();

            

            EDFHeader header = ObjectHolder.EDFHeaderHolder;
            numSignals = Convert.ToInt32(header.getNs());

            lblSaved.Visibility =  System.Windows.Visibility.Hidden;

            for (int i = 0; i < numSignals; i++)
            {
                cbSignals.Items.Add("Signal " + i);
            }

            cbSignals.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //convert the number gotten from the cb - parse "signal 1" -> 1 -> int
            int signalIndex = Convert.ToInt32(cbSignals.SelectedValue.ToString().Split(' ')[1]); 
            Console.WriteLine(signalIndex);

            labels = new List<string>();
            transducerTypes = new List<string>();
            physicalDimensions = new List<string>();
            physicalMinimums = new List<string>();
            physicalMaximums = new List<string>();
            digitalMinimums = new List<string>();
            digitalMaximums = new List<string>();
            prefilterings = new List<string>();
            numSamplesPerRecords = new List<string>();

            for (int i = 0; i < numSignals; i++)
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

            labels[signalIndex] = tbLabel.Text;
            transducerTypes[signalIndex] = tbTransducerType.Text;
            physicalDimensions[signalIndex] = tbPhysicalDimension.Text;
            physicalMinimums[signalIndex] = tbPhysicalMinimum.Text;
            physicalMaximums[signalIndex] = tbPhysicalMaximum.Text;
            digitalMinimums[signalIndex] = tbDigitalMinimum.Text;
            digitalMaximums[signalIndex] = tbDigitalMaximum.Text;
            prefilterings[signalIndex] = tbPrefiltering.Text;
            numSamplesPerRecords[signalIndex] = tbNumSamples.Text;



            lblNotSaved.Visibility = System.Windows.Visibility.Hidden;
            lblSaved.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
