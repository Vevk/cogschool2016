using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace EmotionsTrackerVEV
{
    public class ImageFaceDetectorLocal
    {
        
        IList<DetectedFace> detectedFaces;

        public ImageFaceDetectorLocal()
        {
            
        }

        public async Task<bool> HasFace(string path)
        {
            var bmp = await GetSoftwareBitmap(path);
            FaceDetector faceDetector = await FaceDetector.CreateAsync();

            detectedFaces = await faceDetector.DetectFacesAsync(bmp);

            if (detectedFaces == null)
                return false;
            return true;
        }

        private async Task<SoftwareBitmap> GetSoftwareBitmap(string path)
        {
            //FileOpenPicker photoPicker = new FileOpenPicker();
            //photoPicker.ViewMode = PickerViewMode.Thumbnail;
            //photoPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            ////photoPicker.FileTypeFilter.Add(".jpg");
            ////photoPicker.FileTypeFilter.Add(".jpeg");
            //photoPicker.FileTypeFilter.Add(".png");
            ////photoPicker.FileTypeFilter.Add(".bmp");

            //StorageFile photoFile = await photoPicker.PickSingleFileAsync();
            //if (photoFile == null)
            //{
            //    return null;
            //}
            //IRandomAccessStream fileStream = await photoFile.OpenAsync(FileAccessMode.Read);

            using (FileStream fileStream = File.OpenRead(path))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream.AsRandomAccessStream());

                BitmapTransform transform = new BitmapTransform();
                const float sourceImageHeightLimit = 1280;

                if (decoder.PixelHeight > sourceImageHeightLimit)
                {
                    float scalingFactor = (float)sourceImageHeightLimit / (float)decoder.PixelHeight;
                    transform.ScaledWidth = (uint)Math.Floor(decoder.PixelWidth * scalingFactor);
                    transform.ScaledHeight = (uint)Math.Floor(decoder.PixelHeight * scalingFactor);
                }

                SoftwareBitmap sourceBitmap = await decoder.GetSoftwareBitmapAsync(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);

                // Use FaceDetector.GetSupportedBitmapPixelFormats and IsBitmapPixelFormatSupported to dynamically
                // determine supported formats
                const BitmapPixelFormat faceDetectionPixelFormat = BitmapPixelFormat.Gray8;

                SoftwareBitmap convertedBitmap;

                if (sourceBitmap.BitmapPixelFormat != faceDetectionPixelFormat)
                {
                    convertedBitmap = SoftwareBitmap.Convert(sourceBitmap, faceDetectionPixelFormat);
                }
                else
                {
                    convertedBitmap = sourceBitmap;
                }

                return convertedBitmap;
            }
           
        }
    }
}
