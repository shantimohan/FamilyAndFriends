using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FnF_Tabs_MVVM_XFA.Models;
using System.Diagnostics;
using FnF_Tabs_MVVM_XFA.ViewModels;
using System.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Threading.Tasks;
using PCLStorage;

namespace FnF_Tabs_MVVM_XFA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public Item Item { get; set; }
        public ItemsViewModel itemsViewModel;    //CHANGE: Added view model as field
        private bool newItem;
        private MediaFile photoPicked;
        private string strFullFileName;

        // CHANGE: Added view model to the constructor params
        public ItemDetailPage(ItemsViewModel ivmIn, Item itemIn = null)
        {
            InitializeComponent();

            this.newItem = itemIn == null ? true : false;

            Item = new Item
            {
                Name = $"New {(ivmIn.Title == "Family" ? "Family Member" : "Friend")}",
                DateOfBirth = App.DefaultDate,
                Anniversary = App.DefaultDate,
                Category = ivmIn.Title,
                Notes = "",
                ImageURI = "person.png",
            };

            if (newItem)
            {
                this.Title = Item.Name;
            }
            else
            {
                // Show the details of the selected Item, for update or delete
                Item.Id = itemIn.Id;
                Item.Name = itemIn.Name;
                Item.DateOfBirth = itemIn.DateOfBirth;
                Item.Anniversary = itemIn.Anniversary;
                Item.Category = itemIn.Category;
                Item.Notes = itemIn.Notes;
                Item.ImageURI = itemIn.ImageURI;

                this.Title = $"ID: {(itemIn?.Id)}";
            }

            strFullFileName = Item.ImageURI;

            // CHANGE: Setting the view model field in class constructor,
            //            which will be used to update the collection
            itemsViewModel = ivmIn;

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            // Add a new Item or Update an existing Item.
            try
            {
                Debug.WriteLine($"New Item - Save: last_id = {App.last_id}");

                if (newItem)
                {
                    Item.Id = ++App.last_id;
                    //MessagingCenter.Send(this, "AddItem", Item);
                    // CHANGE: Moved functionality of MessageCenter code here
                    await App.FnfDatabase.AddItemAsync(Item);
                    itemsViewModel.Items.Add(Item);
                }
                else
                {
                    await App.FnfDatabase.UpdateItemAsync(Item);

                    var oldItem = itemsViewModel.Items.Where((Item arg) => arg.Id == Item.Id).FirstOrDefault();
                    int index = itemsViewModel.Items.IndexOf(oldItem);
                    // Remove the old item and insert new one at the same place
                    itemsViewModel.Items.Remove(oldItem);
                    itemsViewModel.Items.Insert(index, Item);

                    await DisplayAlert($"Item {(newItem ? "Added" : "Updated")}", "Successful", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert($"Error {(newItem ? "Adding" : "Editing")} Item", ex.Message, "OK");
            }
            finally
            {
                await Navigation.PopAsync();
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            // Delete an existing Item
            try
            {
                await App.FnfDatabase.DeleteItemAsync(Item);

                var oldItem = itemsViewModel.Items.Where((Item arg) => arg.Id == Item.Id).FirstOrDefault();
                itemsViewModel.Items.Remove(oldItem);

                await DisplayAlert("Item Deleted", "Successful", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error Deleting Item", ex.Message, "OK");
            }
            finally
            {
                await Navigation.PopAsync();
            }
        }

        private async void btnChangeImage_Clicked(object sender, EventArgs e)
        {
            if (newItem)
            {
                await DisplayAlert("Person Image", "You can select the image only in Edit mode. Tap on this person after saving the details", "OK");
            }
            else
            {
                bool b = await CrossMedia.Current.Initialize();

                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    photoPicked = await CrossMedia.Current.PickPhotoAsync();

                    if (photoPicked != null)
                    {
                        //ImageSource imageSource = ImageSource.FromStream(() =>
                        //{
                        //    Stream streamPhoto = photoPicked.GetStream();
                        //    return streamPhoto;
                        //});

                        strFullFileName = photoPicked.Path;
                        int ndx = strFullFileName.LastIndexOf('.');
                        string picFileExtn = strFullFileName.Substring(ndx);

                        //using (Stream srcStream = photoPicked.GetStream())
                        //{
                        //    // Rotate the JPEG picture as required in Android only
                        //    //   iOS & Windows display it correctly whatever be
                        //    //   the orientation of the original picture
                        //    if ( Device.RuntimePlatform == Device.Android &&
                        //        (picFileExtn.ToLower() == ".jpg" || picFileExtn.ToLower() == ".jpeg"))
                        //    {
                        //        var picInfo = ExifReader.ReadJpeg(srcStream);
                        //        ExifOrientation orientation = picInfo.Orientation;

                        //        switch (orientation)
                        //        {
                        //            case ExifOrientation.TopRight:
                        //                await imgContact.RotateTo(90.0);
                        //                break;
                        //            case ExifOrientation.BottomRight:
                        //                await imgContact.RotateTo(180.0);
                        //                break;
                        //            case ExifOrientation.BottomLeft:
                        //                await imgContact.RotateTo(270.0);
                        //                break;
                        //        }
                        //    }
                        //}  // end of: using (Stream srcStream = photoPicked.GetStream())

                        strFullFileName = await CopyPhotoAsync();
                        imgPerson.Source = strFullFileName;
                        Item.ImageURI = strFullFileName;
                    }
                    else
                    {
                        await DisplayAlert("F&F: Pick a Photo", "No photo selected.", "OK");
                    }  // end of: if (photoPicked != null)
                }
                else
                {
                    await DisplayAlert("F&F: Pick a Photo", "Selecting a photo is not supported", "OK");
                }  // end of: if (CrossMedia.Current.IsPickPhotoSupported)

            }  // end of: if (newitem)

        }  // end of: GetImage_Clicked

        private async Task<string> CopyPhotoAsync()
        {
            string contactImageUri = "";
            string FileName = $"PersonImage{Item.Id}";

            // Format the pic file name to copy to
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;

            int ndx = strFullFileName.LastIndexOf('.');
            string picFileExtn = strFullFileName.Substring(ndx);

            string copy2Filename = FileName + picFileExtn;

            // Delete if the pic file already exists
            string existingPicFileExtn = picFileExtn;
            if (existingPicFileExtn.Length != 0 && picFileExtn != existingPicFileExtn)
            {
                //await FileHelper.DeleteFileAsync(d2r.FileName + existingPicFileExtn);
                IFile existingPicFile = await rootFolder.GetFileAsync(FileName + existingPicFileExtn);
                await existingPicFile.DeleteAsync();
            }

            try
            {
                // Copy the picked photo to local storage to be independent of the album
                using (Stream srcStream = photoPicked.GetStream())
                {
                    IFile destPhotoFile = await rootFolder.CreateFileAsync(copy2Filename, CreationCollisionOption.ReplaceExisting);
                    Stream destStream = await destPhotoFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite);

                    await srcStream.CopyToAsync(destStream);
                    destStream.Dispose();

                    contactImageUri = rootFolder.Path + Device.OnPlatform("/", "/", "\\") + copy2Filename;

                }  // end of: using (Stream srcStream = photoPicked.GetStream())
            }
            catch (Exception ex)
            {
                await DisplayAlert("F&F: Error Copying Pic", ex.Message, "OK");
            }

            return contactImageUri;
        }

    }  // end of: class ItemDetailPage : ContentPage

}  // end of: namespace
