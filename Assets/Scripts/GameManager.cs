using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public enum GameState
    {
        Ready, Run, GameOver, Pause
    }

    public GameState gState;
    public Text stateLabel;
    public static GameManager gm;

    GameObject player;
    PlayerMove playerM;

    public GameObject optionUI;

    /*
    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }
    void Start()
    {
        gState = GameState.Ready;
        StartCoroutine(GameStart());
        player = GameObject.Find("Player");
        playerM = player.GetComponent<PlayerMove>();
    }
    IEnumerator GameStart()
    {
        stateLabel.text = "Ready...";
        stateLabel.color = new Color32(233, 182, 13, 255);
        yield return new WaitForSeconds(2.0f);
        stateLabel.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        stateLabel.text = "";
        gState = GameState.Run;
    }

    void Update()
    {
        if (playerM.hp <= 0)
        {
            stateLabel.text = "Game Over...";
            stateLabel.color = new Color32(255, 0, 0, 255);
            gState = GameState.GameOver;
        }
       
    
    }
*/

    public void OpenOptionWindow()
    {
        gState = GameState.Pause;
        Time.timeScale = 0;
        optionUI.SetActive(true);
    }

    public void CloseOptionWindow()
    {

        gState = GameState.Run;
        Time.timeScale = 1.0f;
        optionUI.SetActive(false);
    }

    public void GameRestart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    
}
/*
   ���ӽ����̽� �߰� : using UnityEngine.UI;
 
    enum()
     ���� ���»�� ����
     �� enum���� �������� ����� �ݵ�� ������ ���·� �����ؾ���

    �� �̱������� 

     �ʿ�Ӽ� : ���ӻ��º���(enum), UI�ؽ�Ʈ����, ���ӸŴ�������
     �ʿ�Ӽ� : Player���ӿ�����Ʈ, PlayerMove����,

     Awake()
      1. ���ӸŴ������� ���ٸ� this�� ����

     Start()
      1. �ʱ� ���´� �غ� ���·� ����
      2. GameStart �ڷ�ƾ ����
      3. hp���� ����Ǿ� �ִ� PlayerMove.cs�� ������ �޾ƿ�

    IEnumerator GameStart() : ���� ���Ǳ� ������ �ڷ�ƾ ����
      1. ������� : Ready
      2. ���� ������ ��Ȳ������ ǥ��
      3. 2�� ���
      4. �������� : Go! 
      5. 0.5�� ���
      6. ��������
      7. ���� ���� Ready �� Run

     Update()
      1. if Player�� hp�� 0���϶��
      2.   ������� : Game Over...
      3.   �������� : ������
      4.   ���ӻ��� : Run �� GameOver

 */