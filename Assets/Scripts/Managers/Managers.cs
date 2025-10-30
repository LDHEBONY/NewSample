using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers Instance; // ���ϼ��� �����
    //public static Managers GetInstance() { Initialize(); return Instance;} // ������ �Ŵ����� ������ �´�
    static Managers p_Instance { get { Initialize(); return Instance; } } // ��ü�� �� �Ŵ����� �����ϰ� �ʱ�ȭ�� ������ �Ŵ��� ��ü �ν��Ͻ��� ��ȯ�Ѵ�.

    #region Contents
    GameManager _game = new GameManager();

    public static GameManager Game { get { return p_Instance._game; } }
    #endregion

    #region Core
    InputManager _input = new InputManager(); // ��ǲ�Ŵ����� Ŭ���� ���� ����
    ResourceManager _resource = new ResourceManager(); // ���ҽ� �Ŵ����� Ŭ���� ���� ����
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();
    public static InputManager Input { get { return p_Instance._input; } } // 
    public static ResourceManager Resource {  get { return p_Instance._resource; } }

    public static PoolManager Pool { get { return p_Instance._pool; } }
    public static SceneManagerEx Scene { get { return p_Instance._scene; } }

    public static SoundManager Sound { get { return p_Instance._sound; } }
    public static UIManager UI { get { return p_Instance._ui; } }
    public static DataManager Data {get { return p_Instance._data; } }
    #endregion Core

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ�ȭ
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("[Managers] Update ȣ���"); // �ݵ�� �ߴ°�?
        _input.OnUpdate(); // ��ǲ�Ŵ����� �ִ� �ν��Ͻ��� ������ �ҷ���
    }

    static void Initialize()
    {
        if (Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(go);
            Instance = go.GetComponent<Managers>();

            Instance._data.Init();
            Instance._pool.Init();
            Instance._sound.Init();
        }
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
