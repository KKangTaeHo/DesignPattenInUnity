using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전략패턴
// 프로그램 실행 중 모드가 바뀔 때 마다 검색이 이루어지는 방식, 즉 '전략'이 변경됨.
public class StrategyEx : MonoBehaviour
{
    // ex. 상점팝업에는 다양한 카테고리가 존재한다. 가넷이나 아이템 혹은 이벤트 재화같은 상품들을 위한
    // 카테고리가 나눠저 있다. 이때 조건에 따라 카테고리를 설정해주는 코드를 작성한다고 했을 때, 다음과
    // 같이 나타낼 수 있을 것이다.

    public enum EShopState
    {
        GARNET,
        ITEM,
        EVENT,
        NONE,
    }

    public class ShopPopup
    {
        // 2. 보통 이런식으로 SetCategory 메서드를 하나 만들어서
        // 분기처서 안에다가 여러 내용을 만들어 놓을 것이다.
        public void ShowPopup(EShopState inState)
        {
            switch(inState)
            {
                case EShopState.GARNET:
                    Debug.Log("가넷 카테고리");
                break;
                case EShopState.ITEM:
                    Debug.Log("아이탬 카테고리");
                break;
                case EShopState.EVENT:
                    Debug.Log("이벤트 카테고리");
                break;
            }
        }
    }

    private void Ex01()
    {
        EShopState category = EShopState.EVENT;

        ShopPopup shop = new ShopPopup();
        shop.ShowPopup(category);
    }

    // 3. 위의방식처럼 하게 되면, 카테고리가 늘어날 때 마다 'SetCategory' 내부를 계속해서 수정해주어야한다.
    // 클래스마다 역할지정을 뚜렷하게 해서 모듈화된 소프트웨어를 만드는 객체지향의 철학에도 조금 어긋난다.

    // 전략패턴은 모드마다의 동작 하나하나를 모듈로 따로 분리해서, 카테고리를 설정 할 때마다 각각의 모듈들을
    // 갈아까우는 방식으로 코딩을 짜는 방식이다.

    
    // 4. 전략패턴으로 코드를 짜보면
    public interface Strategy {
        void Excute();
    }

    // 5. 전략을 상속받는 클래스들을 정의
    public class GarentCategory : Strategy
    {
        public void Excute()
        {
            Debug.Log("가넷 카테고리");
        }
    }

    public class ItemCategory : Strategy
    {
        public void Excute()
        {
            Debug.Log("아이탬 카테고리");
        }
    }

    public class EventCategory : Strategy
    {
        public void Excute()
        {
            Debug.Log("이벤트 카테고리");
        }
    }

    public class NewShopPopup
    {
        private Strategy _strategy;
        public void SetCategory(Strategy inStrategy) => _strategy = inStrategy;
        public void ShowPopup() => _strategy.Excute();
    }

    private void Ex02()
    {
        NewShopPopup shop = new NewShopPopup();
        shop.SetCategory(new ItemCategory());
        shop.ShowPopup();
    }

    // 5. 이렇게 만들어놓으면 나중에 추가 할 때, 그냥 클래스만 만들어서 갈아 끼워 넣기만 하면된다.
    // 특정 메서드를 계속해서 수정 할 필요가 없다.
}
