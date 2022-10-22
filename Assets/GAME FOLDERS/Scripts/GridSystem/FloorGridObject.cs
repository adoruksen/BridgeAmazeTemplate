using StackSystem;

namespace GridSystem
{
    public class FloorGridObject : GridObject
    {
        private Floor _floor;

        private void Awake()
        {
            _floor = GetComponent<Floor>();
        }

        private void OnEnable()
        {
            _floor.OnCollected += RemoveFromGrid;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _floor.OnCollected -= RemoveFromGrid;
        }
    }
}