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

		void Awake()
		{
			LoadSettings();
		}

		public void Volume(float volume)
		{
			AudioListener.volume = volume;
			SaveSettings();
		}

		public void ToggleVSync(bool toggle)
		{
			QualitySettings.vSyncCount = System.Convert.ToInt32(toggle);
			SaveSettings();
		}

		public void ToggleShowFPS(bool toggle)
		{
			FramerateCounter.SHOW_FPS = toggle;
			SaveSettings();
		}

		private void SaveSettings()
		{
			var optionsSave = OptionsData();
			var xmlDocument = new XmlDocument();

			XmlElement root = xmlDocument.CreateElement("Options");
			root.SetAttribute("FileName", "Options.xml");

			XmlElement volumeElement = xmlDocument.CreateElement("Volume");
			volumeElement.InnerText = optionsSave.volume.ToString();
			root.AppendChild(volumeElement);

			XmlElement vSyncElement = xmlDocument.CreateElement("VSync");
			vSyncElement.InnerText = optionsSave.vSync.ToString();
			root.AppendChild(vSyncElement);

			XmlElement ShowFpsElement = xmlDocument.CreateElement("ShowFPS");
			ShowFpsElement.InnerText = optionsSave.showFPS.ToString();
			root.AppendChild(ShowFpsElement);

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

			volumeSlider.value = optionsData.volume;
			vSyncToggle.isOn = optionsData.vSync;
			showFPSToggle.isOn = optionsData.showFPS;
		}

		private OptionsData OptionsData()
		{
			var optionsSave = new OptionsData
			{
				volume = volumeSlider.value,
				vSync = vSyncToggle.isOn,
				showFPS = showFPSToggle.isOn
			};

			return optionsSave;
		}
	}
}
