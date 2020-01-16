using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryMethodPattern : MonoBehaviour
{

    // 팩토리 메소드 패턴
    // 객체를 생성하기 위한 인터페이스를 정의하는데, 어떤 클래스를 만들지는 서브 클래스에서 결정
    // > 펙토리 메소드 패턴을 이용하면 클래스의 인스턴스를 만드는 일을 서브 클래스에게 맡기게 된다.

    // 장점
    // 1. 객체의 생성을 한곳에서 관리 할 수 있다.
    // 2. 동일한 인터페이스 구현으로 새로운 객체가 추가되더라도 소스의 수정이 거의 없다. (생성 부분의 수정과 신규 클래스의 추가 정도)
    // 3. 객체를 여러곳에서 생성을 각자하면 동일한 생성을 보장 못하지만, 한군데에서 관리하면, 동일한 생성을 보장함.

    // 사용목적 및 용도
    // 1. 스타크래프트로 예를 들자면, 바로 '유닛 생성 부분'에서 팩토리 메소드 패턴을 적용이 좋은 예시가 된다.
    // 2. 스타에서는 건물에서 생성되는 유닛들은 '이동/공격/죽음' 등의 비슷한 행동과 명령을 가지고 있는데, 이것들을 Unit에서 구현될 인터페이스가 되며
    // 유닛을 상속받은 Marine 이나 DropShip 같은 객체는 기본 행동을 제외한 각자의 특색(마린은 지상이동, 드랍쉽은 공중이동)있는 기능들을 따로 구현한다.
    // 3. Barrack에서 유닛을 생성 할 때, A.유닛 생성시간 > B.유닛을 저녁변수에 등록 > C.유닛 생성 종료 알림 > 4. 유닛 화면 표출 등의
    // 일련의 과정들을 '유닛 생성 부분'이라는 부분을 만들어, 이 부분에서만 유닛을 생성하도록 구현한다.
    // 4. 이렇게 만들어 놓으면, Barracks에 신규 유닛이 추가 될 때, Unit의 인터페이스를 구현하는 클래스(Unit 을 상속받는)를 만들고
    // Barracks에서 해당 유닛을 생성하는 부분만 추가해주면 나머지는 수정이 필요없게 된다.

    // 프로토타입 패턴과 많이 유사해 보이는데.. 차이는 무엇인가?
    // 프로토타입은 서브클래싱이 필요로 하지 않지만, '초기화'동작은 필요하다. 반면, 팩토리 메서드 패턴은 서브클래싱을 필요로 하나, '초기화'동작은 필요하지 않는다

    private void Start()
    {
        // 1. 건물 두개 생성
        Building[] building = new Building[2];
        building[0] = new Barracks();   // 베럭생성
        building[1] = new StarPort();   // 스타포트 생성

        // 2. 유닛 생성
        foreach (Building b in building)
        {
            Debug.Log(b.MakeUnit().Name);
        }
    }

    // 1. Unit 추상 클래스 생성
    public abstract class Unit
    {
        public string Name { get; protected set; }
        public int Damage { get; protected set; }
        protected int _hp;

        public abstract void Move(string inPoint);
        public abstract void Attacked(ref Unit inTarget);
    }

    // 2. Unit을 상속받는 두 클래스 생성
    public class Marine : Unit
    {
        public Marine()
        {
            Name = "마린";
            Damage = 10;
            _hp = 50;
        }
        public override void Attacked(ref Unit inTarget)
        {
            Debug.Log(string.Format("{0}가 공격함. HP : {1}", inTarget.Name, _hp -= inTarget.Damage));
        }

        public override void Move(string inPoint)
        {
            Debug.Log(Name + " 움직임");
        }
    }

    public class DropShip : Unit
    {
        public DropShip()
        {
            Name = "드랍쉽";
            Damage = 0;
            _hp = 150;
        }
        public override void Attacked(ref Unit inTarget)
        {
            Debug.Log(string.Format("{0}가 공격함. HP : {1}", inTarget.Name, _hp -= inTarget.Damage));
        }

        public override void Move(string inPoint)
        {
            Debug.Log(Name + " 움직임");
        }
    }


    // 3. 유닛생성 건물 클래스 구현(배럭, 스타포트)
    public abstract class Building
    {
        public abstract Unit MakeUnit();
    }

    public class Barracks : Building
    {
        public override Unit MakeUnit()
        {
            return new Marine();
        }
    }

    public class StarPort : Building
    {
        public override Unit MakeUnit()
        {
            return new DropShip();
        }
    }
}