﻿#pragma checksum "..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "44F18ECCE6190BC6A0D92DEFBBFE36C2F5A83D6F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PerlinControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PerlinControls {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider RedMix;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider GreenMix;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider BlueMix;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Octaves;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Persistence;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Frequency;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Amplitude;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PerlinControls;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.RedMix = ((System.Windows.Controls.Slider)(target));
            
            #line 28 "..\..\MainWindow.xaml"
            this.RedMix.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.GreenMix = ((System.Windows.Controls.Slider)(target));
            
            #line 40 "..\..\MainWindow.xaml"
            this.GreenMix.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BlueMix = ((System.Windows.Controls.Slider)(target));
            
            #line 52 "..\..\MainWindow.xaml"
            this.BlueMix.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Octaves = ((System.Windows.Controls.Slider)(target));
            
            #line 68 "..\..\MainWindow.xaml"
            this.Octaves.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Persistence = ((System.Windows.Controls.Slider)(target));
            
            #line 79 "..\..\MainWindow.xaml"
            this.Persistence.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Frequency = ((System.Windows.Controls.Slider)(target));
            
            #line 92 "..\..\MainWindow.xaml"
            this.Frequency.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Amplitude = ((System.Windows.Controls.Slider)(target));
            
            #line 105 "..\..\MainWindow.xaml"
            this.Amplitude.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderValChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 109 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Confirm_Changes);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

