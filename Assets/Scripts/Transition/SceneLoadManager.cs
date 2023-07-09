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
    public Transform playerTrans;  // ��ҵ�Transform���

    public Vector3 firstPosition;  // ��ʼλ��
    public Vector3 menuPosition;  // �˵�λ��
    public float fadeDuration;  // �������ʱ��
    [Header("����")]
    public GameSceneSO firstLoadScene;  // ��һ�����صĳ���
    public GameSceneSO menuScene;  // �˵�����
    [SerializeField] private GameSceneSO currentLoadScene;  // ��ǰ���صĳ���

    [Header("�¼�����")]
    public VoidEventSO newGameEvent;

    [Header("�㲥")]
    public SceneLoadEventSO loadEventSO;  // ���ؿ�ʼ�¼�
    public SceneLoadEventSO unloadEventSO; // ж�ؿ�ʼ�¼�
    public VoidEventSO afterSceneLoadedEvent;  // ж�ؽ����¼�
    public FadeEventSO fadeEvent;  // ����Ч���¼�

    private bool isLoading;  // �Ƿ����ڼ��س���
   
    private GameSceneSO sceneToLoad;  // Ҫ���صĳ���
    private Vector3 posToGo;  // ���غ����Ҫ�ƶ�����λ��
    private bool fadeScreen;  // �Ƿ���Ҫ���뵭������Ч��

    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;  // ���ĳ������������¼�
        newGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;  // ȡ�����ĳ������������¼�
        newGameEvent.OnEventRaised -= NewGame;
    }
    private void Start()
    {
        // NewGame();
        loadEventSO.RaiseEvent(menuScene, menuPosition, true);  // �����������������¼�
    }

    
    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        posToGo = firstPosition;
        //OnLoadRequestEvent(firstLoadScene, firstPosition, true);
        loadEventSO.RaiseEvent(sceneToLoad, posToGo, true);  // �����������������¼�
    }

    /// <summary>
    /// ���س���
    /// </summary>
    /// <param name="sceneToLoad">Ҫ���صĳ���</param>
    /// <param name="posToGo">���غ����Ҫ�ƶ�����λ��</param>
    /// <param name="fadeScreen">�Ƿ���Ҫ���뵭������Ч��</param>
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
            //����
            fadeEvent.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        unloadEventSO.RaiseEvent(sceneToLoad, posToGo, fadeScreen);

        yield return currentLoadScene.sceneReference.UnLoadScene();  // ж�ص�ǰ���صĳ���

        //���ؽ�ɫ
        playerTrans.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);  // �����³���
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad;  // ���õ�ǰ���صĳ���Ϊ�¼��صĳ���

        playerTrans.transform.position = posToGo;  // ������ҵ�λ��ΪĿ��λ��

        playerTrans.gameObject.SetActive(true);  // ���������Ϸ����

        if (fadeScreen)
        {
            //����
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        //�����س�����Ϊ�˵�
        if (currentLoadScene.sceneType != SceneType.Menu)
            afterSceneLoadedEvent?.RaiseEvent();  // �����������غ�㲥�¼�
    }
}
