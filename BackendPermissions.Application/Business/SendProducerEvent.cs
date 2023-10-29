using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPermissions.Application.Business
{
    public class ProducerEventKafka
    {
        /// <summary>
        /// SGC - Produce a event to kafka service
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<bool> SendProducerEvent(string key, string value)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync("permission_challenge", new Message<string, string> { Key = key, Value = value });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                    return true;
                }
                catch (ProduceException<string, string> e)
                {
                    throw new Exception("Error calling to register produce Event");
                }
            }
        }

    }

}

