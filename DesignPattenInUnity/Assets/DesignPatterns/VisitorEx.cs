using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 방문자패턴
// - 방문자가 호스트객체 내부 상태에 접근 할 수 있는 방법을 제공하여 호스트 연산을 추가 할 수 있도록 한다.
// - 방문자는 주로 상속 없이 클래스에 메소드를 효과적으로 추가 하기위해 사용하는 패턴이다.
public class VisitorEx : MonoBehaviour
{
    // ex. 백화점내에 '고객등급'에 따른 해택을 지급하는 코드를 작성한다고 했을 때.
    // 1. 일단 고객 인터페이스를 만들고, 등급별로 구체클래스를 만든다. 그리고 인터페이스에는 고객의 해택을
    //    메소드화 해서 만들어놓는다.
    public abstract class Member
    {
        private string name;
        public abstract void GivePoint();
        public abstract void Discount();
    }

    // 2. 그리고 등급별로 클래스에 받을 해택을 정한다.
    public class SliverMember : Member
    {
        public override void Discount() => Debug.Log("10퍼 할인");
        public override void GivePoint() => Debug.Log("구매당 가격의 15% 적립");
    }

    public class GoldMember : Member
    {
        public override void Discount() => Debug.Log("20퍼 할인");
        public override void GivePoint() => Debug.Log("구매당 가격의 25% 적립");
        public void GetFreeTicket() => Debug.Log("무료티켓 지급");
    }

    // 3. 고객 정보를 담을 수 있는 리스트를 생성하고, 해택을 주는 메서드를 작성해보자
    private void SetBenefit()
    {
        List<Member> members = new List<Member>{new SliverMember(),
                                                new GoldMember(),
                                                new SliverMember() };

        members.ForEach(x =>
        {
            // 여기서 처리 하면됨.
        });
    }

    // 4. 자 여기까지 하면 해택을 지급하는 메소드도 작성되었다. 별 문제가 없어보인다.. 하지만,
    //    만약, '멤버마다 하나의 해택만 줄수 있음' 이나, 다른 클래스에 존재하지 않는 해택이 존재한다면?
    //    ForEach문 내부에서 다른 처리를 해줘야한다던지, 아니면 각 클래스 내부에 자신이 어떤 해택을
    //    가지고 있는지에 대한 처리를 위한 코드들을 각각 넣어줘야하 할 것이다.
    //    이럴 때, 방문자 패턴을 사용하면 효율적인 코드를 짤 수 있다.

    // >> 아래 코드를 보면 알겟지만 살짝? 수정이 필요한 예시인거 같다.
    //    아래 방문자패턴에서는 메소드를 호출 할때마다 기능을 호출해주고 있다.

    // 5. 방문자패턴으로 구현
    // 6. 일단, '이익' 인터페이스를 만듬
    public interface Benefit
    {
        void GetBenefit(NewGoldMember gold);
        void GetBenefit(NewSliverMember sliver);
    }

    public class DiscountBenefit : Benefit
    {
        public void GetBenefit(NewGoldMember gold)
            => Debug.Log("할인 20");
        public void GetBenefit(NewSliverMember sliver)
            => Debug.Log("할인 10");
    }

    public class PointBenefit : Benefit
    {
        public void GetBenefit(NewGoldMember gold)
            => Debug.Log("1000포인트");
        public void GetBenefit(NewSliverMember sliver)
            => Debug.Log("100포인트");
    }

    // 7. 새로운 멤버클래스 생성
    public abstract class NewMember
    {
        // 여기서 탬플릿메서드 패턴 쪼금 활용
        List<Benefit> benefits = new List<Benefit>();
        public void GetBenefits()=> benefits.ForEach(x=>{GetBenefit(x);});
        public abstract void GetBenefit(Benefit benefit);
    }

    public class NewGoldMember : NewMember
    {
        public override void GetBenefit(Benefit benefit) => 
            benefit.GetBenefit(this);
    }

    public class NewSliverMember : NewMember
    {
        public override void GetBenefit(Benefit benefit) => 
            benefit.GetBenefit(this);
    }

    public void SetBenefit2()
    {
        List<NewMember> members = new List<NewMember>{new NewSliverMember(),
                                                      new NewGoldMember(),
                                                      new NewSliverMember()};

        members.ForEach(x=>{
            x.GetBenefits();
        });
    }

    // * 순회하는걸 표현 하려면 메서드 호출 시 해당 개체를 넣는것이 아니라, 생성시 넣어줄 필요가 있다.

    // 정리
    // - 보통 클래스내부에 기능을 표현하기 위한 메소드가 존래하는게 일반적인데
    //   방문자패턴은 사용시에 해당 기능(메소드)에 대한 객체를 넣어준다는 것에 대해서는 '전략패턴' 과 유사한 느낌을 받음
    // - 하지만, 결과를 확인해보면 메소드 사용하는 객체가 자기에게 필요한 메소드를 찾는? 이런 느낌을 많이 받는것 같음
    // - 추가적인 기능이 있을 때 마다, 다른 코드를 건드릴 필요가 없음.
    // - 같은 형태로 오버로딩이 되어있는 것도 하나의 포인트. >> 나한테 맞는 파라미터를 찾아서 그거 쓸께 
}
