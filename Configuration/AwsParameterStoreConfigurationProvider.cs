using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;

namespace MovieApp.Configuration
{
    public class AwsParameterStoreConfigurationProvider : ConfigurationProvider
    {
        private readonly IAmazonSimpleSystemsManagement _ssmClient;
        private readonly string _parameterName;

        public AwsParameterStoreConfigurationProvider(IAmazonSimpleSystemsManagement ssmClient, string parameterName)
        {
            _ssmClient = ssmClient;
            _parameterName = parameterName;
        }

        public override void Load()
        {
            var parameterValue = GetParameterValueAsync(_parameterName).GetAwaiter().GetResult();
            if (parameterValue != null)
            {
                Data.Add(_parameterName, parameterValue);
            }
        }

        private async Task<string> GetParameterValueAsync(string parameterName)
        {
            try
            {
                var request = new GetParameterRequest
                {
                    Name = parameterName,
                    WithDecryption = true
                };

                var response = await _ssmClient.GetParameterAsync(request);
                return response.Parameter.Value;
            }
            catch (Exception ex)
            {
                // Handle errors accordingly
                Console.WriteLine($"Error retrieving parameter {parameterName}: {ex.Message}");
                return null;
            }
        }
    }

    public class AwsParameterStoreConfigurationSource : IConfigurationSource
    {
        private readonly IAmazonSimpleSystemsManagement _ssmClient;
        private readonly string _parameterName;

        public AwsParameterStoreConfigurationSource(IAmazonSimpleSystemsManagement ssmClient, string parameterName)
        {
            _ssmClient = ssmClient;
            _parameterName = parameterName;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AwsParameterStoreConfigurationProvider(_ssmClient, _parameterName);
        }
    }

    public class SecretsManagerService
    {
        private readonly IAmazonSecretsManager _secretsManager;

        public SecretsManagerService(IAmazonSecretsManager secretsManager)
        {
            _secretsManager = secretsManager;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            try
            {
                var response = await _secretsManager.GetSecretValueAsync(request);
                if (response.SecretString != null)
                {
                    return response.SecretString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving secret: {e.Message}");
            }
            return null;
        }
    }
}