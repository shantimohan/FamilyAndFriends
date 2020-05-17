using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FnF_Tabs_MVVM_XFA.Views;
using FnF_Tabs_MVVM_XFA.Services;
using System.IO;
using FnF_Tabs_MVVM_XFA.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FnF_Tabs_MVVM_XFA
{
    public partial class App : Application
    {
        public static DateTime DefaultDate = new DateTime(2018, 1, 1);
        public static Item tappedItem;

        //DBOP: Declare database instance
        static FnFDataStore fnfDatabase;
        //DBOP: This last_id will set the Id and will be done away once autoincrement is implemented
        public static int last_id = 0;

        public App()
        {
            // Register syncfusion licence
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU3MjQ4QDMxMzgyZTMxMmUzMEtYMlRxc2hIYXp5TS9OUjhGdnh3aUxNV0pIeTc1M0tSVFJrTExwQnpBM1U9");
            InitializeComponent();

            MainPage = new MainPage();
            //MainPage = new SplashPage();
        }

        //DBOP: Initialize database instance
        public static FnFDataStore FnfDatabase
        {
            get
            {
                if (fnfDatabase == null)
                    fnfDatabase = new FnFDataStore(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PeopleSQLite.db3"));

                return fnfDatabase;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
