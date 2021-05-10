using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class Editor : MonoBehaviour
{
    [SerializeField] private GameObject m_cubeVisual;
    [SerializeField] private GameObject m_cubeVisualRed;
    [SerializeField] private GameObject m_coin;
    [SerializeField] private GameObject m_ball;
    [SerializeField] private GameObject m_ballSpawn;
    [SerializeField] private GameObject m_timer;
    
    [SerializeField] private GameObject m_targetA;
    [SerializeField] private GameObject m_targetA2;
    [SerializeField] private GameObject m_targetB;
    [SerializeField] private GameObject m_targetB2;

    [SerializeField] private GameObject m_backgroundA;
    [SerializeField] private GameObject m_backgroundB;

    [SerializeField] private GameObject m_floorA;
    [SerializeField] private GameObject m_floorB;

    [SerializeField] private Text m_textEdit;
    [SerializeField] private Text m_textScore;


    private GameObject m_target;
    private GameObject m_target2;


    private List<GameObject> m_list = new List<GameObject>();
    private List<Vector3> m_list2 = new List<Vector3>();

    private Vector3 m_timerPos;
    private Vector3 m_timerScale;

    private Vector3 m_ballSpawnCoor;

    private bool isEditor = true;

    private bool isSnow = true;

    private bool isCoinClick = false;
    private bool isBallClick = false;

    private bool isLeftClick = false;
    private bool isRightClick = false;

    // Start is called before the first frame update
    void Start()
    {
        m_ballSpawnCoor = m_ball.transform.position;
        m_timerPos = m_timer.transform.position;
        m_timerScale = m_timer.transform.localScale;
        m_target = m_targetA;
        m_target2 = m_targetA2;
        SwapEditor();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEditor)
            ModeEditor();
        else
            ModePlay();
    }

    void ModeEditor()
    {
        MoveVisualCube();
        m_timer.transform.position = m_timerPos;
        m_timer.transform.localScale = m_timerScale;

        if (isCoinClick)
        {
            MoveCoin();
        }
        else if (isBallClick)
        {
            MoveBall();
        }
        else
        {
            m_ball.transform.position = new Vector3(0f, -30f, 0f);

            InputBool();
            InputTest();
        }
    }

    void MoveCoin()
    {
        m_coin.transform.position = m_cubeVisual.transform.position;
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_ball.GetComponent<BallMovement>().DestroyFocus();
            m_ball.GetComponent<BallMovement>().isClicked = false;
            m_cubeVisualRed.transform.position = new Vector3(-20f, 0f);
            isCoinClick = false;
            isLeftClick = false;
        }
    }

    void MoveBall()
    {
        m_ballSpawn.transform.position = m_cubeVisual.transform.position;
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_ballSpawnCoor = m_ballSpawn.transform.position;
            m_ball.GetComponent<BallMovement>().DestroyFocus();
            m_ball.GetComponent<BallMovement>().isClicked = false;
            m_cubeVisualRed.transform.position = new Vector3(-20f, 0f);
            isBallClick = false;
            isLeftClick = false;
        }
    }

    void ModePlay()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SwapEditor();
    }

    [HideInInspector] public void SwapEditor()
    {
        m_textEdit.text = "L-Click : Add Block / R-Click : Delete Block     |     Drag&Drop : Move Ball & Coin     |     C : Change biome\nR : Reset Map     |     E : Swap Mode (Edition/Play)     |     S : Save Map / L : Load Map";
        Cursor.visible = false;
        isEditor = true;
        m_ballSpawn.transform.position = m_ballSpawnCoor;
        m_ball.transform.position = new Vector3(0f, -30f, 0f);
    }

    void SwapPlay()
    {
        m_textEdit.text = "";
        m_textScore.text = "";
        Cursor.visible = true;
        isEditor = false;
        m_ball.transform.position = m_ballSpawnCoor;
        m_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_ballSpawn.transform.position = new Vector3(0f, -30f, 0f);
        m_cubeVisual.transform.position = new Vector3(0f, -30f, 0f);
        m_timer.transform.position = m_timerPos;
        m_timer.transform.localScale = m_timerScale;
        BallMovement.m_lastShoots = 0;
        Timer.m_lastTime = 0f;
    }

    void MoveVisualCube()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 11.0f; //distance of the plane from the camera
        screenPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        screenPoint.x = (int)screenPoint.x;
        screenPoint.y = (int)screenPoint.y + .5f;
        screenPoint.z = 1f;

        if (screenPoint.x < -11f)
            screenPoint.x = -11f;
        else if (screenPoint.x > 11f)
            screenPoint.x = 11f;
        if (screenPoint.y < 0.5f)
            screenPoint.y = 0.5f;
        else if (screenPoint.y > 9.5f)
            screenPoint.y = 9.5f;

        if (isBallClick || isCoinClick)
        {
            foreach (Vector3 v in m_list2)
            {
                if (screenPoint == v)
                {
                    m_cubeVisualRed.transform.position = screenPoint;
                    return;
                }
            }
            m_cubeVisualRed.transform.position = new Vector3(-20f, 0f);
        }

        m_cubeVisual.transform.position = screenPoint;
    }

    bool isBelow(Vector3 v, Vector3 v2)
    {
        if (v.x == v2.x && v.y - 1f == v2.y)
            return true;
        else
            return false;
    }

    bool isAbove(Vector3 v, Vector3 v2)
    {
        if (v.x == v2.x && v.y + 1f == v2.y)
            return true;
        else
            return false;
    }

    void testAbove(Vector3 i)
    {
        int j = 0;
        foreach (Vector3 v in m_list2)
        {
            if (isAbove(v, i))
            {
                CreateCube(v, m_target2);
                DeleteCube(j);
                return;
            }
            j++;
        }
    }

    void CreateCube(Vector3 i)
    {
        foreach (Vector3 v in m_list2)
        {
            if (v == i)
                return;
        }

        testAbove(i);

        foreach (Vector3 v in m_list2)
        {
            if (isBelow(v, i))
            {
                m_list.Add(Instantiate(m_target2, i, m_cubeVisual.transform.rotation));
                m_list2.Add(i);
                return;
            }
        }
        m_list.Add(Instantiate(m_target, i, m_cubeVisual.transform.rotation));
        m_list2.Add(i);
    }
    void CreateCube(Vector3 i, GameObject obj)
    {
        m_list.Add(Instantiate(obj, i, m_cubeVisual.transform.rotation));
        m_list2.Add(i);
    }
    void DeleteCube(int i)
    {
        int j = 0;
        foreach (Vector3 v in m_list2)
        {
            if (isAbove(m_list2[i], v))
            {
                CreateCube(v, m_target2);
                DeleteCube(j);
                break;
            }
            j++;
        }

        Destroy(m_list[i]);

        m_list.RemoveAt(i);
        m_list2.RemoveAt(i);
    }

    void CubeBehind(Vector3 v)
    {
        int j = 0;
        foreach (Vector3 v2 in m_list2)
        {
            if (v == v2)
            {
                Destroy(m_list[j]);

                m_list.RemoveAt(j);
                m_list2.RemoveAt(j);

                CreateCube(v, m_target);
                return;
            }
            j++;
        }
    }

    void ChangeLevel()
    {
        ResetEditor();
        isSnow = !isSnow;

        if (isSnow)
        {
            Wind.DestroyAll();
            m_backgroundA.transform.position = new Vector3(m_backgroundA.transform.position.x, m_backgroundA.transform.position.y, 200f);
            m_backgroundB.transform.position = new Vector3(m_backgroundB.transform.position.x, m_backgroundB.transform.position.y, 300f);
            m_floorA.transform.position = new Vector3(m_floorA.transform.position.x, 0f , m_floorA.transform.position.z);
            m_floorB.transform.position = new Vector3(m_floorB.transform.position.x, -2f , m_floorB.transform.position.z);
            m_target = m_targetA;
            m_target2 = m_targetA2;
        }
        else
        {
            m_backgroundB.transform.position = new Vector3(m_backgroundB.transform.position.x, m_backgroundB.transform.position.y, 250f);
            m_backgroundA.transform.position = new Vector3(m_backgroundA.transform.position.x, m_backgroundA.transform.position.y, 300f);
            m_floorB.transform.position = new Vector3(m_floorB.transform.position.x, 0f, m_floorB.transform.position.z);
            m_floorA.transform.position = new Vector3(m_floorA.transform.position.x, -2f, m_floorA.transform.position.z);
            m_target = m_targetB;
            m_target2 = m_targetB2;
        }
    }

    void ResetEditor()
    {
        foreach (GameObject g in m_list)
        {
            Destroy(g);
        }
        m_list.Clear();
        m_list2.Clear();

        m_coin.transform.position = new Vector3(-10f, 4.5f, 1f);

        m_ballSpawn.transform.position = new Vector3(-10f, 1.5f, 1f);
        m_ballSpawnCoor = m_ballSpawn.transform.position;
    }

    void InputTest()
    {
        if (isLeftClick)
        {
            if (m_cubeVisual.transform.position == m_coin.transform.position)
            {
                isCoinClick = true;
                isLeftClick = false;
                return;
            }
            else if (m_cubeVisual.transform.position == m_ballSpawn.transform.position)
            {
                isBallClick = true;
                isLeftClick = false;
                return;
            }

            CreateCube(m_cubeVisual.transform.position);
        }

        if (isRightClick)
        {
            int i = 0;
            foreach (Vector3 v in m_list2)
            {
                if (v == m_cubeVisual.transform.position)
                {
                    Destroy(m_list[i]);

                    m_list.RemoveAt(i);
                    m_list2.RemoveAt(i);

                    CubeBehind(new Vector3(m_cubeVisual.transform.position.x, m_cubeVisual.transform.position.y - 1f, m_cubeVisual.transform.position.z));
                    return;
                }
                i++;
            }
        }
    }

    void InputBool()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            isLeftClick = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isLeftClick = false;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            isRightClick = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isRightClick = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeLevel();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetEditor();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapPlay();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLevel();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel();
        }
    }


    void SaveLevel()
    {
        string path = "Assets/Save/MyLevel.xml";

        System.IO.File.WriteAllText(path, string.Empty);

        StreamWriter writer = new StreamWriter(path, true);

        if(isSnow)
            writer.WriteLine("snow");
        else
            writer.WriteLine("sand");

        writer.WriteLine(m_ballSpawn.transform.position.x);
        writer.WriteLine(m_ballSpawn.transform.position.y);
        writer.WriteLine(m_coin.transform.position.x);
        writer.WriteLine(m_coin.transform.position.y);

        foreach (Vector3 v in m_list2)
        {
            writer.WriteLine(v.x);
            writer.WriteLine(v.y);
        }

        writer.WriteLine("exit");
        writer.Close();
    }

    void LoadLevel()
    {
        ResetEditor();

        string path = "Assets/Save/MyLevel.xml";

        StreamReader reader = new StreamReader(path);

        string line = reader.ReadLine();
        float x, y;
        int i = 0;

        if((!isSnow && line == "snow") || (isSnow && line == "sand"))
            ChangeLevel();

        line = reader.ReadLine();

        while (line != "exit")
        {
            x = float.Parse(line);
            line = reader.ReadLine();
            y = float.Parse(line);

            if (i == 0)
                m_ballSpawn.transform.position = new Vector3(x, y, 1f);
            else if (i == 1)
                m_coin.transform.position = new Vector3(x, y, 1f);
            else
                CreateCube(new Vector3(x, y, 1f));
            i++;
            line = reader.ReadLine();
        }

        reader.Close();

        m_ballSpawnCoor = m_ballSpawn.transform.position;
    }
}