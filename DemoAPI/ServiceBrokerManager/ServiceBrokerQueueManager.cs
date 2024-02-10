using DemoAPI.Hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace DemoAPI.ServiceBrokerManager
{
    public class ServiceBrokerQueueManager : IServiceBrokerQueueManager
    {

        private string _connectionString;
        private string _queueName;
        private readonly IHubContext<ApiHub> _hubContext;
        private readonly IConfiguration _configuration;

        public ServiceBrokerQueueManager(IHubContext<ApiHub> hubContext, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ServiceBrokerConfig:ConnectionString").Get<string>();
            _queueName = _configuration.GetSection("ServiceBrokerConfig:SerivceBrokerQueueName").Get<string>();
        }


        public void runServiceBrokerListener()
        {
            Thread _sbListener = new Thread(monitorServiceBroker)
            {
                IsBackground = true
            };
            _sbListener.Start();
        }

        private void monitorServiceBroker()
        {
            try
            {
                try
                {
                    while (true)
                    {
                        ListenServiceBrokerQueue();
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ListenServiceBrokerQueue()
        {
            string query = "WAITFOR (receive top (1) cast(message_body as XML),conversation_handle, message_type_id from [" + _queueName + "])";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    var ds = new DataTable();
                    command.CommandTimeout = 60;
                    using (var da = new SqlDataAdapter(command))
                    {
                        try
                        {
                            connection.Open();
                            da.Fill(ds);
                            foreach (DataRow row in ds.Rows)
                            {
                                var xmlMessage = new XmlDocument();
                                xmlMessage.LoadXml(row.ItemArray[0].ToString());
                                string jsonText = JsonConvert.SerializeXmlNode(xmlMessage);
                                _hubContext.Clients.All.SendAsync("ReceiveData", jsonText);
                            }
                        }
                        catch (SqlException sqlException)
                        {

                        }
                    }
                }
            }
        }


    }
}
