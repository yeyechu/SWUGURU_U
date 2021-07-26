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
   네임스페이스 추가 : using UnityEngine.UI;
 
    enum()
     게임 상태상수 선언
     ★ enum에서 선언해준 상수는 반드시 동일한 형태로 선언해야함

    ★ 싱글톤으로 

     필요속성 : 게임상태변수(enum), UI텍스트변수, 게임매니저변수
     필요속성 : Player게임오브젝트, PlayerMove변수,

     Awake()
      1. 게임매니저값이 없다면 this로 설정

     Start()
      1. 초기 상태는 준비 상태로 설정
      2. GameStart 코루틴 시작
      3. hp값이 저장되어 있는 PlayerMove.cs의 설정을 받아옴

    IEnumerator GameStart() : 자주 사용되기 때문에 코루틴 설정
      1. 문구출력 : Ready
      2. 글자 색상은 주황색으로 표시
      3. 2초 대기
      4. 문구변경 : Go! 
      5. 0.5초 대기
      6. 문구삭제
      7. 게임 상태 Ready → Run

     Update()
      1. if Player의 hp가 0이하라면
      2.   문구출력 : Game Over...
      3.   문구색상 : 빨간색
      4.   게임상태 : Run → GameOver

 */