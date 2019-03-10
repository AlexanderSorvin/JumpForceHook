using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeScreenshotByMouseClick : MonoBehaviour
{
	public Camera mainCamera;

	int counter = 1;

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			ScreenCapture.CaptureScreenshot("Screenshots/Sreenshot" + counter.ToString("00") + "_" + mainCamera.pixelWidth + "x" + mainCamera.pixelHeight + ".png");
			counter++;
		}
	}
}
