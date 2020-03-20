﻿using HorseMoon.Inventory;
using HorseMoon.Inventory.UI;
using HorseMoon.Objects;
using HorseMoon.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

namespace HorseMoon.Speech
{
    public class SpeechUIBehavior : DialogueUIBehaviour
    {
        public DialogueRunner runner;
        public VariableStorageBehaviour variableStorage;

        public Canvas speechCanvas;
        public RawImage bgFade;
        public SpeechBox speech;
        public SpeakerNameBox speakerName;
        public SpeechCharacter leftCharacter;
        public SpeechCharacter rightCharacter;
        public OptionBox optionBox;

        private bool continueLine;
        private bool showingPopup;

        public bool InDialogue => runner.isDialogueRunning || showingPopup;

        public Action SpeechStarted;
        public Action SpeechEnded;

        private void Start()
        {
            optionBox.Hide();
            
            RegisterCommands();
            RegisterFunctions();
            LoadPrograms();
        }

        private void Update()
        {
            if (InDialogue)
            {
                // Press [Use] to continue. -->
                if (Input.GetButtonDown("Use") || Input.GetButtonDown("Cancel")
                || (Input.GetKey(KeyCode.BackQuote) && Input.GetButton("Cancel")))
                {
                    continueLine = true;
                }

                // TEMP -->
                if (showingPopup && continueLine)
                    EndPopup();
            }
            else
            {
                // DEBUG -->
                if (Input.GetKey(KeyCode.BackQuote))
                {
                    if (Input.GetKeyDown(KeyCode.Y))
                        LoadPrograms();

                    if (Input.GetKeyDown(KeyCode.Alpha1))
                        StartDialogue("SpeechTest");
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                        StartDialogue("Intro");
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                        StartDialogue("Thinking");
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                        ShowPopup("This is a message written on the fly. Press your button to close it.");
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                        StartDialogue("ItemDebug");
                    else if (Input.GetKeyDown(KeyCode.Alpha6))
                        StartDialogue("Expr");
                    else if (Input.GetKeyDown(KeyCode.Alpha7))
                        StartDialogue("Music");
                    else if (Input.GetKeyDown(KeyCode.Alpha8))
                        StartDialogue("ChatProgress");
                }
            }
        }

        private void RegisterCommands()
        {
            // leftCharacter <character> -->
            runner.AddCommandHandler("leftCharacter", delegate (string[] p)
            {
                leftCharacter.DataName = p[0];
            });

            // rightCharacter <character> -->
            runner.AddCommandHandler("rightCharacter", delegate (string[] p)
            {
                rightCharacter.DataName = p[0];
            });

            // expression <character> [expression] -->
            runner.AddCommandHandler("expression", delegate (string[] p)
            {
                if (p.Length > 0)
                {
                    SpeechCharacter sc = GetCharacter(p[0]);

                    if (sc != null)
                    {
                        if (p.Length > 1)
                            sc.Expression = p[1];
                        else
                            sc.Expression = "";
                    }
                }
            });

            // speaker [character] -->
            runner.AddCommandHandler("speaker", delegate (string[] p)
            {
                SpeechCharacter sc = null;
                bool lSpeak = false;
                bool rSpeak = false;
                speakerName.Text = "";

                if (p != null)
                {
                    if (IsLeftCharacter(p[0]))
                    {
                        sc = leftCharacter;
                        lSpeak = true;
                        speakerName.BoxLocation = SpeakerNameBox.Location.Left;
                    }
                    else if (IsRightCharacter(p[0]))
                    {
                        sc = rightCharacter;
                        rSpeak = true;
                        speakerName.BoxLocation = SpeakerNameBox.Location.Right;
                    }

                    if (sc != null)
                    {
                        speakerName.Text = sc.Data.names[0];

                        // Tender Till is a special case... -->
                        if (sc.DataName.Equals("TenderTill"))
                        {
                            if (StoryProgress.Instance.GetBool("TenderMet"))
                            {
                                if (StoryProgress.Instance.GetBool("Nicknamed"))
                                    speakerName.Text = sc.Data.names[1];
                            }
                            else
                                speakerName.Text = sc.Data.names[2];
                        }
                    }
                }
                
                leftCharacter.Speaking = lSpeak;
                rightCharacter.Speaking = rSpeak;
            });

            // progress <varName> <value> -->
            runner.AddCommandHandler("progress", delegate (string[] p) {
                StoryProgress.Instance.Set(p[0], p[1]);
            });

            // progressInc <varName> -->
            runner.AddCommandHandler("progressInc", delegate (string[] p) {
                StoryProgress.Instance.Set(p[0], StoryProgress.Instance.GetInt(p[0]) + 1);
            });

            // action <type> <function> -->
            runner.AddCommandHandler("action", delegate (string[] p)
            {
                // It's still bad!!! -->
                MonoBehaviour behaviour = (MonoBehaviour)FindObjectOfType(Type.GetType(p[0]));
                behaviour.Invoke(p[1], 0f);
            });

            // give <item> [quantity] -->
            runner.AddCommandHandler("give", delegate (string[] p) {
                Player.Instance.bag.Add(p[0], p.Length > 1 ? int.Parse(p[1]) : 1);
            });

            // take <item> [quantity] -->
            runner.AddCommandHandler("take", delegate (string[] p)
            {
                Item i = Player.Instance.bag.Get(p[0]);
                if (i == null)
                    return;
                if (p.Length > 1)
                    Player.Instance.bag.Remove(i.info, int.Parse(p[1]));
                else
                    Player.Instance.bag.Remove(i);
            });

            // takeAll -->
            runner.AddCommandHandler("takeAll", delegate (string[] p) {
                Player.Instance.bag.Clear();
            });

            // addMoney <amount> -->
            runner.AddCommandHandler("addMoney", delegate (string[] p) {
                ScoreManager.Instance.Money += int.Parse(p[0]);
            });

            // addWood <amount> -->
            runner.AddCommandHandler("addWood", delegate (string[] p) {
                ScoreManager.Instance.wood += int.Parse(p[0]);
            });

            // addStones <amount> -->
            runner.AddCommandHandler("addStones", delegate (string[] p) {
                ScoreManager.Instance.stones += int.Parse(p[0]);
            });
            
            runner.AddCommandHandler("till", delegate(string[] p) {
                for (int i = 0; i < p.Length; i++) {
                    p[i] = CheckVars(p[i]);
                }
                FindObjectOfType<Till>().HandleYarnCommand(p);
            });

            // music [musicRefName] -->
            runner.AddCommandHandler("music", delegate (string[] p)
            {
                if (p != null)
                {
                    MusicRef musicRef = Resources.Load<MusicRef>($"MusicRef/{p[0]}");
                    if (musicRef != null)
                        FindObjectOfType<MusicPlayer>().PlaySong(musicRef.clip);
                }
                else
                    LocationController.Instance.PlayCurrentMusic();
            });

            // debugChatProgress <progress> -->
            runner.AddCommandHandler("debugChatProgress", delegate (string[] p)
            {
                if (p != null)
                    StoryProgress.Instance.Set("TTChatProgress", int.Parse(p[0]));
                StoryProgress.Instance.OnDayPassed();
            });

            // maxBagWeight -->
            runner.AddCommandHandler("maxBagWeight", delegate (string[] p) {
                Player.Instance.bag.MaxWeight = int.MaxValue;
            });
        }

        private void RegisterFunctions()
        {
            runner.RegisterFunction("getProgressString", 1, delegate (Yarn.Value[] p)
            {
                return StoryProgress.Instance.GetString(p[0].AsString);
            });

            runner.RegisterFunction("getProgressInt", 1, delegate (Yarn.Value[] p)
            {
                return StoryProgress.Instance.GetInt(p[0].AsString);
            });
            
            runner.RegisterFunction("getProgressBool", 1, delegate (Yarn.Value[] p)
            {
                return StoryProgress.Instance.GetBool(p[0].AsString);
            });

            runner.RegisterFunction("canGive", 2, delegate(Yarn.Value[] p)
            {
                return Player.Instance.bag.CanAdd(p[0].AsString, (int)p[1].AsNumber);
            });

            runner.RegisterFunction("has", 2, delegate (Yarn.Value[] p)
            {
                Item i = Player.Instance.bag.Get(p[0].AsString);
                return i != null && i.Quantity >= (int)p[1].AsNumber;
            });

            runner.RegisterFunction("hasAny", 1, delegate (Yarn.Value[] p)
            {
                Item i = Player.Instance.bag.Get(p[0].AsString);
                return i != null;
            });

            runner.RegisterFunction("getItemCount", 0, delegate (Yarn.Value[] p)
            {
                Item i = Player.Instance.bag.Get(p[0].AsString);

                if (i != null)
                    return i.Quantity;
                return 0;
            });

            runner.RegisterFunction("getItemName", 0, delegate (Yarn.Value[] p)
            {
                ItemInfo info = ItemInfo.Get(p[0].AsString);

                if (info != null)
                    return info.displayName;
                return $"[UNKNOWN ITEM: {p[0].AsString}]";
            });

            runner.RegisterFunction("getMoney", 0, delegate (Yarn.Value[] p) {
                return ScoreManager.Instance.Money;
            });

            runner.RegisterFunction("getWood", 0, delegate (Yarn.Value[] p) {
                return ScoreManager.Instance.wood;
            });

            runner.RegisterFunction("getStones", 0, delegate (Yarn.Value[] p) {
                return ScoreManager.Instance.stones;
            });

            runner.RegisterFunction("getTenderTodayNode", 0, delegate (Yarn.Value[] p) {
                return FindObjectOfType<Till>().todayNode;
            });

            runner.RegisterFunction("getTenderNode", 1, delegate (Yarn.Value[] p)
            {
                int index = (int)p[0].AsNumber;
                Till till = FindObjectOfType<Till>();
                if (index > -1 && index < till.chatNodes.Length)
                    return till.chatNodes[index];
                return "null";
            });
        }

        private void LoadPrograms()
        {
            YarnProgram[] programs = Resources.LoadAll<YarnProgram>("Speech");
            foreach (YarnProgram yp in programs)
                runner.Add(yp);
        }

        public override Dialogue.HandlerExecutionType RunCommand(Command command, Action onCommandComplete)
        {
            StartCoroutine(RunCommandHelper(command, onCommandComplete));
            return Dialogue.HandlerExecutionType.ContinueExecution;
        }

        private IEnumerator RunCommandHelper(Command command, Action onCommandComplete)
        {
            Debug.LogWarning("This message should not appear.");
            onCommandComplete();
            yield break;
        }

        public override Dialogue.HandlerExecutionType RunLine(Line line, IDictionary<string, string> strings, Action onLineComplete)
        {
            StartCoroutine(RunLineHelper(line, strings, onLineComplete));
            return Dialogue.HandlerExecutionType.PauseExecution;
        }

        private IEnumerator RunLineHelper(Line line, IDictionary<string, string> strings, Action onLineComplete)
        {
            continueLine = false;

            // Show this line of dialogue. -->
            if (strings.TryGetValue(line.ID, out string text))
            {
                SetSpeech(CheckVars(text));
            }
            else
                Debug.LogWarning($"No translation for line {line.ID}...");

            // Make the background darker if characters are interacting with each other. -->
            bgFade.enabled = leftCharacter.IsVisible || rightCharacter.IsVisible;
            UICanvasController.Instance.Visible = !leftCharacter.IsVisible && !rightCharacter.IsVisible;

            // Wait for the OK! -->
            while (!continueLine)
                yield return null;

            onLineComplete();

            // Close the speech box if that was the last line. -->
            if (!runner.isDialogueRunning)
                EndDialogue();

            yield break;
        }

        public override void RunOptions(OptionSet optionSet, IDictionary<string, string> strings, Action<int> onOptionSelected)
        {
            StartCoroutine(RunOptionsHelper(optionSet, strings, onOptionSelected));
        }

        private IEnumerator RunOptionsHelper(OptionSet optionSet, IDictionary<string, string> strings, Action<int> onOptionSelected)
        {
            speakerName.Text = "";
            leftCharacter.Speaking = false;
            rightCharacter.Speaking = false;

            string[] optionText = new string[optionSet.Options.Length];
            int i = 0;

            foreach (OptionSet.Option option in optionSet.Options)
            {
                if (strings.TryGetValue(option.Line.ID, out string s))
                {
                    optionText[i] = s;
                }
                else
                    Debug.LogWarning("No translation for OPTION {line.ID}!");

                i++;
            }

            ShowOptionBox(optionText);

            while (optionBox.SelectedIndex == -1)
                yield return null;

            HideOptionBox();
            onOptionSelected(optionBox.SelectedIndex);

            // Close the speech box if that was the last line. -->
            if (!runner.isDialogueRunning)
                EndDialogue();

            yield break;
        }

        public void StartDialogue(string node)
        {
            StartCoroutine(StartDialogueHelper(node));
        }

        /// <summary>:NotLikeThis:</summary>
        private IEnumerator StartDialogueHelper(string node)
        {
            yield return null;
            Reset();
            runner.StartDialogue(node);
            Show();
            SpeechStarted?.Invoke();
        }

        private void EndDialogue()
        {
            Hide();
            SpeechEnded?.Invoke();
        }

        public void ShowPopup(string msg)
        {
            StartCoroutine(ShowPopupHelper(msg));
        }

        private IEnumerator ShowPopupHelper(string msg)
        {
            yield return null;
            Reset();
            
            SetSpeech(msg);
            speech.UsePopupFormat = true;

            Show();
            showingPopup = true;
            SpeechStarted?.Invoke();
        }

        private void EndPopup()
        {
            Hide();
            showingPopup = false;
            SpeechEnded?.Invoke();
        }

        private void Reset()
        {
            continueLine = false;
            bgFade.enabled = false;
            leftCharacter.DataName = "";
            leftCharacter.Speaking = false;
            rightCharacter.DataName = "";
            rightCharacter.Speaking = false;
            speech.Text = "";
            speech.UsePopupFormat = false;
            speakerName.Text = "";
            speakerName.BoxLocation = SpeakerNameBox.Location.Left;
            speakerName.Show = true;
            optionBox.Hide();
        }

        private void SetSpeech(string text) {
            speech.Text = text;
        }

        private void Show()
        {
            speechCanvas.enabled = true;
            BagWindow.Instance.Visible = false;
            GameplayManager.Instance.AllowGameplay = false;
        }

        private void Hide()
        {
            speechCanvas.enabled = false;
            BagWindow.Instance.Visible = true;
            UICanvasController.Instance.Visible = true;
            GameplayManager.Instance.AllowGameplay = true;
        }

        private void ShowOptionBox(string[] options)
        {
            speech.UseDim = true;
            speakerName.Show = false;
            optionBox.Show(options);
        }

        private void HideOptionBox()
        {
            speech.UseDim = false;
            speakerName.Show = true;
            optionBox.Hide();
        }

        private SpeechCharacter GetCharacter (string charName)
        {
            if (IsLeftCharacter(charName))
                return leftCharacter;
            else if (IsRightCharacter(charName))
                return rightCharacter;
            return null;
        }

        private bool IsLeftCharacter(string charName) {
            return leftCharacter.DataName.Equals(charName);
        }

        private bool IsRightCharacter(string charName) {
            return rightCharacter.DataName.Equals(charName);
        }

        public string CheckVars(string text)
        {
            char[] input = text.Replace("/n", "\n").ToCharArray();
            string output = "";
            bool checkingCode = false;
            string code = "";

            // Search for [] -->
            for (int i = 0; i < input.Length; i++)
            {
                if (checkingCode)
                {
                    if (input[i] == ']')
                    {
                        checkingCode = false;
                        output += CheckCode(code);
                    }
                    else
                        code += input[i];
                }
                else
                {
                    if (input[i] == '[')
                    {
                        checkingCode = true;
                        code = "";
                    }
                    else if (input[i] != '`')
                        output += input[i];
                }
            }

            return output;
        }

        private string CheckCode(string code)
        {
            char[] input = code.ToCharArray();
            string output = "";

            bool checkingVar = false;
            bool checkingSimpleVar = false;
            bool checkingStringVar = false;
            string varName = "";
            List<string> parsedVars = new List<string>();

            bool checkingFunctionName = false;
            string functionName = "";

            bool checkingFunctionParameters = false;

            for (int i = 0; i < input.Length; i++)
            {
                if (checkingVar)
                {
                    bool endOfVar = checkingStringVar ? input[i] == '"' : (input[i] == ' ' || input[i] == ',');
                    bool endOfFunction = input[i] == ')' && !checkingStringVar;
                    bool endOfText = i == input.Length - 1;

                    if (endOfText && !endOfFunction && !endOfVar)
                        varName += input[i];

                    if (endOfVar || endOfFunction || endOfText)
                    {
                        string varValue = checkingSimpleVar ? varName : ParseVariable("$" + varName);

                        checkingVar = false;
                        checkingSimpleVar = false;
                        checkingStringVar = false;

                        if (checkingFunctionParameters)
                        {
                            parsedVars.Add(varValue);

                            if (endOfFunction)
                            {
                                checkingFunctionParameters = false;
                                output += ParseFunction(functionName, parsedVars.ToArray());
                            }
                        }
                        else
                            output += ParseVariable("$" + varName);
                    }
                    else
                        varName += input[i];
                }
                else if (checkingFunctionParameters)
                {
                    if (input[i] == ',' || input[i] == ' ')
                    {

                    }
                    else if (input[i] == '$')
                    {
                        checkingVar = true;
                        varName = "";
                    }
                    else if (input[i] == ')')
                    {
                        checkingFunctionParameters = false;
                        output += ParseFunction(functionName, parsedVars.ToArray());
                    }
                    else if (input[i] == '"')
                    {
                        checkingVar = true;
                        checkingSimpleVar = true;
                        checkingStringVar = true;
                    }
                    else
                    {
                        checkingVar = true;
                        checkingSimpleVar = true;
                        varName = input[i].ToString();
                    }
                }
                else if (checkingFunctionName)
                {
                    if (input[i] == '(')
                    {
                        checkingFunctionName = false;
                        checkingFunctionParameters = true;
                    }
                    else
                        functionName += input[i];
                }
                else
                {
                    if (input[i] == '$')
                    {
                        checkingVar = true;
                        varName = "";
                    }
                    else
                    {
                        checkingFunctionName = true;
                        functionName += input[i];
                    }
                }
            }

            return output;
        }

        private string ParseFunction(string funName, string[] arguments)
        {
            if (runner.dialogue.library.FunctionExists(funName))
            {
                FunctionInfo functionInfo = runner.dialogue.library.GetFunction(funName);

                if (functionInfo.returningFunction != null)
                {
                    Value[] values = new Value[arguments.Length];

                    for (int i = 0; i < arguments.Length; i++)
                        values[i] = new Value(arguments[i]);

                    return functionInfo.returningFunction.Invoke(values).ToString();
                }
                else
                    Debug.LogWarning($"\"{funName}\" must be a ReturningFunction.");
            }

            return "";
        }

        private string ParseVariable(string varName)
        {
            //Check YarnSpinner's variable storage first
            if (variableStorage.GetValue(varName) != Yarn.Value.NULL)
            {
                return variableStorage.GetValue(varName).AsString;
            }

            //If no variables are found, return the variable name
            Debug.LogWarning($"Variable not set: {varName}");
            return varName;
        }
    }
}