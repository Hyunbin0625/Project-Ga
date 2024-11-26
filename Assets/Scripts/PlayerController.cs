using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;     // 사망시 재생할 오디오 클립
    public float jumpForce = 700f;  // 점프 힘

    private int jumpCount = 0;      // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isMoved = false;
    private bool isDead = false;    // 사망 상태
    public float speed = 8f;

    private Rigidbody playerRigidbody;  // 사용할 리지드바디 컴포넌트
    private Animator animator;          // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio;    // 사용할 오디오 소스 컴포넌트

    public float invincibilityDuration = 2f; // 무적 지속 시간 (초)
    private bool isInvincible = false;       // 무적 상태 여부
    public bool GetIsInvincible() { return isInvincible; }
    private Renderer playerRenderer;        // 플레이어의 Renderer
    private int playerLayer;                // 레이어, Player
    private int monsterLayer;               // 레이어, Monster

    public bool isBossZone;             // player가 bossZone에 들어갔을 때
    public float smoothTime = 0.1f;     // 부드럽게 전환되는 시간
    private Vector3 currentVelocity;    // 현재 속도 (SmoothDamp용)

    private HPManager playerHPManager;
    public PlatformSpawner platformSpawner; // PlatformSpawner 스크립트를 참조

    private void Start()
    {
        // 초기화
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerRenderer = GetComponent<Renderer>();          // 플레이어의 Renderer 컴포넌트 가져오기 (시각적 효과를 위해)
        playerLayer = LayerMask.NameToLayer("Player");      // 레이어, Player
        monsterLayer = LayerMask.NameToLayer("Monster");    // 레이어. Monster

        playerHPManager = GetComponent<HPManager>();
    }

    private void Update()
    {
        if (isDead)
            return;

        if (isBossZone)
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X") * speed, 0f, Space.Self);
            //transform.Rotate(-Input.GetAxis("Mouse Y") * speed, 0f, 0f);

            // Player 이동 x축
            // 수평축의 입력값을 감지하여 저장
            float horizontalInput = Input.GetAxis("Horizontal");    // 수평(x축)
            float verticalInput = Input.GetAxis("Vertical");        // 수직(y축)

            // 수평축의 입력값이 있는지 확인하여 isMoved 설정
            isMoved = !Mathf.Approximately(horizontalInput, 0) || !Mathf.Approximately(verticalInput, 0);

            // 로컬 x축 방향을 기준으로 이동 벡터 계산 (회전된 방향 반영)
            Vector3 moveDirection = (transform.right * horizontalInput + transform.forward * verticalInput) * speed;

            // 기존 y축 속도는 유지하면서 x와 z축 방향 이동 설정
            //Vector3 newVelocity = new Vector3(moveDirection.x, playerRigidbody.velocity.y, moveDirection.z);
            Vector3 newVelocity = new Vector3(moveDirection.x, playerRigidbody.velocity.y, moveDirection.z);


            // 플레이어의 이동 방향에 따라 스프라이트 반전 설정
            if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Rigidbody.velocity
            playerRigidbody.velocity = newVelocity;
        }
        else
        {
            // Player 이동 x축
            // 수평축의 입력값을 감지하여 저장
            float horizontalInput = Input.GetAxis("Horizontal");    // 수평(x축)

            // 수평축의 입력값이 있는지 확인하여 isMoved 설정
            isMoved = !Mathf.Approximately(horizontalInput, 0);     // 수평(x축)

            // 로컬 x축 방향을 기준으로 이동 벡터 계산 (회전된 방향 반영)
            Vector3 moveDirection = transform.right * horizontalInput * speed;
            moveDirection.y = playerRigidbody.velocity.y;

            // 기존 y축 속도는 유지하면서 x와 z축 방향 이동 설정
            //Vector3 newVelocity = new Vector3(moveDirection.x, playerRigidbody.velocity.y, moveDirection.z);
            Vector3 newVelocity = Vector3.SmoothDamp(playerRigidbody.velocity, moveDirection, ref currentVelocity, smoothTime);

            // 플레이어의 이동 방향에 따라 스프라이트 반전 설정
            if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Rigidbody.velocity
            playerRigidbody.velocity = newVelocity;
        }


        // Player 점프    
        // 사용자 입력을 감지하고 점프하는 처리
        if (Input.GetKeyDown("space") && jumpCount < 2)
        {
            // 점프 횟수 증가
            ++jumpCount;
            isGrounded = false;

            // 점프 직전에 속도를 순간적으로 (0, 0)
            // new Vector2(0, 0)
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);

            // 리지드바디에 위쪽으로 힘 추가
            playerRigidbody.AddForce(new Vector3(0, jumpForce));

            // 오디오 소스 재생
            playerAudio.Play();
        }

        // 상태 값에 따라 Animation 적용
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Moved", isMoved);
    }

    public void Die()
    {
        // 사망 처리
        isDead = true;
        animator.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);
        playerRigidbody.velocity = Vector2.zero;  // 플레이어의 물리적 속도를 (0, 0)
        playerRigidbody.isKinematic = true;
        //playerRigidbody.position = new Vector3(-6, 1, 0);   // 물리적인 상호작용(중력, 충돌 등)을 비활성화

        GameManager.instance.OnPlayerDead();
    }

    private IEnumerator ActivateInvincibility()
    {
        isInvincible = true; // 무적 상태 활성화

        // 무적 시각적 효과 (깜빡임 효과)
        for (float i = 0; i < invincibilityDuration; i += 0.2f)
        {
            playerRenderer.enabled = !playerRenderer.enabled; // 깜빡임 효과
            yield return new WaitForSeconds(0.2f);            // 0.2초 간격으로 깜빡임
        }
        playerRenderer.enabled = true; // 무적 해제 시 원래 상태로 복구

        isInvincible = false; // 무적 상태 해제

        // 충돌 무시
        Physics.IgnoreLayerCollision(playerLayer, monsterLayer, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.gameObject.name == "Deadzone" && !isDead)
        {
            Die();
        }
        if (other.gameObject.name == "Bosszone" && !isDead)
        {
            isBossZone = true;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>().isBossView = true;
            platformSpawner.RemoveAllPlatforms();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster") && !isInvincible && !isDead)
        {
            playerHPManager.TakeDamage(1);

            Collider monsterCollider = collision.collider;

            // 플레이어와 몬스터 간 충돌 비활성화
            Physics.IgnoreLayerCollision(playerLayer, monsterLayer, true);

            StartCoroutine(ActivateInvincibility()); // 무적 상태 활성화 코루틴 시작

            // 플레이어가 몬스터와 충돌했을 때 뒤로 밀리게 하는 힘 추가
            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;

            Vector3 moveDirection = transform.right * knockbackDirection.x * speed;  // 방향에 따른 x, z값 구하기

            float knockbackForce = 700f; // 뒤로 밀리는 힘의 크기
            knockbackDirection = new Vector3(moveDirection.x, 1f, moveDirection.z) * knockbackForce; // X축으로만 움직이도록 Y, Z값을 0으로 설정

            playerRigidbody.AddForce(knockbackDirection);

        }
        else 
        {
            if (collision.contacts[0].normal.y > 0.7f)   // 충돌한 것 중 첫 번째,  0.7f는 각도
            {
                // 바닥에 닿았음을 감지하는 처리
                isGrounded = true;
                jumpCount = 0;
            }

            
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        // 몬스터와 충돌한 경우
        if (collision.gameObject.CompareTag("Monster"))
        {
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            // 몬스터와 충돌을 벗어난 경우
        }
        else
        {

        }
    }
}