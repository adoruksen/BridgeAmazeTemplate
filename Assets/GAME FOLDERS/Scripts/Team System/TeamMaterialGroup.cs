using System;
using UnityEngine;

namespace TeamSystem
{
    public class TeamMaterialGroup
    {
        public enum Type
        {
            Character,
            Floor,
        }

        public Material Character;
        public Material Floor;

        public TeamMaterialGroup(Team team, params Material[] materials)
        {
            Character = new Material(materials[0]) { color = team.CharacterColor };
            Floor = new Material(materials[1]) { color = team.FloorColor };
        }

        public Material GetMaterialByType(Type type)
        {
            return type switch
            {
                Type.Character => Character,
                Type.Floor => Floor,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}