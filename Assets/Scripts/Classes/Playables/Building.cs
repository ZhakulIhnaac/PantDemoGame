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

        public virtual void RightMouseClick()
        {
            GameController.Instance.EmptyTheSelected();
        }
    }
}
