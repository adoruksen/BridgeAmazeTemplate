using System;
using System.Collections.Generic;
using Character;
using InteractionSystem;
using StackSystem;
using Managers;
using AmazeSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StackSystem.FillArea
{
    [Serializable]
    public class StackFillAreaGenerator
    {
        [SerializeField] private StackFillArea _fillArea;
        [SerializeField] private Transform _wall;
        [SerializeField] private FillObject _fillObject;
        [SerializeField] private float _fillObjectScale;
        [SerializeField] private float _fillAreaWidth;

        [SerializeField] private int _rowStackAmount;

        public List<StackFillArea> Generate(Transform parent, AmazeData[] amazeArray)
        {
            var areas = new List<StackFillArea>();
            for (var i = 0; i < amazeArray.Length; i++)
            {
                var xOffset = (-((amazeArray.Length - 1) / 2f) + i) * _fillAreaWidth;
                var offset = Vector3.left * xOffset;
                var fillArea = Generate(parent, offset, amazeArray[i]);
                areas.Add(fillArea);
            }
            GenerateWalls(parent, amazeArray.Length);
            return areas;
        }

        public StackFillArea Generate(Transform parent, Vector3 position, AmazeData amazeData)
        {
            var fillArea = Object.Instantiate(_fillArea, parent);
            fillArea.transform.localPosition = position;
            var amazePosition = fillArea.transform.position + new Vector3(0,.5f,5.5f);
            var amaze = Object.Instantiate(amazeData.Amaze, amazePosition, Quaternion.identity);
            var fillVisual = fillArea.GetComponent<StackFillAreaVisualController>();
            var fillTrigger = fillArea.GetComponent<CharacterAmazeStateTrigger>();

            var rows = amazeData.Cost / _rowStackAmount;
            var offset = Vector3.left * (.5f * (rows - 1));

            GenerateFillArea(fillArea, rows, amazeData.Cost);
            GenerateFillObjects(fillVisual, offset, rows);
            GenerateFillTrigger(fillTrigger, amaze);
            return fillArea;
        }

        private void GenerateFillArea(StackFillArea fillArea, int rows, int size)
        {
            fillArea.SetSize(size);

            var wallScale = (8 - rows) / 2f;
            var wallPosition = Vector3.right * (wallScale / 2f + rows / 2f);
            SpawnWall(fillArea.transform, wallPosition, wallScale);
            wallPosition = Vector3.left * (wallScale / 2f + rows / 2f);
            SpawnWall(fillArea.transform, wallPosition, wallScale);
        }

        private void GenerateFillObjects(StackFillAreaVisualController fillVisual, Vector3 position, int rows)
        {
            var fillObjects = new List<FillObject>();
            var parent = fillVisual.transform.Find("FillObjects");
            var fillObjectsParent = GameObject.FindObjectOfType<GridSystem.GridController>().transform.parent.GetChild(1);
            for (int i = 0; i < fillObjectsParent.childCount; i++)
            {
                var fillObject = fillObjectsParent.GetChild(i).GetComponent<FillObject>();
                fillObjects.Add(fillObject);
            }
            //for (var z = 0; z < _rowStackAmount; z++)
            //{
            //    for (var x = 0; x < rows; x++)
            //    {
            //        var fillObject = Object.Instantiate(_fillObject, parent);
            //        fillObject.transform.localPosition = position + new Vector3(x, 0, (z + .5f) * _fillObjectScale);
            //        fillObject.transform.localScale = Vector3.zero;
            //        fillObjects.Add(fillObject);
            //    }
            //}
            fillVisual.SetFillObjects(fillObjects.ToArray());
            fillVisual.SetFillObjectsReadyAtStart();
        }

        private void GenerateFillTrigger(CharacterAmazeStateTrigger fillTrigger, AmazeController amaze)
        {
            fillTrigger.SetAmaze(amaze);
        }

        private void GenerateWalls(Transform parent, int areaCount)
        {
            if (areaCount >= 3) return;

            var size = (24 - (areaCount * _fillAreaWidth)) / 2;
            var position = 12 - size / 2;
            SpawnWall(parent, Vector3.left * position, size);
            SpawnWall(parent, Vector3.right * position, size);
        }

        private void SpawnWall(Transform parent, Vector3 wallPosition, float wallScale)
        {
            var wall = Object.Instantiate(_wall, parent);
            wall.localPosition = wallPosition;
            wall.localScale = new Vector3(wallScale, 1f, 1f);
        }
    }
}