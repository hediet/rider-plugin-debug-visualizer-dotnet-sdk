using System.Collections.Generic;

namespace DebugVisualizer.DataExtraction.Extractors
{
    public abstract class GenericDataExtractor<T> : IDataExtractor
    {
        public void GetExtractions(object? value, IDataExtractorContext context)
        {
            if (value is T val)
            {
                GetExtractions(val, context);
            }
        }

        public abstract void GetExtractions(T value, IDataExtractorContext context);
    }
}