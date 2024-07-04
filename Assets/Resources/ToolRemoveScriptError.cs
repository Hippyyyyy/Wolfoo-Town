using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
#if UNITY_EDITOR
public class FindMissingScriptsWindow : EditorWindow
{
    [MenuItem("Tools/Missing Script Window")]
    private static void Init()
    {
        GetWindow<FindMissingScriptsWindow>("Missing Script Finder").Show();
    }

    public List<GameObject> results = new List<GameObject>();

    private void OnGUI()
    {
        if (GUILayout.Button("Search Project"))
            SearchProject();
        if (GUILayout.Button("Search Selected Objects"))
            SearchSelected();
        GUILayout.Space(20);
        if (GUILayout.Button("Search scene"))
            SearchScene();
        if (GUILayout.Button("Remove Error Scripts"))
            RemoveScripts();
        // src: https://answers.unity.com/questions/859554/editorwindow-display-array-dropdown.html
        var so = new SerializedObject(this);
        var resultsProperty = so.FindProperty(nameof(results));
        EditorGUILayout.PropertyField(resultsProperty, true);
        so.ApplyModifiedProperties();
    }

    private void SearchProject()
    {
        results = new List<GameObject>();
        results = AssetDatabase.FindAssets("t:Prefab")
           .Select(AssetDatabase.GUIDToAssetPath)
           .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
           .Where(x => IsMissing(x, true))
           .Distinct()
           .ToList();
    }

    private void SearchScene()
    {
        results = new List<GameObject>();
        results = FindObjectsOfType<GameObject>()
           .Where(x => IsMissing(x, false))
           .Distinct()
           .ToList();
    }

    private void SearchSelected()
    {
        results = new List<GameObject>();
        results = Selection.gameObjects
           .Where(x => IsMissing(x, false))
           .Distinct()
           .ToList();
    }
    //private void AddScript()
    //{
    //    var target = GameObject.Find("LevelSuccessArea");
    //    target.AddComponent<TargetFinish>();
    //}

    private static bool IsMissing(GameObject go, bool includeChildren)
    {
        var components = includeChildren
           ? go.GetComponentsInChildren<Component>()
           : go.GetComponents<Component>();

        return components.Any(x => x == null);
    }
    private void RemoveScripts()
    {
        //  AddScript();
        int count = results.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
        Debug.Log($"Removed {count} missing scripts");
    }
}
public class SceneTools : Editor
{
    const string ProjectPath = "Games/";
    public static string BeachVillaPath = "Assets/_WolfooBeachVilla/WFBeachVilla_Scene.unity";
    public static string CampingParkPath = "Assets/_WolfooCampingPark/WFCampingPark_Scene.unity";
    public static string CityPath = "Assets/_WolfooCity/WolfooCity_Scene.unity";
    public static string HospitalPath = "Assets/_WolfooHospital/WFHospital_Scene.unity";
    public static string HousePath = "Assets/_WolfooHouse/WFHouse_Scene.unity"; 
    public static string OperaPath = "Assets/_WolfooOpera/WFOpera_Scene.unity";
    public static string PlayGroundPath = "Assets/_WolfooPlayground/WFPlayground_Scene.unity";
    public static string SchoolPath = "Assets/_WolfooSchool/WolfooSchool_Scene.unity";
    public static string ShoppingMallPath = "Assets/_WolfooShoppingMall/WolfooShoppingMall_Scene.unity";
    public static string LoadScenePath = "Assets/_Base/LoadScene.unity";
    public static string CharacterScenePath = "Assets/Character Creator/Scene/Character Creator.unity";
    public static string MediterraneanScenePath = "Assets/_WolfooSelf-Governing_House/SelfHouse-Mediterranean/WFMediterranean_Scene.unity";
    [MenuItem(ProjectPath + "Play", false, 0)]
    private static void PlayGame()
    {
        OpenLoadScene();
        EditorApplication.isPlaying = true;
    }
    [MenuItem(ProjectPath + "Scenes/Open Character Scene", false, 1)]
    private static void OpenCharacterScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(CharacterScenePath);
    }
    [MenuItem(ProjectPath + "Scenes/Open Load Scene", false, 1)]
    private static void OpenLoadScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LoadScenePath);
    }
    [MenuItem(ProjectPath + "Scenes/Open BeachVilla Scene", false, 2)]
    private static void OpenBeachVillaScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(BeachVillaPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open CampingPark Scene", false, 3)]
    private static void OpenCampingParkScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(CampingParkPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open City Scene", false, 4)]
    private static void OpenCityScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(CityPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open Hospital Scene", false, 5)]
    private static void OpenHospitalScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(HospitalPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open House Scene", false, 6)]
    private static void OpenHouseScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(HousePath);
    }
    [MenuItem(ProjectPath + "Scenes/Open Opera Scene", false, 7)]
    private static void OpenOperaScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(OperaPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open Playground Scene", false, 8)]
    private static void OpenPlaygroundScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(PlayGroundPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open School Scene", false, 9)]
    private static void OpenSchoolScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(SchoolPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open ShoppingMall Scene", false, 10)]
    private static void OpenShoppingMallScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ShoppingMallPath);
    }
    [MenuItem(ProjectPath + "Scenes/Open Self-House/Open Mediterranean Scene", false, 0)]
    private static void OpenMediterraneanScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(MediterraneanScenePath);
    }

}
//public class ConvertLevel : EditorWindow
//{
//    [MenuItem("Tools/ConvertLevel")]
//    private static void Init()
//    {
//        GetWindow<ConvertLevel>("Convert Level").Show();
//    }
//    public List<GameObject> CharacterMerge = new List<GameObject>();
//    public HorizontalMovement PrefabCharMerge = null;
//    public GameObject PlayerPack = null;
//    public BattleLevelData BattleLevelData = null;
//    //public Boss BossPack = null;
//    public LineSaw LineSaw = null;
//    public LineSaw LineSawShort = null;
//    public FinishBoxing FinishBoxing = null;
//    public BombTrigger BombTrigger = null;
//    public FoodObstacle FoodObstacle = null;
//    public JumpTrigger JumpTrigger = null;
//    public Pendulum Pendulum = null;
//    public Knife Knife = null;
//    public CoinObject CoinObject = null;
//    public RotatingBlade RotatingBlade = null;
//    public CircleSaw CircleSaw = null;
//    public Hammer Hammer = null;
//    public BladeExtended BladeExtended = null;
//    public SpikesGround SpikesGround = null;
//    public BoxingGloveRoot BoxingGloveRoot = null;
//    public DestroyButtonTrigger DestroyButtonTrigger = null;
//    public BoostTrigger BoostTrigger = null;
//    public CubeWithSpikes CubeWithSpikes = null;
//    public SawRotate2 SawRotate2 = null;

//    private void OnGUI()
//    {
//        if (GUILayout.Button("Unpack PrefabLevel"))
//            UnpackPrefabLevel();
//        GUILayout.Space(20);
//        if (GUILayout.Button("ValidateLevel"))
//            ChangeCharacters();
//        var so = new SerializedObject(this);
//        var CharacterMerge_ = so.FindProperty(nameof(CharacterMerge));
//        var prefabCharMerge_ = so.FindProperty(nameof(PrefabCharMerge));
//        var prefabPlayer_ = so.FindProperty(nameof(PlayerPack));
//        //var prefabBoss_ = so.FindProperty(nameof(BossPack));
//        GUILayout.Space(20);
//        EditorGUILayout.PropertyField(CharacterMerge_, true);
//        GUILayout.Space(10);
//        EditorGUILayout.PropertyField(prefabCharMerge_, true);
//        GUILayout.Space(10);
//        EditorGUILayout.PropertyField(prefabPlayer_, true);
//        GUILayout.Space(10);
//        //EditorGUILayout.PropertyField(prefabBoss_, true);
//        so.ApplyModifiedProperties();
//    }
//    //[MenuItem("Tools/Validate WallBox")]
//    float maxPower = 0;
//    Transform bossPosition = null;
//    LevelManager levelManager = null;
//    BattleLevelData battleLevelData = null;

//    Vector3 cachePosBoxing = Vector3.zero;
//    private void UnpackPrefabLevel()
//    {
//        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
//        {
//            if (gameObj.name.Contains("Level_") || gameObj.name.Contains("Battle Level"))
//            {
//                PrefabUtility.UnpackPrefabInstance(gameObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
//            }
//        }
//    }
//    private void ChangeCharacters()
//    {
//        CharacterMerge = new List<GameObject>();
//        PrefabCharMerge = Resources.Load<HorizontalMovement>("Objects/PlayerGet");
//        PlayerPack = Resources.Load<GameObject>("Objects/PlayerPack");
//        //BossPack = Resources.Load<Boss>("Objects/Boss");
//        LineSaw = Resources.Load<LineSaw>("Objects/LineSaw");
//        LineSawShort = Resources.Load<LineSaw>("Objects/LineSawShort");
//        FinishBoxing = Resources.Load<FinishBoxing>("Objects/FinishBoxing");
//        BombTrigger = Resources.Load<BombTrigger>("Objects/BombTrigger");
//        FoodObstacle = Resources.Load<FoodObstacle>("Objects/FoodObstacle");
//        JumpTrigger = Resources.Load<JumpTrigger>("Objects/JumpTrigger");
//        Pendulum = Resources.Load<Pendulum>("Objects/Pendulum");
//        Knife = Resources.Load<Knife>("Objects/Knife");
//        CoinObject = Resources.Load<CoinObject>("Objects/CoinObject");
//        RotatingBlade = Resources.Load<RotatingBlade>("Objects/RotatingBlade");
//        CircleSaw = Resources.Load<CircleSaw>("Objects/CircleSaw");
//        BladeExtended = Resources.Load<BladeExtended>("Objects/BladeExtended");
//        Hammer = Resources.Load<Hammer>("Objects/Hammer");
//        SpikesGround = Resources.Load<SpikesGround>("Objects/SpikesGround");
//        BoxingGloveRoot = Resources.Load<BoxingGloveRoot>("Objects/BoxingGloveRoot");
//        DestroyButtonTrigger = Resources.Load<DestroyButtonTrigger>("Objects/DestroyButtonTrigger");
//        BoostTrigger = Resources.Load<BoostTrigger>("Objects/BoostTrigger");
//        SawRotate2 = Resources.Load<SawRotate2>("Objects/SawRotate2");
//        CubeWithSpikes = Resources.Load<CubeWithSpikes>("Objects/CubeWithSpikes");
//        GameObject parent = null;
//        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
//        {
//            if (gameObj.name.Contains("CharacterJelly"))
//            {
//                CharacterMerge.Add(gameObj);
//            }
//            if (gameObj.name == "PLAYERS")
//            {
//                parent = gameObj;
//            }
//            if (gameObj.name.Contains("FinishTrigger"))
//            {
//                if (gameObj.GetComponent<FinishLine>() == null)
//                    gameObj.AddComponent<FinishLine>();
//            }
//            if (gameObj.name.Contains("CoinTrigger"))
//            {
//                if (gameObj.GetComponent<CoinObject>() == null)
//                    gameObj.AddComponent<CoinObject>();
//            }
//            if (gameObj.name.Contains("Level_"))
//            {
//                if (gameObj.GetComponent<LevelManager>() == null)
//                    levelManager = gameObj.AddComponent<LevelManager>();
//            }

//            if (gameObj.name.Contains("OBSTACKLES"))
//            {
//                var childCount = gameObj.transform.childCount;
//                var listCache = new List<GameObject>();
//                for (int i = 0; i < childCount; i++)
//                {
//                    var child = gameObj.transform.GetChild(i);
//                    if (child.name.Contains("LineSawShort"))
//                    {
//                        LineSaw lineSawShort = PrefabUtility.InstantiatePrefab(LineSawShort, gameObj.transform) as LineSaw;
//                        lineSawShort.transform.position = child.transform.position;
//                        lineSawShort.transform.localEulerAngles = child.transform.localEulerAngles;
//                        lineSawShort.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("LineSaw"))
//                    {
//                        LineSaw lineSaw = PrefabUtility.InstantiatePrefab(LineSaw, gameObj.transform) as LineSaw;
//                        lineSaw.transform.position = child.transform.position;
//                        lineSaw.transform.localEulerAngles = child.transform.localEulerAngles;
//                        lineSaw.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("BombTrigger"))
//                    {
//                        BombTrigger bombTrigger = PrefabUtility.InstantiatePrefab(BombTrigger, gameObj.transform) as BombTrigger;
//                        bombTrigger.transform.position = child.transform.position;
//                        bombTrigger.transform.localEulerAngles = child.transform.localEulerAngles;
//                        bombTrigger.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("Food"))
//                    {
//                        FoodObstacle foodObstacle = PrefabUtility.InstantiatePrefab(FoodObstacle, gameObj.transform) as FoodObstacle;
//                        foodObstacle.transform.position = child.transform.position;
//                        foodObstacle.transform.localEulerAngles = child.transform.localEulerAngles;
//                        foodObstacle.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("Knife"))
//                    {
//                        Knife knife = PrefabUtility.InstantiatePrefab(Knife, gameObj.transform) as Knife;
//                        knife.transform.position = child.transform.position;
//                        knife.transform.localEulerAngles = child.transform.localEulerAngles;
//                        knife.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("Pendulum"))
//                    {
//                        Pendulum pendulum = PrefabUtility.InstantiatePrefab(Pendulum, gameObj.transform) as Pendulum;
//                        pendulum.transform.position = child.transform.position;
//                        pendulum.transform.localEulerAngles = child.transform.localEulerAngles;
//                        pendulum.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("rotating_blade"))
//                    {
//                        RotatingBlade rotatingBlade = PrefabUtility.InstantiatePrefab(RotatingBlade, gameObj.transform) as RotatingBlade;
//                        rotatingBlade.transform.position = child.transform.position;
//                        rotatingBlade.transform.localEulerAngles = child.transform.localEulerAngles;
//                        rotatingBlade.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("circle_saw"))
//                    {
//                        CircleSaw circleSaw = PrefabUtility.InstantiatePrefab(CircleSaw, gameObj.transform) as CircleSaw;
//                        circleSaw.transform.position = child.transform.position;
//                        circleSaw.transform.localEulerAngles = child.transform.localEulerAngles;
//                        circleSaw.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("Hammer"))
//                    {
//                        Hammer hammer = PrefabUtility.InstantiatePrefab(Hammer, gameObj.transform) as Hammer;
//                        hammer.transform.position = child.transform.position;
//                        hammer.transform.localEulerAngles = child.transform.localEulerAngles;
//                        hammer.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("Blade_Extended"))
//                    {
//                        BladeExtended bladeExtended = PrefabUtility.InstantiatePrefab(BladeExtended, gameObj.transform) as BladeExtended;
//                        bladeExtended.transform.position = child.transform.position;
//                        bladeExtended.transform.localEulerAngles = child.transform.localEulerAngles;
//                        bladeExtended.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("spikes_ground"))
//                    {
//                        SpikesGround spikesGround = PrefabUtility.InstantiatePrefab(SpikesGround, gameObj.transform) as SpikesGround;
//                        spikesGround.transform.position = child.transform.position;
//                        spikesGround.transform.localEulerAngles = child.transform.localEulerAngles;
//                        spikesGround.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("BoxingGloveRoot"))
//                    {
//                        BoxingGloveRoot boxingGloveRoot = PrefabUtility.InstantiatePrefab(BoxingGloveRoot, gameObj.transform) as BoxingGloveRoot;
//                        boxingGloveRoot.transform.position = child.transform.position;
//                        boxingGloveRoot.transform.localEulerAngles = child.transform.localEulerAngles;
//                        boxingGloveRoot.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("SawRotate2"))
//                    {
//                        SawRotate2 sawRotate2 = PrefabUtility.InstantiatePrefab(SawRotate2, gameObj.transform) as SawRotate2;
//                        sawRotate2.transform.position = child.transform.position;
//                        sawRotate2.transform.localEulerAngles = child.transform.localEulerAngles;
//                        sawRotate2.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("CubeWithSpikes"))
//                    {
//                        CubeWithSpikes cubeWithSpikes = PrefabUtility.InstantiatePrefab(CubeWithSpikes, gameObj.transform) as CubeWithSpikes;
//                        cubeWithSpikes.transform.position = child.transform.position;
//                        cubeWithSpikes.transform.localEulerAngles = child.transform.localEulerAngles;
//                        cubeWithSpikes.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("Wall_Breakable"))
//                    {
//                        listCache.Add(child.gameObject);
//                    }
//                }
//                foreach (var item in listCache)
//                {
//                    DestroyImmediate(item.gameObject);
//                }
//            }
//            if (gameObj.name.Contains("Battle Level"))
//            {
//                if(gameObj.GetComponent<BattleLevelData>() == null)
//                {
//                    battleLevelData = gameObj.AddComponent<BattleLevelData>();
//                }

//                var listCache = new List<GameObject>();

//                for (int i = 0; i < gameObj.transform.childCount; i++)
//                {
//                    var enemy = gameObj.transform.GetChild(i);
//                    listCache.Add(enemy.gameObject);
//                    var child = enemy.GetChild(1);
//                    Debug.Log(child.name);
//                    for (int j = 0; j < child.childCount; j++)
//                    {
//                        var skin = child.GetChild(j).gameObject;
//                        if (skin.activeInHierarchy)
//                        {
//                            battleLevelData.enemyData.Add(skin.transform.GetSiblingIndex());
//                        }
//                    }
//                    if (enemy.name.Contains("Range Character"))
//                    {
//                        battleLevelData.enemyType.Add(1);
//                    }
//                    else if (enemy.name.Contains("Melee Character"))
//                    {
//                        battleLevelData.enemyType.Add(0);
//                    }
                    
//                }
//                foreach (var item in listCache)
//                {
//                    DestroyImmediate(item.gameObject);
//                }
//            }
//            if (gameObj.name.Contains("TRIGGERS"))
//            {
//                var childCount = gameObj.transform.childCount;
//                var listCache = new List<GameObject>();
//                for (int i = 0; i < childCount; i++)
//                {
//                    var child = gameObj.transform.GetChild(i);
//                    if (child.name.Contains("JumpTrigger"))
//                    {
//                        JumpTrigger jumpTrigger = PrefabUtility.InstantiatePrefab(JumpTrigger, gameObj.transform) as JumpTrigger;
//                        jumpTrigger.transform.position = child.transform.position;
//                        jumpTrigger.transform.localEulerAngles = child.transform.localEulerAngles;
//                        jumpTrigger.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    if (child.name.Contains("DestroyButtonTrigger"))
//                    {
//                        DestroyButtonTrigger destroyButtonTrigger = PrefabUtility.InstantiatePrefab(DestroyButtonTrigger, gameObj.transform) as DestroyButtonTrigger;
//                        destroyButtonTrigger.transform.position = child.transform.position;
//                        destroyButtonTrigger.transform.localEulerAngles = child.transform.localEulerAngles;
//                        destroyButtonTrigger.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    if (child.name.Contains("BoostTrigger"))
//                    {
//                        BoostTrigger boostTrigger = PrefabUtility.InstantiatePrefab(BoostTrigger, gameObj.transform) as BoostTrigger;
//                        boostTrigger.transform.position = child.transform.position;
//                        boostTrigger.transform.localEulerAngles = child.transform.localEulerAngles;
//                        boostTrigger.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                    else if (child.name.Contains("SlipTrigger") || child.name.Contains("KeyCurrencyTrigger"))
//                    {
//                        listCache.Add(child.gameObject);
//                    }
//                }
//                foreach (var item in listCache)
//                {
//                    DestroyImmediate(item.gameObject);
//                }
//            }
//            if (gameObj.name.Contains("BONUSES") && gameObj.transform.childCount > 0)
//            {
//                var childCount = gameObj.transform.childCount;
//                var listCache = new List<GameObject>();
//                for (int i = 0; i < childCount; i++)
//                {
//                    var child = gameObj.transform.GetChild(i);
//                    if (child.name.Contains("CoinTrigger"))
//                    {
//                        CoinObject coinObject = PrefabUtility.InstantiatePrefab(CoinObject, gameObj.transform) as CoinObject;
//                        coinObject.transform.position = child.transform.position;
//                        coinObject.transform.localEulerAngles = child.transform.localEulerAngles;
//                        coinObject.transform.localScale = child.transform.localScale;
//                        listCache.Add(child.gameObject);
//                    }
//                }
//                foreach (var item in listCache)
//                {
//                    DestroyImmediate(item.gameObject);
//                }
//            }
//            if (gameObj.name == "FinishBoxingController")
//            {
//                cachePosBoxing = gameObj.transform.position;
//                // gameObj.SetActive(false);
//            }
//            if (gameObj.name == ("Water"))
//            {
//                DestroyImmediate(gameObj);
//            }
//        }
//        FinishBoxing finishBoxing_ = PrefabUtility.InstantiatePrefab(FinishBoxing, levelManager.transform) as FinishBoxing;
//        finishBoxing_.transform.position = cachePosBoxing;
//        bossPosition = finishBoxing_.BosPosition;

//        DestroyImmediate(levelManager.transform.GetChild(0).gameObject);
//        for (int i = 0; i < CharacterMerge.Count; i++)
//        {
//            if (CharacterMerge[i].transform.position.z > 3)
//            {
//                HorizontalMovement charMerge_ = PrefabUtility.InstantiatePrefab(PrefabCharMerge, parent.transform) as HorizontalMovement;
//                charMerge_.transform.position = CharacterMerge[i].transform.position;
//                //    charMerge.transform.localEulerAngles = new Vector3(0, 180, 0);
//                var scale = CharacterMerge[i].transform.localScale.x;
//                var multiple = (scale / .25f) - 3;
//                var power = multiple > 1 ? 10 * Mathf.Pow(2.1f, multiple) : 10f;
//                power = Mathf.FloorToInt(power);
//                //charMerge_.Power = power;
//                //charMerge_.ChangeMeshByPower();
//                maxPower += power;
//            }
//            DestroyImmediate(CharacterMerge[i].gameObject);
//        }
//        Debug.Log("maxPower" + maxPower);
//        while (maxPower > 10)
//        {
//            CheckCanSpawnBoss();
//        }
//        var player = PrefabUtility.InstantiatePrefab(PlayerPack, parent.transform) as GameObject;
//    }
//    private void CheckCanSpawnBoss()
//    {
//        var TempMax = 0;
//        for (int i = 0; i < 26; i++)
//        {
//            var check = i > 1 ? 10f * Mathf.Pow(2.1f, i) : 10f;
//            if (check < maxPower)
//            {
//                TempMax = Mathf.FloorToInt(check);
//            }
//            else
//            {
//                break;
//            }
//        }
//        maxPower -= TempMax;
//        //Boss boss = PrefabUtility.InstantiatePrefab(BossPack, bossPosition.transform.parent) as Boss;
//        //boss.transform.localPosition = bossPosition.transform.localPosition;
//        //boss.transform.localPosition += new Vector3(Random.Range(-3f, 3f), 0, 0);
//        //boss.CurPower = TempMax;
//        //boss.ChangeMeshByPower();
//        //levelManager.Boss.Add(boss as Character);
//        Debug.Log("Spawn boss" + TempMax);
//    }
//}
//public class ReplaceMesh : EditorWindow
//{
//    [MenuItem("Tools/Replace Mesh Door")]
//    private static void Init()
//    {
//        GetWindow<ReplaceMesh>("Replace Mesh Door").Show();
//    }
//    public List<GameObject> ListObject = new List<GameObject>();
//    //public Mesh meshOld;
//    public Mesh meshReplace;
//    private void OnGUI()
//    {
//        if (GUILayout.Button("Replace Mesh Door"))
//            ReplaceMeshs();

//        var so = new SerializedObject(this);
//        var triggerWall_ = so.FindProperty(nameof(ListObject));
//        EditorGUILayout.PropertyField(triggerWall_, true);
//        //var meshOld_ = so.FindProperty(nameof(meshOld));
//        //EditorGUILayout.PropertyField(meshOld_, true);
//        var meshReplace_ = so.FindProperty(nameof(meshReplace));
//        EditorGUILayout.PropertyField(meshReplace_, true);
//        so.ApplyModifiedProperties();
//    }
//    [MenuItem("Tools/Replace Mesh")]
//    private void ReplaceMeshs()
//    {
//        ListObject = new List<GameObject>();
//        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
//        {
//            if (gameObj.GetComponent<MeshFilter>() != null)
//            {
//                if (gameObj.name == "Right" || gameObj.name == "Left")
//                {
//                    ListObject.Add(gameObj);
//                }
//            }
//        }
//        for (int i = 0; i < ListObject.Count; i++)
//        {
//            var obj = ListObject[i];
//            obj.GetComponent<MeshFilter>().sharedMesh = meshReplace;
//            obj.transform.localScale -= new Vector3(0.5f, 0, 0);
//            if(obj.name == "Left")
//            {
//                obj.transform.localEulerAngles = new Vector3(0, 90, 0);
//            }
//        }
//    }

//}
public static class CopyPasteTransformComponent
{
    struct TransformData
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;

        public TransformData(Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
        {
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            this.localScale = localScale;
        }
    }

    private static TransformData _data;
    private static Vector3? _dataCenter;

    [MenuItem("Edit/Copy Transform Values &c", false, -101)]
    public static void CopyTransformValues()
    {
        if (Selection.gameObjects.Length == 0) return;
        var selectionTr = Selection.gameObjects[0].transform;
        _data = new TransformData(selectionTr.localPosition, selectionTr.localRotation, selectionTr.localScale);
    }

    [MenuItem("Edit/Paste Transform Values &v", false, -101)]
    public static void PasteTransformValues()
    {
        foreach (var selection in Selection.gameObjects)
        {
            Transform selectionTr = selection.transform;
            Undo.RecordObject(selectionTr, "Paste Transform Values");
            selectionTr.localPosition = _data.localPosition;
            selectionTr.localRotation = _data.localRotation;
            selectionTr.localScale = _data.localScale;
        }
    }

    [MenuItem("Edit/Copy Center Position &k", false, -101)]
    public static void CopyCenterPosition()
    {
        if (Selection.gameObjects.Length == 0) return;
        var render = Selection.gameObjects[0].GetComponent<Renderer>();
        if (render == null) return;
        _dataCenter = render.bounds.center;
    }

    [MenuItem("Edit/Paste Center Position &l", false, -101)]
    public static void PasteCenterPosition()
    {
        if (_dataCenter == null) return;
        foreach (var selection in Selection.gameObjects)
        {
            Undo.RecordObject(selection.transform, "Paste Center Position");
            selection.transform.position = _dataCenter.Value;
        }
    }
}
#endif
