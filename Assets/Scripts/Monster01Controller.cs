using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster01Controller : MonoBehaviour
{
    public enum Direction { Left, Right } // 방향을 나타내는 enum

    public float chaseDistance = 7f;  // 플레이어를 추격할 거리
    public float speed = 3f;           // 몬스터의 이동 속도
    public float idleTime = 1f;         // Idle 상태 유지 시간
    public float moveTime = 2f;         // Move 상태 유지 시간
    private float currentTime = 0f;     // 흐른 시간 저장

    private Transform player;           // 플레이어의 위치를 저장할 변수
    private Animator animator;          // 몬스터 애니메이션을 제어할 애니메이터 컴포넌트
    private new Rigidbody rigidbody;    // 사용할 리지드바디 컴포넌트
    private Animator anim;              // 사용할 애니메이션 컴포넌트

    private bool isChasing = false;    // 현재 몬스터가 추격 중인지 여부를 저장하는 변수
    private bool isMovingRandomly = false; // Move와 Idle 상태를 반복하는 상태인지 여부
    private bool isMoving = false;  // StopChasing의 상태에서 현재 움직이고 있는지
    private bool isInPlatformZone = false; // platformzone과 충돌 여부를 저장하는 변수
    private bool isAttack = false;  // 현재 몬스터가 공격 중인지 여부를 저장하는 변수

    private Direction currentDirection;   // 현재 이동 방향을 저장하는 변수


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  // "Player" 태그가 있는 게임 오브젝트의 위치를 가져옴
        animator = GetComponent<Animator>();                            // 애니메이터 컴포넌트를 가져옴
        rigidbody = GetComponent<Rigidbody>();                          // Rigidbody 컴포넌트를 가져옴
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);  // 몬스터와 플레이어 사이의 거리를 계산

        if (!isAttack && distanceToPlayer <= chaseDistance)  // 플레이어가 추격 거리 내에 있지만 공격 거리 밖에 있는지 확인
        {
            isMovingRandomly = false;   // 추격 상태가 아니니, false 초기화

            StartChasing();  // 추격 시작
        }
        else if (!isAttack)
        {
            StopChasing();  // 플레이어가 추격 거리 밖에 있으면 추격 중지
        }

        // 현재 애니메이션이 체크하고자 하는 애니메이션인지 확인
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Monster01_Attack") == true)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }


        if (isChasing && !isAttack)  // 추격 중이고 공격 중이 아닐 때만 추격
        {
            if (isInPlatformZone)   // 추적 상태에서 platformzone에 걸려 있을 때
            {
                Vector3 direction = (player.position - transform.position).normalized;  // 현재 몬스터 기준 플레이어 방향
                if (currentDirection == Direction.Left && direction.x > 0)      // 플레이어가 뛰어서 몬스터를 넘어갔을 때
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;    // 반대 방향으로 이동해 충돌 해제
                    animator.SetBool("Moved", true);  // 충돌 해제 후 플레이어를 추적하기 위해, 미리 애니메이션 셋팅
                }
                else if (currentDirection == Direction.Right && direction.x < 0)
                {
                    transform.position += Vector3.left * speed * Time.deltaTime;    // 반대 방향으로 이동해 충돌 해제
                    animator.SetBool("Moved", true);  // 충돌 해제 후 플레이어를 추적하기 위해, 미리 애니메이션 셋팅
                }
                else
                    animator.SetBool("Moved", false);  // 플레이어가 넘어가지 않은 경우, "isRunning" 애니메이션을 비활성화
            }
            else
            {
                ChasePlayer();  // 플레이어를 추격
            }
        }
    }

    void StartChasing()
    {
        isChasing = true;  // 추격 중 상태로 설정
    }

    void StopChasing()
    {
        isChasing = false;  // 추격 중 상태 해제

        RandomMoveIdleRoutine(); // Move/Idle 반복 시작
    }

    void ChasePlayer()
    {
        animator.SetBool("Moved", true);  // "isRunning" 애니메이션을 활성화

        Vector3 direction = (player.position - transform.position).normalized;  // 몬스터에서 플레이어로의 방향을 계산하고 정규화
        Vector3 moveDirection = transform.right * direction.x * speed;  // 방향에 따른 x, z값 구하기

        transform.position += new Vector3(moveDirection.x, 0, moveDirection.z) * Time.deltaTime;  // 방향을 따라 속도에 맞춰 몬스터 이동

        if (direction.x > 0)
        {
            currentDirection = Direction.Right;
            transform.localScale = new Vector3(1, 1, 1); // 오른쪽을 향하도록 스케일 변경
        }
        else
        {
            currentDirection = Direction.Left;
            transform.localScale = new Vector3(-1, 1, 1); // 왼쪽을 향하도록 스케일 변경
        }
    }

    private void RandomMoveIdleRoutine()
    {
        if (!isMovingRandomly)  // isMovingRandomly가 false 일때, 함수 처음 실행
        {
            isMovingRandomly = true;    // true을 저장해, 해당 if문 한번만 실행
            currentTime = 0f;           // 현재 시간 초기화
            isMoving = false;           // isMoving 값 초기화, Idle부터 실행
            SetRandomDirection(); // 미리 다음 방향을 설정
        }

        if (isMoving)
        {
            currentTime += Time.deltaTime;      // 흐른 시간 저장
            animator.SetBool("Moved", true);    // Move 상태 시작

            Vector3 moveDirection = transform.right * speed;

            if (currentDirection == Direction.Left) // 랜덤 방향, 왼쪽
            {
                transform.position += new Vector3(Vector3.left.x * moveDirection.x, 0, -moveDirection.z) * Time.deltaTime;    // 해당 방향으로 이동
                transform.localScale = new Vector3(-1, 1, 1); // 왼쪽을 향하도록 스케일 변경
            }
            else if (currentDirection == Direction.Right)   // 랜덤 방향, 오른쪽
            {
                transform.position += new Vector3(Vector3.right.x * moveDirection.x, 0, moveDirection.z) * Time.deltaTime;    // 해당 방향으로 이동
                transform.localScale = new Vector3(1, 1, 1); // 오른쪽을 향하도록 스케일 변경
            }

            if (currentTime > moveTime) // moveTime, 최대 시간보다 크다면
            {
                currentTime = 0f;   // 사용한 currentTime 초기화
                isMoving = false;   // 다음 순서인 Idle 실행을 위해, false 저장
                SetRandomDirection(); // 미리 다음 방향을 설정
            }
            else if (isInPlatformZone)   // 벽과 충돌 했을 때
            {
                currentTime = 0f;   // 사용한 currentTime 초기화
                isMoving = false;   // 다음 순서인 Idle 실행을 위해, false 저장
                // 벽과 충돌했을 때 반대 방향으로 이동하기 위해
                currentDirection = (Direction)((int)currentDirection ^ 1);  // XOR 연산을 사용해 0이면 1, 1이면 0
                //isInPlatformZone = false;
            }
        }
        else
        {
            currentTime += Time.deltaTime;      // 흐른 시간 저장
            animator.SetBool("Moved", false);   // Idle 상태 시작

            if (currentTime > idleTime) // idleTime, 최대 시간보다 크다면
            {
                currentTime = 0f;   // 사용한 currentTime 초기화
                isMoving = true;    // 다음 순서인 Move 실행을 위해, true 저장
            }
        }
    }

    void SetRandomDirection()
    {
        // 랜덤하게 방향을 설정
        currentDirection = (Random.value > 0.5f) ? Direction.Left : Direction.Right;
    }

    private void OnTriggerStay(Collider other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.gameObject.name == "Platformzone_L")
        {
            isInPlatformZone = true;
            if (!isChasing)
                transform.position += Vector3.right * speed * Time.deltaTime;    // 반대 방향으로 이동
        }
        else if (other.gameObject.name == "Platformzone_R")
        {
            isInPlatformZone = true;
            if (!isChasing)
                transform.position += Vector3.left * speed * Time.deltaTime;    // 반대 방향으로 이동
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.gameObject.name == "Platformzone_L" || other.gameObject.name == "Platformzone_R")
        {
            isInPlatformZone = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌 시 Rigidbody를 kinematic 상태로 설정하여 밀리지 않도록 함
            rigidbody.isKinematic = true;

            isAttack = true;
            animator.SetTrigger("Attack");  // "Attack" 애니메이션 트리거 실행
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌이 끝났을 때 kinematic 상태를 해제하여 다시 물리 엔진의 영향을 받게 함
            rigidbody.isKinematic = false;
        }
    }
}
