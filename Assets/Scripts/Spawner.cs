using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IColorable
{
    [SerializeField] private T _object;
    [SerializeField] private TMP_Text _textObjectCreate;
    [SerializeField] private TMP_Text _textObjectActive;

    private ObjectPool<T> _pool;

    private int _countObjectCreate;
    private int _countObjectActive;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
        createFunc: Create,
        actionOnGet: ReceiveObject,
        actionOnRelease: (gameObject) => gameObject.gameObject.SetActive(false));
    }

    public virtual void ReceiveObject(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.SetStartColor();
    }

    public virtual T Create()
    {
        T obj = Instantiate(_object);
        InitializeObject(obj, Vector3.zero);
        _countObjectCreate++;
        _textObjectCreate.text = _countObjectCreate.ToString($"Количество созданных {GetObjectName()}: {_countObjectCreate}");
        return obj;
    }

    public virtual void ReturnItem(T obj)
    {
        _pool.Release(obj);
        _countObjectActive--;
        _textObjectActive.text = _countObjectActive.ToString($"Количество активных  {GetObjectName()}: {_countObjectActive}");
    }

    public T GetObject()
    {
        _countObjectActive++;
        _textObjectActive.text = _countObjectActive.ToString($"Количество активных {GetObjectName()}: {_countObjectActive}");
        return _pool.Get();
    }

    protected abstract string GetObjectName();
    protected abstract void InitializeObject(T obj, Vector3 position);
}
