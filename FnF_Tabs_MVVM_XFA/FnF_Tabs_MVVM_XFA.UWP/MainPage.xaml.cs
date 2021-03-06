﻿using Syncfusion.ListView.XForms.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FnF_Tabs_MVVM_XFA.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            //string dbPath = FileAccessHelper.GetLocalFilePath("people.db3");
            SfListViewRenderer.Init();
            LoadApplication(new FnF_Tabs_MVVM_XFA.App());
        }
    }
}
