using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private static GameManager gameManager;

    [SerializeField] private Vector2[] path;
    [SerializeField] private int platformType;

    private float velocity = 2;
    private int index = 0;

    private Player attachedPlayer = null;
    private int from;

    [SerializeField] private Shader lineShader;
    private Color lineColor = Color.white;
    private float lineWidth = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Vector3 start = new Vector3(path[0].x, path[0].y, 2);
        Vector3 end = new Vector3(path[1].x, path[1].y, 2);
        DrawLine(start, end, lineColor);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.GetActiveType() == platformType) Move();
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject lineObject = new GameObject();
        lineObject.transform.position = start;
        lineObject.AddComponent<LineRenderer>();
        LineRenderer line = lineObject.GetComponent<LineRenderer>();
        line.material = new Material(lineShader);
        line.SetColors(color, color);
        line.SetWidth(lineWidth, lineWidth);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    private void Move()
    {
        int next = (index + 1) % path.Length;
        Vector2 position = transform.position;
        Vector2 direction = path[next] - position;
        if (Time.deltaTime * velocity >= direction.magnitude)
        {
            transform.position = path[next];
            index = next;
        }
        else
        {
            transform.Translate(direction.normalized * velocity * Time.deltaTime);
        }

        if (attachedPlayer != null)
        {
            MoveAttachedPlayer(position);
            Detach();
        }
    }

    private void MoveAttachedPlayer(Vector2 prevPos)
    {
        float delta;
        if (from == 0) // above
        {
            float dx = transform.position.x - prevPos.x;
            float dy = Mathf.Max(0, transform.position.y - prevPos.y);
            attachedPlayer.transform.Translate(dx, dy, 0);
        }
        else if (from == 1) // right
        {
            delta = Mathf.Max(0, transform.position.x - prevPos.x);
            attachedPlayer.transform.Translate(delta, 0, 0);
        }
        else if (from == 2) // below
        {
            delta = Mathf.Min(0, transform.position.y - prevPos.y);
            attachedPlayer.transform.Translate(0, delta, 0);
        }
        else // left
        {
            delta = Mathf.Min(0, transform.position.x - prevPos.x);
            attachedPlayer.transform.Translate(delta, 0, 0);
        }
    }

    public void Attach(Player player, int from)
    {
        if (attachedPlayer == null && gameManager.GetActiveType() == platformType)
        {
            Debug.Log("MovingPlatform received a player");
            attachedPlayer = player;
            this.from = from;
        }
    }

    private void Detach()
    {
        attachedPlayer = null;
    }
}
