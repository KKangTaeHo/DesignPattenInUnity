using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBufferPatten : MonoBehaviour
{
    // 일단 '버퍼'란 무엇인가?
    // 코딩을 하면서 '버퍼'라는 말을 자주 듣지만 그냥 '버퍼'라고 이해하고 넘어갔었다.
    // '버퍼'는 데이터를 한곳에서 다른 곳으로 전송 하는 동안 일시적으로 그 데이터를 보관하는 메모리영역이다.
    // > 쉽게 말해서 '임시 저장 공간'

    // 그럼 '버퍼링'이란?
    // 버퍼를 채우는 동작을 의미함.

    // '프래임 버퍼'란? 무엇인가?
    // 화면에 그려질 화면 전체에 대한 정보를 담는 공간, 메모리(메모리에 할당된 픽셀들의 배열)
    // 해상도에 그려질 화면에 대한 정보를 담는 곳

    // 일반적으로 비디오 카드에서는
    // GPU <> 프레임 버퍼 > 비디오 컨트롤러 > 화면
    // 요런식으로 구성이 되어있는데,
    // GPU는 그려질 화면을 프레임버퍼에 쓰는 일을하고, 비디오 컨트롤러는 그려진 화면을 출력하는 일을 한다.

    // 그런데!
    // 'GPU가 프레임 버퍼에 쓰는 작업은 비디오 컨트롤러가 프레임 버퍼를 읽는 속도에 비해 훨씬 느리기' 때문에
    // 깜빡거리는 현상이 발생하곤 한다.

    // 이 문제를 극복하기 위해 '이중 버퍼'가 필요하다.

    // 이중 버퍼링(더블버퍼링)
    // 프론트 버퍼, 백퍼버 이렇게 2개를 만들어 사용함.
    // 비디오 컨트롤러가 프론트버퍼를 읽는동안, GPU는 백버퍼에 다음에 그려질 내용을 쓴다.
    // GPU가 백버퍼에 내용을 다 썻다면, 비디오 컨트롤러가 백버퍼로 스위칭 후 새로운 내용을 화면에 그린다.
    // 동시에 GPU는 프론트버퍼로 스위칭하며, 프론트버퍼에 그려질 내용을 다시 쓴다.

    // 이중 버퍼 패턴은 언제쓰는게 좋을 것인가?
    // 1. 순차적으로 변경해야 하는 상태가 있다.
    // 2. 이 상태는 변경 도중에도 접그이 가능해야 한다.
    // 3. 바깥 코드에서는 작업 중인 상태에 접근 할 수 없어야한다.
    // 4. 상태에 값을 쓰는 도중에도 기다리지 않고 바로 접근 할 수 있어야 한다.
    // 요 조건을 만족 하는 경우, 이중 버퍼 패턴을 쓰면 좋다.

    // 이중 버퍼 패턴의 단점
    // 1. 교체 연산 자체에 시간이 걸린다.
    // 2. 버퍼가 두개 필요하다 > 메모리가 더 필요함.

    void Start()
    {
        // 1. Actor 
        Player billy = new Player("빌리");
        Player taeho = new Player("태호");
        Player parkang = new Player("박강");

        billy.SetTarget(taeho);     // 빌리 > 태호
        taeho.SetTarget(parkang);   // 태호 > 박강
        parkang.SetTarget(billy);   // 박강 > 빌리

        Stage stage = new Stage();

        stage.Add(billy, 0);
        stage.Add(taeho, 1);
        stage.Add(parkang, 2);
        
        billy.Slap();
        stage.Update();

        // 인덱스의 번호가 변경되는 즉시, Update의 내부가 달라짐


        // 더블버퍼를 활용하여 정의
        stage.Add(billy, 0);
        stage.Add(taeho, 1);
        stage.Add(parkang, 2);

        billy.Slap_DoubleBuffer();
        stage.Update_DoubleBuffer();

        stage.Update_DoubleBuffer();

        stage.Update_DoubleBuffer();

        stage.Update_DoubleBuffer();

        stage.Add(billy, 0);     // 내부에 변경하더라도 그대로 값이 나오는 것을 알 수 있다.
        stage.Add(taeho, 2);
        stage.Add(parkang, 1);

        stage.Update_DoubleBuffer();
    }


    // 패턴
    // 정보를 읽을 때는 '현재버퍼', 정보를 쓸 때는 '다음버퍼'에 접근하는 것이 포인트
    // 변경이 끝나면 다음버퍼와 현재 버퍼를 교채해 다음버퍼가 보여지게 한다.

    // 2. 프레임버퍼 클래스 구현
    public class FrameBuffer
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 10;

        private char WHITE = '0';
        private char BLACK = '1';

        private char[] _pixels = new char[WIDTH * HEIGHT];

        public void Clear()
        {
            for (int i = 0; i < WIDTH * HEIGHT; i++)
            {
                _pixels[i] = WHITE;
            }
        }

        /// <summary>
        /// 해당 픽셀에 색칠
        /// </summary>
        /// <param name="inX"></param>
        /// <param name="inY"></param>
        public void Draw(int inX, int inY)
        {
            _pixels[(WIDTH * inY) + inX] = BLACK;
        }
    }

    public class Scene
    {
        private FrameBuffer[] _buffers = new FrameBuffer[2];    // 버퍼 2개 생성

        private FrameBuffer _cur;
        private FrameBuffer _next;


        public Scene()
        {
            _cur = _buffers[0];
            _next = _buffers[1];
        }

        private void Swap()
        {
            FrameBuffer temp = _cur;
            _cur = _next;
            _next = temp;
        }

        public void Draw()
        {
            _next.Clear();
            _next.Draw(1, 1);
            _next.Draw(2, 2);
            _next.Draw(3, 3);
            Swap();
        }

        // 랜더링 할 때는 _next가 참조하는 버퍼에 그리고, 비드오 드라이버는 _curr에 접근해 픽셀을 읽도록 하는것이 포인트
    }

    // 그래픽스 외에 이중 버퍼 패턴을 활용하는 방법은
    // '어떤 상태를 변경하는 코드가, 동시에 지금 변경하려는 상태를 읽는 경우' 이다.
    // 특히 물리나 인공지능같이 객체가 서로 상호작용 할 때 이런 경우를 쉽게 볼 수 있다.

    // Ex. 행동 시스템을 만든다고 가정 했을 때.
    // 1. 상위 클래스 Actor 생성
    // Actor들은 서로 상호 작용할 수 있는데, 여러개의 Actor를 만들고 Update에서 Actor의 Slap() 메서들을 통해 다른 Actor를 때리고
    // WasSlapped() 으로 맞았는지 여부를 확인 할 수 있는 메서드를 구현
    public class Actor
    {
        public string Name { get; private set; }
        private bool _isSlap;

        public Actor(string inName)
        {
            Name = inName;

            _isSlap = false;
            _currSlapped = false;
        }
        public void Slap()
        {
            _isSlap = true;
        }

        public bool WasSlapped()
        {
            return _isSlap;
        }

        public void Reset()
        {
            _isSlap = false;
        }

        public virtual void Update() { }
        public virtual void Update_DoubleBuffer() { }



        // 더블 버퍼 전용
        private bool _currSlapped;
        private bool _nextSlapped;

        public void Swap()
        {
            _currSlapped = _nextSlapped;
            _nextSlapped = false;
        }

        public void Slap_DoubleBuffer()
        {
            _nextSlapped = true;
        }

        public bool WasSlapped_DoubleBuffer()
        {
            return _currSlapped;
        }
    }

    public class Player : Actor
    {
        private Actor _target;

        public Player(string inName) : base(inName)
        {

        }

        public void SetTarget(Actor inActor)
        {
            _target = inActor;
        }

        public override void Update()
        {
            if (WasSlapped())
            {
                Debug.Log("다음 타켓 : " + _target.Name);
                _target.Slap();
            }
        }

        public override void Update_DoubleBuffer()
        {
            if (WasSlapped_DoubleBuffer())
            {
                Debug.Log("다음 타켓 : " + _target.Name);
                _target.Slap_DoubleBuffer();
            }
        }
    }

    // 2. Actor의 상태를 확인 할 수 있는 Stage 클래스 구현
    // Actor의 업데이트 관리 for문을 looping 하면서 하나씩 업데이트
    public class Stage
    {
        Actor[] _actors = new Actor[3];

        public void Add(Actor inActor, int inIndex)
        {
            _actors[inIndex] = inActor;
        }

        public void Update()
        {
            foreach (Actor actor in _actors)
            {
                actor.Update();
                actor.Reset();
            }
        }

        // 더블 버퍼 전용
        public void Update_DoubleBuffer()
        {
            foreach (Actor actor in _actors)
            {
                actor.Update_DoubleBuffer();
            }

            foreach (Actor actor in _actors)
            {
                actor.Swap();
            }
        }
    }

    public class Actor_DoubleBuffer
    {
        private bool _currSlapped;
        private bool _nextSlapped;

        public Actor_DoubleBuffer()
        {
            _currSlapped = false;
        }

        public void Swap()
        {
            _currSlapped = _nextSlapped;
            _nextSlapped = false;
        }

        public void Slap()
        {
            _nextSlapped = true;
        }

        public bool WasSlapped()
        {
            return _currSlapped;
        }
    }
}
