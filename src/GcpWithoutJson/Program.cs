using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GcpWithoutJson;

public class Program
{
    public static async Task Main(string[] s)
    {
        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false);

        var config = builder.Build();

        var creds = new GCPCredentials();
        config.GetSection("GCP").Bind(creds);

        var credJson = JsonSerializer.Serialize(creds);
        var gcpCreds = GoogleCredential.FromJson(credJson);

        var firestoreDbBuilder = new FirestoreDbBuilder
        {
            ProjectId = creds.ProjectId,
            ChannelCredentials = gcpCreds.ToChannelCredentials()
        };
        var firestoreDb = await firestoreDbBuilder.BuildAsync();

        await firestoreDb.Collection("blogs").AddAsync(new { Hello = "Hello World!!!" });
    }
}