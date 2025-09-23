using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;

/// <summary>
/// Provides functionality to create bug work items in Azure DevOps using REST API.
/// </summary>
public class AzureDevOpsBugCreator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AzureDevOpsBugCreator"/> class.
    /// Loads Azure DevOps configuration from the appsettings.json file.
    /// </summary>
    public AzureDevOpsBugCreator()
    {
        // ...
    }

    /// <summary>
    /// Creates a new bug work item in Azure DevOps with the specified title.
    /// </summary>
    /// <param name="bugTitle">The title of the bug to be created.</param>
    public void CreateBug(string bugTitle)
    {
        // ...
    }
}

namespace SeleniumTests
{
    public class AzureDevOpsBugCreator
    {
        private readonly string azureDevOpsUrl;
        private readonly string project;
        private readonly string personalAccessToken;

        public AzureDevOpsBugCreator()
        {
            try
            {
                var config = File.ReadAllText("appsettings.json");
                dynamic settings = JsonConvert.DeserializeObject(config);
                azureDevOpsUrl = settings.AzureDevOpsUrl;
                project = settings.Project;
                personalAccessToken = settings.PersonalAccessToken;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is JsonException)
            {
                Console.WriteLine($"Configuration error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during initialization: {ex.Message}");
                throw;
            }
        }

        public void CreateBug(string bugTitle)
        {
            var client = new RestClient($"{azureDevOpsUrl}/{project}/_apis/wit/workitems/$Bug?api-version=6.0");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json-patch+json");
            string authToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}"));
            request.AddHeader("Authorization", $"Basic {authToken}");

            var bugData = new[]
            {
                new { op = "add", path = "/fields/System.Title", value = bugTitle },
                new { op = "add", path = "/fields/System.Description", value = "Bug created automatically due to failed Selenium test." },
                new { op = "add", path = "/fields/System.AssignedTo", value = "shahab@tecoholic.com" },
                new { op = "add", path = "/fields/Microsoft.VSTS.TCM.ReproSteps", value = "See attached logs for detailed error." }
            };

            request.AddParameter("application/json-patch+json", JsonConvert.SerializeObject(bugData), ParameterType.RequestBody);

            try
            {
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    Console.WriteLine("Bug created successfully in Azure DevOps.");
                }
                else
                {
                    Console.WriteLine($"Failed to create bug: {response.ErrorMessage ?? response.Content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while creating bug: {ex.Message}");
            }
        }
    }
}
