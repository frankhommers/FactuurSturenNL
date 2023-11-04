using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class InvoiceLine
{
  public InvoiceLine() : this(0, string.Empty, 0, 0)
  {
  }

  public InvoiceLine(double amount, string description, double taxRate, double price) : this(amount, null, description,
    taxRate, price)
  {
  }

  public InvoiceLine(double amount, string amountDesc, string description, double taxRate, double price,
    double? discountPercentage = null)
  {
    Amount = amount;
    AmountDesc = amountDesc;
    Description = description;
    TaxRate = taxRate;
    Price = price;
    DiscountPercentage = discountPercentage;
  }

  [JsonPropertyName("amount")]
  public double Amount { get; set; }

  [JsonPropertyName("amount_desc")] public string AmountDesc { get; set; }

  [JsonPropertyName("description")]
  public string Description { get; set; }

  [JsonPropertyName("tax_rate")] public double TaxRate { get; set; }

  [JsonPropertyName("price")]
  public double Price { get; set; }

  [JsonPropertyName("discountpct")]
  public double? DiscountPercentage { get; set; }
  [JsonPropertyName("linetotal")]
  public double LineTotal { get; set; }
}