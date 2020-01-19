using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdapterPattern : MonoBehaviour
{
    // 어뎁터 패턴
    // - 한 클래스의 인터페이스를 클라이언트에서 사용하고자하는 다른 인터페이스로 변환한다. (?)
    // - 인터페이스 호환성 문제 때문에 같이 쓸 수 없는 클래스들을 연결해서 쓸 수가 있다.
    // - 신식의 클래스에 구식의 클래스를 연동해서 사용이 가능하게 할 수가 있다.

    // 사용용도 및 장점
    // - 서로 달라서 같이 쓰지 못하는 클래스들을 연결해서 하나의 인터페이스로 사용이 가능하다.
    // - 두개의 소스는 전혀 수정이 되지 않고, 어댑터 클래스가 추가되어 두개의 사이를 연결해준다.
    // - 기존의 클래스에 새로운 기능을 추가해서 신규 클래스 같은 기능을 하게 할 수 있다.

    // 즉, A와 B의 클래스의 속성은 같지만 연결관계가 아닐 때, 새로운 클래스(어뎁터)를 하나 더 만들어
    // A와 B를 연결해준다.

    public abstract class Unit
    {
        public abstract void Move();
    }

    public class Marine : Unit
    {
        public override void Move() => Debug.Log("마린이동");
    }

    public class FireBat
    {
        public void Run() => Debug.Log("파이어벳 달림");
    }

    // 예를 들면, 스타에서 확장팩이 나와 새로운 캐릭터를 제작해야 하는데, 어떤어떤 사정으로(그럴일은 없겠지만)
    // 기존의 Unit Base 클래스를 사용하지 않고, 따로 제작을 하게 되었다.
    // 새로운 신규유닛의 클래스가 기본 유닛을 클래스와 조상이 달라 관리가 어려울때, 어뎁터패턴을 쓰면 좋다.

    public class FireBatUnit : Unit
    {
        FireBat fb = new FireBat();
        public override void Move() => fb?.Run();
    }

    private void Start()
    {
        List<Unit> _unit = new List<Unit>();
        _unit.Add(new Marine());
        _unit.Add(new FireBatUnit());

        _unit.ForEach(o => { o.Move(); });
    }
}
