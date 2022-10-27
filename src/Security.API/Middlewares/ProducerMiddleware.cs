using System.Net;
using System.Text.Json;
using Confluent.Kafka;
using N5.Challenge.Services.Security.API.Application.Adapters;

namespace N5.Challenge.Services.Security.API.Middlewares
{
    public sealed class ProducerMiddleware : IMiddleware
    {
        private IConfiguration Configuration;

        public ProducerMiddleware(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string bootstrapServers = Configuration["KafkaConfiguration:Uri"];
            string topic = Configuration["KafkaConfiguration:Topic"];
            var operationName = context.Request.Method;
            var dto = new OperationDTO() { Id = Guid.NewGuid().ToString(), OperationName = operationName };
            var message = JsonSerializer.Serialize(dto);

            ProducerConfig config = new ProducerConfig {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using(var producer = new ProducerBuilder<Null, string> (config).Build())
                {
                    var result = await producer.ProduceAsync(topic, new Message <Null, string> {
                        Value = message
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            await next(context);
        }
    }
}