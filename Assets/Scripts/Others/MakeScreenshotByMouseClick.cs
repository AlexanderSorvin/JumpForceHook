using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeScreenshotByMouseClick : MonoBehaviour
{ 
    int counter = 1;

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			ScreenCapture.CaptureScreenshot("Screenshots/Sreenshot" + counter.ToString("00") + "_" + Camera.main.pixelWidth + "x" + Camera.main.pixelHeight + ".png");
			counter++;
		}
	}
}
