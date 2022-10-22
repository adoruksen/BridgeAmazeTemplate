using StackSystem;

namespace Managers
{
    public class FloorManager : PooledObjectManager<Floor>
    {
        public static FloorManager Instance;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
        }

        protected override void OnObjectSpawned(Floor obj)
        {
            obj.SetInteractable(true);
        }
    }
}