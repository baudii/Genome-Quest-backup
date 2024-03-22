using Cinemachine;
using System.Collections;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cabinetCamera;
    [SerializeField] CinemachineVirtualCamera introCamera;
    [SerializeField] Animator blackScreenAnimator;
    [SerializeField] AudioCrossFade crossFade;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] UI_DialogueController inboxController;
    [SerializeField] Speaker speaker;
    IEnumerator Start()
    {
        playerMovement.FreezeInput();
        playerMovement.SetFlipState(-1);
        introCamera.Priority = 1000;
        blackScreenAnimator.gameObject.SetActive(true);
        crossFade.MuteSource();
        yield return new WaitForSeconds(3);
        blackScreenAnimator.speed = 0.1f;
        blackScreenAnimator.Play("BlackScreenFadeOut");
        yield return new WaitForSeconds(3);
        introCamera.Priority = 0;
        cabinetCamera.Priority = 10;
        yield return new WaitForSeconds(7);
        inboxController.InitiateDialogue(false);
        speaker.StartAnimation();
        bool canContinue = false;
        inboxController.GenerateOutput("Doctor Lee, urgent attention needed in Patient Room. New critical patient admitted. Your expertise required for immediate assessment and treatment. Please proceed promptly.", 
        () => {
            speaker.StopAnimation();
            canContinue = true;
        });
        yield return new WaitUntil(() => canContinue);
        yield return new WaitUntil(() => Input.anyKeyDown);
        inboxController.CloseDialogue();
        Objective.Instance.ObjectiveGoToPatient();
        Destroy(gameObject);
    }
}
