using HorseMoon.Inventory;
using HorseMoon.Inventory.UI;
using HorseMoon.Objects;
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

            // Load YarnPrograms -->
            YarnProgram[] programs = Resources.LoadAll<YarnProgram>("Speech");

            foreach (YarnProgram yp in programs)
                runner.Add(yp);
        }

        private void Update()
        {
            if (InDialogue)
            {
                // Press [Use] to continue. -->
                if (Input.GetButtonDown("Use"))
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
                if (p.Length > 0)
                {
                    SpeechCharacter sc = null;

                    if (IsLeftCharacter(p[0]))
                    {
                        sc = leftCharacter;
                        speakerName.BoxLocation = SpeakerNameBox.Location.Left;
                    }
                    else if (IsRightCharacter(p[0]))
                    {
                        sc = rightCharacter;
                        speakerName.BoxLocation = SpeakerNameBox.Location.Right;
                    }

                    if (sc != null)
                    {
                        speakerName.Text = sc.data.names[0];

                        // Tender Till is a special case... -->
                        if (sc.DataName.Equals("TenderTill"))
                        {
                            if (StoryProgress.GetBool("TenderMet"))
                            {
                                if (StoryProgress.GetBool("Nicknamed"))
                                    speakerName.Text = sc.data.names[1];
                            }
                            else
                                speakerName.Text = sc.data.names[2];
                        }
                    }
                    else
                        speakerName.Text = "";
                }
                else
                    speakerName.Text = "";
            });

            // progress <varName> <value> -->
            runner.AddCommandHandler("progress", delegate (string[] p)
            {
                StoryProgress.Set(p[0], p[1]);
            });

            // action <action> [...] -->
            runner.AddCommandHandler("action", delegate (string[] p)
            {
                // Not a good way to do this. -->
                switch (p[0]) {
                    case "repairWell":
                        FindObjectOfType<BrokenWell>().Repair();
                        break;
                    default:
                        Debug.LogWarning($"Unknown command action \"{p[0]}\"");
                        break;
                }
            });

            // give <item> <quantity> -->
            runner.AddCommandHandler("give", delegate (string[] p)
            {
                Player.Instance.bag.Add(p[0], int.Parse(p[1]));
            });

            // take <item> <quantity> -->
            runner.AddCommandHandler("take", delegate (string[] p)
            {
                Player.Instance.bag.Remove(p[0], int.Parse(p[1]));
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
        }

        private void RegisterFunctions()
        {
            runner.RegisterFunction("storyString", 1, delegate (Yarn.Value[] p)
            {
                return StoryProgress.GetString(p[0].AsString);
            });

            runner.RegisterFunction("storyInt", 1, delegate (Yarn.Value[] p)
            {
                return StoryProgress.GetInt(p[0].AsString);
            });
            
            runner.RegisterFunction("storyBool", 1, delegate (Yarn.Value[] p)
            {
                return StoryProgress.GetBool(p[0].AsString);
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

            runner.RegisterFunction("getMoney", 0, delegate (Yarn.Value[] p) {
                return ScoreManager.Instance.Money;
            });

            runner.RegisterFunction("getWood", 0, delegate (Yarn.Value[] p) {
                return ScoreManager.Instance.wood;
            });

            runner.RegisterFunction("getStones", 0, delegate (Yarn.Value[] p) {
                return ScoreManager.Instance.stones;
            });
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

            while (optionBox.selectedIndex == -1)
                yield return null;

            HideOptionBox();
            onOptionSelected(optionBox.selectedIndex);

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

        public IEnumerator ShowPopupHelper(string msg)
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
            rightCharacter.DataName = "";
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
            TimeController.Instance.runWorldTime = false;
            Player.Instance.playerController.enabled = false;
        }

        private void Hide()
        {
            speechCanvas.enabled = false;
            BagWindow.Instance.Visible = true;
            TimeController.Instance.runWorldTime = true;
            Player.Instance.playerController.enabled = true;
        }

        private void ShowOptionBox(string[] options)
        {
            speech.UseDim = true;
            speakerName.Show = false;
            optionBox.ShowOptions(options);
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

        /// <summary>Code borrowed from: https://github.com/YarnSpinnerTool/YarnSpinner/issues/25#issuecomment-227475923 </summary>
        private string CheckVars(string input)
        {
            string output = string.Empty;
            bool checkingVar = false;
            string currentVar = string.Empty;

            int index = 0;
            while (index < input.Length)
            {
                if (input[index] == '[')
                {
                    checkingVar = true;
                    currentVar = string.Empty;
                }
                else if (input[index] == ']')
                {
                    checkingVar = false;
                    output += ParseVariable(currentVar);
                    currentVar = string.Empty;
                }
                else if (checkingVar)
                {
                    currentVar += input[index];
                }
                else
                {
                    output += input[index];
                }
                index += 1;
            }

            return output;
        }

        private string ParseVariable(string varName)
        {
            //Check YarnSpinner's variable storage first
            if (variableStorage.GetValue(varName) != Yarn.Value.NULL)
            {
                return variableStorage.GetValue(varName).AsString;
            }

            //If no variables are found, return the variable name
            return varName;
        }
    }
}