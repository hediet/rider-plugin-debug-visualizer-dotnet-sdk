using System;
using System.Collections.Generic;
using DebugVisualizer.DataExtraction.Data;

namespace DebugVisualizer.DataExtraction.Extractors
{
    public interface IDataExtractor
    {
        void GetExtractions(object? value, IDataExtractorContext context);
    }

    public interface IDataExtractorContext
    {
        IEnumerable<Extraction> GetExtractions(object? value);
        void AddExtraction(Extraction extraction);
        void AddExtraction(Func<VisualizationData> getVisualizationData, DataExtractorInfo extractorInfo);
    }

    internal class DataExtractorContext : IDataExtractorContext
    {
        private readonly List<Extraction> _extractions;
        private readonly IDataExtractor _mainExtractor;
        private readonly int _recursionLevel;

        public DataExtractorContext(List<Extraction> extractions, IDataExtractor mainExtractor, int recursionLevel = 0)
        {
            _extractions = extractions;
            _mainExtractor = mainExtractor;
            _recursionLevel = recursionLevel;
        }

        // Limited to 5 recursion levels
        public IEnumerable<Extraction> GetExtractions(object? value)
        {
            var result = new List<Extraction>();
            if (_recursionLevel < 5)
            {
                _mainExtractor.GetExtractions(value,
                    new DataExtractorContext(result, _mainExtractor, _recursionLevel + 1));
            }

            return result;
        }

        public void AddExtraction(Extraction extraction)
        {
            _extractions.Add(extraction);
        }
        
        public void AddExtraction(Func<VisualizationData> getVisualizationData, DataExtractorInfo extractorInfo)
        {
            AddExtraction(new Extraction(getVisualizationData, extractorInfo));
        }
    }

    public sealed class Extraction
    {
        private readonly Func<VisualizationData> _getVisualizationData;

        public Extraction(Func<VisualizationData> getVisualizationData, DataExtractorInfo extractorInfo)
        {
            ExtractorInfo = extractorInfo;
            _getVisualizationData = getVisualizationData;
        }

        public VisualizationData GetVisualizationData()
        {
            return _getVisualizationData();
        }

        public DataExtractorInfo ExtractorInfo { get; }
    }
}