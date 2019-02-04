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
using Microsoft.Xna.Framework;
using StackExchange.Redis;
using static PerlinControls.SignalPass;

namespace PerlinControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// https://stackoverflow.com/questions/1405739/mvvm-tutorial-from-start-to-finish
    public partial class MainWindow : Window
    {
        private IDatabase DB;
        private PerlinVarsModel Model;
        private SignalPass SwapTime;
        private double OldRed;
        private double OldGreen;
        private double OldBlue;
        private int OldOctaves;
        private double OldPersistence;
        private int OldFrequency;
        private int OldAmplitude;

        public MainWindow()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            DB = redis.GetDatabase();
            Model = new PerlinVarsModel();
            InitializeComponent();

            RedMix.Value = OldRed = Model.RedValueMultiplier;
            GreenMix.Value = OldGreen = Model.GreenValueMultiplier;
            BlueMix.Value = OldBlue = Model.BlueValueMultiplier;
            Octaves.Value = OldOctaves = Model.PerlinOctaves;
            Persistence.Value = OldPersistence = Model.PerlinPersistence;
            Frequency.Value = OldFrequency = Model.PerlinFrequency;
            Amplitude.Value = OldAmplitude = Model.PerlinAmplitude;
            ;
        }

        public void SetSwap(SignalPass swap)
        {
            SwapTime = swap;
        }


        public void SliderValChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DependencyObject dobj = sender as DependencyObject;
            string name = dobj.GetValue(FrameworkElement.NameProperty) as string;
            //switch (name)
            //{
            //    case "RedMix":
            //        Model.RedValueMultiplier = e.NewValue;x
            //        break;
            //    case "GreenMix":
            //        Model.GreenValueMultiplier = e.NewValue;
            //        break;
            //    case "BlueMix":
            //        Model.BlueValueMultiplier = e.NewValue;
            //        break;

            //}
        }

        public void Confirm_Changes(object sender, RoutedEventArgs e)
        {

            if ((int)Octaves.Value != OldOctaves || Persistence.Value != OldPersistence ||
                (int)Frequency.Value != OldFrequency || (int)Amplitude.Value != OldAmplitude)
            {
                SwapTime.InitState = ReInit.All;
            }
            else
            {
                SwapTime.InitState = ReInit.Color;
            }

            OldRed = Model.RedValueMultiplier = RedMix.Value;
            OldGreen = Model.GreenValueMultiplier = GreenMix.Value;
            OldBlue = Model.BlueValueMultiplier = BlueMix.Value;
            OldOctaves = Model.PerlinOctaves = (int)Octaves.Value;
            OldPersistence = Model.PerlinPersistence = Persistence.Value;
            OldFrequency = Model.PerlinFrequency = (int)Frequency.Value;
            OldAmplitude = Model.PerlinAmplitude = (int)Amplitude.Value;


        }
    }
}
