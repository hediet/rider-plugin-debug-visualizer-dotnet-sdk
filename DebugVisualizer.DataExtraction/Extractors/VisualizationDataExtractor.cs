using System.Collections.Generic;
using DebugVisualizer.DataExtraction.Data;

namespace DebugVisualizer.DataExtraction.Extractors
{
    class VisualizationDataExtractor : GenericDataExtractor<VisualizationData>
    {
        public override void GetExtractions(VisualizationData value, IDataExtractorContext context)
        {
            context.AddExtraction(
                () => value,
                new DataExtractorInfo("VisualizationDataExtractor", "Visualization", 1000)
            );
        }
    }
}