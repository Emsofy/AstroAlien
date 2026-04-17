using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class DevConsole : MonoBehaviour
{
   
    public GameObject panel;
    public TMP_InputField inputField;
    public TextMeshProUGUI logText;

    private Dictionary<string, Action<string[]>> commands;

    void Start()
    {
        panel.SetActive(false);
        RegisterCommands();
    }

    void Update()
    {
        // Toggle console panel
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            panel.SetActive(!panel.activeSelf);

            if (panel.activeSelf)
                inputField.ActivateInputField();
        }

        // Submit command
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            SubmitCommand(inputField.text);
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    void RegisterCommands()
    {
        commands = new Dictionary<string, Action<string[]>>
        {
            { "save", _ =>
                {
                    GameManager.Instance.SaveGame();
                    Log("Game saved");
                }
            },

            { "load", _ =>
                {
                    GameManager.Instance.LoadGame();
                    Log("Game loaded");
                }
            },

            { "seen", args =>
                {
                    if (args.Length > 0)
                    {
                        GameManager.Instance.MarkDialogue(args[0]);
                        Log("Dialogue marked as seen: " + args[0]);
                    }
                    else
                        Log("Usage: seen <dialogue_id>");
                }
            },

            { "reset_dialogue", args =>
                {
                    if (args.Length > 0)
                    {
                        GameManager.Instance.ResetDialogue(args[0]);
                        Log("Dialogue reset: " + args[0]);
                    }
                    else
                        Log("Usage: reset_dialogue <dialogue_id>");
                }
            },

            { "reset_memory", _ =>
                {
                    GameManager.Instance.ResetMemory();
                    Log("Memory reset");
                }
            },

            { "reset_disk", _ =>
                {
                    GameManager.Instance.ResetSaveFile();
                    Log("Disk save + memory reset");
                }
            },

            { "time", args =>
                {
                    if (args.Length > 0 && int.TryParse(args[0], out int seconds))
                    {
                        GameManager.Instance.HandleDebugTime(seconds);
                        Log("Simulated time set: " + seconds);
                    }
                    else
                        Log("Usage: time <seconds>");
                }
            },
            { "tree_time_all", args =>
                {
                    if(args.Length > 0 && int.TryParse(args[0], out int seconds))
                    {
                        foreach (var tree in GameManager.Instance.activeTrees)
                        {
                            tree.SetRemainingSeconds(seconds);
                        }
                        Log("Set all tree timers to " + seconds + " seconds");
                    }
                    else
                        Log("Usage: time_time_all <seconds");
                }

            },

            {"tree_list", _ =>
                {
                    foreach (var tree in GameManager.Instance.activeTrees)
                    {
                        Log("Tree " + tree.id + " | " + tree.GetRemainingSeconds() + "s");
                    }
                }

            },
            {"delete_trees", _ =>
                {
                    var treelist = GameManager.Instance.activeTrees;
                    foreach (var tree in treelist)
                    {
                        if( tree != null)
                        {
                            Log("Deleted tree " + tree.id);
                            UnityEngine.Object.Destroy(tree.gameObject);
                         }
                    }
                    treelist.Clear();
                }
            },

            {"egg_time_all", args =>
                {
                    if(args.Length > 0 && int.TryParse(args[0], out int seconds))
                    {
                        foreach (var chickenNew in GameManager.Instance.activeChicks)
                        {
                            chickenNew.SetRemainingSeconds(seconds);
                        }
                        Log("Set all chicken timers to " + seconds + " seconds");
                    }
                    else
                        Log("Usage: time_time_all <seconds");
                }

            },

            {"chicken_list", _ =>
                {
                    foreach (var chickenNew in GameManager.Instance.activeChicks)
                    {
                        Log("Chicken " + chickenNew.id + " | " + chickenNew.GetRemainingSeconds() + "s");
                    }
                }


            }

           
            
        };
    }

    void SubmitCommand(string input)
    {
        Log("> " + input);

        string[] split = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (split.Length == 0) return;

        string cmd = split[0];

        if (commands.ContainsKey(cmd))
        {
            try
            {
                commands[cmd](split.Length > 1 ? split[1..] : new string[0]);
            }
            catch (Exception e)
            {
                Log("Error running command: " + e.Message);
            }
        }
        else
        {
            Log("Unknown command");
        }
    }

    void Log(string msg)
    {
        logText.text += "\n" + msg;
    }
}