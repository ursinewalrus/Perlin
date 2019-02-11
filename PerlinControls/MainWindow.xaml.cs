using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region dumb args
        private IDatabase DB;
        private PerlinVarsModel Model;
        private List<SignalPass.ReInit> SwapTime;
        private double OldRed;
        private double OldGreen;
        private double OldBlue;
        private int OldColorGradients;
        private int OldOctaves;
        private double OldPersistence;
        private int OldFrequency;
        private int OldAmplitude;

        private bool OldHortLines;
        private bool OldVertLines;
        private int OldHorizontalPer;
        private int OldVerticalPer;
        #endregion

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
            ColorGradients.Value = Model.NumberOfColorGradients;
            HorizontalLines.IsChecked = OldHortLines = Model.HorizontalLines;
            VerticalLines.IsChecked = OldVertLines = Model.VerticalLines;
            HorizontalPer.Value = OldHorizontalPer = Model.HorizontalLinesPer;
            VerticalPer.Value = OldVerticalPer = Model.VerticalLinesPer;

            ;
        }

        public void SetSwap(List<SignalPass.ReInit> swap)
        {
            SwapTime = swap;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void Confirm_Changes(object sender, RoutedEventArgs e)
        {

            if ((int)Octaves.Value != OldOctaves || Persistence.Value != OldPersistence ||
                (int)Frequency.Value != OldFrequency || (int)Amplitude.Value != OldAmplitude)
            {
                SwapTime.Add(ReInit.All);
            }
            if ((int)ColorGradients.Value != OldColorGradients)
            {
                SwapTime.Add(ReInit.NumGradients);
            }
            if (OldVertLines != VerticalLines.IsChecked || OldHortLines != HorizontalLines.IsChecked || OldHorizontalPer != (int)HorizontalPer.Value || OldVerticalPer != (int)VerticalPer.Value)
            {
                SwapTime.Add(ReInit.Lines);
            }
            if(OldRed != RedMix.Value || OldGreen != GreenMix.Value || OldBlue != BlueMix.Value)
            {
                SwapTime.Add(ReInit.Color);
            }

            OldRed = Model.RedValueMultiplier = RedMix.Value;
            OldGreen = Model.GreenValueMultiplier = GreenMix.Value;
            OldBlue = Model.BlueValueMultiplier = BlueMix.Value;
            OldColorGradients = Model.NumberOfColorGradients = (int)ColorGradients.Value;
            OldOctaves = Model.PerlinOctaves = (int)Octaves.Value;
            OldPersistence = Model.PerlinPersistence = Persistence.Value;
            OldFrequency = Model.PerlinFrequency = (int)Frequency.Value;
            OldAmplitude = Model.PerlinAmplitude = (int)Amplitude.Value;

            OldHortLines = Model.HorizontalLines = (bool)HorizontalLines.IsChecked;
            OldVertLines = Model.VerticalLines = (bool)VerticalLines.IsChecked;
            OldHorizontalPer = Model.HorizontalLinesPer = (int) HorizontalPer.Value;
            OldVerticalPer = Model.VerticalLinesPer = (int)VerticalPer.Value;

            var l = LineColor.SelectedColor.Value;

            ;


        }

    }
}
