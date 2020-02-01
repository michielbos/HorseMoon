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
        public SpeechBox speech;
        public SpeakerNameBox speakerName;
        public Image leftSprite;
        public Image rightSprite;
        public OptionBox optionBox;

        private Dictionary<string, SpeechCharacterSprite> characterSprite;

        private bool continueLine;

        public Action SpeechStarted;
        public Action SpeechEnded;

        private void Start()
        {
            optionBox.Hide();

            RegisterCommands();

            // Load YarnPrograms -->
            YarnProgram[] programs = Resources.LoadAll<YarnProgram>("Speech");

            foreach (YarnProgram yp in programs)
                runner.Add(yp);

            // Character Sprites -->
            SpeechCharacterSprite[] allSCS = Resources.LoadAll<SpeechCharacterSprite>("Speech/Sprites");

            characterSprite = new Dictionary<string, SpeechCharacterSprite>();
            foreach (SpeechCharacterSprite scs in allSCS)
                characterSprite.Add(scs.name, scs);
        }

        private void Update()
        {
            if (runner.isDialogueRunning)
            {
                // Press [Use] to continue. -->
                if (Input.GetButtonDown("Use"))
                {
                    continueLine = true;
                }
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
                }
            }
        }

        private void RegisterCommands()
        {
            // speaker : The name of the speaking horse. -->
            runner.AddCommandHandler("speaker", delegate (string[] p)
            {
                if (p.Length > 0)
                    speakerName.Text = CheckVars(p[0].Replace('/', ' '));
                else
                    speakerName.Text = "";
            });

            // leftSprite -->
            runner.AddCommandHandler("leftSprite", delegate (string[] p)
            {
                leftSprite.enabled = characterSprite[p[0]].sprite != null;
                leftSprite.sprite = characterSprite[p[0]].sprite;
            });

            // rightSprite -->
            runner.AddCommandHandler("rightSprite", delegate (string[] p)
            {
                rightSprite.enabled = characterSprite[p[0]].sprite != null;
                rightSprite.sprite = characterSprite[p[0]].sprite;
            });
        }

        public override Dialogue.HandlerExecutionType RunCommand(Command command, Action onCommandComplete)
        {
            StartCoroutine(RunCommandHelper(command, onCommandComplete));
            return Dialogue.HandlerExecutionType.ContinueExecution;
        }

        private IEnumerator RunCommandHelper(Command command, Action onCommandComplete)
        {
            Debug.LogWarning("This message should not appear; commands are set to auto-run.");
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
                speech.Text = CheckVars(text);
            }
            else
                Debug.LogWarning($"No translation for line {line.ID}...");

            // Wait for the OK! -->
            while (!continueLine)
                yield return null;

            onLineComplete();

            // Close the speech box if that was the last line. -->
            if (!runner.isDialogueRunning)
            {
                Hide();
                SpeechEnded?.Invoke();
            }

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
            yield break;
        }

        public void StartDialogue(string node)
        {
            leftSprite.enabled = false;
            rightSprite.enabled = false;
            speakerName.Text = "";
            speakerName.Show = true;
            optionBox.Hide();

            runner.StartDialogue(node);
            Show();
            SpeechStarted?.Invoke();
        }

        private void Show()
        {
            speechCanvas.enabled = true;
        }

        private void Hide()
        {
            speechCanvas.enabled = false;
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