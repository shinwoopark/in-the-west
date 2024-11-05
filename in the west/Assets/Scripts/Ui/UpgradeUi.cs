using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeUi : MonoBehaviour
{
    public Image[] Images;
    public TextMeshPro[] Texts;

    public Sprite[] ModuleImages;

    private List<string> _explanationModules = new List<string>
    {
        "�⺻ ���� �� ������ �Ѿ���\n�߰� ���ظ� �ݴϴ�",
        "����ź�� ���ظ� �����ϴ�",
        "���� Ȯ���� �⺻ ������\n���� ��� ��ŵ�ϴ� (���� X)",
        "������ ���� �⺻ ������\n�߰� ���ظ� �ݴϴ�",
        "ü���� 1�̸�\n�⺻ ������ ��ȭ�˴ϴ�",
        "�� óġ�� ������ ȹ��\nȮ���� �����մϴ�",
        "����ź�� ��Ÿ� �����մϴ�",
        "���� Ȯ���� ������ ����\n����ź�� ����ϴ�",
        "���� �ð����� �����⸦\n������� ������\n���� �����Ⱑ ���� ���ĳ��ϴ�",
        "���� �������� ȹ���մϴ�",
        "ü���� 1ȸ���մϴ�",
        "���� ������ �� ��� �������� 1�� ȹ���մϴ�"
    };

    private int[] _currentModules;

    public void SetModule(int[] modules, int disposableModule)
    {
        for (int i = 0; i < modules.Length; i++)
        {
            _currentModules[i] = modules[i];
            Images[i].sprite = ModuleImages[modules[i]];
            Texts[i].text = _explanationModules[modules[i]];
        }

        if (disposableModule != 0)
        {
            int changeModule = Random.Range(0, 3);

            _currentModules[changeModule] = disposableModule;
            Images[changeModule].sprite = ModuleImages[disposableModule];
            Texts[changeModule].text = _explanationModules[disposableModule];
        }
    }

    private void CloseShop()
    {
        GameInstance.instance.bUpgrading = false;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    //Buttons

    public void ChooseModules(int number)
    {
        if (_currentModules[number] < UpgradeManager.upgradeManager.UpgradeModule.Count)
        {
            UpgradeManager.upgradeManager.UpgradeModule[_currentModules[number]]++;
        }
        else
        {
            switch (_currentModules[number])
            {
                case 9:
                    GameInstance.instance.Item4 = true;
                    break;
                case 10:
                    GameInstance.instance.PlayerHp++;
                    break;
                case 11:
                    for (int i = 0; i < GameInstance.instance.ItemInventroy.Length; i++)
                        GameInstance.instance.ItemInventroy[i]++;
                    break;
            }
        }
    }
}