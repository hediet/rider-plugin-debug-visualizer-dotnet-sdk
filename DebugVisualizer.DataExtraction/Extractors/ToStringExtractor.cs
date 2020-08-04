using System.Collections.Generic;
using DebugVisualizer.DataExtraction.Data;

namespace DebugVisualizer.DataExtraction.Extractors
{
    class ToStringExtractor : IDataExtractor
    {
        public void GetExtractions(object? value, IDataExtractorContext context)
        {
            context.AddExtraction(
                () => new TextData(value?.ToString() ?? "(null)"),
                new DataExtractorInfo("ToString", "ToString", 100)
            );
        }
    }
}