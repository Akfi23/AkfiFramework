using System;
using System.IO;
using _Source.Code._AKFramework.AKCore.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[Serializable]
	public class ScreenshotTool : IAKEditorTool
	{
		public string Title => "Screenshots";

		[SerializeField]
		private int resWidth = 2160;
		[SerializeField]
		private int resHeight = 3840;

		[SerializeField]
		private Camera myCamera;
		[SerializeField]
		[InfoBox("Режим скриншота по умолчанию — обрезка, поэтому выберите правильную ширину и высоту. Масштаб является фактором " +
		         "умножать или увеличивать рендеры без потери качества.", InfoMessageType.None)]
		private int scale = 1;

		private string path = "Store/Screenshots";
		[SerializeField]
		private bool showPreview = true;

		[SerializeField]
		private bool isTransparent = false;
		private string lastScreenshot = "";


		[Button]
		private void TakeScreenshot()
		{
			if(myCamera == null)
			{
				myCamera = Camera.main;
			}

			if (myCamera == null)
			{
				Debug.LogError("Can't find camera");
			}

			if(path == "")
			{
				path = EditorUtility.SaveFolderPanel("Path to Save Images",path,Application.dataPath);
				Debug.Log(path);
				Debug.Log("Path Set");
				TakeHiResShot();
			}
			else
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				TakeHiResShot();
			}
		}

		[ButtonGroup()]
		private void OpenLastScreenshot()
		{
			if (lastScreenshot == "") return;
			
			Application.OpenURL(lastScreenshot);
			Debug.Log("Opening File " + lastScreenshot);
		}

		[ButtonGroup()]
		private void OpenFolder()
		{
			Debug.Log($"Open folder: {path}");
			Application.OpenURL($"file://{Application.dataPath.Replace("Assets", "")}" + path);
		}


		public string ScreenShotName(int width, int height)
		{
			var strPath = "";

			strPath = $"{path}/screen_{width}x{height}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
			lastScreenshot = $"file://{Application.dataPath.Replace("Assets", "")}{strPath}";

			return strPath;
		}



		public void TakeHiResShot()
		{
			Debug.Log("Taking Screenshot");
			var resWidthN = resWidth * scale;
			var resHeightN = resHeight * scale;
			var rt = new RenderTexture(resWidthN, resHeightN, 24);
			myCamera.targetTexture = rt;

			var tFormat = isTransparent ? TextureFormat.ARGB32 : TextureFormat.RGB24;


			var screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
			myCamera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
			myCamera.targetTexture = null;
			RenderTexture.active = null;
			var bytes = screenShot.EncodeToPNG();
			var filename = ScreenShotName(resWidthN, resHeightN);

			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log($"Took screenshot to: {filename}");
			if(showPreview) Application.OpenURL(lastScreenshot);
		}
	}
}

