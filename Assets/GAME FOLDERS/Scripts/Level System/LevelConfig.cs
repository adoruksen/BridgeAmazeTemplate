using UnityEngine;
using TeamSystem;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Level", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        public Team[] Teams;
        [SerializeReference] public LevelPart[] Parts;
    }
}