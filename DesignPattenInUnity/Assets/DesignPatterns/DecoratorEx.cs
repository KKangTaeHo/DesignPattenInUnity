using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데코레이터 패턴
// 특정 클래스의 객체들이 할 수 있는 일을 여러가지 두고
// 각 객체마다 사용자가 원하는대로 골라 시키거나 기능들을 필요에 따라 장착 할 수 있도록 할 때 사용하는 패턴
public class DecoratorEx : MonoBehaviour
{
    // ex. 써브웨이 센드위치를 만드는 코드를 작성한다고 했을 때
    public interface Sandwich
    {
        void Make();
    }

    public class NormalSandwich : Sandwich
    {
        public void Make()
        {
            Debug.Log("빵추가");
        }
    }

    public class CheeseDecorator : Sandwich
    {
        private Sandwich _sandwich;

        public CheeseDecorator(Sandwich s) => _sandwich = s;
        public void Make()
        {
            _sandwich.Make();
            Debug.Log("치즈추가");
        }
    }

    public class BaconDecorator : Sandwich
    {
        private Sandwich _sandwich;

        public BaconDecorator(Sandwich s) => _sandwich = s;
        public void Make()
        {
            _sandwich.Make();
            Debug.Log("베이컨추가");
        }
    }

    public class EggDecorator : Sandwich
    {
        private Sandwich _sandwich;

        public EggDecorator(Sandwich s) => _sandwich = s;
        public void Make()
        {
            Debug.Log("달걀추가");
        }
    }


    private void Start()
    {
        Sandwich sandwich = new NormalSandwich();
        sandwich = new CheeseDecorator(sandwich); // 치즈 추가
        sandwich = new BaconDecorator(sandwich);
    }
}
