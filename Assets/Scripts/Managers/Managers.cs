using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers Instance; // 유일성이 보장됨
    //public static Managers GetInstance() { Initialize(); return Instance;} // 유일한 매니저를 가지고 온다
    static Managers p_Instance { get { Initialize(); return Instance; } } // 객체가 된 매니저를 선언하고 초기화를 실행한 매니저 객체 인스턴스로 반환한다.

    #region Contents
    GameManager _game = new GameManager();

    public static GameManager Game { get { return p_Instance._game; } }
    #endregion

    #region Core
    InputManager _input = new InputManager(); // 인풋매니저의 클래스 변수 선언
    ResourceManager _resource = new ResourceManager(); // 리소스 매니저의 클래스 변수 선언
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
        // 초기화
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("[Managers] Update 호출됨"); // 반드시 뜨는가?
        _input.OnUpdate(); // 인풋매니저에 있는 인스턴스를 생성후 불러옴
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
