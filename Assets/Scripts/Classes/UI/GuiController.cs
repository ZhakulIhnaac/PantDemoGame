using System.Collections;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Classes.Playables;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes.UI
{

    public class GuiController : MonoBehaviour
    {
        public static GuiController Instance;
        public TextMeshProUGUI WarningText;
        public TextMeshProUGUI PowerAmountText;
        public TextMeshProUGUI SelectedGameObjectName;
        public RawImage SelectedGameObjectImage;
        public GameObject MessagePanel;

        void Awake()
        {
            if (Instance == null) // We will only have one MainGame object in out scene. Thus, we just make it unique (Singleton)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            WarningText.enabled = false;
            MessagePanel.SetActive(false);
            Playable.GiveWarning += GiveWarning;
            BuildingTemplate.GiveWarning += GiveWarning;
            GameController.UpdatePowerText += UpdatePowerText; // PowerPlant's ProducePower is an event send by the power plant buildings.
            GameController.UpdateSelectedGameObject += UpdateSelectedGameObjectGui;
            ObjectPooling.GiveWarning += GiveWarning;
        }

        public void UpdatePowerText(float pPowerAmount) // Update the power amount text in GUI.
        {
            PowerAmountText.text = "Power Amount: " + pPowerAmount;
        }

        public void UpdateSelectedGameObjectGui([CanBeNull] GameObject pSelectedGameObject) // Update the power amount text in GUI.
        {
            if (pSelectedGameObject != null)
            {
                Instance.SelectedGameObjectImage.texture = pSelectedGameObject.GetComponent<SpriteRenderer>().sprite.texture;
                Instance.SelectedGameObjectName.text = pSelectedGameObject.name;
            }
            else
            {
                Instance.SelectedGameObjectImage.texture = null;
                Instance.SelectedGameObjectName.text = null;
            }
        }

        private void GiveWarning(string pMessage)
        {
            StartCoroutine(GiveWarningMessage(pMessage));
        }

        public IEnumerator GiveWarningMessage(string pMessage)
        {
            MessagePanel.SetActive(true);
            WarningText.text = pMessage;
            WarningText.enabled = true;
            yield return new WaitForSeconds(3f);
            WarningText.enabled = false;
            MessagePanel.SetActive(false);
        }
    }
}
