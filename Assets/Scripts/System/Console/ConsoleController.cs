using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SM;
using Condition;

public class ConsoleController : MonoBehaviour
{
    public static ConsoleController CC;

    StateMachine ConsoleSM;
    [HideInInspector]
    public bool isConsoleActive = false;

    public bool ShowConsoleOnNewMessage = false;

    public enum LogType { Standard, Command, Error }

    public List<Color> TextColours = new List<Color>();

    public Text LogText;
    public InputField ConsoleInput;
    public GameObject Scrollbar;
    public Image LogWindow;
    ConsoleFunctions conFuncs;

    public GameObject ConsoleObject;

    List<string> CommandHistory = new List<string>();
    int CommandHistItr = 0;

    bool showLogScreen = false;
    float ShowLogTime = 5;
    float LogScreenTimer = 0;

    void Start()
    {
        CC = this;
        conFuncs = new ConsoleFunctions();
        conFuncs.CC = this;
        LogText.supportRichText = true;
        setupStateMachine();
        conFuncs.Help();
    }

    void Update()
    {
        ConsoleSM.SMUpdate();       
    }

    void GetActiveConsoleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --CommandHistItr;
            UpdateInputText();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++CommandHistItr;
            UpdateInputText();
        }
    }

    void UpdateInputText()
    {
        CommandHistItr = Mathf.Clamp(CommandHistItr, 0, CommandHistory.Count);
        if(CommandHistItr < CommandHistory.Count)
        {
            ConsoleInput.text = CommandHistory[CommandHistItr];
            ConsoleInput.caretPosition = ConsoleInput.text.Length;
        }
        else
        {
            ConsoleInput.text = "";
        }
    }

    public void EndEdit(string Text)
    {
        if (Text != "" && Text != "`")
        {
            LogCommand(Text);
            // Find if there is a function related to the command.
            MethodSearch(Text);

            CleanInputField();
        }
    }

    void MethodSearch(string command)
    {
        string[] commandParts = command.Split(' ');
        MethodInfo theMethod = typeof(ConsoleFunctions).GetMethod(commandParts[0], BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        if(theMethod != null)
        {
            object[] MethodArgs = GetMethodArgs(commandParts);
            ParameterInfo[] methodParams = theMethod.GetParameters();
            if (methodParams.Length == MethodArgs.Length)
            {
                theMethod.Invoke(conFuncs, MethodArgs);
            }
            else
            {
                PrintToConsole((methodParams.Length > MethodArgs.Length ? "Missing" : "Too Many") + " Method Arguments\nMethod Requires " + methodParams.Length + " you supplied " + MethodArgs.Length, LogType.Error);
            }
        }
        else
        {
            PrintToConsole("Invalid method : " + commandParts[0], LogType.Error);
            //conFuncs.Help();
        }
    }

    object[] GetMethodArgs(string[] commandParts)
    {
        List<object> args = new List<object>();
        if(commandParts.Length > 1)
        {
            for(int i = 1; i < commandParts.Length; ++i)
            {
                args.Add(commandParts[i]);
            }
        }
        return args.ToArray();
    }

    void CleanInputField()
    {
        ConsoleInput.text = "";
        ConsoleInput.Select();
        ConsoleInput.ActivateInputField();
    }
    
    public void PrintToConsole(string text, LogType logType = LogType.Standard)
    {
        string RichTextPart1 = "<color=";
        string RichTextPart2 = "</color>";

        RichTextPart1 += ColourToHex(TextColours[(int)logType]) + ">";

        LogText.text += "\n" + RichTextPart1 + text + RichTextPart2;

        if(ConsoleSM.CurrentState.Name == "In-Active" && ShowConsoleOnNewMessage)
        {
            showLogScreen = true;
        }
    }

    string ColourToHex(Color col)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}", (byte)(Mathf.Clamp01(col.r) * 255), (byte)(Mathf.Clamp01(col.g) * 255), (byte)(Mathf.Clamp01(col.b) * 255));
    }

    void LogCommand(string command)
    {
        // print the command to the console window.
        PrintToConsole(command, LogType.Command);
        // add command to stack of commands.
        CommandHistory.Add(command);
        CommandHistItr = CommandHistory.Count;
    }

    bool IsConsoleActive()
    {
        return isConsoleActive;
    }

    bool ShowLogScreen()
    {
        return showLogScreen;
    }

    bool HideLogScreen()
    {
        if(LogScreenTimer >= ShowLogTime || isConsoleActive)
        {
            LogScreenTimer = 0;
            showLogScreen = false;
            return true;
        }
        return false;
    }

    void BeginLogScreen()
    {
        ConsoleInput.gameObject.SetActive(false);
        Scrollbar.gameObject.SetActive(false);

        ConsoleObject.SetActive(true);

        // Lower the alpha level of the log window.
        LogText.color = new Color(LogText.color.r, LogText.color.g, LogText.color.b, 0.5f);
        LogWindow.color = new Color(LogWindow.color.r, LogWindow.color.g, LogWindow.color.b, 0.5f);
    }

    void LogScreenUpdate()
    {
        LogScreenTimer += Time.deltaTime;
    }

    void EndLogScreen()
    {
        ConsoleInput.gameObject.SetActive(true);
        Scrollbar.gameObject.SetActive(true);

        ConsoleObject.SetActive(false);

        // Lower the alpha level of the log window.
        LogText.color = new Color(LogText.color.r, LogText.color.g, LogText.color.b, 1);
        LogWindow.color = new Color(LogWindow.color.r, LogWindow.color.g, LogWindow.color.b, 1);
    }

    void ActivateConsole()
    {
        ConsoleObject.SetActive(true);
        CleanInputField();
        Time.timeScale = 0;
    }

    void DeactivateConsole()
    {
        ConsoleObject.SetActive(false);
        Time.timeScale = 1;
    }

    void setupStateMachine()
    {
        // Conditions
        BoolCondition isConsoleActive = new BoolCondition();
        isConsoleActive.Condition = IsConsoleActive;
        NotCondition notIsConsoleActive = new NotCondition();
        notIsConsoleActive.Condition = isConsoleActive;
        BoolCondition ShowLogScreenCond = new BoolCondition(ShowLogScreen);
        BoolCondition HideLogScreenCond = new BoolCondition(HideLogScreen);

        // Transitions
        Transition ActivateConsoleTransFunc = new Transition("Activate Console", isConsoleActive, ActivateConsole);
        Transition DeactivateConsoleTransFunc = new Transition("de-Activate Console", notIsConsoleActive, DeactivateConsole);
        Transition GotoLogScreen = new Transition("Go to Log Screen", ShowLogScreenCond);
        Transition LeaveLogScreen = new Transition("Leave Log Screen", HideLogScreenCond);

        // States
        State Active = new State("Active",
            new List<Transition>() { DeactivateConsoleTransFunc },
            null,
            new List<Action>() { GetActiveConsoleInput },
            null);

        State InActive = new State("In-Active",
            new List<Transition>() { ActivateConsoleTransFunc, GotoLogScreen },
            null,
            null,
            null);

        State ShowLog = new State("Show Log",
            new List<Transition>() { LeaveLogScreen },
            new List<Action>() { BeginLogScreen },
            new List<Action>() { LogScreenUpdate },
            new List<Action>() { EndLogScreen });

        // Set target States for transitions
        ActivateConsoleTransFunc.SetTargetState(Active);
        DeactivateConsoleTransFunc.SetTargetState(InActive);
        GotoLogScreen.SetTargetState(ShowLog);
        LeaveLogScreen.SetTargetState(InActive);

        // Create the machine and initialise it.
        ConsoleSM = new StateMachine(null, InActive, Active, ShowLog);
        ConsoleSM.InitMachine();
    }

}
