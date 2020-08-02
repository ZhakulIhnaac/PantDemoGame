using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Classes.Gameplay
{

    public class GuiController : MonoBehaviour
    {
        public static GuiController Instance;
        public TextMeshProUGUI WarningText;
        public TextMeshProUGUI PowerAmountText;
        public TextMeshProUGUI SelectedGameObjectName;

        void Awake()
        {
            if (Instance == null) // We will only have one MainGame object in out scene. Thus, we just make it unique (Singleton)
            {
                Instance = this;
            }
        }

        void Start()
        {
            WarningText.enabled = false;
            Playable.GiveWarning += GiveWarning;
            BuildingTemplate.GiveWarning += GiveWarning;
        }

        private void GiveWarning(string pMessage)
        {
            StartCoroutine(GiveWarningMessage(pMessage));
        }

        public IEnumerator GiveWarningMessage(string pMessage)
        {
            WarningText.text = pMessage;
            WarningText.enabled = true;
            yield return new WaitForSeconds(3f);
            WarningText.enabled = false;
        }
    }
}
