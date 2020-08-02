using System.Collections;

namespace Assets.Scripts.Interfaces
{
    public interface IPowerPlant : IBuilding
    {
        IEnumerator SupplyPower();
    }
}
