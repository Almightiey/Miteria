using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour, ITickUpdate
{
    public LayerMask layerSavePoint;
    public Text textCoinsLink;
    private static Text textCoins;
    public static Coins coins;
    public AudioClip questStart;
    public AudioSource clickAudio;
    public GameObject loadObject;
    public DialogParameters dialogParameters;
    public RuneParameters runeParameters;
    public static RuneManager runeManager;


    public static Coroutine routine;
    public static GameObject gameManagerObject;
    public static Player player;
    public static Transform respawnPlayerPosition;
    public static bool isPlayerStop;
    public static bool isInvertoryOpen;
    public static ShakeCamera shakeCamera;

    public static MainScipt mainObjects { get; set; }


    public delegate void EventGame();
    public static event EventGame GameOver;
    public static event EventGame endGame;

    private void Awake()
    {
        gameManagerObject = gameObject;
        mainObjects = new MainScipt();
        mainObjects.playerActor = FindObjectOfType<PlayerActor>();


        mainObjects.playerActor.OnAwake();
        player.FlipIsGround += mainObjects.playerActor.effectsPlayer.DustPlay;
        mainObjects.playerActor.hitDamage.GameOver += _GameOver;



        respawnPlayerPosition = new GameObject("[Respawn Position]").transform;
        respawnPlayerPosition.position = player.transform.position;


        UpdateManager.ticks.Add(this);
        var dialogueManager = new DialogueManager(dialogParameters);
        mainObjects.dialogueManager = dialogueManager;
        coins = new Coins();
        textCoins = textCoinsLink;
        textCoins.text = "Cold: " + coins.Golds.ToString();

        QuestManager questManager = new QuestManager(questStart);
        mainObjects.questManager = questManager;
        KeyManager keyManager = new KeyManager();
        mainObjects.keyManager = keyManager;
        runeManager = new RuneManager(runeParameters);

        
    }

    public static void ActiveRune()
    {
        runeManager.Active();
    }


    public void OnUpdate()
    {
        isInvertoryOpen = mainObjects.invertory.backGround.activeSelf;
        player.isStop = isInvertoryOpen || !mainObjects.dialogueManager.isEnd || isPlayerStop || Input.GetKey(KeyCode.E);
        var isSavePoint = Physics2D.OverlapCircle(player.transform.position, 1.0f, layerSavePoint);
        if (isSavePoint)
            respawnPlayerPosition.position = player.transform.position;


        if (Input.GetKeyDown(KeyCode.P))
            AddCoins(this, 500);
    }



    public static void AddCoins(object sender, int value)
    {
        coins.Add(value);
        textCoins.text = "Cold: " + coins.Golds.ToString();
    }


    public static void SpendCoins(object sender, int value)
    {
        coins.Spend(value);
        textCoins.text = "Gold " + coins.Golds.ToString();
    }

    public void Replay()
    {
        clickAudio.Play();
        EndGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void Exit()
    {
        clickAudio.Play();
        EndGame();
        Time.timeScale = 1;
        loadObject.SetActive(true);
    }


    private void _GameOver()
    {
        GameOver?.Invoke();
        if (runeManager.activeRune != null)
        {
            runeManager.activeRune.runeActive -= ActiveRune;
            runeManager.isNewActiveRune = true;
        }
        mainObjects.playerActor.hitDamage.GameOver -= _GameOver;
        Coroutines.StopAllRoutine();
        Coroutines.StartRoutine(Death());
    }

    public void EndGame()
    {
        endGame?.Invoke();
        UpdateManager.ticks.Remove(this);
        if (runeManager.activeRune != null)
        {
            runeManager.activeRune.runeActive -= ActiveRune;
            runeManager.isNewActiveRune = true;
        }
        var fastGlitch = mainObjects.UIplayerController.fastGlitch;
        fastGlitch.ChromaticGlitch = 0.0f;
        fastGlitch.FrameGlitch = 0.0f;
        fastGlitch.PixelGlitch = 0.0f;
        Coroutines.StopAllRoutine();
    }

    public static GameObject CreateObject(GameObject original, Transform transformObject)
    {
        var gameObject = Instantiate(original, transformObject);
        return gameObject;
    }

    public static void Delete(GameObject game)
    {
        Destroy(game);
    }

    private IEnumerator Death()
    {
        var uIPlayer = mainObjects.UIplayerController;
        uIPlayer.audioDeath.Play();
        uIPlayer.fastGlitch.ChromaticGlitch = 0.6f;
        uIPlayer.fastGlitch.FrameGlitch = 0.7f;
        uIPlayer.fastGlitch.PixelGlitch = 1.2f;
        player.rb.velocity = Vector2.zero;
        player.animator.SetBool(nameof(player.isDeath), player.isDeath);
        yield return new WaitForSeconds(2.5f);
        Coroutines.StopAllRoutine();
        EndGame();
    }
}
