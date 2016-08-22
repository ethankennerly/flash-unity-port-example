using UnityEngine;
using Finegamedesign.Utils/*<DebugUtil>*/;

public class Email
{
	public static string EscapeURL(string url)
	{
		return WWW.EscapeURL(url).Replace("+", "%20");
	}

  	public string to = "kennerly@finegamedesign.com";
  	public string subject = EscapeURL("Anagram Attack metrics");

	public void Send(string body)
	{
		DebugUtil.Log(body);
		body = EscapeURL(body);
		Application.OpenURL("mailto:" + to
			+ "?subject=" + subject 
			+ "&body=" + body);
	}
}
