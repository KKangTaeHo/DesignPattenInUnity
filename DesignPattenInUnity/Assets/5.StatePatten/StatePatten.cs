using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatten : MonoBehaviour
{
    // 상태 패턴(StatePatten)
    // 객체의 내부 상태가 바뀜에 따라서 객체의 행동을 바꿀 수 있다.
    // 마치 객체의 클래스가 바뀌는 것과 같은 결과를 얻을 수 있음.

    // 간단히 이야기하자면
    // 상태 패턴은, 행동과 상태를 나눈 패턴으로 행동을 인터페이스로 정의하고
    // 상태에 따라 행동들을 분류 시킨다.

    // 즉, 유한 상태 기계(FSM)에  맞추어 패턴화시킴
    // 요점은
    // - 한번에 '한가지 상태'만 될 수 있음
    // - '입력'이나 '이벤트'가 기계에 전달된다. > 게임에선 버튼 누르기, 때기가 여기에 해당함
    // - 각 상태는 입력에 따라 다음 상태로 변경되는 '전이'가 있다.
    // 대충 요렇게 있다.

    
    // 1. 대상 클래스 생성
    public class Human
    {
        private HumanState _state;

        // 4. 동작을 상태에 위임하기
        public virtual void HandleInput(KeyCode inKey) => _state.HandleInput(this, inKey);
        public virtual void Update() => _state.Update(this);

        public void Run() => Debug.Log("달리기");
        public void Idle() => Debug.Log("정지(서있음)");
        public void Attack() => Debug.Log("공격");
    }


    // 2. 행동을 인터페이스화 시킨다.
    public abstract class HumanState
    {
        public abstract void HandleInput(Human inHuman, KeyCode inCode);
        public abstract void Update(Human inHuman);

        // 5. _state에 새로운 객체 할당을 위한 방법 - 정적 객체 생성
        // 물론, 여기서 사용할 때는 모든 객체 상태를 싱글톤 형태로 사용하는 것이 바람직하다.
        public static Run running;
        public static Attack attacking;
    }

    // 3. 상태별 클래스를 만든다.
    public class Run : HumanState
    {
        public override void HandleInput(Human inHuman, KeyCode inCode)
        {
            if(Input.GetKeyDown(inCode))
            {
                inHuman.Run();
            }
            else if(Input.GetKeyUp(inCode))
            {
                inHuman.Idle();
            }
        }

        public override void Update(Human inHuman)
        {
            // 기타처리를 위한 업데이트 동작
            // ex) run 상태에 프레임마다 부스트를 쓰기위한 에너지를 모으던지,, 등등
        }
    }

    public class Attack:HumanState
    {
        public override void HandleInput(Human inHuman, KeyCode inCode)
        {

        }

        public override void Update(Human inHuman)
        {
            // 기타처리를 위한 업데이트 동작
            // ex) run 상태에 프레임마다 부스트를 쓰기위한 에너지를 모으던지,, 등등
        }
    }


    // 위의 예시는 책의 예제를 보고 따라한 것이나, 책 상태가 너무 이상해서 구글링을 통해 다시 예제를 찾음
    // 혹시.. 다음에 정확하게 이해하고 완성할 수 있으니 남겨두도록 하자

    // 1. 상태를 캡슐화한 인터페이스 선언
    public interface PowerState
    {
        public void PowerPush();
    }

    // 2. 인터페이스를 상속 받은 상태클래스 구현
    public class On : PowerState
    {
        public void PowerPush() => Debug.Log("On");
    }

    public class Off : PowerState
    {
        public void PowerPush() => Debug.Log("Off");
    }

    public class Save : PowerState
    {
        public void PowerPush() => Debug.Log("Save");
    }

    // 3. 상태를 가지고 있을 수 있는 클래스 구현
    public class DeskTop
    {
        private PowerState _state;

        public DeskTop() => _state = new Off(); // 초기화는 'Off'로

        public void SetPowerState(PowerState inState) => _state = inState;  // 상태정의

        public void PowerPush() => _state.PowerPush();
    }
}
