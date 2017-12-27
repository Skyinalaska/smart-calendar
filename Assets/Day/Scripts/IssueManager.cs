﻿using System.Collections.Generic;
using UnityEngine;

public class IssueManager : MonoBehaviour
{
    private Dictionary<System.DateTime, List<GameObject>> issuesMap = new Dictionary<System.DateTime, List<GameObject>>();

    public GameObject dialogWindow, prototype, closeButton;
    public Transform issuesContainer;

    private DialogWindowManager dialogWindowManager;
    private CloseButtonDriver closeButtonDriver;

    private const int minIssueLength = 4;

    private System.DateTime selectedDate = System.DateTime.Today;

    private void Awake()
    {
        dialogWindowManager = dialogWindow.GetComponent<DialogWindowManager>();
        dialogWindow.SetActive(false);
        closeButtonDriver = closeButton.GetComponent<CloseButtonDriver>();
    }

    public void SetDate(System.DateTime date)
    {
        selectedDate = date;
        Issue[] allIssues = issuesContainer.GetComponentsInChildren<Issue>();
        foreach (Issue currentIssue in allIssues) //disables all issues
        {
            currentIssue.gameObject.SetActive(false);
        }
        List<GameObject> selectedDateIssues;
        if (issuesMap.TryGetValue(selectedDate, out selectedDateIssues)) //if there are assigned issues on selected date
        {
            foreach (GameObject currentIssue in selectedDateIssues) //enable them
            {
                currentIssue.SetActive(true);
            }
        }
    }

    private void OnMouseUp()
    {
        if (dialogWindow.activeSelf)
        {
            if (dialogWindowManager.IssueText.Length >= minIssueLength) //Create new issue if length is not too short
            {
                string selectedTime = dialogWindowManager.SelectedHours + ":" + dialogWindowManager.SelectedMinutes;
                Issue newIssue = Instantiate(prototype, issuesContainer).GetComponent<Issue>();
                newIssue.IssueText = dialogWindowManager.IssueText;
                newIssue.IssueTime = selectedTime;
                if (!issuesMap.ContainsKey(selectedDate))
                {
                    issuesMap.Add(selectedDate, new List<GameObject>());
                }
                issuesMap[selectedDate].Add(newIssue.gameObject);
                dialogWindowManager.Deactivate();
                closeButtonDriver.Deactivate();
            }
            else
            {
                dialogWindowManager.ClearInputField(minIssueLength);
            }
        }
        else
        {
            closeButtonDriver.Activate();
            dialogWindowManager.Activate();
        }
    }
}