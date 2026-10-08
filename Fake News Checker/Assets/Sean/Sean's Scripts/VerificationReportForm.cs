using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class VerificationReportForm : MonoBehaviour
{
    [Header("Dropdown Configuration")]
    public TMP_Dropdown articleIdDropdown;
    public List<ArticlePageData> shiftArticlesList; // Populate this with all articles available today

    [Header("Sector UI Toggles")]
    public Toggle toggleLocalNews;
    public Toggle togglePopCulture;
    public Toggle toggleFinance;
    public Toggle toggleHealthScience;
    public Toggle toggleBreakingNews;

    [Header("Discrepancy UI Toggles")]
    public Toggle toggleUnverifiedAuthor;
    public Toggle toggleFalsifiedCredentials;
    public Toggle toggleImageManipulation;
    public Toggle toggleOutOfContextMedia;
    public Toggle toggleInaccurateStats;
    public Toggle toggleFalseTimeline;
    public Toggle toggleUncitedClaims;
    public Toggle toggleTypos;

    [Header("Editorial Decision UI Toggles")]
    public Toggle toggleApprove;
    public Toggle toggleReject;

    [Header("Global Progression Parameters")]
    public Button submitButton;

    private int currentTrustScore = 0;
    private ArticlePageData selectedArticle;
    private List<string> shiftLogReport = new List<string>();

    void Start()
    {
        if (submitButton != null) submitButton.onClick.AddListener(EvaluatePlayerSubmission);
        if (articleIdDropdown != null)
        {
            articleIdDropdown.onValueChanged.AddListener(OnDropdownIndexChanged);
            PopulateIdDropdown();
        }
    }

    void PopulateIdDropdown()
    {
        articleIdDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var art in shiftArticlesList)
        {
            if (art != null) options.Add(art.articleID);
        }
        articleIdDropdown.AddOptions(options);
        if (shiftArticlesList.Count > 0) OnDropdownIndexChanged(0);
    }

    void OnDropdownIndexChanged(int index)
    {
        if (index >= 0 && index < shiftArticlesList.Count)
        {
            selectedArticle = shiftArticlesList[index];
        }
    }

    public void EvaluatePlayerSubmission()
    {
        if (selectedArticle == null) return;

        ArticleCheatSheet sheet = selectedArticle.cheatSheet;
        bool isCorrectSector = GetSelectedSector() == sheet.correctSector;
        bool isCorrectDecision = toggleApprove.isOn == sheet.approveAndPublish;

        // Verify if player checked the exact discrepancies noted on the cheat sheet
        bool discrepanciesMatch =
            (toggleUnverifiedAuthor.isOn == sheet.unverifiedAuthor) &&
            (toggleFalsifiedCredentials.isOn == sheet.falsifiedCredentials) &&
            (toggleImageManipulation.isOn == sheet.imageManipulation) &&
            (toggleOutOfContextMedia.isOn == sheet.outOfContextMedia) &&
            (toggleInaccurateStats.isOn == sheet.inaccurateStats) &&
            (toggleFalseTimeline.isOn == sheet.falseTimeline) &&
            (toggleUncitedClaims.isOn == sheet.uncitedClaims) &&
            (toggleTypos.isOn == sheet.typos);

        // Grading Outcome Branch
        string loggingStatus = "";
        if (isCorrectSector && isCorrectDecision && discrepanciesMatch)
        {
            currentTrustScore = Mathf.Clamp(currentTrustScore + 10, -100, 100);
            loggingStatus = $"{selectedArticle.articleID}: Verified Successfully. +10 Trust.";
        }
        else
        {
            currentTrustScore = Mathf.Clamp(currentTrustScore - 10, -100, 100);
            loggingStatus = $"{selectedArticle.articleID}: Audit Discrepancy Flagged. -10 Trust.";
        }

        shiftLogReport.Add(loggingStatus);
        Debug.Log($"[SHIFT AUDIT LOG] {loggingStatus} Current Global Trust: {currentTrustScore}");
        ClearFormFields();
    }

    SectorType GetSelectedSector()
    {
        if (togglePopCulture.isOn) return SectorType.PopCulture;
        if (toggleFinance.isOn) return SectorType.Finance;
        if (toggleHealthScience.isOn) return SectorType.HealthScience;
        if (toggleBreakingNews.isOn) return SectorType.BreakingNews;
        return SectorType.LocalNews;
    }

    void ClearFormFields()
    {
        toggleLocalNews.isOn = false; togglePopCulture.isOn = false; toggleFinance.isOn = false;
        toggleHealthScience.isOn = false; toggleBreakingNews.isOn = false;
        toggleUnverifiedAuthor.isOn = false; toggleFalsifiedCredentials.isOn = false;
        toggleImageManipulation.isOn = false; toggleOutOfContextMedia.isOn = false;
        toggleInaccurateStats.isOn = false; toggleFalseTimeline.isOn = false;
        toggleUncitedClaims.isOn = false; toggleTypos.isOn = false;
        toggleApprove.isOn = false; toggleReject.isOn = false;
    }
}
