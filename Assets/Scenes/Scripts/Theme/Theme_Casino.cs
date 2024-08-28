using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Theme_Casino : MonoBehaviour
{
    // ThemeManager로 Theme프리팹을 ThemeManager 자식으로 추가/제거하는 방식
    // 턴 시작 시 GameManager에서 ThemeManager 호출해서 턴 시작 시 효과 호출할 것
    // 무력감 디버프 추가할 것
    // 콜 효과 제작할 것
    private ThemeManager _ThemeManager;

    public int[] playingCard = new int[5];     // 빈 플래잉 카드는 -1로 표기 (적:1, 청:2, 황:3, 백:4)
    private int playingCardNum = 0;

    public SkillData uniqueSkill_01;
    public SkillData uniqueSkill_02;

    public bool onShuffle;
    private bool onCardTrick;

    void Awake()
    {
        _ThemeManager = transform.GetComponentInParent<ThemeManager>();

        for (int i = 0; i < 5; i++) {
            playingCard[i] = -1;
        }
        playingCardNum = 0;

        onShuffle = false;
        onCardTrick = false;
    }

    private void FixedUpdate() {
        // 드로우
        if (_ThemeManager.onTurnEffect) {
            _ThemeManager.onTurnEffect = false;

            if (playingCardNum < 5) {
                playingCard[playingCardNum] = UnityEngine.Random.Range(1, 5);

                Debug.Log("플레잉카드_드로우!\n" + playingCard[playingCardNum]);

                playingCardNum++;
            }
        }

        // 테마 스킬 취소 확인
        if (onShuffle || onCardTrick) {
            if (!_ThemeManager.onThemeSkill) {
                onShuffle = false;
                onCardTrick = false;
            }
        }

        // 테마 스킬 사용 확인
        if (_ThemeManager.useThemeSkill) {
            // 셔플 확인
            if (onShuffle) {
                for (int i = 0; i < _ThemeManager.usedPaintNum - 1; i++) 
                {
                    if (playingCardNum >= 5) {
                        break;
                    }
                    playingCard[playingCardNum] = UnityEngine.Random.Range(1, 5);

                    Debug.Log("플레잉카드_셔플!\n" + playingCard[playingCardNum]);

                    playingCardNum++;
                }
                // 셔플 효과 완료
                onShuffle = false;
            }
            // 카드 트릭 확인
            else if (onCardTrick) {
                if (playingCardNum < 5 && _ThemeManager.colorType_FirstSub != 0) {
                    for (int i = 0; i < 2; i++)
                    {
                        if (playingCardNum >= 5) {
                            break;
                        }

                        playingCard[playingCardNum] = _ThemeManager.colorType_FirstSub;

                        Debug.Log("플레잉카드_카드 트릭!\n" + playingCard[playingCardNum]);

                        playingCardNum++;
                    }
                }
            }
            _ThemeManager.onThemeSkill = false;
            _ThemeManager.useThemeSkill = false;
        }
        
        // 콜 : 플레잉카드 확인 후 효과 발동
        if (playingCardNum >= 5) {
            Debug.Log("플레잉카드_콜!");

            // 콜 시 효과 제작할 것

            for (int i = 0; i < 5; i++)
            {
                playingCard[i] = -1;
            }
            playingCardNum = 0;
        }
    }

    public void UseUniqueSkill_01()
    {
        // 스킬 설정
        GameManager.instance.usingSkill = uniqueSkill_01;

        // 메인 팔레트 설정 (흰색 1)
        GameManager.instance._PaintScripts[3].UseThemeSkill(4, 1);     // (paintType, 물감 요구치)

        // 테마 스킬 On
        _ThemeManager.onThemeSkill = true;
        onShuffle = true;
    }

    public void UseUniqueSkill_02()
    {
        // 스킬 설정
        GameManager.instance.usingSkill = uniqueSkill_02;

        // 메인 팔레트 설정 (흰색 2)
        GameManager.instance._PaintScripts[3].UseThemeSkill(4, 2);     // (paintType, 물감 요구치)

        // 테마 스킬 On
        _ThemeManager.onThemeSkill = true;
        onCardTrick = true;
    }
}
