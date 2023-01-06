using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class UnitsBehaviour : CompositeRoot
{
    private readonly int WalkHash = Animator.StringToHash("Walking");

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _toLineTime;

    [Space]

    [SerializeField] private Unit[] _startUnits;
    [SerializeField] private Transform _offset;
    [SerializeField] private Transform _root;

    [Space]

    [SerializeField] private Drawing _drawing;
    [SerializeField] private Camera _camera;

    private float _zOffset;

    private List<Unit> _units = new List<Unit>();

    private void Update()
    {
        _root.Translate(_movementSpeed * Time.deltaTime * Vector3.forward);

        _zOffset -= _movementSpeed * Time.deltaTime;
    }

    private void OnEnable()
    {
        _drawing.DrawingEnded += UnitsToPoints;
    }

    private void OnDisable()
    {
        _drawing.DrawingEnded -= UnitsToPoints;
    }

    public override void Compose()
    {
        foreach (Unit unit in _startUnits)
        {
            _units.Add(unit);

            unit.Animator.SetBool(WalkHash, true);

            unit.OnDie += ClearUnit;
        }
    }

    public void UnitsToPoints(Vector2[] points)
    {
        if (points.Length == 0 || _units.Count == 0) return;

        List<Unit> destroying = new();

        float step = points.Length / _units.Count;
        float stepOffset = (float)points.Length / _units.Count;

        print(stepOffset);

        int indexStep = Mathf.RoundToInt(step) == 0 ? 1 : Mathf.RoundToInt(step);
        int index = indexStep;

        step = (step - (int)step) == 0 ? 1 : step - (int)step;

        foreach (Unit unit in _units)
        {
            if (unit.Died == true)
            {
                destroying.Add(unit);

                continue;
            }

            Vector2 point = Vector3.zero;

            if (stepOffset < 1f)
            {
                point = points[Random.Range(0, points.Length)];
            }
            else
            {
                point = points[index - 1] * step;
            }

            Ray ray = _camera.ScreenPointToRay(point);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 worldPosition = hit.point + _offset.position;

                worldPosition.y = _offset.position.y;

                unit.DOKill();
                unit.Transform.DOMove(worldPosition, _toLineTime);
            }

            index += indexStep;
        }

        foreach (var item in destroying)
        {
            _units.Remove(item);
        }
    }

    public void AddUnit(Unit unit)
    {
        if (_units.Find(_ => _ == unit) != null) return;

        _units.Add(unit);

        unit.Animator.SetBool(WalkHash, true);

        unit.Transform.eulerAngles = Vector3.zero;
        unit.Transform.SetParent(_root);
    }

    public void ClearUnit(Unit unit)
    {
        List<Unit> destroying = new();

        _units.Remove(unit);

        foreach (var item in _units)
        {
            if (item.Died) destroying.Add(item);
        }

        foreach (var destroy in destroying)
        {
            _units.Remove(destroy);
        }

        unit.OnDie -= ClearUnit;

        if(_units.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
