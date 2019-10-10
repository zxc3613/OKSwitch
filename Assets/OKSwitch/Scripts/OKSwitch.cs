using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKSwitch : MonoBehaviour
{
    public bool isOn;                   //스위치 온/오프 상태
    [Range(0, 3)]                       //슬라이더바 0~3
    public float moveDuration = 3f;     //스위치 이동 애니메이션 시간
    //Color
    public Color handleColor = Color.green;
    public Color offBackgroundColor = Color.white;
    public Color onBackgroundColor = Color.red;

    const float totalHandleMoveLength = 72f;
    const float halfMoveLength = totalHandleMoveLength / 2;

    Image backgroundImage;                  //스위치 배경 이미지
    Image handleImage;                      //스위치 핸들 이미지
    RectTransform handleRectTransform;      //스위치 핸들 RectTransform

    //Coroutine
    Coroutine moveHandleCoroutine;                  //핸들 이동 코루틴
    Coroutine ChangeBackgroundColorColorutine;      //배경색 변경 코루틴
    void Start()
    {
        //핸들 초기화
        GameObject handleObject = transform.Find("Handle").gameObject;
        handleRectTransform = handleObject.GetComponent<RectTransform>();

        //Handle Image  핸들 이미지
        handleImage = handleObject.GetComponent<Image>();
        handleImage.color = handleColor;

        //BackgroundImage Image 백그라운드 이미지
        backgroundImage = GetComponent<Image>();
        backgroundImage.color = offBackgroundColor;
        if (isOn)
        {
            handleRectTransform.anchoredPosition = new Vector2(halfMoveLength, 0);
        }
        else
        {
            handleRectTransform.anchoredPosition = new Vector2(-halfMoveLength, 0);
        }
    }
    public void OnClickSwitch()
    {
        isOn = !isOn;

        Vector2 fromPosition = handleRectTransform.anchoredPosition;
        Vector2 toPosition = (isOn) ? new Vector2(halfMoveLength, 0) : new Vector2(-halfMoveLength, 0);
        Vector2 distance = toPosition - fromPosition;

        float ratio = Mathf.Abs(distance.x) / totalHandleMoveLength;
        float duration = moveDuration * ratio;

        //Handle Move Coroutine 핸들 무브 코루틴
        if (moveHandleCoroutine != null)
        {
            StopCoroutine(moveHandleCoroutine);
            moveHandleCoroutine = null;
        }
        moveHandleCoroutine = StartCoroutine(moveHandle(fromPosition, toPosition, duration));
        //Background Color Change Coroutine 백그라운드 컬러 체인지 코루틴
        Color fromColor = backgroundImage.color;
        Color toColor = (isOn) ? onBackgroundColor : offBackgroundColor;
        if (ChangeBackgroundColorColorutine != null)
        {
            StopCoroutine(ChangeBackgroundColorColorutine);
            ChangeBackgroundColorColorutine = null;
        }
        ChangeBackgroundColorColorutine = StartCoroutine(changeBackgroundColor(fromColor, toColor, duration));

    }

    /// <summary>
    /// 핸들을 이동하는 함수
    /// </summary>
    /// <param name="fromPosition">핸들의 시작 위치</param>
    /// <param name="toPosition">핸들의 목적지 위치</param>
    /// <param name="duration">핸들이 이동하는 시간</param>
    /// <returns>없음</returns>
    IEnumerator moveHandle(Vector2 fromPosition, Vector2 toPosition, float duration)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float t = currentTime / duration;
            Vector2 newPosition = Vector2.Lerp(fromPosition, toPosition, t);
            handleRectTransform.anchoredPosition = newPosition;

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    /// 스위치 배경색 변경 함수
    /// </summary>
    /// <param name="fromColor">초기 색상</param>
    /// <param name="toColor">변경할 색상</param>
    /// <param name="duration">변경 시간</param>
    /// <returns>없음</returns>
    IEnumerator changeBackgroundColor(Color fromColor, Color toColor, float duration)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float t = currentTime / duration;
            Color newColor = Color.Lerp(fromColor, toColor, t);
            backgroundImage.color = newColor;

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
