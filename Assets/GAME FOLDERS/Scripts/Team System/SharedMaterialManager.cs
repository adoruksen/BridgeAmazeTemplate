using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamSystem
{
    public class SharedMaterialManager : MonoBehaviour
    {
        public static SharedMaterialManager Instance;

        [SerializeField] private Material _characterMaterial;
        [SerializeField] private Material _floorMaterial;

        [ShowInInspector, ReadOnly] private Dictionary<Team, TeamMaterialGroup> _teamMaterials = new Dictionary<Team, TeamMaterialGroup>();

        private void Awake()
        {
            Instance = this;
        }

        public Material GetTeamMaterial(Team team, TeamMaterialGroup.Type type)
        {
            if (team == null)
            {
                return GetDefaultMaterialByType(type);
            }
            var teamRegistered = _teamMaterials.TryGetValue(team, out var group);
            if (!teamRegistered) group = RegisterTeam(team);

            return group.GetMaterialByType(type);
        }

        private TeamMaterialGroup RegisterTeam(Team team)
        {
            var group = new TeamMaterialGroup(team, _characterMaterial, _floorMaterial);
            _teamMaterials.Add(team, group);
            return group;
        }

        private Material GetDefaultMaterialByType(TeamMaterialGroup.Type type)
        {
            return type switch
            {
                TeamMaterialGroup.Type.Character => _characterMaterial,
                TeamMaterialGroup.Type.Floor => _floorMaterial,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
