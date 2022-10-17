using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill3AnalogController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Vector2 lastJoystickDirection;
    private Rigidbody2D playerRb;
    public List<Collider2D> allTargetAllWithinArea;

    public FixedJoystick joystick;
    public GameObject visualCircleRadius;
    public Transform[] visualCircleArea;
    public Transform player;
    public GameObject playerClonePrefab;
    public Image cooldownImg;
    public float currentCd;

    private void Awake()
    {
        visualCircleRadius.SetActive(false);
    }

    private void Update()
    {
        if (visualCircleRadius.activeInHierarchy)
        {
            visualCircleRadius.transform.position = player.position;
            //Vector3 moveVector = (Vector3.up * joystick.Horizontal + Vector3.left * joystick.Vertical);
            Vector3 moveVector = joystick.Direction * PlayerTechTreeSkillManager.Instance.skill3Range;

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                visualCircleArea[(int)PlayerTechTreeSkillManager.Instance.skill3AreaType].position = moveVector + visualCircleRadius.transform.position;
            }
        }

        if (currentCd > 0)
        {
            currentCd -= Time.deltaTime;
            cooldownImg.fillAmount = 1 - (currentCd / PlayerTechTreeSkillManager.Instance.skill3Cd);
        }
    }

    private void ShowVisualSkillEffectRange()
    {
        visualCircleRadius.SetActive(true);

        foreach (var item in visualCircleArea)
        {
            item.gameObject.SetActive(false);
        }

        visualCircleArea[(int)PlayerTechTreeSkillManager.Instance.skill3AreaType].gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentCd <= 0)
        {
            ShowVisualSkillEffectRange();
            PlayerTechTreeSkillManager.Instance.OpenSkillCancelButton();
        }
        else
        {
            GameplaySceneController.Instance.ShowPromptMessage("Skill is Cooldown!");
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentCd > 0) return;
        if (!PlayerTechTreeSkillManager.Instance.isAbilityCanceled)
        {
            InitiateSkill3Effects();
        }
        else
        {
            print("Skill Canceled");
        }

        visualCircleRadius.SetActive(false);
        PlayerTechTreeSkillManager.Instance.CloseSkillCancelButton();
    }

    private void InitiateSkill3Effects()
    {
        currentCd = PlayerTechTreeSkillManager.Instance.skill3Cd;
        cooldownImg.fillAmount = 1;

        if (PlayerTechTreeSkillManager.Instance.skill3AreaType == PlayerTechTreeSkillManager.SkillType.B)
        {
            Instantiate(playerClonePrefab, visualCircleArea[(int)PlayerTechTreeSkillManager.Instance.skill3AreaType].position, Quaternion.identity);
        }
        else
        {
            int counter = 0;
            List<GameObject> deathRow = new List<GameObject>();

            if (allTargetAllWithinArea.Count == 0) return;

            foreach (var item in allTargetAllWithinArea)
            {
                deathRow.Add(item.gameObject);
                counter++;
                if (counter == PlayerTechTreeSkillManager.Instance.skill3MaxTarget) break;
            }

            foreach (var item in deathRow)
            {
                Destroy(item);
            }
        }        
    }
}
