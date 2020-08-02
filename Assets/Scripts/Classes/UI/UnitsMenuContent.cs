using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class UnitsMenuContent : MonoBehaviour, IUnitsMenuContent
    {
        public static UnitsMenuContent UnitsMenuContentInstance;
        public List<GameObject> UnitList;

        void Awake()
        {
            if (UnitsMenuContentInstance == null) // We will only have one UnitsMenu in our UI. Thus, we just make it unique (Singleton)
            {
                UnitsMenuContentInstance = this;
            }
        }

        public void DisplayProducibles(List<GameObject> produciblesList = null)
        {
            UnitList.Clear(); // Clear the list for buttons.

            foreach (Transform child in transform) // Destroy every button created so far.
            {
                Destroy(child.gameObject);
            }

            if (produciblesList != null)
            {
                foreach (var unitToAdd in produciblesList) // Add the newly selected game object's buttons to the menu's list.
                {
                    UnitList.Add(unitToAdd);
                }

                Populate(); // Add the newly selected game object's buttons to the menu in the game.
            }
        }

        public void Populate()
        {
            foreach (var unit in UnitList)
            {
                Instantiate(unit, transform);
            }
        }


    }
}
