using UnityEngine;
using UnityEngine.SceneManagement;

internal class Restart : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Unit unit))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}