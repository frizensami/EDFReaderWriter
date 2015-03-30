using BCILib.Amp;
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
using Microsoft.Win32;

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
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "EEG cnt file|*.cnt";
            if (dlg.ShowDialog() == true)
            {
                zeoCnt(dlg.FileName);
            }
        }


        private void zeoCnt(string filename)
        {
            EEGCntFile file = new EEGCntFile();

            //file.ReadCnt(System.IO.Path.GetFullPath(@"test3.cnt"));
            file.ReadCnt(filename);
            float[,] eeg_data = file.eeg_data;
            float[,] digital_data = eeg_data; //just assigning it so i don't have to initialise everything

            float highest = 0;
            float lowest = float.MaxValue;
            for (int i = 0; i < eeg_data.GetLength(0); i++)
            {
                Console.WriteLine(i);
                for (int j = 0; j < eeg_data.GetLength(1); j++)
                {




                    float rawdata = eeg_data[i, j];
                    Int16 digitaldata = (Int16) ((rawdata * 32768) / 318);
                    digital_data[i, j] = digitaldata; //converts the uV output of the zeo to 16 bit digital output

                    //Console.WriteLine("{0} | {1}", i, digitaldata);
                    if (digitaldata < lowest)
                        lowest = eeg_data[i, j];
                    if (eeg_data[i, j] > highest)
                        highest = digitaldata;
                }
            }
            Console.WriteLine("Lowest: {0}", lowest);
            Console.WriteLine(BitConverter.GetBytes(lowest).Length);
            Console.WriteLine("Highest: {0}", highest);
            Console.WriteLine(BitConverter.GetBytes(highest).Length);

            /*
            Int16[] fakeData = EDFLibrary.FakeDataGenerator.generateData(20);
            foreach (Int16 int16data in fakeData)
            {
                Console.Write(int16data);
            }
             * */
        }
    }
}
