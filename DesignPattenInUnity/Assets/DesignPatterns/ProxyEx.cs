using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


// 프록시패턴
// - 간단히 이야기하면 '필요할 때만 실제 객체를 생성하고, 필요하지 않을 때는 그걸 대신 하는 
//   객체를 불러오는' 패턴
// - 쉽게 말하자면 큰일을 하기 전에 작은일을 하는 객체 '프록시'를 두는 일이다
public class ProxyEx : MonoBehaviour
{
    // ex. 유튜브에서 썸네일 노출을 예로 들 수 있다.
    // 마우스 커서를 갖대 대기전엔 이미지로 노출시키고, 커서를 갖다 댔을 땐 동영상이 노출되도록 할 때
    // 큰일인 동영상은 커서가 올라와 있을때만 처리되도록 하고, 그 이전엔 이미지 노출 즉 '프록시'를 사용
    // 하도록 한다.

    List<Thumbnail> thumbnails = new List<Thumbnail>{
        new ProxyThumbnail("깃강좌 1", "URL"),
        new ProxyThumbnail("깃강좌 2", "URL"),
        new ProxyThumbnail("깃강좌 3", "URL"),
        new ProxyThumbnail("깃강좌 4", "URL"),
    };

    public void Start()
    {
        // 2. 프록시 노출
        thumbnails.ForEach(x =>
        {
            x.ShowTitle();
        });
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // 3. 레이에 닿으면 노출되도록 만들면 되는데 여기서는 패스 하겠음.

            thumbnails[0].ShowPreview();
        }
    }
}

// 1. 썸네일 인터페이스 구현
public interface Thumbnail
{
    void ShowTitle();
    void ShowPreview();
}

public class RealThumbnail : Thumbnail
{
    private string _title;
    private string _downladURL;

    public RealThumbnail(string title, string downloadURL) =>
    (_title, _downladURL) = (title, downloadURL);


    public void ShowPreview()
    {
        try
        {

            using (WebClient clinet = new WebClient())
            {
                clinet.DownloadFile(_downladURL, "VideoName");
                // 다운 받은 후 데이터 처리
                // 자세한건 나중에 찾아봄
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void ShowTitle()
    {
        Debug.Log("재목실행");
    }
}

public class ProxyThumbnail : Thumbnail
{
    private string _title;
    public string _movieURL;

    RealThumbnail realThumbnail;
    public ProxyThumbnail(string title, string movieURL) =>
    (_title, _movieURL) = (title, movieURL);

    public void ShowPreview()
    {
        if (realThumbnail == null)
            realThumbnail = new RealThumbnail(_title, _movieURL);
    }

    public void ShowTitle()
    {
        Debug.Log("제목");
    }
}

// 보호 프록시(Protection Proxy)
// 프록시 객체가 사용자의 실제 객체에 대한 접근을 제어하는 것.

// ex. 인사팀에서 인사정보에 대한 데이터 접근을 직책단위로 세분화 하려고 한다. 직책에 따라 조직원의
//     인사정보 접근을 제어하는 업무를 수행 한다고 가정했을 경우

public enum EGrade
{
    STAFF,
    MANAGER,
    VICEPRESIDENT,
}

// 1. 직원 인터페이스 작성
public abstract class Employee
{
    public string Name { get; set; }
    public EGrade Grade { get; set; }
    public abstract string GetInformation(Employee employee);
}

// 2. 일반직원 클래스 작성
public class NormalEmployee : Employee
{
    public NormalEmployee(string name, EGrade grade) =>
    (Name, Grade) = (name, grade);

    // 열람 할 수 있는 코드
    public override string GetInformation(Employee viewer) =>
        $"name : {viewer.Name}, grade : {viewer.Grade}";
}

// 3. 인사정보가 보호된 구성원(인사 정보 열람 권한 없으면 예외 발생)
public class ProtectedEmployee : Employee
{
    private Employee _employee;

    public ProtectedEmployee(Employee employee) =>
        _employee = employee;

    public override string GetInformation(Employee viewer)
    {
        // 4. 내 정보라면 그냥 노출
        if (_employee.Grade == viewer.Grade &&
            _employee.Name == viewer.Name)
            return _employee.GetInformation(viewer);

        // 5. 내 정보가 아닐경우 조건에 따라 노출여부 확인(객체에 대한 접근 제한)
        switch (viewer.Grade)
        {
            case EGrade.VICEPRESIDENT:
                break;
            case EGrade.MANAGER:
                if (_employee.Grade == EGrade.STAFF)
                    return _employee.GetInformation(viewer);
                break;
            case EGrade.STAFF:
                Debug.Log("아무것도 못봄");
                break;
            default:
                break;
        }

        return null;
    }
}

