using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class UnitProduction : MonoBehaviour, IUnitProduction
    {
        [SerializeField] private GameObject _unit;

        public void SendToProduction()
        {
            GameController.Instance.selectedGameObject.GetComponent<Barrack>().Produce(_unit);
        }
    }

}
