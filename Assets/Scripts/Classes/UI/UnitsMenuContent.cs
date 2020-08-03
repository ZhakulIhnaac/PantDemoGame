using System.Collections.Generic;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Classes.Playables;
using Assets.Scripts.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Classes.UI
{
    public class UnitsMenuContent : MonoBehaviour, IUnitsMenuContent
    {
        public static UnitsMenuContent UnitsMenuContentInstance;
        public List<GameObject> UnitList;

        private void Awake()
        {
            if (UnitsMenuContentInstance == null) // Singleton
            {
                UnitsMenuContentInstance = this;
            }
        }

        private void Start()
        {
            GameController.UpdateProduciblesList += DisplayProducibles;
            Barrack.UpdateProductionButtons += DisplayProducibles;
        }

        /*
         Recreate every producible unit's produce button fed by the selected game object.
         */
        public void DisplayProducibles([CanBeNull] List<GameObject> pProduciblesList)
        {
            UnitList.Clear(); // Clear the list for buttons.

            foreach (Transform child in transform) // Destroy every button created so far.
            {
                Destroy(child.gameObject);
            }

            if (pProduciblesList != null)
            {
                foreach (var unitToAdd in pProduciblesList) // Add the newly selected game object's buttons to the menu's list.
                {
                    UnitList.Add(unitToAdd);
                }

                Populate(); // Add the newly selected game object's buttons to the menu in the game.
            }
        }

        /*
         Place the unit producing button.
         */
        public void Populate()
        {
            foreach (var unit in UnitList)
            {
                Instantiate(unit, transform);
            }
        }

    }
}
