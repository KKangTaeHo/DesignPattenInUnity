using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 브짓지패턴
// - 구조패턴 중 하나
// - 서브시스템 사이의 결합도를 줄임으로써 각 서브 시스템이 서로에 영향을 미치지 않으면서 변경 될 수 있도록 한다.
// - 즉, 기능과 구현을 별도의 클래스로 정의해서 서로를 분리하는 방법이다.

// * 구조패턴
// - 작은 클래스들을 상속과 합성을 이용하여 더 큰 클래스를 생성하는 방법을 제공하는 패턴
// - 이 패턴을 사용하면 서로 독립적으로 개발한 클래스 라이브러리를 마치 하나인양 사용 할 수 있다.
//   또 여러 인터페이스를 합성(Composite)하여 서로 다른 인터페이스들의 통일된 추상을 제공한다.

public class BridgeEx : MonoBehaviour
{
    // ex. 1. 스파벅스에서 판매하는 물품에 대한 클래스를 작성한다고 해보자.
    // 보통 우리가 스타벅스에 가는 이유는 대부분 커피를 마시러 가니 다음과 같이 추상클래스가 만들어 질 수 있다.
    public abstract class StarbucksMenu
    {
        public string name;
        public abstract void RecipeInfo();
    }

    // 2. 이제 메뉴를 상속받는 서브클래스들을 만들어보자
    public class Ameriano : StarbucksMenu
    {
        public override void RecipeInfo()
        {
            Debug.Log("애스프레소에 물탐");
        }
    }

    public class Latte : StarbucksMenu
    {
        public override void RecipeInfo()
        {
            Debug.Log("에스프레소에 우유탐");
        }
    }

    // 3. 여기까지는 괜찮다. 스타벅스에서 판매하고 있는 상품은 보통 음료이니깐.. 그래 빵까지도 괜찮다
    // 그런데, 만약 텀블러를 정의해야한다면..? 혹은 기프티카드를 정의해야한다면, 상위 클래스에 있는
    // 레시피 메서드가 필요할까..? 현재의 스타벅스 추상클래스를 상속받으면 필요없는 메서드까지
    // 가져야 하는 것이다(강한 결합). 이 문제를 해결하기 위해 브릿지 패턴을 사용한다.

    public interface Menu
    {
        void MenuInfo();
    }

    public class Coffee : Menu
    {
        public void MenuInfo()
        {
            Debug.Log("레시비 정보를 담는다");
        }
    }

    public class Tumblr : Menu
    {
        public void MenuInfo()
        {
            Debug.Log("텀블러의 재질 정보를 담는다.");
        }

        public bool GetFreeDrink()
        {
            return false;
        }

        // 텀블러는 환불도 할 수 있고
        public bool IsRefund() => false;
    }

    // 4. 새로운 스타벅스 추상클래스 생성
    public abstract class Starbucks
    {
        public Menu menu; // 서브시스템과의 완전한 분리

        public abstract void Info();
    }

    public class SessionTumblr : Starbucks
    {
        public SessionTumblr()
        {
            menu = new Tumblr();
        }

        public override void Info()
        {
            menu.MenuInfo();
        }
    }

    // 이런식으로 사용하므로써 기능 자체를 구현객체의 결합에서 완전히 벗어나도록 하는게 브릿지 패턴이다.
}

// Q. 기능을 꽂아주는 형식으로 봐서는 전략패턴과 비슷한 모습을 보임
// Q. 여러책에서는 어뎁터패턴과 유사하다고 하는데,, 잘 모르겠음.
