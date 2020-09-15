using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using DebugVisualizer.Brokerage.Data;
using DebugVisualizer.Brokerage.Brokers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.Brokerage
{
    public static class VisualizationBrokerService
    {
        public static ComposedVisualizationBroker MainVisualizationBroker { get; } =
            new ComposedVisualizationBroker(new List<IVisualizationBroker>());

        static VisualizationBrokerService()
        {
            MainVisualizationBroker.DataExtractors.Add(new ToStringVisualizationBroker());
            MainVisualizationBroker.DataExtractors.Add(new GetVisualizationVisualizationBroker());
            MainVisualizationBroker.DataExtractors.Add(new JsonVisualizationBroker());
            MainVisualizationBroker.DataExtractors.Add(new VisualizationDataVisualizationBroker());
        }

        // global::DebugVisualizer.Brokerage.VisualizationBrokerService.BrokerJson(3, "{ \"preferredDataExtractorId\": \"myExtractor\" }")

        // ReSharper disable once UnusedMember.Global
        public static string BrokerJson(object? value, string? optionsJson = null)
        {
            var options = optionsJson != null ? JsonConvert.DeserializeObject<BrokerOptions>(optionsJson) : null;
            var result = Broker(value, options);
            return JsonConvert.SerializeObject(result);
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        public static VisualizationDataWithBrokers Broker(object? value, BrokerOptions? options)
        {
            if (options == null)
            {
                options = new BrokerOptions(null);
            }
            
            try
            {
                var context = new VisualizationBrokerContext(new List<BrokeredVisualizationData>(), MainVisualizationBroker);
                var extractions = context.Broker(value)
                    .OrderByDescending(e => e.BrokerInfo.Priority)
                    .ToList();

                var extraction =
                    extractions.FirstOrDefault(e => e.BrokerInfo.Id == options.PreferredBrokerId) ??
                    extractions.FirstOrDefault();

                var extractedData = extraction != null
                    ? new VisualizationDataWithBroker(extraction.GetVisualizationData(), extraction.BrokerInfo.Id)
                    : null;
                
                return new VisualizationDataWithBrokers(extractedData, extractions.Select(e => e.BrokerInfo).ToList());
            }
            catch (Exception e)
            {
                return VisualizationDataWithBrokers.FromVisualizationData(new TextData(e.ToString()));
            }
        }
    }
}