using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TriviaGameLoop : MonoBehaviour {

    public struct Question {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;

        public Question(string questionText, string[] answers, int correctAnswerIndex) {
            this.questionText = questionText;
            this.answers = answers;
            this.correctAnswerIndex = correctAnswerIndex;
        }
    }

    public Question currentQuestion = new Question("What is your favorite color?", new string[] { "blue", "red", "yellow", "white", "black" }, 0);
    public Button[] answerButtons;
    public Text questionText;

    private Question[] questions = new Question[14];
    private int currentQuestionIndex;
    private int[] questionNumbersChoosen = new int[7];
    private int questionsFinished;

    public GameObject[] TriviaPanels;
    public GameObject finalResultsPanel;
    public Text resultsText;
    private int numberOfCorrectAnswers;
    private bool allowSelection = true;
    public GameObject feedbackText;
    public AudioSource feedbackAudio1;
    public AudioSource feedbackAudio2;

    void Start() {


        questions[0] = new Question("Pambansang Puno", new string[] { "Narra", "Apitong", "Sampalok", "Saging", "Niyog" }, 0);
        questions[1] = new Question("Pambansang Dahon", new string[] { "Sabila", "Oregano", "Anahaw", "Banaba", "Sambong" }, 2);
        questions[2] = new Question("Pambansang Prutas", new string[] { "Saging", "Mansanas", "Bayabas", "Mangga", "Melon" }, 3);
        questions[3] = new Question("Pambansang Ibon", new string[] { "Maya", "Agila", "Parot", "Kalapati", "Adarna" }, 1);
        questions[4] = new Question("Pambansang Hayop", new string[] { "Kalabaw", "Kambing", "Baka", "Aso", "Pusa" }, 0);
        questions[5] = new Question("Pambansang Isda", new string[] { "Galunggong", "Sapsap", "Bangus", "Tilapia", "Dalagang Bukid" }, 2);
        questions[6] = new Question("Pambansang Bulaklak ", new string[] { "Sampaguita", "Gumamela", "Santan", "Orkids", "Waling-Waling" }, 0);
        questions[7] = new Question("Pambansang Pagkain", new string[] { "Adobo", "Menudo", "Sinigang", "Lechon", "Paksiw" }, 3);
        questions[8] = new Question("Pambansang Sayaw", new string[] { "Itik-Itik", "Tinikling", "Carinosa", "Pandanggo sa Ilaw", "Sayaw sa Bangko" }, 1);
        questions[9] = new Question("Pambansang Laro", new string[] { "Basketbol", "Taguan", "Sipa", "Arnis", "Luksong Baka" }, 2);
        questions[10] = new Question("Pambansang Bayani", new string[] { "Emilio Aguinaldo", "Jose Rizal", "Andres Bonifacio", "Antonio Luna", "Apolinario Mabini" }, 1);
        questions[11] = new Question("Pambansang Sasakyan ", new string[] { "Bus", "Dyip", "Bisikleta", "Kalesa", "Motorsiklo" }, 3);
        questions[12] = new Question("Pambansang Hiyas", new string[] { "Perlas", "Diamante", "Opal", "Ruby", "Emerald" }, 0);
        questions[13] = new Question("Pambansang Awit", new string[] { "Ang Bayang Ko", "Pilipinas Kong Mahal", "Anak", "Bayang Magiliw", "Lupang Hinirang" }, 4);

        chooseQuestions();
        assignQuestion(questionNumbersChoosen[0]);

    }

    void Update() {
        quitGame();
    }

    void assignQuestion(int questionNum)
    {
        currentQuestion = questions[questionNum];
        questionText.text = currentQuestion.questionText;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
        }
    }

    public void checkAnswer(int buttonNum) {
        if (allowSelection) { 
        if (buttonNum == currentQuestion.correctAnswerIndex)
        {
            print("correct");
            numberOfCorrectAnswers++;
            feedbackText.GetComponent<Text>().text = "Tama ang iyong sagot";
            feedbackText.GetComponent<Text>().color = Color.green;
            feedbackAudio1.Play();
            }
        else {
            print("incorrect");
            feedbackText.GetComponent<Text>().text = "Mali ang iyong sagot";
            feedbackText.GetComponent<Text>().color = Color.red;
            feedbackAudio2.Play();
            }
        StartCoroutine("continueAfterFeedback");
    }
}

    void chooseQuestions() {
        for (int i = 0; i < questionNumbersChoosen.Length; i++) {
            int questionNum = Random.Range(0, questions.Length);
            if (numberNotContained(questionNumbersChoosen, questionNum))
            {
                questionNumbersChoosen[i] = questionNum;
            }
            else {
                i--;
            }
        }
        currentQuestionIndex = Random.Range(0, questions.Length);
    }

    bool numberNotContained(int[] numbers, int num) {
        for (int i = 0; i < numbers.Length; i++) {
            if (num == numbers[i]) {
                return false;
            }
        }
        return true;
    }

    public void moveToNextQuestion() {
        assignQuestion(questionNumbersChoosen[questionNumbersChoosen.Length - 1 - questionsFinished]);
    }

    void displayResults() {
        switch (numberOfCorrectAnswers) {
            case 7:
                resultsText.text = "7/7 ang iyong tamang sagot";
                break;
            case 6:
                resultsText.text = "6/7 ang iyong tamang sagot";
                break;
            case 5:
                resultsText.text = "5/7 ang iyong tamang sagot";
                break;
            case 4:
                resultsText.text = "4/7 ang iyong tamang sagot";
                break;
            case 3:
                resultsText.text = "3/7 ang iyong tamang sagot";
                break;
            case 2:
                resultsText.text = "2/7 ang iyong tamang sagot";
                break;
            case 1:
                resultsText.text = "1/7 ang iyong tamang sagot";
                break;
            case 0:
                resultsText.text = "0/7 ang iyong tamang sagot";
                break;
            default:
                print("");
                break;
        }
    }

    public void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator continueAfterFeedback() {
        allowSelection = false;
        feedbackText.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        if (questionsFinished < questionNumbersChoosen.Length - 1)
        {
            moveToNextQuestion();
            questionsFinished++;
        }
        else
        {
            foreach (GameObject p in TriviaPanels)
            {
                p.SetActive(false);
            }
            finalResultsPanel.SetActive(true);
            displayResults();
        }
        allowSelection = (true);
        feedbackText.SetActive(false);
    }

    void quitGame() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
