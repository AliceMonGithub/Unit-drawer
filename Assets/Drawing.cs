using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

internal class Drawing : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public event Action<Vector2[]> DrawingEnded;

    [SerializeField] private float _lineExtendDistance;

    [Space]

    [SerializeField] private UILineRenderer _lineRendering;
    [SerializeField] private Transform _offset;

    private bool _pointerDown;

    private Vector2 LastPoint => _lineRendering.Points[^1];

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_pointerDown == false) return;

        TryExtendLine(eventData);

        print(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DeletePoints();

        _pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pointerDown = false;

        DrawingEnded?.Invoke(_lineRendering.Points);
    }

    private void TryExtendLine(PointerEventData data)
    {
        Vector2 point = data.position;

        if(_lineRendering.Points.Length == 1)
        {
            _lineRendering.Points[0] = point - (Vector2)_offset.position;

            AddPointToLine(point);

            return;
        }

        Vector2 lastPoint = LastPoint;

        if(Vector3.Distance(lastPoint, point) > _lineExtendDistance)
        {
            AddPointToLine(point);
        }
    }

    private void DeletePoints()
    {
        _lineRendering.Points = new Vector2[1];

        _lineRendering.RelativeSize = false;
        _lineRendering.drivenExternally = true;

    }

    private void AddPointToLine(Vector2 point)
    {
        Array.Resize(ref _lineRendering.m_points, _lineRendering.Points.Length + 1);

        _lineRendering.Points[^1] = point - (Vector2)_offset.position;

        _lineRendering.RelativeSize = false;
        _lineRendering.drivenExternally = true;
    }
}