using FnF_Tabs_MVVM_XFA.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FnF_Tabs_MVVM_XFA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void BtnRecreateDB_Clicked(object sender, EventArgs e)
        {
            try
            {
                await App.FnfDatabase.RecreateDB();

                await DisplayAlert("F&F: DB recreated", "If you want you can create Sample Data also. To make recreated DB effective, please restart the app.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("F&F: Error Recreating DB", ex.Message, "OK");
            }
        }

        private async void BtnSampleData_Clicked(object sender, EventArgs e)
        {
            try
            {
                //DBOP:  Get item count from database
                int itemCount = await App.FnfDatabase.GetTotalItemCount();

                //DBOP:  Create and Save Sample Data if there are no items
                if (itemCount == 0)
                {
                    // Create Sample Data
                    var sampleData = new List<Item>
                    {
                        new Item { Id = 1, Name="First Family Member", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Family", Notes="This is the notes for First Family Member. Rest all is just garbage. No flkdflkg kgkskgdskf kksdajkdfa jkfiaudieuiw09r8 fsdfkds af fksdfisriqwek ng;n sari kfksdmnfl;ds irpwee rlkjidasfmfssdjfgkfdweir pv f dsfkjds; ;fkasp[dsfigmsd;sfgkds; g] ;lskf;ksdfkdsirewk;k;lgk;/df/sd/f;askfwe    dfiwoekrmf'sdkf/f/,dsfieokfmdmf;lasdjkf; posfksas sdif ewpori r ewifsdafksf;asdktr", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 2, Name="Second Family Member", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Family", Notes="This is the notes for Second Family Member", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 3, Name="First Friend", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Friends", Notes="This is the notes for First Friend", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 4, Name="Second Friend", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Friends", Notes="This is the notes for Second Friend", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 5, Name="Third Family Member", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Family", Notes="This is the notes for Third Family Member", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 6, Name="Third Friend", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Friends", Notes="This is the notes for Third Friend", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 7, Name="Fourth Family Member", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Family", Notes="This is the notes for Fourth Family Member", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 8, Name="Fourth Friend", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Friends", Notes="This is the notes for Fourth Friend", ImageURI="person.png", ShowActionMenu=false },
                        new Item { Id = 9, Name="Fifth Family Member", DateOfBirth=App.DefaultDate, Anniversary=App.DefaultDate, Category="Family", Notes="This is the notes for Fifth Family Member", ImageURI="person.png", ShowActionMenu=false },
                    };

                    //DBOP: Save the sample data to people table
                    foreach (var item in sampleData)
                    {
                        await App.FnfDatabase.AddItemAsync(item);
                    }

                    App.last_id = 9;

                    await DisplayAlert("F&F: Sample Data", "Created Successfully. You may have to restart the app.", "OK");
                }
                else
                    await DisplayAlert("F&F: Error Creating Sample Data", "Recreate DB before Creating Sample Data", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("F&F: Error Creating Sample Data", ex.Message, "OK");
            }
        }
    }
}