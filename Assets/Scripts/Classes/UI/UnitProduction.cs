using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Classes.Playables;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.UI
{
    public class UnitProduction : MonoBehaviour, IUnitProduction
    {
        [SerializeField] private GameObject _unit;

        /*
         SendToProduction sends the defined unit to the production in selected building.
         */
        public void SendToProduction()
        {
            GameController.Instance.SelectedGameObject.GetComponent<Barrack>().Produce(_unit);
        }
    }
}
