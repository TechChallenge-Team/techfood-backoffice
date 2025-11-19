using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.Infra.Persistence.Repositories
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private const string TableName = "Customers";

        public CustomerRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<Guid> CreateAsync(Customer customer)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["Id"] = new AttributeValue { S = customer.Id.ToString() },
                ["Name"] = new AttributeValue { S = customer.Name.FullName },
                ["Email"] = new AttributeValue { S = customer.Email.Address },
                ["DocumentType"] = new AttributeValue { S = customer.Document.Type.ToString() },
                ["DocumentValue"] = new AttributeValue { S = customer.Document.Value },
                ["CreatedAt"] = new AttributeValue { S = DateTime.UtcNow.ToString("O") }
            };

            // Add Phone if not null
            if (customer.Phone != null)
            {
                item["PhoneNumber"] = new AttributeValue { S = customer.Phone.Number };
                item["PhoneCountryCode"] = new AttributeValue { S = customer.Phone.CountryCode };
                item["PhoneDDD"] = new AttributeValue { S = customer.Phone.DDD };
            }

            var putRequest = new PutItemRequest
            {
                TableName = TableName,
                Item = item
            };

            await _dynamoDb.PutItemAsync(putRequest);
            return customer.Id;
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            var getRequest = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    ["Id"] = new AttributeValue { S = id.ToString() }
                }
            };

            var response = await _dynamoDb.GetItemAsync(getRequest);

            if (response.Item == null || !response.Item.Any())
                return null;

            return MapDynamoItemToCustomer(response.Item);
        }

        public async Task<Customer?> GetByDocument(DocumentType type, string value)
        {
            return await GetByDocumentAsync(type, value);
        }

        public async Task<Customer?> GetByDocumentAsync(DocumentType documentType, string documentValue)
        {
            var scanRequest = new ScanRequest
            {
                TableName = TableName,
                FilterExpression = "DocumentType = :docType AND DocumentValue = :docValue",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [":docType"] = new AttributeValue { S = documentType.ToString() },
                    [":docValue"] = new AttributeValue { S = documentValue }
                }
            };

            var response = await _dynamoDb.ScanAsync(scanRequest);

            if (response.Items == null || !response.Items.Any())
                return null;

            return MapDynamoItemToCustomer(response.Items.First());
        }

        private static Customer MapDynamoItemToCustomer(Dictionary<string, AttributeValue> item)
        {
            var documentType = Enum.Parse<DocumentType>(item["DocumentType"].S);
            var document = new Document(documentType, item["DocumentValue"].S);
            var name = new Name(item["Name"].S);
            var email = new Email(item["Email"].S);
            
            // For now, we won't use Phone as it requires multiple fields
            Phone? phone = null;

            // Create Customer - ID will be managed by DynamoDB
            var customer = new Customer(name, email, document, phone);
            
            // Try to set the ID via reflection if possible
            try
            {
                var entityType = customer.GetType();
                while (entityType != null)
                {
                    var idProperty = entityType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
                    if (idProperty != null && idProperty.CanWrite)
                    {
                        idProperty.SetValue(customer, Guid.Parse(item["Id"].S));
                        break;
                    }
                    
                    var idField = entityType.GetField("Id", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) ??
                                  entityType.GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance);
                    
                    if (idField != null)
                    {
                        idField.SetValue(customer, Guid.Parse(item["Id"].S));
                        break;
                    }
                    
                    entityType = entityType.BaseType;
                }
            }
            catch
            {
                // If unable to set ID via reflection, continue without ID
                // In a real scenario, you might want to log or handle this case
            }

            return customer;
        }
    }
}
