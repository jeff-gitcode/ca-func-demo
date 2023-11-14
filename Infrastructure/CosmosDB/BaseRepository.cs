using Application.Abstraction;
using Domain;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CosmosDB
{
    public abstract class CosmosDbBaseRepository
    {
        public DocumentClient Client { get; private set; }

        public CosmosDbBaseRepository(string endpointUrl, string authorizationKey)
        {
            var client = new DocumentClient(
                new Uri(endpointUrl),
                authorizationKey,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );

            Client = client;
        }
    }
}
