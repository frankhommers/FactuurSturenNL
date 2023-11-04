using System.Text;

namespace FactuurSturenNL.Exceptions;

public class DetailedHttpRequestException : HttpRequestException
{
  public DetailedHttpRequestException()
  {
    
  }

  public DetailedHttpRequestException(string message) : base(message)
  {
  }
  
  public  DetailedHttpRequestException(string message, Exception innerException) : base(message, innerException)
  {
  }
  public string? Request { get; set; }
  public string? Response { get; set; }

  public override string ToString()
  {
    StringBuilder sb = new(base.ToString() ?? "HttpRequestException");
    if (!string.IsNullOrWhiteSpace(Request))
    {
     sb.AppendLine();
     sb.Append("Request: ");
     sb.AppendLine(Request);
    }
    if (!string.IsNullOrWhiteSpace(Response))
    {
      sb.AppendLine();
      sb.Append("Response: ");
      sb.AppendLine(Response);
    }
    return sb.ToString();
  }
}