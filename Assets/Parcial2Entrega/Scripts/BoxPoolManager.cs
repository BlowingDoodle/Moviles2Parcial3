using UnityEngine;

public class BoxPoolManager : MonoBehaviour
{
    public static BoxPoolManager Instance { get; private set; }

    public ObjectPool bombPool;
    public ObjectPool scoreBoxPool;
    public ObjectPool emptyBoxPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetBox(BoxBehaviour.BoxType boxType)
    {
        switch (boxType)
        {
            case BoxBehaviour.BoxType.Bomb:
                return bombPool.GetObject();
            case BoxBehaviour.BoxType.Score:
                return scoreBoxPool.GetObject();
            case BoxBehaviour.BoxType.Empty:
                return emptyBoxPool.GetObject();
            default:
                return null;
        }
    }

    public void ReturnBox(GameObject box)
    {
        BoxBehaviour behavior = box.GetComponent<BoxBehaviour>();
        if (behavior != null)
        {
            switch (behavior.boxType)
            {
                case BoxBehaviour.BoxType.Bomb:
                    bombPool.ReturnObject(box);
                    break;
                case BoxBehaviour.BoxType.Score:
                    scoreBoxPool.ReturnObject(box);
                    break;
                case BoxBehaviour.BoxType.Empty:
                    emptyBoxPool.ReturnObject(box);
                    break;
            }
        }
    }
}

