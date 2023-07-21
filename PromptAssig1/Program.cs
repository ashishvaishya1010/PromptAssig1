using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        const string inputFilePath = "C:\\Users\\ashish.vaishya\\source\\repos\\PromptAssig1\\PromptAssig1\\input.txt";
        const string outputFilePath = "C:\\Users\\ashish.vaishya\\source\\repos\\PromptAssig1\\PromptAssig1\\output.txt";
        const string apiKey = "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"; // Replace with your actual API key

        // Get user prompt from the console
        Console.Write("Enter your prompt : ");
        string prompt = Console.ReadLine();

        // Step 1: Save the prompt to the input file
        SavePromptToInputFile(inputFilePath, prompt);

        // Step 2: Read the prompt from the input file
        string inputContent = ReadInputFromFile(inputFilePath);

        // Step 3: Perform computation on the input and interact with ChatGPT API
        string assistantResponse = await GenerateChatGptResponse(apiKey, inputContent);

        // Step 4: Save the response to the output file
        if (!string.IsNullOrEmpty(assistantResponse))
        {
            SaveResponseToOutputFile(outputFilePath, assistantResponse);
        }
    }

    static async Task<string> GenerateChatGptResponse(string apiKey, string prompt)
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        string authorizationHeader = "Bearer " + apiKey;

        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                string requestData = $@"{{
                    ""model"": ""gpt-3.5-turbo"",
                    ""messages"": [{{
                        ""role"": ""system"",
                        ""content"": ""{prompt}""
                    }}]
                }}";
                StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseJson = await response.Content.ReadAsStringAsync();

                // Extract the assistant response from the JSON response
                // Assuming responseJson contains a valid JSON structure, you may want to add error handling for production code.
                dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);
                string assistantResponse = responseData.choices[0].message.content.ToString().Trim();

                Console.WriteLine("Assistant: " + assistantResponse);
                return assistantResponse;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred during ChatGPT API request: " + ex.Message);
            return null;
        }
    }

    static void SavePromptToInputFile(string inputFilePath, string prompt)
    {
        File.WriteAllText(inputFilePath, prompt, Encoding.UTF8);
    }

    static string ReadInputFromFile(string inputFilePath)
    {
        return File.ReadAllText(inputFilePath, Encoding.UTF8);
    }

    static void SaveResponseToOutputFile(string outputFilePath, string response)
    {
        File.WriteAllText(outputFilePath, response, Encoding.UTF8);
        Console.WriteLine("Response saved to " + outputFilePath);
    }
}







//using System;
//using System.IO;

//class Program
//{
//    static void Main()
//    {
//        string inputFile = "C:\\Users\\ashish.vaishya\\source\\repos\\PromptAssig1\\PromptAssig1\\input.txt";
//        string inputDirectory = Path.GetDirectoryName(inputFile); // Get the directory path of the input file
//        string outputFile = Path.Combine(inputDirectory, "output.txt"); // Combine the directory path with the output file name

//        // Step 1: Process the input file and compute something
//         string  inputData = File.ReadAllText(inputFile);

//        if (!int.TryParse(inputData, out int value))
//        {
//            Console.WriteLine("Invalid input. Please provide a valid numeric value in the input file.");
//            return;
//        }

//        int computedResult = ComputeSomething(value);

//        // Step 2: Generate output based on the computed result and save it to the output file
//        string outputData = GenerateOutput(computedResult);
//        File.WriteAllText(outputFile, outputData);

//        Console.WriteLine("Processing complete. Output saved to {0}.", outputFile);
//    }

//    static int ComputeSomething(int input)
//    {
//        // Perform computation on the input and return the result
//        int computedResult = input * 4;

//        return computedResult;
//    }

//    static string GenerateOutput(int result)
//    {
//        // Generate the output based on the computed result
//        string output = "The computed result is: " + result.ToString();

//        return output;
//    }
//}
