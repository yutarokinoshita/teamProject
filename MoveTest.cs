using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float Life = 500f;

    // ここに格納したオブジェクトを呼び出す
    public GameObject BombPrefab;

    public string targetTag;

    private Vector3 moveDirection = Vector3.zero;

    private float angle = 180;
    private bool inputNow = false;
    private bool inputOld = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 接地時のみ移動を許可する
        if (characterController.isGrounded)
        {
            // キー入力で移動
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            // スペースキーでジャンプ
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
            }

        }
        // ジャンプ・移動処理
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        // 向き変え
        if (Input.GetKey(KeyCode.UpArrow))
        {
            angle = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            angle = 90;
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            angle = 180;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angle = 270;
        }
        transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));

        // 爆弾設置
        if (Input.GetKey(KeyCode.Z))
        {
            inputNow = true;
        }
        else
        {
            inputNow = false;
        }

        // 前フレームキー未入力時処理
        if (inputNow && !inputOld)
        {
            //Bomb.transform.position = transform.position;
            // 接地時の場合処理を行う
            if (characterController.isGrounded)
            {
                Bom();
            }
        }
        inputOld = inputNow;


    }

    // 爆弾設置
    public void Bom()
    {
        // プレイヤーと同じ座標にオブジェクトを設置
        Vector3 pos = transform.position;
        pos.x = Mathf.RoundToInt(transform.position.x);
        pos.z = Mathf.RoundToInt(transform.position.z);
        GameObject Bomb = Instantiate(
            BombPrefab,
            //transform.position,
            pos,
            Quaternion.identity
            );
    }

    // すり抜け判定
    void OnTriggerStay(Collider other)
    {
        // ライフ(仮)が0より上の場合
        if (Life > 0)
        {
            // 相手のタグがtargetTagと同じ場合のみ処理を行う
            if (other.gameObject.tag == targetTag)
            {
                Debug.Log("当たってる");
                Life--;
                Debug.Log(Life);
            }
        }
    }

    // 当たり判定
    void OnTriggerEnter(Collider other)
    {
        // 相手のタグがtargetTagと同じ場合のみ処理を行う
        if (other.gameObject.tag == targetTag)
        {
            Debug.Log("当たった");
        }
    }
}
