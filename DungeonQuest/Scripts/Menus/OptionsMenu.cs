using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Data;

namespace DungeonQuest.Menus
{
	public class OptionsMenu : MonoBehaviour
	{
		[Header("Options Config:")]
		[SerializeField] private Slider volumeSlider;
		[Space(10f)]
		[SerializeField] private Toggle vSyncToggle;
		[SerializeField] private Toggle showFPSToggle;
		[SerializeField] private Toggle enableConsoleToggle;

		void Awake()
		{
			LoadSettings();
		}

		public void Volume(float volume) // Called by Slider
		{
			AudioListener.volume = volume;
			SaveSettings();
		}

		public void ToggleVSync(bool toggle) // Called by Toggle
		{
			QualitySettings.vSyncCount = System.Convert.ToInt32(toggle);
			SaveSettings();
		}

		public void ToggleShowFPS(bool toggle) // Called by Toggle
		{
			FramerateCounter.SHOW_FPS = toggle;
			SaveSettings();
		}

		public void ToggleConsole(bool toggle) // Called by Toggle
		{
			Debuging.DebugConsole.ENABLE_CONSOLE = toggle;
			SaveSettings();
		}

		private void SaveSettings()
		{
			var optionsSave = OptionsData();
			var xmlDocument = new XmlDocument();

			if (!Directory.Exists(Application.dataPath + "/Data"))
			{
				Directory.CreateDirectory(Application.dataPath + "/Data");
			}

			XmlElement root = xmlDocument.CreateElement("Options");
			root.SetAttribute("FileName", "Options.xml");

			XmlElement volumeElement = xmlDocument.CreateElement("Volume");
			volumeElement.InnerText = optionsSave.volume.ToString();
			root.AppendChild(volumeElement);

			XmlElement vSyncElement = xmlDocument.CreateElement("VSync");
			vSyncElement.InnerText = optionsSave.vSync.ToString();
			root.AppendChild(vSyncElement);

			XmlElement showFpsElement = xmlDocument.CreateElement("ShowFPS");
			showFpsElement.InnerText = optionsSave.showFPS.ToString();
			root.AppendChild(showFpsElement);

			XmlElement enableConsoleElement = xmlDocument.CreateElement("EnableConsole");
			enableConsoleElement.InnerText = optionsSave.enableConsole.ToString();
			root.AppendChild(enableConsoleElement);

			xmlDocument.AppendChild(root);

			xmlDocument.Save(Application.dataPath + "/Data/Options.xml");			
		}

		public void LoadSettings()
		{
			if (!File.Exists(Application.dataPath + "/Data/Options.xml")) return;

			var optionsData = new OptionsData();
			
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(Application.dataPath + "/Data/Options.xml");

			XmlNodeList volume = xmlDocument.GetElementsByTagName("Volume");
			optionsData.volume = float.Parse(volume[0].InnerText);

			XmlNodeList vSync = xmlDocument.GetElementsByTagName("VSync");
			optionsData.vSync = bool.Parse(vSync[0].InnerText);

			XmlNodeList showFPS = xmlDocument.GetElementsByTagName("ShowFPS");
			optionsData.showFPS = bool.Parse(showFPS[0].InnerText);

			XmlNodeList enableConsole = xmlDocument.GetElementsByTagName("EnableConsole");
			optionsData.enableConsole = bool.Parse(enableConsole[0].InnerText);

			volumeSlider.value = optionsData.volume;
			vSyncToggle.isOn = optionsData.vSync;
			showFPSToggle.isOn = optionsData.showFPS;
			enableConsoleToggle.isOn = optionsData.enableConsole;
		}

		private OptionsData OptionsData()
		{
			var optionsSave = new OptionsData
			{
				volume = volumeSlider.value,
				vSync = vSyncToggle.isOn,
				showFPS = showFPSToggle.isOn,
				enableConsole = enableConsoleToggle.isOn
			};

			return optionsSave;
		}
	}
}
