using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Classes.Playables
{
    public class Building : Playable, IBuilding
    {
        /*
         LeftMouseClick is triggered when left mouse button is clicked while the building is selected.
         */
        public override void LeftMouseClick()
        {
            GameController.Instance.EmptyTheSelected();
        }

        /*
         RightMouseClick is triggered when right mouse button is clicked while the building is selected.
         */
        public override void RightMouseClick()
        {
            GameController.Instance.EmptyTheSelected();
        }
    }
}
