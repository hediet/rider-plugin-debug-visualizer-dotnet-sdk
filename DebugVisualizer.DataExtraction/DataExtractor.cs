using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using DebugVisualizer.DataExtraction.Data;
using DebugVisualizer.DataExtraction.Extractors;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.DataExtraction
{
    public static class DataExtractor
    {
        public static ComposedDataExtractor MainDataExtractor { get; } =
            new ComposedDataExtractor(new List<IDataExtractor>());

        static DataExtractor()
        {
            MainDataExtractor.DataExtractors.Add(new ToStringExtractor());
            MainDataExtractor.DataExtractors.Add(new GetVisualizationDataExtractor());
            MainDataExtractor.DataExtractors.Add(new JsonDataExtractor());
            MainDataExtractor.DataExtractors.Add(new VisualizationDataExtractor());
        }

        // global::DebugVisualizer.DataExtraction.DataExtractor.Extract(3, "{ \"preferredDataExtractorId\": \"myExtractor\" }").ToString()
        public static JsonString<DataExtractionResult> Extract(object? value,
            string? optionsJson = null)
        {
            
                
            var options = optionsJson != null
                ? JsonString.FromJson<DataExtractionOptions>(optionsJson).Parse()
                : new DataExtractionOptions(null);

            var context = new DataExtractorContext(new List<Extraction>(), MainDataExtractor);
            var extractions = context.GetExtractions(value)
                .OrderByDescending(e => e.ExtractorInfo.Priority)
                .ToList();

            var extraction = extractions.FirstOrDefault(e => e.ExtractorInfo.Id == options.PreferredDataExtractorId) ??
                             extractions.FirstOrDefault();

            var extractedData = extraction != null
                ? new ExtractedData(extraction.ExtractorInfo.Id, extraction.GetVisualizationData())
                : null;
            
            Console.WriteLine(extractedData?.DataExtractorId);

            return JsonString.FromValue(
                new DataExtractionResult(extractedData, extractions.Select(e => e.ExtractorInfo).ToList())
            );
        }
    }
}