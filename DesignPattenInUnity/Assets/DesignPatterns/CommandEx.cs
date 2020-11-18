using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 명령패턴
// - 전략패턴과 매우 유사함.
// - 근본적인 차이는 전략패턴은 같은일을 하되 그 알고리즘이나 방식이 변경되는 거라면(ex. 상점팝업의 탭창)
//   명령패턴은 하는일 자체가 달라진다.
// - 명령패턴은 전략패턴처럼 명령을 갈아 끼워 넣는 방식으로 작성도 하고
// - 명령들을 목록으로 만들어서 순서대로 만들도록 할 수 있다.
public class CommandEx : MonoBehaviour
{
    // 1. 캐릭터 클래스 작성
    public class Actor
    {
        public enum EDirection
        {
            LEFT,
            RIGHT,
            UP,
            DOWN,
        }

        public void Move(EDirection direction, int speed) => 
            Debug.Log($"Direction : {direction}, Speed :{speed}");
        public void Jump(int speed) => Debug.Log("Jump");
        public void Attack()=>Debug.Log("Attack");
    }

    // 2. 명령 추상클래스 작성
    public abstract class Command{
        protected Actor actor;

        public void SetActor(Actor actor) => this.actor = actor;

        public abstract void Excute();
    }

    public class MoveCommand : Command
    {
        int speed;
        Actor.EDirection direction;

        public MoveCommand(Actor.EDirection direction, int speed)
        {
            this.direction = direction;
            this.speed = speed;
        }

        public override void Excute()
        {
            actor.Move(direction,speed);
        }
    }

    public class JumpCommand : Command
    {
        int speed;
        public JumpCommand(int speed)
        {
            this.speed =speed;
        }

        public override void Excute()
        {
            actor.Jump(speed);
        }
    }

    public class AttackCommand : Command
    {
        public AttackCommand()
        {
        }

        public override void Excute()
        {
            actor.Attack();
        }
    }

    // 3. 적 클래스 작성
    public class Enemy
    {
        public Actor actor = new Actor();
        public List<Command> commands = new List<Command>();

        public void AddCommand(Command command) => commands.Add(command);
        
        // 명령에 따라 적의 행동들을 순차적으로 나타냄
        public void Run() =>commands.ForEach(o=>{
            o.SetActor(actor);
            o.Excute();
        });
    }


}
