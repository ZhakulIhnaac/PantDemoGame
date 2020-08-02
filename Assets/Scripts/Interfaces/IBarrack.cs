using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IBarrack : IBuilding
    {
        void Produce(GameObject unitToProduce);
    }
}
