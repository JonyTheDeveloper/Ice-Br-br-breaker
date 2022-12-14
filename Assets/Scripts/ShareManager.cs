using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShareManager : MonoBehaviour
{
	string shareMessage;

	public void shareButton()
    {
		shareMessage = "I just scored a new score of " + GetComponent<GameManager>().score + " in Ice Br-br-breaker! Join and see if you can beat my highscore!";

		StartCoroutine(TakeScreenshotAndShare());
    }

	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, ss.EncodeToPNG());

		// To avoid memory leaks
		Destroy(ss);

		new NativeShare().AddFile(filePath)
			.SetSubject("Ice Br-br-breaker").SetText(shareMessage).SetUrl("https://play.google.com/store/apps/details?id=com.JonyTheDeveloper.IceBrbrbreaker")
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();
	}
}
