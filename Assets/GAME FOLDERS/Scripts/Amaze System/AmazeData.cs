using System;
using UnityEngine;

namespace AmazeSystem
{
    [CreateAssetMenu(menuName = "Game/Amaze/AmazeData")]
    public class AmazeData : ScriptableObject
    {
        public AmazeController Amaze => _amaze;
        [SerializeField] private AmazeController _amaze;

        public int Cost => _cost;
        [SerializeField] private int _cost;

    }
}
