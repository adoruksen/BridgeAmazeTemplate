using UnityEngine;

namespace TeamSystem
{
    [CreateAssetMenu(menuName = "Game/TeamSystem/Team", order = -399)]
    public class Team : ScriptableObject
    {
        public Color CharacterColor => _characterColor;
        [SerializeField] private Color _characterColor;

        public Color FloorColor => _floorColor;
        [SerializeField] private Color _floorColor;
    }
}
