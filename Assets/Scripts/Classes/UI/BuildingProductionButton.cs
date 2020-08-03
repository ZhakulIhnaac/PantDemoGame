using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.UI
{
    public class BuildingProductionButton : MonoBehaviour, IBuildingProductionButton
    {
        [SerializeField] private GameObject _buildingTemplate;

        /*
         AssignBuildingTemplate is assigning the _buildingTemplate to the BuildingTemplate variable of
         cursor object.
         */
        public void AssignBuildingTemplate()
        {
            Cursor.CursorInstance.BuildingTemplate = Instantiate(_buildingTemplate, Cursor.CursorInstance.transform);
        }
    }
}
