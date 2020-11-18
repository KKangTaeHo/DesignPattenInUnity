using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태패턴
// - 외부객체의 개입없이 상태객체 내부에서 현재상태에 따라 객체 상태를 변경시킴
public class StateEx : MonoBehaviour
{
    // ex. FPS에서 특정키를 눌렀을 때, 무기가 순서대로 변경하는 코드를 짜본다고 해보자
    // 전략패턴과 굉장히 유사하지만, 전략패턴 같은 경우에는 외부객체에서 전략을 처리할 대상들을 끼워넣어
    // 모듈을 변경시킨거라면, 상태패턴은 외부에서 어떤모듈을 끼워주는것 아니라 내부에서 현재상태에 따라
    // 상태를 변경시켜 주는 것이라 생각하면된다.

    // 1. 무기 인터페이스를 작성하고 그에 따른 세부클래스를 작성하도록한다.
    // - 무기를 세팅할 때마다 다음
    public interface IWeapon
    {
        void SetWeapon(WeaponBox weapon);
    }

    public class Gun : IWeapon
    {
        public void SetWeapon(WeaponBox weapon)
        {
            Debug.Log("총으로 변경");
            weapon.SetState(new Knife());
        }
    }

    public class Knife : IWeapon
    {
        public void SetWeapon(WeaponBox weapon)
        {
            Debug.Log("칼로 변경");
            weapon.SetState(new Bomb());
        }
    }

    public class Bomb : IWeapon
    {
        public void SetWeapon(WeaponBox weapon)
        {
            Debug.Log("칼로 변경");
            weapon.SetState(new Gun());
        }
    }

    // 2. 무기들을 관리하는 클래스를 작성한다.
    public class WeaponBox
    {
        // 3. 현재 상태를 지정하기 위한 무기 인터페이스를 맴버로 정한다.
        private IWeapon _weapon = new Gun();
        public void SetState(IWeapon weapon) => _weapon = weapon;
        public void SetWeapon()=>_weapon.SetWeapon(this);
    }

    
    // 3. 이제 해당 클래스 사용한다고 가정하면
    
    WeaponBox weaponBox;
    
    private void Start()
    {
        weaponBox = new WeaponBox();
    }

    private void Update()
    {
        // 4. R버튼이 눌러질때 무기가 변경된다.
        // 외부에서 무기박스 내부객체에 접근하지 않더라도 상태가 변경된다.
        if(Input.GetKey(KeyCode.R))
        {
            weaponBox.SetWeapon();
        }
    }
}
