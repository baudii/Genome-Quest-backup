using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using OpenAI;
using TMPro;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CharacterDialogueComponent : MonoBehaviour
{
    [SerializeField] bool testRequest;
    [SerializeField] CharacterDialogueComponent sameNPCinFutureDay;
    [SerializeField] ContextAction[] contextActions;
    [SerializeField] SO_CharacterInfo characterInfo;
    [SerializeField, TextArea(10,40)] string instructionForDay;
    OpenAIApi openai = new OpenAIApi();
    List<ChatMessage> messages;

    string context;
    string instructions;
    string daySpecificContext;
    bool daySpecificInstructionsSent;
    string liamsCondition;
    void Start()
    {
        if (testRequest)
        {
            TestRequest();
            return;
        }

        messages = new List<ChatMessage>();
        context = $"Character description: {characterInfo._description}";
/*        instructions = "###Instructions: You are pretending to be " + characterInfo._name + ". I am Dr. Lee. You must limit your knowledge to the knowledge of " + characterInfo._name
            + " within the give context.\n Assess the incoming message on a scale of 1 to 4. Where: \n1) Message is nonsensical, has no meaning\n2) Message is conceivable, but was it was addressed to the AI and not {" + "\n3) Message is conceivable and was addressed to the character, " +
            "but has typos that can be decoded\n4) Message " +
            "was addressed to the character and has no typos\nMessage that must be evaluated will be tagged like this: \"<msg>:\". " +
            "In your response, include your assessment exactly as a first symbol and then the content of response. It must look exactly like this example: \"1the content of response\"";*/
        instructions = $"Instructions: You are pretending to be game character {characterInfo._name}. Player is Dr. Lee. Your relationship with player: he is your {characterInfo._relationToPlayer}. " +
            $"You must limit your knowledge to the knowledge of {characterInfo._name} within the given context. " +
            $"You can support small-talk. If user greets you, you must greet him back. If he agrees with you, you must generate random response within a context\n" +
            $"Assess the incoming message on this scale of 1 to 5:" +
            $"1: message is good and mostly in context of the game world," +
            $"2: message is all right, and something someone might say but unlikely," +
            $"3: message is bad, unnecessarily vulgar for no reason, based on the past conversation," +
            $"4: message is immersion breaking or meta and acknowledging this is a game," +
            $"5: message is non-sensical in any context, you can't understand its meaning. " +
            $"Message that must be evaluated will be tagged like this: \"<msg>:\". Write your assessment as the first character. " +
            $"Examples: \"4) I'm feeling quite unwell today, doctor\", \"5) Thank you very much, Lee!\"";
    }

    async void TestRequest()
    {
        var testMessage = new ChatMessage()
        {
            Role = "user",
            Content = "test"
        };

        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo",
            Messages = new List<ChatMessage> { testMessage },
            MaxTokens = 10,
        }); 
        
        if (completionResponse.Choices == null || completionResponse.Choices.Count <= 0) 
        {
            ErrorHandler.Instance.EnableErrorMessage();
        }
    }
    async Task SendRequest(string message, bool isGPTfirst = false, float temperature = 1.3f)
    {
        if (!isGPTfirst)
            message = "<msg>:" + message;

        var chatMessage = new ChatMessage() { Role = "user", Content = message };

        if (messages.Count == 0)
        {
            chatMessage.Content = context + "\n" + daySpecificContext + "\n" + instructions + "\n" + message;
        }
        else if (!daySpecificInstructionsSent)
        {
            chatMessage.Content = daySpecificContext + "\n" + message;
            daySpecificInstructionsSent = true;
        }

        messages.Add(chatMessage);

        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo",
            Messages = messages,
            MaxTokens = 200,
            Temperature = temperature
        });

        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
        {
            var response = completionResponse.Choices[0].Message;
            response.Content = response.Content.Trim();

            print(response.Content);
            var output = response.Content;
            if (!isGPTfirst)
            {
                char rating = response.Content[0];
                if (rating == '4' || rating == '5')
                {
                    var badResponse = new ChatMessage()
                    {
                        Content = "Can't answer that",
                        Role = "assistant"
                    };
                    messages.Add(badResponse);
                    DisplayResponse(badResponse.Content);
                    return;
                }
                output = response.Content.Remove(0, 3);
            }

            CheckContext(output);
            DisplayResponse(output);

            messages.Add(response);
        }
        else
        {
            ErrorHandler.Instance.EnableErrorMessage();
        }
    }

    bool lineUsed;
    public void StartDialogue(string line = "")
    {
        var dc = UI_DialogueController.Instance;

        dc.InitiateDialogue(OnInputSubmit: (message) =>
        {
            _ = SendRequest(message);
        });

        if (line != "" && !lineUsed)
        {
            lineUsed = true;
            /*            lineUsed = true;
                        var lines = line.Split('|');
                        var messageFromAI = "1) " + line.Replace('|', ' ');
                        print("ta: " + messageFromAI);
                        if (messages.Count == 0)
                        {
                            messages.Add(new ChatMessage()
                            {
                                Role = "user",
                                Content = context + "\n" + daySpecificContext + "\n" + instructions + "\n" + "<msg>:Hello"
                            });
                        }
                        messages.Add(new ChatMessage()
                        {
                            Role = "assistant",
                            Content = messageFromAI
                        });
                        if (lines.Length <= 1)
                            dc.GenerateOutput(line);
                        else
                            StartCoroutine(PrintStartingLines(lines, dc));*/
            dc.Wait();
            _ = SendRequest("You start the conversation with a line within a given context. Your line should be similar to this: " + line, true, 0.3f);
        }
    }

    public void Init()
    {
        DaysSystem.Instance.OnDayStart.AddListener(SetDaySpecificInstructions);
        DaysSystem.Instance.OnDayEnd.AddListener(TransferMessages);
    }

    private void OnDestroy()
    {
        DaysSystem.Instance.OnDayStart.RemoveListener(SetDaySpecificInstructions);
    }

    

    public void RecieveMessages(List<ChatMessage> prevMessages)
    {
        messages = prevMessages;
    }

    public void TransferMessages()
    {
        if (sameNPCinFutureDay != null)
        {
            sameNPCinFutureDay.RecieveMessages(messages);
        }
    }

    public void SetDaySpecificInstructions(int day, int correctGuesses, int previousDayGuesses)
    {
        if (day == 0)
        {
            if (instructionForDay == "" || instructionForDay == null)
                return;

            daySpecificContext = "Context: " + instructionForDay;
            daySpecificInstructionsSent = true;
            return;
        }

        string condition = "feeling the same as yesterday. Dr. Lee needs to change some of the treatments";
        if (correctGuesses > previousDayGuesses)
        {
            if (correctGuesses == 3)
                condition = "feeling much better. Dr. Lee found the correct strategy with his treatments. He just needs to keep it until day 3";
            else 
                condition = "feeling better than yesterday. Dr. Lee is on the right track with his treatments, but some chosen medications should be changed";
        }
        else if (correctGuesses < previousDayGuesses)
        {
            condition = "feeling worse than yesterday. Dr. Lee removed some treatment that worked";
        }

        string daysPassed = day + " days";
        if (day == 1)
        {
            daysPassed = day + " day";
        }

        daySpecificInstructionsSent = false;
        daySpecificContext = $"Context: {daysPassed} passed since Liam arrived at Clinic. Today he is {condition}. Doctor Lee got only {4-day} days to find him a cure until he's redirected" +
            $" to another hospital. {instructionForDay}";
    }

/*    void OnCloseDialogue()
    {
        if (lineBeforeLeave == null || lineBeforeLeave == "")
            return;

        var dc = UI_DialogueController.Instance;
        dc.InitiateDialogue(canAnswer: false);
        dc.GenerateOutput(lineBeforeLeave, dc.CloseDialogue);
    }*/

    IEnumerator PrintStartingLines(string[] lines, UI_DialogueController dc)
    {
        foreach (var line in lines)
        {
            bool canContinue = false;
            dc.GenerateOutput(line, () =>  canContinue = true);
            yield return new WaitUntil(() => canContinue);
            dc.DisableInputField();
            yield return null;
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        yield return null;
        dc.EnableInputField();
    }

    public void DisplayResponse(string response)
    {
        UI_DialogueController.Instance.GenerateOutput(response);
    }

    public void EndDialogue()
    {
        UI_DialogueController.Instance.CloseDialogue();
    }

    

    void CheckContext(string response)
    {
        foreach (var contextAction in contextActions)
        {
            bool foundContext = true;
            foreach (var keyword in contextAction.contextKeywords)
            {
                if (response.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    foundContext = false;
                    break;
                }
            }
            if (foundContext)
                contextAction.action?.Invoke();
        }
    }
}
[Serializable]
public class ContextAction
{
    public string[] contextKeywords;
    public UnityEvent action;
}
