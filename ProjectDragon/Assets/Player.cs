using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    public GameObject weaponSelection;

    //대각 속도
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 5.0f;
    public GameObject DeadPanel;


    //JoyStick
    protected JoyPad joyPad;
    public Animator playerAnimationStateChanger;
    // player controll vector
    public Vector2 playerControllerVec;
    public Rigidbody2D rigidbody2d;
    public GameObject joypadinput;
    public Vector3 joystickPos;
    private Vector3 normalVec = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
        HP = 100;
        ATKChanger(10);
        ATKSpeedChanger(1.0f);
        MoveSpeed = 1;
        myState = State.None;
        myAttackType = AttackType.None;
        AtkRangeChanger(3);
        myState = State.ATTACK;
        myAttackType = AttackType.SHORTRANGE;
     
        //myRotat = new Vector3(0, 0, 0);

        joyPad = FindObjectOfType<JoyPad>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        ////Battlemanager have

        //if(other ==null)
        //{
        //    other = 
        //}
      

        myPos = gameObject.transform.position;
        DistanceCheck(3.0f);
        joystickPos = joypadinput.GetComponent<UIJoystick>().position;
        //키보드 세팅
        //float h = horizontalSpeed * Input.GetAxis("Horizontal");
        //float v = verticalSpeed * Input.GetAxis("Vertical");
        float h = horizontalSpeed * joystickPos.x;
        float v = verticalSpeed * joystickPos.y;

        playerControllerVec = new Vector3(h, v, 0);

        transform.Translate(Vector2.right * Time.deltaTime * h, Space.World);
        transform.Translate(Vector2.up * Time.deltaTime * v, Space.World);

        //Angle of joystick and normalVec
        float Angle = GetAngle(joystickPos, normalVec);
        AnimationChanger(Angle);
        Debug.Log(Angle);
    }
    IEnumerator DeadFade()
    {
        yield return new WaitForSeconds(2.0f);
        DeadPanel.SetActive(true);
    }

    public static float GetAngle(Vector3 Start, Vector3 End)
    {
        Vector3 v = End - Start;

        return Quaternion.FromToRotation(Vector3.up, End - Start).eulerAngles.z;
    }

    #region Animation Changer
    public void CharacterAnimator(string animationtype)
    {
        //Animator[] childAnimator;
        //childAnimator = GetComponentsInChildren<Animator>();
        //foreach (Animator anim in childAnimator)
        //{
        gameObject.GetComponent<Animator>().Play(animationtype);
        //}
    }
    public void AnimationChanger(float angle)
    {
        #region SwordMan
        if (myAttackType.Equals(AttackType.SHORTRANGE))
        {
            switch (myState)
            {
                case State.WALK:
                    weaponSelection.SetActive(true);
                    if (angle == 0)
                    {

                    }
                    if (angle < 22.5)
                    {
                        CharacterAnimator("MaleSwordManWalkFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkFront");
                    }
                    else if (angle < 112.5)
                    {
                        CharacterAnimator("MaleSwordManWalkRight");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkRight");
                    }
                    else if (angle < 112.5 + 45)
                    {
                        CharacterAnimator("MaleSwordManWalkRightSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkRightSide");
                    }
                    else if (angle < 112.5 + 90)
                    {
                        CharacterAnimator("MaleSwordManWalkUp");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkUp");
                    }
                    else if (angle < 112.5 + 135)
                    {
                        CharacterAnimator("MaleSwordManWalkLeft");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkLeft");
                    }
                    else if (angle < 112.5 + 180)
                    {
                        CharacterAnimator("MaleSwordManWalkLeftSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkLeftSide");
                    }
                    else
                    {
                        CharacterAnimator("MaleSwordManWalkFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkFront");
                    }
                
                    break;

                case State.ATTACK:
                    weaponSelection.SetActive(true);
                    if (angle == 0)
                    {

                    }
                    if (angle < 22.5)
                    {
                        CharacterAnimator("MaleSwordManAttackFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordFront");
                    }
                    else if(angle < 112.5)
                    {
                        CharacterAnimator("MaleSwordManAttackRight");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordRight");
                    }
                    else if(angle < 112.5+45)
                    {
                        CharacterAnimator("MaleSwordManAttackRightSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordRightSide");
                    }
                    else if(angle < 112.5+90)
                    {
                        CharacterAnimator("MaleSwordManAttackUp");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordUp");
                    }
                    else if (angle < 112.5 + 135)
                    {
                        CharacterAnimator("MaleSwordManAttackLeftSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordLeftSide");
                    }
                    else if (angle < 112.5 + 180)
                    {
                        CharacterAnimator("MaleSwordManAttackLeft");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordLeft");
                    }
                    else
                    {
                        CharacterAnimator("MaleSwordManAttackFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordFront");
                    }
                    break;
                case State.DEAD:
                    CharacterAnimator("Dead");
                    joypadinput.SetActive(false);
                    weaponSelection.SetActive(false);
                    StartCoroutine("DeadFade");
                    break;
                case State.SKILL:
                    break;

            } 
        }
        #endregion
        #region Archer
        if (myAttackType.Equals(AttackType.MIDDLERANGE))
        {
            switch (myState)
            {
                case State.WALK:
                    weaponSelection.SetActive(true);
                    if (angle == 0)
                    {

                    }
                    if (angle < 22.5)
                    {
                        CharacterAnimator("MaleArcherManWalkFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkFront");
                    }
                    else if (angle < 112.5)
                    {
                        CharacterAnimator("MaleArcherManWalkRight");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkRight");
                        Debug.Log("Right");
                    }
                    else if (angle < 112.5 + 45)
                    {
                        CharacterAnimator("MaleArcherManWalkRightSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkRightSide");
                    }
                    else if (angle < 112.5 + 90)
                    {
                        CharacterAnimator("MaleArcherManWalkUp");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkUp");
                    }
                    else if (angle < 112.5 + 135)
                    {
                        CharacterAnimator("MaleArcherManWalkLeftSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkLeft");
                    }
                    else if (angle < 112.5 + 180)
                    {
                        CharacterAnimator("MaleArcherManWalkLeft");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkLeftSide");
                    }
                    else
                    {
                        CharacterAnimator("MaleArcherManWalkFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordWalkFront");
                    }

                    break;

                case State.ATTACK:
                    weaponSelection.SetActive(true);
                    if (angle == 0)
                    {

                    }
                    if (angle < 22.5)
                    {
                        CharacterAnimator("MaleArcherManAttackFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordFront");
                    }
                    else if (angle < 112.5)
                    {
                        CharacterAnimator("MaleArcherManAttackRight");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordRight");
                    }
                    else if (angle < 112.5 + 45)
                    {
                        CharacterAnimator("MaleArcherManAttackRightSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordRightSide");
                    }
                    else if (angle < 112.5 + 90)
                    {
                        CharacterAnimator("MaleArcherManAttackUp");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordUp");
                    }
                    else if (angle < 112.5 + 135)
                    {
                        CharacterAnimator("MaleArcherManAttackLeftSide");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordLeftSide");
                    }
                    else if (angle < 112.5 + 180)
                    {
                        CharacterAnimator("MaleArcherManAttackLeft");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordLeft");
                    }
                    else
                    {
                        CharacterAnimator("MaleArcherManAttackFront");
                        weaponSelection.GetComponent<Animator>().Play("NormalSwordFront");
                    }
                    break;
                case State.DEAD:
                    CharacterAnimator("Dead");
                    joypadinput.SetActive(false);
                    weaponSelection.SetActive(false);
                    StartCoroutine("DeadFade");
                    break;
                case State.SKILL:
                    break;

            }
        }
        #endregion
    }
    #endregion
}

