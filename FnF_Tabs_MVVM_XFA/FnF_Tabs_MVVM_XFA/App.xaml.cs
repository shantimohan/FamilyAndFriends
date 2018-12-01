using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FnF_Tabs_MVVM_XFA.Views;
using FnF_Tabs_MVVM_XFA.Services;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FnF_Tabs_MVVM_XFA
{
    public partial class App : Application
    {
        public static DateTime DefaultDate = new DateTime(2018, 1, 1);

        //DBOP: Declare database instance
        static FnFDataStore fnfDatabase;
        //DBOP: This last_id will set the Id and will be done away once autoincrement is implemented
        public static int last_id = 0;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
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
