using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.Core;
using MyToolkit.Model;
using Windows.Media.FaceAnalysis;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace EmotionsTrackerVEV
{

    public class EmotionsAnalyzer: ObservableObject
    {
        public EmotionsAnalyzer()
        {
            _pathToPhotosSet = @"big300";
            _pathToRecognitionSet = @"recognitionSet";

            // Create face detection
            _localFaceDetector = new ImageFaceDetectorLocal();

            _faceClient = new FaceServiceClient("d7a8434e6462457690ce79baa1ba26dc");
            _emotionClient = new EmotionServiceClient("7fb7e23cbe644c44a2b37dc2e92f62bb");
        }

        readonly string personGroupId = "myfriends";

        private ImageFaceDetectorLocal _localFaceDetector;
        private FaceServiceClient _faceClient;

        private EmotionServiceClient _emotionClient;
        private Face[] Faces { get; set; }

        private string _pathToPhotosSet;

        public string PathToPhotosSet
        {
            get { return _pathToPhotosSet; }
            set
            {
                if (Set(ref _pathToPhotosSet, value))
                {
                    
                }
            }
        }

        private string _pathToRecognitionSet;

        public string PathToRecognitionSet
        {
            get { return _pathToRecognitionSet; }
            set
            {
                if (Set(ref _pathToRecognitionSet, value))
                {

                }
            }
        }

        public async Task Start()
        {
            //await TrainBoy();
            Dictionary<int, Scores> chartData = new Dictionary<int, Scores>();


            var pathsToImagesToAnalyse = GetImagesPathsInDirectory(_pathToPhotosSet);
            foreach (var path in pathsToImagesToAnalyse)
            {
                var locRes = await _localFaceDetector.HasFace(path);
                if (locRes)
                {
                    var rect = await TryToRecognizeBoyAsync(path);
                    if (rect != null)
                    {
                        using (Stream s = File.OpenRead(path))
                        {
                            var res = await _emotionClient.RecognizeAsync(s,
                                new Rectangle[] {new Rectangle()
                                {
                                    Height = rect.Height,
                                    Width = rect.Width,
                                    Left = rect.Left,
                                    Top = rect.Top
                                } });
                            if (res != null)
                            {
                                var i = int.Parse(path.Replace(String.Format("{0}\\out", _pathToPhotosSet), "")
                                    .Replace(".png", ""));
                                chartData.Add(i, res[0].Scores);
                            }

                            await Task.Delay(3000);
                        }
                    }
                }
            }

            await WriteChartDataToCsv(chartData);
        }

        private async Task WriteChartDataToCsv(Dictionary<int, Scores> chartData)
        {
            var ordererd = chartData.OrderBy(o => o.Key);
            var csv = new StringBuilder();

            foreach (var score in ordererd)
            {
               var newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", score.Key, score.Value.Anger, score.Value.Contempt, score.Value.Disgust,
                   score.Value.Fear, score.Value.Happiness, score.Value.Neutral, score.Value.Sadness, score.Value.Surprise);
                csv.AppendLine(newLine);
            }

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add(".csv");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                //Append string to file
                await Windows.Storage.FileIO.AppendTextAsync(file, csv.ToString());
                // Application now has read/write access to the picked file
                //File.WriteAllText(file.Path, csv.ToString());
            }

            
            //File.AppendAllText(filePath, csv.ToString());
        }

        private async Task<FaceRectangle> TryToRecognizeBoyAsync(string path)
        {
            using (Stream s = File.OpenRead(path))
            {
                var faces = await _faceClient.DetectAsync(s);
                var faceIds = faces.Select(face => face.FaceId).ToArray();
                await Task.Delay(3000);
                if (faceIds != null && faceIds.Count()>0)
                {
                    var results = await _faceClient.IdentifyAsync(personGroupId, faceIds);
                    foreach (var identifyResult in results)
                    {
                        await Task.Delay(3000);

                        if (identifyResult.Candidates.Length == 0)
                        {
                            return null;
                        }
                        else
                        {
                            // Get top 1 among all candidates returned
                            var candidateId = identifyResult.Candidates[0].PersonId;
                            var person = await _faceClient.GetPersonAsync(personGroupId, candidateId);
                        
                            if (person.Name == "Boy")
                            {
                                return faces.FirstOrDefault(o => o.FaceId == identifyResult.FaceId).FaceRectangle;
                            }
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        private async Task TrainBoy()
        {
            var paths = GetImagesPathsInDirectory(_pathToRecognitionSet);

            // Create an empty person group
            // _faceClient.CreatePersonGroupAsync(personGroupId, "My Friends");

            // Define Anna
            CreatePersonResult friend1 = await _faceClient.CreatePersonAsync(
                // Id of the person group that the person belonged to
                personGroupId,
                // Name of the person
                "Boy"
            );

            // Directory contains image files of Anna
            foreach (string imagePath in paths)
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add to Anna
                    await _faceClient.AddPersonFaceAsync(
                        personGroupId, friend1.PersonId, s);

                    await Task.Delay(1000);
                }
            }

            await _faceClient.TrainPersonGroupAsync(personGroupId);

            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await _faceClient.GetPersonGroupTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }

            //List<FaceAttributeType> faceAg = new List<FaceAttributeType>();
            //faceAg.Add(FaceAttributeType.Age);
            //faceAg.Add(FaceAttributeType.Gender);

            //try
            //{
            //    using (Stream imageFileStream = File.OpenRead(path))
            //    {
            //        Faces = await _faceClient.DetectAsync(imageFileStream, true, false, faceAg); 
            //        var faceRects = Faces.Select(face => face.FaceRectangle); 
            //        var faceA = Faces.Select(face => face.FaceAttributes);
            //        var faceAtr = faceA.ToArray();
            //    }
            //}
            //catch (Exception)
            //{

            //}
            //if (Faces != null)
            //    return Faces.FirstOrDefault().FaceId;
            //return null;
        }


        private List<string> GetImagesPathsInDirectory(string dir)
        {
            return Directory.GetFiles(dir).ToList();
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles("*.png"); 
            List<string> images = new List<string>();
            foreach (FileInfo file in files)
            {
                images.Add(file.Name);
            }
            return images;
        }

        
    }
}
