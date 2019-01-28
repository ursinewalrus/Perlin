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
        private int[] SwapTime;

        public MainWindow()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            DB = redis.GetDatabase();
            Model = new PerlinVarsModel();
            InitializeComponent();

            RedMix.Value = Model.RedValueMultiplier;
            GreenMix.Value = Model.GreenValueMultiplier;
            BlueMix.Value = Model.BlueValueMultiplier;

        }

        public void SetSwap(int[] swap)
        {
            SwapTime = swap;
        }


        public void RBGValChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DependencyObject dobj = sender as DependencyObject;
            string name = dobj.GetValue(FrameworkElement.NameProperty) as string;
            //switch (name)
            //{
            //    case "RedMix":
            //        Model.RedValueMultiplier = e.NewValue;
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
            Model.RedValueMultiplier = RedMix.Value;
            Model.GreenValueMultiplier = GreenMix.Value;
            Model.BlueValueMultiplier = BlueMix.Value;
            SwapTime[0] = 10;

        }
    }
}
