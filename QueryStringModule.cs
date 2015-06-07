#region Using

using System;
using System.IO;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using Software.Factory;

#endregion

/// <summary>
/// Summary description for QueryZetStringModule
/// </summary>
public class QueryZetStringModule : IHttpModule
{

  #region IHttpModule Members

  public void Dispose()
  {
    // rutina dispose
  }

  public void Init(HttpApplication context)
  {
    context.BeginRequest += new EventHandler(context_Begin);
  }

  #endregion

  public const string PARAMETER_NAME = "QuerySF=";
  public const string ENCRYPTION_KEY = "daniel|javier|ralf|alfredo|798798798798";

    //
  void context_Begin(object sender, EventArgs e)
  {
    HttpContext context = HttpContext.Current;
    if (context.Request.Url.OriginalString.Contains("aspx") && context.Request.RawUrl.Contains("?"))
    {
      string query = ExtractQuery(context.Request.RawUrl);
      string path = GetVirtualPath();

      if (query.StartsWith(PARAMETER_NAME, StringComparison.OrdinalIgnoreCase))
      {
        
        string rawQuery = query.Replace(PARAMETER_NAME, string.Empty);
        string decryptedQuery = Encryption.Decrypt(rawQuery,ENCRYPTION_KEY);
        context.RewritePath(path, string.Empty, decryptedQuery);
      }
      else if (context.Request.HttpMethod == "GET")
      {
       
          string encryptedQuery = Encryption.Encrypt(query,ENCRYPTION_KEY,PARAMETER_NAME);
        context.Response.Redirect(path + encryptedQuery);
      }
    }
  }

  
  private static string GetVirtualPath()
  {
    string path = HttpContext.Current.Request.RawUrl;
    path = path.Substring(0, path.IndexOf("?"));
    path = path.Substring(path.LastIndexOf("/") + 1);
    return path;
  }

 
  private static string ExtractQuery(string url)
  {
    int index = url.IndexOf("?") + 1;
    return url.Substring(index);
  }



}
