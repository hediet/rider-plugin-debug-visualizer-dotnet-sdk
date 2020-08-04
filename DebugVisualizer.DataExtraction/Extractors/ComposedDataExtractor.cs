using System.Collections.Generic;
using System.Linq;

namespace DebugVisualizer.DataExtraction.Extractors
{
    public sealed class ComposedDataExtractor : IDataExtractor
    {
        public List<IDataExtractor> DataExtractors { get; }
        
        public ComposedDataExtractor(List<IDataExtractor> dataExtractors)
        {
            DataExtractors = dataExtractors;
        }

        public void GetExtractions(object? value, IDataExtractorContext context)
        {
            foreach (var d in DataExtractors)
            {
                d.GetExtractions(value, context);
            }
        }
    }
}