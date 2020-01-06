using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPatten : MonoBehaviour
{
    // 명령패턴(COmmand Patten)
    // 실행될 기능을 캡슐화함으로써 주어진 여러기능을 실행 할 수 있는 재사용성이 높은 클래스를 설계하는 패턴
    // '행위 패턴'중의 하나

    // 예를 들어,
    // if(Input.GetKeyDown(Keycode.W))
    // {
    //      Jump();
    // }
    // else if(Input.GetKEyDown(Keycode.S)) 
    // {
    //      Back();
    // }
    // 와 같이 유저의 입력을 처리하는 코드가 있을 때, 입력받는 키의 내용이 자주 변경되는 경우, 행동들을 객체화 시켜 관리함.

    InputHandler inputHandler;      // 입력 핸들러

    Warrior warrior;                
    Minion minion;

    GameActor gameActor;
    private void Start()
    {
        // 1. 핸들러 생성
        inputHandler = new InputHandler();
        // 2. 행동 객체 생성
        JumpCommand jc = new JumpCommand();
        RunCommand rc = new RunCommand();
        // 3. 명령 바인딩
        inputHandler.BindCommand(jc, rc);

        // 위의 형태가 '명령패턴'의 핵심!
        // 하지만, 위의 패턴은 특정 캐릭터(여기서는 플레이어)만 움직이게 할 수 있다는 단점이 있다.
        // (즉, 캐럭터 외의 다른것들에게 명령을 내리기에는 제한적)
        // 이런 문제를 해결 하기 위해, 제어하려는 객체를 객체 내부에서 찾아서 쓰는 것이 아니라, 
        // 매개변수로 받아서 밖에서 전달 받을 수 있도록 함. 

        // 4. GameActor 객체 생성
        // 생성된 두 객체는 Update에서 객체가 변경될 때마다, 어떻게 명령이 들어가는지 테스트 해봄
        warrior = new Warrior();
        minion = new Minion();

        // 명령 패턴을 활용하면 어떤 명령 객체가 어떤 작업을 실행하고 취소 시킬 수 있는 '취소'기능을
        // 쉽게 만들 수 있다.
    }
    void Update()
    {
        // inputHandler?.HandleInput();

        if (Input.GetKeyDown(KeyCode.Alpha1)) gameActor = warrior;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) gameActor = minion;

        Command cmd = inputHandler.ReturnHandleInput();
        cmd?.Execute(gameActor);
    }

    // A. 명령의 상위 클래스 
    public abstract class Command
    {
        public abstract void Execute();
        public abstract void Execute(GameActor ga);
    }

    // B. 각 행동별 하위 클래스 생성
    public class JumpCommand : Command
    {
        public override void Execute()
        {
            Debug.Log("JumpCommand 호출!");   // 해당 메소드에 Jump 같은 메소드를 넣음
        }

        // F. GameActor를 매개변수로 받는 Execute 메서드 생성
        // 이제 이거 하나만 있으면 게임에 등장하는 어떤 캐릭터라도 Jump를 호출 시킬 수 있다.
        public override void Execute(GameActor ga) => ga.Jump();
    }

    public class RunCommand : Command
    {
        public override void Execute()
        {
            Debug.Log("RunCommand 호출!");
        }

        public override void Execute(GameActor ga) => ga.Run();
    }

    // C. 입력 핸들러 생성
    public class InputHandler
    {
        private Command _buttonX;
        private Command _buttonY;

        public void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.X)) _buttonX.Execute();
            else if (Input.GetKeyDown(KeyCode.Y)) _buttonY.Execute();
        }

        // G. 명령객체를 반환하는 메서드 작성
        public Command ReturnHandleInput()
        {
            if (Input.GetKeyDown(KeyCode.X)) return _buttonX;
            else if (Input.GetKeyDown(KeyCode.Y)) return _buttonY;
            return null;
        }

        public void BindCommand(Command inBtnX, Command inBtnY)
        {
            _buttonX = inBtnX;
            _buttonY = inBtnY;
        }
    }

    // D. 캐릭터를 대표하는 GameActor 클래스 생성
    public class GameActor
    {
        public virtual void Jump() => Debug.Log("GameActor의 Jump 호출");
        public virtual void Run() => Debug.Log("GameActor의 Run 호출");
    }

    // E. GameActor를 상속받는 클래스 생성
    public class Warrior : GameActor
    {
        public override void Jump() => Debug.Log("Warrior 점프!");
        public override void Run() => Debug.Log("Warrior 달리기!");
    }

    public class Minion : GameActor
    {
        public override void Jump() => Debug.Log("Minion 점프!");
        public override void Run() => Debug.Log("Minion 달리기!");
    }
}
