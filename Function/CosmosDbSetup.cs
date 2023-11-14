//using Infrastructure.CosmosDB;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;

//namespace Function
//{
//    public class CosmosDbSetup : IConfigureOptions<CosmosDbOptions>
//    {
//        private const string SectionName = CosmosDbOptions.CosmosDb;

//        private readonly IConfiguration _configuration;

//        public CosmosDbSetup(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public void Configure(CosmosDbOptions options)
//        {
//            _configuration.GetSection(SectionName).Bind(options);
//        }
//    }
//}
