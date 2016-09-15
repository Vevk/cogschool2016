using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using MyToolkit.Command;
using MyToolkit.Mvvm;

namespace EmotionsTrackerVEV
{
    public class MainPageModel : ViewModelBase
    {
        public EmotionsAnalyzer Analyzer { get; private set; }

        public AsyncRelayCommand StartAnalysisCommand { get; set; }

        public MainPageModel()
        {
            Analyzer = new EmotionsAnalyzer();
            //StartAnalysisCommand = new AsyncRelayCommand(StartAnalysis);
        }

        //    private async Task StartAnalysis()
        //    {
        //        return await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Analyzer.Start()); 
        //    }
    }
}
