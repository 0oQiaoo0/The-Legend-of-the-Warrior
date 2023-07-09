using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public Transform playerTrans;  // 玩家的Transform组件

    public Vector3 firstPosition;  // 初始位置
    public Vector3 menuPosition;  // 菜单位置
    public float fadeDuration;  // 渐变持续时间
    [Header("场景")]
    public GameSceneSO firstLoadScene;  // 第一个加载的场景
    public GameSceneSO menuScene;  // 菜单场景
    [SerializeField] private GameSceneSO currentLoadScene;  // 当前加载的场景

    [Header("事件监听")]
    public VoidEventSO newGameEvent;

    [Header("广播")]
    public SceneLoadEventSO loadEventSO;  // 加载开始事件
    public SceneLoadEventSO unloadEventSO; // 卸载开始事件
    public VoidEventSO afterSceneLoadedEvent;  // 卸载结束事件
    public FadeEventSO fadeEvent;  // 渐变效果事件

    private bool isLoading;  // 是否正在加载场景
   
    private GameSceneSO sceneToLoad;  // 要加载的场景
    private Vector3 posToGo;  // 加载后玩家要移动到的位置
    private bool fadeScreen;  // 是否需要淡入淡出过渡效果

    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;  // 订阅场景加载请求事件
        newGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;  // 取消订阅场景加载请求事件
        newGameEvent.OnEventRaised -= NewGame;
    }
    private void Start()
    {
        // NewGame();
        loadEventSO.RaiseEvent(menuScene, menuPosition, true);  // 触发场景加载请求事件
    }

    
    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        posToGo = firstPosition;
        //OnLoadRequestEvent(firstLoadScene, firstPosition, true);
        loadEventSO.RaiseEvent(sceneToLoad, posToGo, true);  // 触发场景加载请求事件
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneToLoad">要加载的场景</param>
    /// <param name="posToGo">加载后玩家要移动到的位置</param>
    /// <param name="fadeScreen">是否需要淡入淡出过渡效果</param>
    private void OnLoadRequestEvent(GameSceneSO sceneToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        this.sceneToLoad = sceneToLoad;
        this.posToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
        
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            //渐入
            fadeEvent.FadeIn(fadeDuration);
        }
        
        yield return new WaitForSeconds(fadeDuration);

        unloadEventSO.RaiseEvent(sceneToLoad, posToGo, fadeScreen);

        yield return currentLoadScene.sceneReference.UnLoadScene();  // 卸载当前加载的场景

        //隐藏角色
        playerTrans.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);  // 加载新场景
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad;  // 设置当前加载的场景为新加载的场景

        playerTrans.transform.position = posToGo;  // 设置玩家的位置为目标位置

        playerTrans.gameObject.SetActive(true);  // 激活玩家游戏对象

        if (fadeScreen)
        {
            //淡出
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        //若加载场景不为菜单
        if (currentLoadScene.sceneType != SceneType.Menu)
            afterSceneLoadedEvent?.RaiseEvent();  // 触发场景加载后广播事件
    }
}
