using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class Building : Playable, IBuilding
    {
        public override void LeftMouseClick()
        {
            GameController.Instance.EmptyTheSelected();
        }

        public override void RightMouseClick()
        {
            GameController.Instance.EmptyTheSelected();
        }
    }
}
