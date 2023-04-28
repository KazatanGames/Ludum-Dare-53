namespace KazatanGames.Framework
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;
    using System;
    /**
     * App Manager
     * 
     * Kazatan Games Framework - should not require customization per game.
     * 
     * The App Manager is the main entry point.
     * An instance of which should exist on the Load Scene (which is the entrypoint scene).
     * It performs any system specific configuration, then creates any prefabs (typically
     * singleton managers), then instantiates an AppModel and loads the intiial scene.
     */
    public abstract class AppManager<T, U> : SingletonMonoBehaviour<T>
        where T : AppManager<T, U>
        where U : AppConfigSO
    {
        [SerializeField]
        protected List<GameObject> prefabsToCreate;
        [SerializeField]
        protected List<GameObject> introElements;
        [SerializeField]
        protected U config;

        public AppModel AppModel { get; protected set; }
        public U AppConfig => config;

        protected int introElementsComplete = 0;

        public event Action OnIntrosComplete;

        protected override void Initialise()
        {
            // 1. System specific configuration

            // 2. Create AppModel
            CreateAppModel();

            // 3. Attach Intros
            introElementsComplete = 0;
            AttachToIntros();
        }

        protected void CreateAppModel()
        {
            AppModel = new AppModel
            {
                debugMode = config.debugMode
            };
        }

        protected void AttachToIntros()
        {
            foreach (GameObject go in introElements)
            {
                BaseIntroElement iie = go.GetComponent<BaseIntroElement>();
                if (iie == null || iie.IsComplete)
                {
                    introElementsComplete++;
                    Destroy(go);
                }
                else
                {
                    iie.OnIntroElementComplete += OnIntroElementComplete;
                }
            }
            CheckIntrosComplete();
        }

        protected void OnIntroElementComplete(BaseIntroElement introElement)
        {
            introElement.OnIntroElementComplete -= OnIntroElementComplete;
            introElementsComplete++;
            CheckIntrosComplete();
        }

        protected void CheckIntrosComplete()
        {
            if (introElementsComplete == introElements.Count)
            {
                CreatePrefabs();

                OnIntrosComplete?.Invoke();

                if (config.autoLoadInitialScene && config.initialScene != null)
                {
                    SceneManager.LoadScene(config.initialScene, LoadSceneMode.Single);
                }
            }
        }

        protected void CreatePrefabs()
        {
            for (int i = 0; i < prefabsToCreate.Count; i++)
            {
                GameObject.Instantiate(prefabsToCreate[i]);
            }
        }

        private void Update()
        {
            if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}