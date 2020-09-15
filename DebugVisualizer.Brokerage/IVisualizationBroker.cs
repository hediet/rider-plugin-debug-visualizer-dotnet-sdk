using System;
using System.Collections.Generic;
using DebugVisualizer.Brokerage.Data;

namespace DebugVisualizer.Brokerage
{
    public interface IVisualizationBroker
    {
        void Broker(object? value, IVisualizationBrokerContext context);
    }

    public interface IVisualizationBrokerContext
    {
        IEnumerable<BrokeredVisualizationData> Broker(object? value);
        void Add(BrokeredVisualizationData brokeredVisualizationData);
        void Add(Func<VisualizationData> getVisualizationData, VisualizationBrokerInfo extractorInfo);
    }

    internal class VisualizationBrokerContext : IVisualizationBrokerContext
    {
        private readonly List<BrokeredVisualizationData> _brokerages;
        private readonly IVisualizationBroker _mainBroker;
        private readonly int _recursionLevel;

        public VisualizationBrokerContext(List<BrokeredVisualizationData> brokerages, IVisualizationBroker mainBroker, int recursionLevel = 0)
        {
            _brokerages = brokerages;
            _mainBroker = mainBroker;
            _recursionLevel = recursionLevel;
        }

        // Limited to 5 recursion levels
        public IEnumerable<BrokeredVisualizationData> Broker(object? value)
        {
            var result = new List<BrokeredVisualizationData>();
            if (_recursionLevel < 5)
            {
                _mainBroker.Broker(value,
                    new VisualizationBrokerContext(result, _mainBroker, _recursionLevel + 1));
            }

            return result;
        }

        public void Add(BrokeredVisualizationData brokeredVisualizationData)
        {
            _brokerages.Add(brokeredVisualizationData);
        }
        
        public void Add(Func<VisualizationData> getVisualizationData, VisualizationBrokerInfo extractorInfo)
        {
            Add(new BrokeredVisualizationData(getVisualizationData, extractorInfo));
        }
    }

    public sealed class BrokeredVisualizationData
    {
        private readonly Func<VisualizationData> _getVisualizationData;

        public BrokeredVisualizationData(Func<VisualizationData> getVisualizationData, VisualizationBrokerInfo brokerInfo)
        {
            BrokerInfo = brokerInfo;
            _getVisualizationData = getVisualizationData;
        }

        public VisualizationData GetVisualizationData()
        {
            return _getVisualizationData();
        }

        public VisualizationBrokerInfo BrokerInfo { get; }
    }
}