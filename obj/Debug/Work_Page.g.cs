﻿#pragma checksum "..\..\Work_Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ADC2BF9B1F504EF92CCD4EDBB171FB4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Project_Worker;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace Project_Worker {
    
    
    /// <summary>
    /// Page5
    /// </summary>
    public partial class Page5 : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 38 "..\..\Work_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button List_btn;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\Work_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Finish_button;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\Work_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Left_button;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\Work_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Right_button;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\Work_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost WinFormHost;
        
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
            System.Uri resourceLocater = new System.Uri("/Project_Worker;component/work_page.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Work_Page.xaml"
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
            this.List_btn = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\Work_Page.xaml"
            this.List_btn.Click += new System.Windows.RoutedEventHandler(this.List_button);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Finish_button = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\Work_Page.xaml"
            this.Finish_button.Click += new System.Windows.RoutedEventHandler(this.Finish_button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Left_button = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\Work_Page.xaml"
            this.Left_button.Click += new System.Windows.RoutedEventHandler(this.Left_btn);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Right_button = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\Work_Page.xaml"
            this.Right_button.Click += new System.Windows.RoutedEventHandler(this.Right_btn);
            
            #line default
            #line hidden
            return;
            case 5:
            this.WinFormHost = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

