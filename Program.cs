/// <summary>
/// The entry point of the AzureDevOpsBugCreator application.
/// Initializes the automation process for a specified test case ID.
/// </summary>
/// <param name="args">
/// Command-line arguments (not used in this implementation).
/// </param>
class Program
{
    static void Main(string[] args)
    {
        int testCaseId = 7;  // Replace with the actual Test Case ID
        DevOpsAutomation.RunAutomation(testCaseId);
    }
}
