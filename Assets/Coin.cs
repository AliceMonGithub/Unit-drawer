using UnityEngine;

internal class Coin : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private CoinsCounter _coinsCounter;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Unit unit))
        {
            _coinsCounter.AddCoin(100);

            Instantiate(_particles, transform.position,Quaternion.identity);

            Destroy(gameObject);
        }
    }
}