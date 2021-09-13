using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class bird : MonoBehaviour
{
    private bool isClick = false;
    public float maxDis = 3;
    [HideInInspector]
    public SpringJoint2D sp;
    protected Rigidbody2D rg;

    public LineRenderer right;
    public Transform rightPos;
    public LineRenderer left;
    public Transform leftPos;

    public GameObject boom;

    protected TestMyTrail myTrail;
    [HideInInspector]
    public bool canMove = false;
    public float smooth = 3;

    public AudioClip select;
    public AudioClip fly;

    private bool isFly = false;
    [HideInInspector]
    public bool isReleased = false;

    public Sprite hurt;
    protected SpriteRenderer render;

    public void Awake()
    {
        sp = GetComponent<SpringJoint2D>();
        rg = GetComponent<Rigidbody2D>();
        myTrail = GetComponent<TestMyTrail>();
        render = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (canMove)
        {
            AudioPlay(select);
        isClick = true;
        rg.isKinematic = true;
        }     
    }

    private void OnMouseUp()
    {
        if (canMove)
        {
        isClick = false;
        rg.isKinematic = false;
        Invoke("Fly", 0.1f);
        right.enabled = false;
        left.enabled = false;
        canMove = false;
        }       
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (isClick)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += new Vector3(0, 0, -Camera.main.transform.position.z);
            if (Vector3.Distance(transform.position, rightPos.position) > maxDis)
            {
                Vector3 pos = (transform.position - rightPos.position).normalized;
                pos *= maxDis;
                transform.position = pos + rightPos.position;
            }
            Line();
        }

        float posX = transform.position.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,new Vector3(Mathf.Clamp(posX,0,10),Camera.main.transform.position.y,
        Camera.main.transform.position.z),smooth*Time.deltaTime);

        if (isFly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShowSkill();
            }
        }
    }

    void Fly()
    {
        isReleased = true;
        isFly = true;
        AudioPlay(fly);
        myTrail.StartTrails();
        sp.enabled = false;
        Invoke("Next", 5);
    }

    void Line()
    {
        right.enabled = true;
        left.enabled = true;

        right.SetPosition(0, rightPos.position);
        right.SetPosition(1, transform.position);

        left.SetPosition(0, leftPos.position);
        left.SetPosition(1, transform.position);
    }

    protected virtual void Next()
    {
        gamemanager._instance.birds.Remove(this);
        Destroy(gameObject);
        Instantiate(boom, transform.position, Quaternion.identity);
        gamemanager._instance.NextBird();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFly = false;
        myTrail.ClearTrails();
        //render.sprite = hurt;
    }

    public void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip,transform.position);
    }

    public virtual void ShowSkill()
    {
        isFly = false;

    }

    public void Hurt()
    {
        render.sprite = hurt;
    }
}
