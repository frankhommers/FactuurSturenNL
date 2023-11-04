using System.Net;
using FactuurSturenNL.Model;

namespace FactuurSturenNL.TestClient;

/// <summary>
///   Test application for testing the C# FactuurSturen.nl library.
/// </summary>
internal class Program
{
  private static async Task Main(string[] args)
  {
    Console.WriteLine("FactuurSturen.nl API Test Client");

    NetworkCredential credentials = GetCredentialsViaEnvironmentVariables() ?? GetCredentialsViaPrompt();
    Console.WriteLine("Connecting...");

    FactuurSturenClient client = new(usePollyTransientFaultHandling: false);

    await client.LoginAsync(credentials.UserName, credentials.Password);
    Console.WriteLine("Connected.");

    
     Invoice invoiceGet = await client.GetInvoiceAsync("2023034");
    IEnumerable<Invoice> invoices = await client.GetInvoicesAsync();
    Tax[] taxes = (await client.GetTaxesAsync()).ToArray();
    byte[] myBytes = await client.GetInvoicePdfAsync("2023034");
    
    Tax? tax = await client.GetTaxTypeByRateAsync(TaxRate.High);
    int taxRate = tax.Percentage; 
    Client to = await client.GetClientAsync(1); 
    Invoice invoice = new(to, InvoiceAction.Save, SendMethod.Email);
    InvoiceLine line1 = new(1, "Test line", taxRate, 125);
    invoice.AddLine(line1);
    invoice.SaveName = "Test";
    await client.CreateDraftInvoiceAsync(invoice);
    //var products = await client.GetProducts();

    //var product = await client.GetProduct(1);
    //var retVal = await client.CreateProduct(product);
    //await client.DeleteProduct(product);


    //var invoice = await client.GetInvoice("20160086");
  }

  /// <summary>
  ///   Get user credentials from environment variables.
  /// </summary>
  /// <returns></returns>
  private static NetworkCredential? GetCredentialsViaEnvironmentVariables()
  {
    string? username = Environment.GetEnvironmentVariable("FACTUURSTURENNL_USERNAME");
    string? apiKey = Environment.GetEnvironmentVariable("FACTUURSTURENNL_APIKEY");
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(apiKey)) return null;

    return new NetworkCredential(username, apiKey);
  }


  /// <summary>
  ///   Prompt the user for a user name and apikey.
  /// </summary>
  /// <returns></returns>
  private static NetworkCredential GetCredentialsViaPrompt()
  {
    Console.WriteLine("Please enter your FactuurSturen.nl credentials.");
    Console.WriteLine("Note: You can store your credentials in a file, so that you don't have to type them in.");
    Console.Write("User name: ");
    string username = Console.ReadLine();
    Console.Write("API Key: ");
    string apiKey = ReadPassword();
    Console.WriteLine();

    return new NetworkCredential(username, apiKey);
  }

  // Taken from http://stackoverflow.com/a/7049688/393367

  /// <summary>
  ///   Like System.Console.ReadLine(), only with a mask.
  /// </summary>
  /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
  /// <returns>the string the user typed in </returns>
  public static string ReadPassword(char mask)
  {
    const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
    int[] FILTERED = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

    Stack<char> pass = new();
    char chr = (char)0;

    while ((chr = Console.ReadKey(true).KeyChar) != ENTER)
      if (chr == BACKSP)
      {
        if (pass.Count > 0)
        {
          Console.Write("\b \b");
          pass.Pop();
        }
      }
      else if (chr == CTRLBACKSP)
      {
        while (pass.Count > 0)
        {
          Console.Write("\b \b");
          pass.Pop();
        }
      }
      else if (FILTERED.Count(x => chr == x) > 0)
      {
      }
      else
      {
        pass.Push(chr);
        Console.Write(mask);
      }

    Console.WriteLine();

    return new string(pass.Reverse().ToArray());
  }

  /// <summary>
  ///   Like System.Console.ReadLine(), only with a mask.
  /// </summary>
  /// <returns>the string the user typed in </returns>
  public static string ReadPassword()
  {
    return ReadPassword('*');
  }
}