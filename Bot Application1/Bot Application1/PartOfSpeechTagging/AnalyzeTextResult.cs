using System;

namespace Bot_Application1
{
    /// <summary>
    /// This is the result of analysis from one analyzer.
    /// </summary>
    public class AnalyzeTextResult
    {
        /// <summary>
        /// The unique ID of the analyzer; see Analyzer for more information.
        /// </summary>
        public Guid AnalyzerId { get; set; }

        /// <summary>
        /// The resulting analysis, encoded as JSON. See the documentation for the relevant analyzer kinC:\Users\ekate_000\Desktop\Bot Application1\Bot Application1\LinguisticsClient\AnalyzeTextResult.csd for more information on formatting.
        /// </summary>
        public object Result { get; set; }
    }
}