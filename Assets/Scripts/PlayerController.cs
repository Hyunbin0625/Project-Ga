using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
    public AudioClip deathClip; // 사망시 재생할 오디오 클립
    public float jumpForce = 700f; // 점프 힘
    
    private int jumpCount = 0; // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isMoved = false;
    private bool isDead = false; // 사망 상태
    public float speed = 8f;
    
    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트
    
    private void Start() {
        // 초기화
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }
    
    private void Update() {
        if (isDead)
            return;
    

        // Player 이동 x축
        // 수평축의 입력값을 감지하여 저장
        float xInput = Input.GetAxis("Horizontal");     // 수평(x축)

        // 수평축의 입력값이 있는지 확인하여 isMoved 설정
        isMoved = !Mathf.Approximately(xInput, 0);     // 수평(x축)

        // 실제 이동 속도를 입력값과 이동 속력을 사용해 설정
        float xSpeed = xInput * speed;
        
        // 플레이어의 이동 방향에 따라 스프라이트 반전 설정
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        // Vector2 속도를 (xSpeed, 원래 ySpeed) 설정
        Vector2 newVelocity = new Vector2(xSpeed, playerRigidbody.velocity.y);
        
        // Rigidbody.velocity
        playerRigidbody.velocity = newVelocity;
    

        // Player 점프
        // 사용자 입력을 감지하고 점프하는 처리
        if (Input.GetKeyDown("space") && jumpCount < 2)
        {
            // 점프 횟수 증가
            ++jumpCount;
            
            // 점프 직전에 속도를 순간적으로 (0, 0)
            // new Vector2(0, 0)
            playerRigidbody.velocity = Vector2.zero;
            
            // 리지드바디에 위쪽으로 힘 추가
            playerRigidbody.AddForce(new Vector2 (0, jumpForce));
            
            // 오디오 소스 재생
            playerAudio.Play();
        }
    
        // 상태 값에 따라 Animation 적용
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Moved", isMoved);
    }
    
    private void Die() {
        // 사망 처리
        animator.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);
        //playerRigidbody.velocity = Vector2.zero;  // 플레이어의 물리적 속도를 (0, 0)
        //playerRigidbody.isKinematic = true;
        playerRigidbody.position = new Vector3(-6, 1, 0);   // 물리적인 상호작용(중력, 충돌 등)을 비활성화
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.gameObject.name == "Deadzone")
        {
            isDead = true;
            Die();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
         // 바닥에 닿았음을 감지하는 처리
         isGrounded = true;
         jumpCount = 0;
    }
    
    private void OnCollisionExit2D(Collision2D collision) {
         // 바닥에서 벗어났음을 감지하는 처리
         isGrounded = false;
    }
}