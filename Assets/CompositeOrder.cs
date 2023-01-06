using UnityEngine;

internal class CompositeOrder : MonoBehaviour
{
    [SerializeField] private CompositeRoot[] _order;

    private void Awake()
    {
        foreach (CompositeRoot item in _order)
        {
            item.Compose();
        }
    }
}

internal abstract class CompositeRoot : MonoBehaviour
{
    public abstract void Compose();
}
