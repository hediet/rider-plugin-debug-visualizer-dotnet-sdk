using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DebugVisualizer.DataExtraction.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.DataExtraction.Extractors
{
    public interface IVisualizable
    {
        public object GetVisualizationData();
    }

    class GetVisualizationDataExtractor : IDataExtractor
    {
        public void GetExtractions(object? value, IDataExtractorContext context)
        {
            if (value == null)
            {
                return;
            }

            var type = value.GetType();
            var m = type.GetMethod("GetVisualizationData",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (m == null)
            {
                return;
            }

            object? result;
            try
            {
                result = m.Invoke(value, null);
            }
            catch (Exception e)
            {
                return;
            }

            foreach (var extraction in context.GetExtractions(result))
            {
                context.AddExtraction(
                    extraction.GetVisualizationData,
                    new DataExtractorInfo($"GetVisualizationData-{extraction.ExtractorInfo.Id}",
                        $"GetVisualizationData ({extraction.ExtractorInfo.Name})",
                        1000 + extraction.ExtractorInfo.Priority)
                );
            }
        }
    }
}