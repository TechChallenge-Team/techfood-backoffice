using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.Infra.Persistence.DynamoDB;

public static class DynamoDbInitializer
{
    public static async Task InitializeTablesAsync(IAmazonDynamoDB dynamoDb)
    {
        await CreateCustomersTableAsync(dynamoDb);
    }

    private static async Task CreateCustomersTableAsync(IAmazonDynamoDB dynamoDb)
    {
        const string tableName = nameof(Customer);
        
        try
        {
            // Check if table already exists
            var describeTableRequest = new DescribeTableRequest { TableName = tableName };
            await dynamoDb.DescribeTableAsync(describeTableRequest);
            Console.WriteLine($"Table {tableName} already exists.");
            return;
        }
        catch (ResourceNotFoundException)
        {
            // Table doesn't exist, let's create it
        }

        var createTableRequest = new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    AttributeName = "Id",
                    AttributeType = ScalarAttributeType.S
                }
            },
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement
                {
                    AttributeName = "Id",
                    KeyType = KeyType.HASH
                }
            },
            BillingMode = BillingMode.PAY_PER_REQUEST
        };

        try
        {
            var response = await dynamoDb.CreateTableAsync(createTableRequest);
            Console.WriteLine($"Table {tableName} created successfully. Status: {response.TableDescription.TableStatus}");
            
            // Wait for table to become active
            await WaitForTableToBeActive(dynamoDb, tableName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating table {tableName}: {ex.Message}");
            throw;
        }
    }

    private static async Task WaitForTableToBeActive(IAmazonDynamoDB dynamoDb, string tableName)
    {
        var maxAttempts = 5;
        var attempt = 0;
        
        while (attempt < maxAttempts)
        {
            try
            {
                var describeResponse = await dynamoDb.DescribeTableAsync(tableName);
                if (describeResponse.Table.TableStatus == TableStatus.ACTIVE)
                {
                    Console.WriteLine($"Table {tableName} is active.");
                    return;
                }
                
                Console.WriteLine($"Waiting for table {tableName} to become active. Current status: {describeResponse.Table.TableStatus}");
                await Task.Delay(2000); // Wait 2 seconds
                attempt++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking table {tableName} status: {ex.Message}");
                await Task.Delay(2000);
                attempt++;
            }
        }
        
        throw new TimeoutException($"Timeout waiting for table {tableName} to become active after {maxAttempts * 2} seconds.");
    }
}
