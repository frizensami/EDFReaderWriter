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

namespace EDFReaderWriter
{
    /// <summary>
    /// Interaction logic for DataSources.xaml
    /// </summary>
    public partial class DataSources : Window
    {
        public DataSources()
        {
            EEGCntFile file = new EEGCntFile();
            
            file.ReadCnt(System.IO.Path.GetFullPath(@"test3.cnt"));
            float[,] eeg_data = file.eeg_data;
            float highest = 0;
            float lowest = float.MaxValue;
            for(int i = 0; i < eeg_data.GetLength(0); i++)
            {
                Console.WriteLine(i);
                for (int j = 0; j < eeg_data.GetLength(1); j++)
                {
                    float data = eeg_data[i, j];
                    //Console.WriteLine("{0} | {1}", i, data);
                    if (data < lowest)
                        lowest = eeg_data[i,j];
                    if (eeg_data[i,j] > highest)
                        highest = data;
                }
            }
            Console.WriteLine("Lowest: {0}", lowest);
            Console.WriteLine(BitConverter.GetBytes(lowest).Length);
            Console.WriteLine("Highest: {0}", highest);
            Console.WriteLine(BitConverter.GetBytes(highest).Length);

            FakeDataGenerator.generateData(16);
            InitializeComponent();
        }
    }
}
