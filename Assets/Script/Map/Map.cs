using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public interface IMap {
    IMapUnit GetMapUnit(int x, int y);
}

[Serializable]
public class Map : IMap {
    [SerializeField]
    private List<List<IMapUnit>> data = new List<List<IMapUnit>>();
    private Vector2Int lastSize = Vector2Int.zero;
    public Vector2Int size;
    public float angle;
    public float length;
    public Vector2 UAxis {
        get {
            return Vector2Util.RotateAxisDir(angle / 2 + 90, length);
        }
    }
    public Vector2 VAxis {
        get {
            return Vector2Util.RotateAxisDir(90 - angle / 2, length);
        }
    }
    public Vector2 originPoint;
    public IEnumerable<IMapUnit> GetAllUnits() {
        foreach(var unitRow in data) {
            foreach(var unit in unitRow) {
                if(unit != null) {
                    yield return unit;
                }
            }
        }
    }
    public void Update() {
        List<IMapUnit> units = new List<IMapUnit>(GetAllUnits());
        if(lastSize != size) {
            data.Clear(); 
            var unitRow = new List<IMapUnit>();
            for(int i = 0; i < size.y; i++) {
                unitRow.Add(null);
            }
            for(int i = 0; i < size.x; i++) {
                data.Add(unitRow);
            }
            foreach(var unit in units) {
                InsertMapUnit(unit);
            }
        }
        lastSize = size;
    }
    public Vector2Int WorldToMapPoint(Vector2 worldPoint) {
        //Debug.Log("world: " + worldPoint.ToString());
        Vector2 offset = worldPoint - originPoint;
        Vector2 u = UAxis;
        Vector2 v = VAxis;
        float a = u.y * v.x - u.x * v.y;
        float fv = (u.y * offset.x - u.x * offset.y) / a;
        float fu = (v.x * offset.y - v.y * offset.x) / a;
        Vector2Int result = ClampPosition(new Vector2Int(Mathf.RoundToInt(fu), Mathf.RoundToInt(fv)));
        //Debug.Log("to map: " + result.ToString());
        return result;
    }
    public Vector2 MapToWorldPoint(Vector2Int mapPoint) {
        //Debug.Log("map: " + mapPoint);
        Vector2 result = mapPoint.x * UAxis + mapPoint.y * VAxis + originPoint;
        //Debug.Log("to world: " + result);
        return result;
    }
    public bool AreaEmpty(IEnumerable<Vector2Int> positions) {
        foreach(Vector2Int position in positions) {
            if(GetMapUnit(position) != null) return false;
        }
        return true;
    }
    public bool AreaEmpty(IMapUnit unit) {
        return AreaEmpty(unit.GetPositions());
    }
    public void InsertMapUnit(IMapUnit unit) {
        Debug.Assert(AreaEmpty(unit));
        foreach(var pos in unit.GetPositions()) {
            SetMapUnit(pos, unit);
        }
    }
    public Vector2Int ClampPosition(Vector2Int position) {
        position.x = Mathf.Clamp(position.x, 0, size.x - 1);
        position.y = Mathf.Clamp(position.y, 0, size.y - 1);
        return position;
    }
    public void UpdateMapUnit(IMapUnit unit, Vector2Int newPosition) {
        DeleteMapUnit(unit);
        unit.SetPosition(newPosition);
        Debug.Assert(AreaEmpty(unit));
        InsertMapUnit(unit);
    }
    public void DeleteMapUnit(IMapUnit unit) {
        foreach(var pos in unit.GetPositions()) {
            SetMapUnit(pos, null);
        }
    }

    public IMapUnit GetMapUnit(Vector2Int position)
    {
        return GetMapUnit(position.x, position.y);
    }
    public IMapUnit GetMapUnit(int x, int y)
    {
        return data[x][y];
    }
    public void SetMapUnit(int x, int y, IMapUnit mapUnit) {
        data[x][y] = mapUnit;
    }
    public void SetMapUnit(Vector2Int position, IMapUnit mapUnit) {
        SetMapUnit(position.x, position.y, mapUnit);
    }
}