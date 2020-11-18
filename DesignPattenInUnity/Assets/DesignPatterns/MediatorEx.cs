using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 중재자 패턴
// - 중재자를 두어 객체들 사이의 복잡한 관계를 캠슐화 하는 단일 객체를 정의한다.
public class MediatorEx : MonoBehaviour
{
    // 1. 중재자에의해 관리될 수 있는 인터페이스 생성

    public enum EMode
    {
        LIST,
        GALLERY,
        NONE
    }
    public interface ModeListener
    {
        void OnModeChange(EMode mode);
    }

    public class ListView : ModeListener
    {
        public void OnModeChange(EMode mode)
        {
            
        }
    }

    public class GalleryView : ModeListener
    {
        public void OnModeChange(EMode mode)
        {

        }
    }

    public class DataDown : ModeListener
    {
        public void OnModeChange(EMode mode)
        {

        }
    }

    // 2. 중재자 생성
    public class ModeMediator
    {
        private List<ModeListener> listeners = new List<ModeListener>();
        public void AddListener(ModeListener listener) => listeners.Add(listener);
        public void OnModeChange(EMode mode) => listeners.ForEach(o => { o.OnModeChange(mode); });
    }

    // 3. 중재자 사용
    public class ModeSwitch
    {
        EMode mode = EMode.LIST;
        ModeMediator mediator;

        public void SetModeMediator(ModeMediator mediator) =>
        this.mediator = mediator;

        public void ToggleMode()
        {
            mode = mode == EMode.LIST ? EMode.GALLERY: EMode.LIST;

            // 모드가 변경될때 중재자에 있는 모든 대상에게 알린다.
            mediator?.OnModeChange(mode);
        }
    }

    
    void Start()
    {
        ModeSwitch modeSwitch = new ModeSwitch();
        ModeMediator modeMediator = new ModeMediator();

        // 필요에 의해 중재자는 언제든지 변경 될 수 있다.
        modeSwitch.SetModeMediator(modeMediator);

        modeMediator.AddListener(new ListView());
        modeMediator.AddListener(new GalleryView());
        modeMediator.AddListener(new DataDown());
    }
}
