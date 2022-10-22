using System.Collections.Generic;
using PoolSystem;
using TeamSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class PooledObjectManager<T> : MonoBehaviour where T : MonoBehaviour, IHaveTeam
    {
        [SerializeField] private T Obj;

        public Transform PoolParent;

        [ShowInInspector, ReadOnly] private Dictionary<Team, List<T>> _availableObjects;
        [ShowInInspector, ReadOnly] private List<T> _availableTeamlessObjects;

        [ShowInInspector, ReadOnly] private ObjectPool<T> _pool;

        protected virtual void Awake()
        {
            _availableObjects = new Dictionary<Team, List<T>>();
            _availableTeamlessObjects = new List<T>();
            _pool = new ObjectPool<T>(Obj);
        }

        public IReadOnlyList<T> GetAvailableObjects(Team team)
        {
            if (team == null) return _availableTeamlessObjects;
            CheckAndRegisterTeam(team);
            return _availableObjects[team];
        }

        public void SetObjectAvailable(T obj, bool available)
        {
            if (available)
            {
                AddToAvailableBricks(obj.Team, obj);
                return;
            }

            RemoveFromAvailableBricks(obj);
        }

        public T SpawnObject(Team team)
        {
            var obj = _pool.Get();
            obj.AssignTeam(team);
            obj.transform.SetParent(GameManager.Instance.DefaultParent);
            obj.gameObject.SetActive(true);
            OnObjectSpawned(obj);

            return obj;
        }

        protected virtual void OnObjectSpawned(T obj) { }

        public void RemoveObject(T obj)
        {
            obj.transform.SetParent(PoolParent);
            obj.gameObject.SetActive(false);
            _pool.Return(obj);
            SetObjectAvailable(obj, false);
        }

        public void RemoveAllObjects()
        {
            foreach (var team in _availableObjects.Keys)
            {
                var objs = GetAvailableObjects(team);
                while (objs.Count > 0)
                {
                    RemoveObject(objs[0]);
                }
            }

            while (_availableTeamlessObjects.Count > 0)
            {
                RemoveObject(_availableTeamlessObjects[0]);
            }
        }

        private void AddToAvailableBricks(Team team, T obj)
        {
            if (team == null)
            {
                _availableTeamlessObjects.Add(obj);
                return;
            }
            CheckAndRegisterTeam(team);
            _availableObjects[team].Add(obj);
        }

        private void RemoveFromAvailableBricks(T obj)
        {
            if (obj.Team == null)
            {
                _availableTeamlessObjects.Remove(obj);
                return;
            }
            _availableObjects[obj.Team].Remove(obj);
        }

        private void CheckAndRegisterTeam(Team team)
        {
            var teamRegistered = _availableObjects.ContainsKey(team);
            if (!teamRegistered) _availableObjects.Add(team, new List<T>());
        }
    }
}