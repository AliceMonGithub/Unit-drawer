using UnityEngine;

internal class AddedToUnits : MonoBehaviour
{
    [SerializeField] private UnitsBehaviour _unitsBehaviour;
    [SerializeField] private Unit _unit;

    private bool _added;

    private void OnCollisionEnter(Collision collision)
    {
        if (_added) return;

        if(collision.gameObject.TryGetComponent(out Unit unit))
        {
            _unitsBehaviour.AddUnit(_unit);

            _added = true;
        }
    }
}
