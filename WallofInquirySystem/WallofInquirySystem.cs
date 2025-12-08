using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using RichTextSubstringHelper;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using DanielLochner.Assets.SimpleScrollSnap;
using System.Linq;


public class WallofInquirySystem : MonoBehaviour
{
    public event EventHandler BasicWallofWord;
    public event EventHandler PointWallofWord;
    public event EventHandler BasicMouseChange;

    public event EventHandler RineCorrectWallofInquiry;
    public event EventHandler MarcelCorrectWallofInquiry;

    [Header("UI References")]
    public TextMeshProUGUI[] textSlots;  // 3개의 TextMeshProUGUI
    public Transform buttonPanel;        // 클릭된 단어들이 저장될 패널
    public GameObject wordButtonPrefab;  // 클릭 단어 버튼 프리팹
    public Image Chracter;
    public Sprite[] Rinesprites; // 캐릭터 표정


    [Header("마르셀 레퍼런스")]
    public TextMeshProUGUI[] textSlotsMarcel;
    public Transform buttonPanelMarcel;
    public Image ChracterMarcel;
    public Sprite[] MarcelSprites; // 마르셀 표정

    private bool mousePointcheck = false;
    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float waitTime = 3f;

    public GameObject wallofInquiryScrollPanel;
    public GameObject wallofInquiryScrollPanel_Marcel;

    [Header("마음의 벽 문장 완성")]
    public TextMeshProUGUI wallofMindWordText;
    public TextMeshProUGUI wallofMindWordText_02;

    public bool rineRunning;
    public bool marcelRunning;


    public GameObject wallofInquirySystem;
    public GameObject wallofInquiryPanel;
    public GameObject wallofInquiryPanel_Marcel;

    public GameObject correctPanel;
    public GameObject failPanel;
    public GameObject marcelcorrectPanel;
    public GameObject marcelfailPanel;


    private string originalSentence = "리네는 []으로 나가면서 [] []를 남겼다.";
    private string targetSentence = "리네는 창문으로 나가면서 피 냄새를 남겼다.";

    private string marceloriginalSentence = "마르셀이 []에 남긴 []은 [] []다.";
    private string marceltargetSentecnt = "마르셀이 피해자방에 남긴 흔적은 보이지않는 냄새다.";

    private int filledCount = 0;      // 현재 몇 개 채워졌는지
    private int totalBlanks = 0;      // 전체 [] 개수

    private int filledCount_02 = 0;
    private int totalBlanks_02 = 0;


    private List<Button> createdButtons = new List<Button>();

    private List<Button> createdButtons_02 = new List<Button>();


    private string[] sentences = new string[12]
   {
        "아니야... 아니야...!!",
        "저는 정말로 그냥 나왔어요...",
        "<size=120%><color=red>창문</color></size>은 불가능해요...!!",
        "그, 그래! <size=120%><color=red>비상계단</color></size>으로 왔어요.",
        "제가... 잘못 얘기했던 거에요.",
        "그래서 <size=120%><color=red>엘리베이터</color></size>에서 못 봤던 거예요.",
        "분명 그 이상한 <size=120%><color=red>냄새</color></size> 때문이에요...!",
        "저도 기억이 애매했던게, 사실 맡았던 거죠.",
        "맞아요, 그게 분명해요!",
        "만약에 거기로 뛰어 내렸어도",
        "그 높이에서 떨어지면 크게 다칠 거고",
        "지금쯤 저는 <size=120%><color=red>피</color></size>투성이였을 거예요!"
   };

    private string[] sentencesMarcel = new string[15]
    {
        "제가 루시라니, 흥미로워요.",
        "근데 결국 피해자를 죽인 거는 마물이죠?",
        "그렇다면 루시도 마물이라는 뜻이잖아요?",
    "그 근거가 어디에 있죠?",
        "<size=120%><color=red>피해자방</color></size>에 있었나요? 로비에 있었나요?",
        "아니면 <size=120%><color=red>복도</color></size>? 그것도 아니면 <size=120%><color=red>의무실</color></size>?",
    "애초에 단서가 있는 건가요?",
        "루시라는 여자가 여기에 왔는지도 <size=120%><color=red>확실하지않고</color></size>",
        "뭔가 <size=120%><color=red>흔적</color></size>을 남겼다면 이미 보셨겠죠",
    "설마 <size=120%><color=red>보이지않는</color></size>다고 하실 건가요?",
        "아직 <size=120%><color=red>못찾았다</color></size> 고 하실 건가요?",
        "혹은 어디에 <size=120%><color=red>숨긴건가</color></size>요?",
    "눈으로 직접 보신 <size=120%><color=red>물건</color></size>이 있는 거죠?",
        "아니면 코로 맡은 <size=120%><color=red>냄새</color></size> 가 있는 건가요?",
        "그것도 아니면 귀로 들은 <size=120%><color=red>소리</color></size> 가 단서인가요?"

    };


    private string[] lastTalk = new string[2]
        {
        "제가 창문으로 탈출했다는 결정적인 증거는 없어요...!!", "루시의 정체를 알려주는 단서가 있기는 한가요?"
        };

    private int currentSetIndex = 0;
    private bool isTyping = false;


    //마우스 포인트 변경 이벤트 구독자 시그니처
    public void BasicWallofWordChanger(object sender, EventArgs eventArgs)
    {
        Debug.Log("찾기 마우스 포인터 기본 변경");
    }

    public void PointWallofWordChanger(object sender, EventArgs eventArgs)
    {
        Debug.Log("기본 마우스 포인터 연결");
    }

    public void BasicMouseChanger(object sender, EventArgs eventArgs)
    {
        Debug.Log("베이직 마우스 포인터 연결");
    }


    void Start()
    {
        
        // 정규식 이용
        totalBlanks = Regex.Matches(originalSentence, @"\[\]").Count;
        totalBlanks_02 = Regex.Matches(marceloriginalSentence, @"\[\]").Count;

        StartRine();
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Camera cam = Camera.main;

        // 폴백 클릭 처리
        if (Input.GetMouseButtonDown(0))
        {

            ProcessClick(mousePos, cam);
        }

        if (IsMouseOverRedText(mousePos, cam))
        {
            PointWallofWord(this, EventArgs.Empty);
            mousePointcheck = false;
        }
        else if (!IsMouseOverRedText(mousePos, cam) && !mousePointcheck)
        {

            BasicWallofWord(this, EventArgs.Empty);
            mousePointcheck = true;
        }

    }
    
    public void StartMarcel()
    {
        BasicWallofWord(this, EventArgs.Empty);
        StartCoroutine(DisplayMarcelSentences());
    }

    public void StartRine()
    {
        BasicWallofWord(this, EventArgs.Empty);
        StartCoroutine(DisplaySentences());
    }

    private IEnumerator DisplaySentences()
    {
        if (rineRunning) yield break;
        wallofInquiryScrollPanel.GetComponent<SimpleScrollSnap>().Setup();
    
        rineRunning = true;
        int checknum = 0;

        int sets = Mathf.Max(1, sentences.Length / textSlots.Length);

        while (true)
        {
            for (int i = 0; i < textSlots.Length; i++)
            {
                int sentenceIndex = currentSetIndex * textSlots.Length + i;
                sentenceIndex %= sentences.Length;
                yield return StartCoroutine(TypeSentence(textSlots[i], sentences[sentenceIndex]));
            }

            for (int i = 0; i < textSlots.Length; i++)
            {

                Color originalColor = textSlots[i].color;
                float elapsed = 0f;

                while (elapsed < 1) //텍스트 불투명도 조절, (임의, 기획 상의 필요) 일단 2초 설정하였음
                {
                    elapsed += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, elapsed / 1);
                    textSlots[i].color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                    yield return null;
                }

            }
            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < textSlots.Length; i++)
            {
                textSlots[i].text = "";
                textSlots[i].color = new Color(1, 1, 1, 1); //알파값 돌려줌

                RectTransform rect = textSlots[i].rectTransform;


                Vector3 rot = rect.localEulerAngles;
                rot.z *= -1f;
                rect.localEulerAngles = rot;


                checknum++;
            }

            currentSetIndex = (currentSetIndex + 1) % sets;


            if (checknum == 3)
            {
                Chracter.sprite = Rinesprites[1];
            }
            if (checknum == 9)
            {
                Chracter.sprite = Rinesprites[2];
            }


            if (checknum == sentences.Length)
            {

                ButtonEvents();
                textSlots[1].text = lastTalk[0];
                wallofInquiryScrollPanel.GetComponent<SimpleScrollSnap>().enabled = true;
                rineRunning = false;
                yield return new WaitForSeconds(4f);
                textSlots[1].text = "";
                StartCoroutine(DisplaySentences());
                yield break;
            }
        }
    }


    private IEnumerator DisplayMarcelSentences()
    {
        if (marcelRunning) yield break;
        wallofInquiryScrollPanel_Marcel.GetComponent<SimpleScrollSnap>().Setup();

        marcelRunning = true;
        int checknum = 0;

        int sets = Mathf.Max(1, sentencesMarcel.Length / textSlotsMarcel.Length);
        ChracterMarcel.sprite = MarcelSprites[0];

        while (true)
        {
            for (int i = 0; i < textSlotsMarcel.Length; i++)
            {
                int sentenceIndex = currentSetIndex * textSlotsMarcel.Length + i;
                sentenceIndex %= sentencesMarcel.Length;
                yield return StartCoroutine(TypeSentence(textSlotsMarcel[i], sentencesMarcel[sentenceIndex]));
            }

            for (int i = 0; i < textSlotsMarcel.Length; i++)
            {

                Color originalColor = textSlotsMarcel[i].color;
                float elapsed = 0f;

                while (elapsed < 1) 
                {
                    elapsed += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, elapsed / 1);
                    textSlotsMarcel[i].color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                    yield return null;
                }

            }
            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < textSlotsMarcel.Length; i++)
            {
                textSlotsMarcel[i].text = "";
                textSlotsMarcel[i].color = new Color(1, 1, 1, 1);

                RectTransform rect = textSlotsMarcel[i].rectTransform;


                Vector3 rot = rect.localEulerAngles;
                rot.z *= -1f;
                rect.localEulerAngles = rot;


                checknum++;
            }

            currentSetIndex = (currentSetIndex + 1) % sets;


            if (checknum == 3)
            {
                ChracterMarcel.sprite = MarcelSprites[1];
            }
            if (checknum == 12)
            {
                ChracterMarcel.sprite = MarcelSprites[2];
            }


            if (checknum == sentencesMarcel.Length)
            {

                MarcelButtonEvents();
                textSlotsMarcel[1].text = lastTalk[1]; // 마르셀
                wallofInquiryScrollPanel_Marcel.GetComponent<SimpleScrollSnap>().enabled = true;
                marcelRunning = false;
                yield return new WaitForSeconds(4f);
                textSlotsMarcel[1].text = "";
                StartCoroutine(DisplayMarcelSentences());
                yield break;
            }
        }
    }


    private IEnumerator TypeSentence(TextMeshProUGUI textComponent, string sentence)
    {
        isTyping = true;
        textComponent.text = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            textComponent.text = sentence.RichTextSubString(0, i + 1);
            //렌더/메쉬 정보 갱신
            textComponent.ForceMeshUpdate();
            yield return new WaitForSeconds(typingSpeed);
        }

        textComponent.ForceMeshUpdate();
        isTyping = false;
    }




    // 실제 클릭 처리 공통 로직
    private void ProcessClick(Vector2 screenPosition, Camera pressCamera)
    {
        if (textSlots == null) return;


        foreach (var text in textSlots.Concat(textSlotsMarcel))
        {
            if (text == null || !text.gameObject.activeInHierarchy) continue;

         
            text.ForceMeshUpdate();

            if (text.textInfo == null || text.textInfo.characterCount == 0) continue;

           
            if (cam == null) cam = Camera.main;

            int charIndex = TMP_TextUtilities.FindIntersectingCharacter(text, screenPosition, cam, true);
            if (charIndex == -1) continue;

            // 안전 체크
            if (charIndex < 0 || charIndex >= text.textInfo.characterCount) continue;

            var cInfo = text.textInfo.characterInfo[charIndex];
            if (!cInfo.isVisible) continue;

     
            int meshIndex = cInfo.materialReferenceIndex;
            int vertexIndex = cInfo.vertexIndex;

            // 안전 범위 체크
            if (meshIndex < 0 || meshIndex >= text.textInfo.meshInfo.Length) continue;
            var meshInfo = text.textInfo.meshInfo[meshIndex];
            if (vertexIndex < 0 || vertexIndex + 3 >= meshInfo.colors32.Length) continue;

            Color32 c0 = meshInfo.colors32[vertexIndex];
            Color32 c1 = meshInfo.colors32[vertexIndex + 1];
            Color32 c2 = meshInfo.colors32[vertexIndex + 2];
            Color32 c3 = meshInfo.colors32[vertexIndex + 3];

            int avgR = (c0.r + c1.r + c2.r + c3.r) / 4;
            int avgG = (c0.g + c1.g + c2.g + c3.g) / 4;
            int avgB = (c0.b + c1.b + c2.b + c3.b) / 4;

            // 평균 색 계산 처리
            if (ApproximatelyRed(avgR, avgG, avgB))
            {
                string picked = ExtractWordAtPosition(text, screenPosition, cam, charIndex);
                if (!string.IsNullOrEmpty(picked))
                {
                
                    if (textSlots.Contains(text))
                    {
                        AddWordButton(picked);
                        Debug.Log($"[리네 쪽] {picked}");
                    }
                    else if (textSlotsMarcel.Contains(text))
                    {
                        MarcelAddWordButton(picked);
                        Debug.Log($"[마르셀 쪽] {picked}");
                    }
                    else return; // 첫 발견만 처리


                }
                else
                {
                    Debug.Log($"R{avgR} G{avgG} B{avgB} 빨강 ㄴㄴ");
                }

            }
        }
    }

    private bool IsMouseOverRedText(Vector2 screenPosition, Camera cam)
    {
        if (textSlots == null) return false;

        foreach (var text in textSlots.Concat(textSlotsMarcel))
        {
            if (text == null || !text.gameObject.activeInHierarchy) continue;

            text.ForceMeshUpdate();
            if (text.textInfo == null || text.textInfo.characterCount == 0) continue;

            int charIndex = TMP_TextUtilities.FindIntersectingCharacter(text, screenPosition, cam, true);
            if (charIndex == -1) continue;

            var cInfo = text.textInfo.characterInfo[charIndex];
            if (!cInfo.isVisible) continue;

            int meshIndex = cInfo.materialReferenceIndex;
            int vertexIndex = cInfo.vertexIndex;
            var meshInfo = text.textInfo.meshInfo[meshIndex];

            if (vertexIndex + 3 >= meshInfo.colors32.Length) continue;

            Color32 c0 = meshInfo.colors32[vertexIndex];
            Color32 c1 = meshInfo.colors32[vertexIndex + 1];
            Color32 c2 = meshInfo.colors32[vertexIndex + 2];
            Color32 c3 = meshInfo.colors32[vertexIndex + 3];

            int avgR = (c0.r + c1.r + c2.r + c3.r) / 4;
            int avgG = (c0.g + c1.g + c2.g + c3.g) / 4;
            int avgB = (c0.b + c1.b + c2.b + c3.b) / 4;

            if (CursorApproximatelyRed(avgR, avgG, avgB))
            {
                return true;
            }
        }

        return false;
    }

    private bool CursorApproximatelyRed(int r, int g, int b)
    {
       
        bool isDominantRed = r > g + 40 && r > b + 40;  ’
        bool isBrightEnough = r > 120;                  

        return isDominantRed && isBrightEnough;
    }

    private bool ApproximatelyRed(int r, int g, int b)
    {
        //컬러 임계 조정
        return r >= 200 && g <= 110 && b <= 110;
    }

    private string ExtractWordAtPosition(TextMeshProUGUI text, Vector2 screenPos, Camera cam, int fallbackCharIndex)
    {
        int wordIndex = TMP_TextUtilities.FindIntersectingWord(text, screenPos, cam);
        if (wordIndex != -1 && wordIndex < text.textInfo.wordCount)
        {
            var wInfo = text.textInfo.wordInfo[wordIndex];
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < wInfo.characterCount; i++)
            {
                int charIdx = wInfo.firstCharacterIndex + i;
                if (charIdx < 0 || charIdx >= text.textInfo.characterInfo.Length) continue;

                char c = text.textInfo.characterInfo[charIdx].character;
                sb.Append(c);
             
            }

            string word = sb.ToString();

           
            word = word.TrimEnd(',', '.', '!', '?', '은', '에', '요', '이', '을') ;
          
            string[] suffixes = { "으로", "에서", "투성이였을", "투성이였", "다고" };


            string pattern = "(" + string.Join("|", suffixes) + ")$";

            word = Regex.Replace(word, pattern, "");

            return word;
        }


        if (fallbackCharIndex >= 0 && fallbackCharIndex < text.textInfo.characterInfo.Length)
            return text.textInfo.characterInfo[fallbackCharIndex].character.ToString();

        return null;
    }

    private void AddWordButton(string word)
    {
        if (buttonPanel == null || wordButtonPrefab == null)
        {
            Debug.LogWarning("buttonPanel 또는 wordButtonPrefab이 설정되지 않았습니다.");
            return;
        }

        // 중복 체크
        for (int i = 0; i < buttonPanel.childCount; i++)
        {
            var child = buttonPanel.GetChild(i);
            var tmp = child.GetComponentInChildren<TextMeshProUGUI>();



            if (tmp != null && tmp.text == word) return;
        }


        wallofInquiryScrollPanel.GetComponent<SimpleScrollSnap>().Setup();

        var go = Instantiate(wordButtonPrefab, buttonPanel);
        var txt = go.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null) txt.text = word;


    }

    private void MarcelAddWordButton(string word)
    {
      
        // 중복 체크
        for (int i = 0; i < buttonPanelMarcel.childCount; i++)
        {
            var child = buttonPanelMarcel.GetChild(i);
            var tmp = child.GetComponentInChildren<TextMeshProUGUI>();



            if (tmp != null && tmp.text == word) return;
        }

        wallofInquiryScrollPanel_Marcel.GetComponent<SimpleScrollSnap>().Setup();
     

        var go = Instantiate(wordButtonPrefab, buttonPanelMarcel);
        var txt = go.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null) txt.text = word;
    }

    private void ButtonEvents()
    {
        wallofMindWordText.text = originalSentence;
        // 정규식 이용
        totalBlanks = Regex.Matches(originalSentence, @"\[\]").Count;
        
        if (buttonPanel.childCount <= 0)
        {
            StartCoroutine(DisplaySentences());
        }

        for (int i = 0; i < buttonPanel.childCount; i++)
        {
           

            var child = buttonPanel.GetChild(i);
            var tmp = child.GetComponentInChildren<TextMeshProUGUI>();
            Button btn = child.GetComponent<Button>();

            if (btn == null) continue;

         
            if (createdButtons.Contains(btn))
                continue;

            createdButtons.Add(btn);

            
            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(() => OnWordButtonClick(btn, tmp.text));
        }

    }


    private void MarcelButtonEvents()
    {
        wallofMindWordText_02.text = marceloriginalSentence;
        // 정규식 이용
        totalBlanks_02 = Regex.Matches(marceloriginalSentence, @"\[\]").Count;
        Debug.Log(totalBlanks_02);

        if (buttonPanelMarcel.childCount <= 0)
        {
            StartCoroutine(DisplayMarcelSentences());
        }

        for (int i = 0; i < buttonPanelMarcel.childCount; i++)
        {
            var child = buttonPanelMarcel.GetChild(i);
            var tmp = child.GetComponentInChildren<TextMeshProUGUI>();
            Button btn = child.GetComponent<Button>();

            if (btn == null) continue;

            if (createdButtons_02.Contains(btn))
                continue;

            createdButtons_02.Add(btn);

    
            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(() => MarcelOnWordButtonClick(btn, tmp.text));
        }

    }

    private void CheckAnswer()
    {
        if (wallofMindWordText.text == targetSentence)
        {
            StopCoroutine(DisplaySentences());
            StartCoroutine(Correct());

        }
        else
        {
            Debug.Log("오답");
            filledCount = 0;
            Debug.Log(filledCount);
       
            StopCoroutine(DisplaySentences());
            for (int i = 0; i < textSlots.Length; i++)
            {
                textSlots[i].text = "";
            }
            wallofMindWordText.text = "";
            failPanel.SetActive(true);


            foreach (Button btn in createdButtons)
            {
                btn.interactable = true;
            }

            wallofMindWordText.text = originalSentence;
            filledCount = 0;

            StartCoroutine(WaitDisplay("rine"));


        }
    }

    private void MarcelCheckAnswer()
    {
        if (wallofMindWordText_02.text == marceltargetSentecnt)
        {
            StopCoroutine(DisplayMarcelSentences());
            StartCoroutine(MarcelCorrect());

        }
        else
        {
            filledCount_02 = 0;
            Debug.Log("오답");
            StopCoroutine(DisplayMarcelSentences());
            for (int i = 0; i < textSlotsMarcel.Length; i++)
            {
                textSlotsMarcel[i].text = "";
            }
            wallofMindWordText_02.text = "";
            marcelfailPanel.SetActive(true);



            foreach (Button btn in createdButtons_02)
            {
                btn.interactable = true;
            }


            wallofMindWordText_02.text = marceloriginalSentence;
            filledCount_02 = 0;

            StartCoroutine(WaitDisplay("marcel"));

        }

    }

    private IEnumerator Correct()
    {
        //정답 코루틴
        correctPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        BasicMouseChange(this, EventArgs.Empty);
        wallofInquiryPanel.SetActive(false);
        RineCorrectWallofInquiry(this, EventArgs.Empty);
    }

    private IEnumerator MarcelCorrect()
    {
        //정답 코루틴
        marcelcorrectPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        BasicMouseChange(this, EventArgs.Empty);
        wallofInquiryPanel_Marcel.SetActive(false);
        MarcelCorrectWallofInquiry(this, EventArgs.Empty);
    }


    private IEnumerator WaitDisplay(string who)
    {

        yield return new WaitForSeconds(3F);

        if (who=="rine")
        {
            //오답 로직 추가
            StartCoroutine(DisplaySentences());
            yield break;
        }
        else
        {
            StartCoroutine(DisplayMarcelSentences());
            yield break;
        }
       
    }



    private void OnWordButtonClick(Button btn, string word)
    {
       // btn.interactable = false;


        wallofMindWordText.text = ReplaceFirst(wallofMindWordText.text, "[]", word);
        filledCount++;
        Debug.Log(filledCount);


        if (filledCount >= totalBlanks)
        {
            Debug.Log(filledCount);
            Debug.Log(totalBlanks);
            CheckAnswer();
        }
    }

    private void MarcelOnWordButtonClick(Button btn, string word)
    {


        wallofMindWordText_02.text = ReplaceFirst(wallofMindWordText_02.text, "[]", word);
        filledCount_02++;

        if (filledCount_02 >= totalBlanks_02)
        {
            MarcelCheckAnswer();
        }
    }


    private string ReplaceFirst(string source, string target, string replacement)
    {
        int index = source.IndexOf(target);
        if (index < 0) return source;

        return source.Substring(0, index) + replacement + source.Substring(index + target.Length);
    }


}
