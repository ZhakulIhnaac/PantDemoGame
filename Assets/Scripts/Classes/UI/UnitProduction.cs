using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Classes.Playables;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class UnitProduction : MonoBehaviour, IUnitProduction
    {
        [SerializeField] private GameObject _unit;

        public void SendToProduction()
        {
            GameController.Instance.SelectedGameObject.GetComponent<Barrack>().Produce(_unit);
        }
    }

}
