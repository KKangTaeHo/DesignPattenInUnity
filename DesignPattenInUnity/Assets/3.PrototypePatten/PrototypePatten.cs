using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePatten : MonoBehaviour
{
    // 원형패턴(PrototypePatten)
    // 객체에 의해 생성 될 객체의 타입이 결정되는 생산 디자인 패턴
    // 미리 만들어진 개체를 복사하여 개체를 생성하는 패턴

    // 장점
    // 객체를 생성해주기 위한 별도의 객체 생성 클래스가 불필요하다.
    // 객체의 각 부분을 조합해서 생성되는 형태에도 적용 가능하다.

    private void Start()
    {
        // 2. 스포너에 등록
        Spawner spawner = new Spawner(new Ghost());

        Spawner spawnerNew = new SpawnerFor<Ghost>();
    }

    // Ex. 예를 들어 몬스터와 여러몬스터를 스폰 할 수 있는 클래스를 만들고자 한다면
    public class MonsterNormal { }
    public class GhostNormal : MonsterNormal { }
    public class DemonNormal : MonsterNormal { } 
    // 이런식으로 몬스터 클래스를 정의하고

    public  abstract class SpawnerNormal
    {
        public abstract MonsterNormal SpawnMonster();
    }

    public class GhostNormalSpawner : SpawnerNormal
    {
        public override MonsterNormal SpawnMonster()
        {
            return new GhostNormal(); 
        }
    }
    // 요런 식으로 줄줄이 만들어야하기 때문에 스폰이 생성될 때마다 계속만들어야하는 어려움이 있다.
    // 이럴때 프로토타입 패턴을 활요하면 좀더 효율적인 코드를 작성 할 수 있다.

    // 1. 몬스터 및 상속 클래스 구현
    // _hp나 _mp같은 맴버를 초기화 시킨 후, 해당 객체를 Clone을 통해 복제한다.
    // 같은 값을 가지는 복제 데이터 생성
    public abstract class Monster
    {
        protected int _hp;
        protected int _mp;
        public abstract Monster Clone();
    }

    public class Ghost : Monster
    {
        public override Monster Clone() => new Ghost();
    }

    public class Demon : Monster
    {
        public override Monster Clone()=> new Demon();
    }

    // 2. Spawner 클래스 구현
    public class Spawner
    {
        protected Monster _prototype;

        public Spawner()
        {

        }

        public Spawner(Monster inPrototype)
        {
            _prototype = inPrototype;
        }

        public virtual Monster SpawnMonster() => _prototype.Clone();
    }

    // 3. 제너릭을 사용한 Spawner 클래스 구현
    public class SpawnerFor<T> : Spawner where T: Monster, new()
    { 
        public SpawnerFor()
        {
            _prototype = SpawnMonster();
        }
        public SpawnerFor(Monster inPrototype) : base(inPrototype)
        {
            _prototype = inPrototype;
        }

        public override Monster SpawnMonster() => new T();
    }
}
