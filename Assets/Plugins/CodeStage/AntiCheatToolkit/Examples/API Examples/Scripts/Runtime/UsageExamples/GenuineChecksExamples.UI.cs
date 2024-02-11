#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

using CodeStage.AntiCheat.Genuine.Android;

namespace CodeStage.AntiCheat.Examples
{
	using UnityEngine;

	internal partial class GenuineChecksExamples
	{
		public void DrawUI()
		{
			DrawBuildHashUI();
			GUILayout.Space(10);
			DrawAppInstallSourceUI();
		}

		private void DrawBuildHashUI()
		{
			GUILayout.Label("<b>Code Hash Generator</b>");
			GUILayout.Space(10);
			using (new GUILayout.VerticalScope(GUI.skin.box))
			{
				GUILayout.Label("ACTk can hash the binaries included into the build to let you compare the runtime hash with " +
								"the genuine hash to figure out if any of the build code binaries were altered.\n" +
								"Use server-side validation to keep it unreachable for bad actors.");
				
				GUILayout.Space(5);
				
				if (IsHashingSupported && !Application.isEditor)
				{
					// just to make sure it's added to the scene and Instance will be not empty
					Init();

					if (!IsHashingBusy)
					{
						if (BuildHashResult != null)
						{
							if (BuildHashResult.Success)
							{
								GUILayout.Label($"Generated Summary Hash: {BuildHashResult.SummaryHash}");
								GUILayout.Label($"Time spent: {BuildHashResult.DurationSeconds:F2} secs");
								GUILayout.Label($"Files hashed: {BuildHashResult.FileHashes.Count}");
							}
							else
							{
								GUILayout.Label($"Error: {BuildHashResult.ErrorMessage}");
							}
						}
						else
						{
							if (GUILayout.Button("Generate Hash"))
							{
								StartGeneration();
							}
							if (GUILayout.Button("Generate Hash Async"))
							{
								StartGenerationAsync();
							}
						}
					}
					else
					{
						GUILayout.Label("Generating...");
					}
				}
				else
				{
					GUILayout.Label(ExamplesGUI.Colorize("Code Hash Generator works only in Standalone Windows and Android builds.",
						ExamplesGUI.YellowColor));
				}
			}
		}
		
		private void DrawAppInstallSourceUI()
		{
			GUILayout.Label("<b>Android Installation Source Validation</b>");
			GUILayout.Space(10);
			using (new GUILayout.VerticalScope(GUI.skin.box))
			{
				GUILayout.Label(
					$"You can figure out where app was installed from using {nameof(AppInstallationSourceValidator)}" +
					"This can be used to detect unauthorized redistribution of your app.");
				
				GUILayout.Space(5);

				if (Application.platform != RuntimePlatform.Android)
				{
					GUILayout.Label(ExamplesGUI.Colorize("Android Installation Source detection works only in Android builds.", 
							ExamplesGUI.YellowColor));
				}
				else
				{
					if (AppInstallationResult != null)
					{
						GUILayout.Label($"Installation source: {AppInstallationResult.PackageName}");
						GUILayout.Label($"Detected store: {AppInstallationResult.DetectedSource}");
					}
					else if (GUILayout.Button("Get Android Installation Source"))
					{
						AppInstallationResult = GetAndroidInstallationSource();
					}
				}
			}
		}
	}
}