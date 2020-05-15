using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FnF_Tabs_MVVM_XFA.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GetImage : ContentPage
	{
        private MediaFile photoPicked;

        public GetImage ()
		{
			InitializeComponent ();

            imgCopied.Source = lblCopiedPath.Text;
        }

        async void btnGetImage_Clicked(object sender, EventArgs e)
        {
            bool b = await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                photoPicked = await CrossMedia.Current.PickPhotoAsync();

                if (photoPicked != null)
                {
                    imgPerson.Source = ImageSource.FromStream(() =>
                    {
                        Stream streamPhoto = photoPicked.GetStream();
                        return streamPhoto;
                    });

                    lblImagePath.Text = photoPicked.Path;
                    int ndx = lblImagePath.Text.LastIndexOf('.');
                    string picFileExtn = lblImagePath.Text.Substring(ndx);

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

                    lblCopiedPath.Text =await  CopyPhotoAsync();
                    imgCopied.Source = lblCopiedPath.Text;
                }
                else
                {
                    await DisplayAlert("D2R: Pick a Photo", "No photo selected.", "OK");
                }  // end of: if (photoPicked != null)
            }
            else
            {
                await DisplayAlert("D2R: Pick a Photo", "Selecting a photo is not supported", "OK");
            }  // end of: if (CrossMedia.Current.IsPickPhotoSupported)

        }  // end of: GetImage_Clicked

        private async Task<string> CopyPhotoAsync()
        {
            string contactImageUri = "";
            string FileName = "PersonImage";

            // Format the pic file name to copy to
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;

            int ndx = lblImagePath.Text.LastIndexOf('.');
            string picFileExtn = lblImagePath.Text.Substring(ndx);

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

                    string strFolderSeparator = "/";
                    if (Device.RuntimePlatform == Device.UWP)
                        strFolderSeparator = "\\";
                    contactImageUri = rootFolder.Path + strFolderSeparator + copy2Filename;

                }  // end of: using (Stream srcStream = photoPicked.GetStream())
            }
            catch (Exception ex)
            {
                await DisplayAlert("D2R: Error Copying Pic", ex.Message, "OK");
            }

            return contactImageUri;
        }
    }
}