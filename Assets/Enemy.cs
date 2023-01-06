using UnityEngine;

internal class Enemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Unit unit))
        {
            unit.Die();
        }
    }
}
