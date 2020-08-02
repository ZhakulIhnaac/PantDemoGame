using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISoldier : IPlayable
    {
        void Move(Vector2 moveTarget);
    }
}
