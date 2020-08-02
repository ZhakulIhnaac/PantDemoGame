using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class BuildingProductionButton : MonoBehaviour, IBuildingProductionButton
    {
        [SerializeField] private GameObject _buildingTemplate;

        public void assignBuildingTemplate()
        {
            Cursor.CursorInstance.BuildingTemplate = Object.Instantiate(_buildingTemplate, Cursor.CursorInstance.transform);
        }
    }
}
