using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman
{
    public partial class Form1 : Form
    {
        private string Word;
        private string HiddenWord;
        private int Mistake;
        private char Key;
        public bool GameReady;
        private List<string> Words = new List<string>();
        private List<Image> images = new List<Image>();

        public Form1()
        {
            GameReady = false;
            InitializeComponent();
            LoadResources();
            GameReady = Init();
        }
        private void GetWords()
        {
            FileStream file = new FileStream("words.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream)
            {
                string container = sr.ReadLine();
                Words.Add(container);
            }
        }
        private void LoadImages()
        {
            images.Add(Image.FromFile("graphics/h1.png"));
            images.Add(Image.FromFile("graphics/h2.png"));
            images.Add(Image.FromFile("graphics/h3.png"));
            images.Add(Image.FromFile("graphics/h4.png"));
            images.Add(Image.FromFile("graphics/h5.png"));
            images.Add(Image.FromFile("graphics/h6.png"));
            images.Add(Image.FromFile("graphics/h7.png"));
            images.Add(Image.FromFile("graphics/h8.png"));
            images.Add(Image.FromFile("graphics/h9.png"));
            images.Add(Image.FromFile("graphics/h10.png"));

        }

        public bool Init()
        {
            Key = '#';
            Mistake = 0;
            LoadImages();
            Word = GenNewWord();
            HiddenWord = HideWord(Word);
            LabelWord.Text = HiddenWord;
            PBImage.Image = images[Mistake];
            return true;
        }
        private void LoadResources()
        {
            GetWords();
        }
        private string GenNewWord()
        {
            Random rnd = new Random();
            return Words[rnd.Next(Words.Count - 1)];
        }

        private void ButtonLetter_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            if (!Word.ToString().Contains(((Button)sender).Text))
            {
                Mistake++;
            }
            else
            {
                HiddenWord = ShowLetters(HiddenWord, ((Button)sender).Text[0]);
                LabelWord.Text = HiddenWord;
                if (!IsLetterHidden())
                {
                    DialogResult result = MessageBox.Show("Congratulation! \n Would you like to play again? ", "You have won.", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Init();
                        UnlockButtons();
                    }
                    else
                    {
                        Quit();
                    }
                }
            }
            if (Mistake >= 9)
            {
                PBImage.Image = images[Mistake];
                GameLost();
            }
            else
            {
                PBImage.Image = images[Mistake];
            }
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            Init();
            UnlockButtons();
        }

        private void GameLost()
        {
            DialogResult result = MessageBox.Show("You Lost \n Would you like to restart the game?", "Game over", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Init();
                UnlockButtons();
            }
            else
            {
                Quit();
            }
        }
        private void UnlockButtons()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button && (string)c.Tag == "LetterButton")
                {
                    c.Enabled = true;
                }
            }
        }
        private string HideWord(string Word)
        {
            string hiddenWord = "";
            for (int i = 0; i < Word.Length; i++)
            {
                hiddenWord += Key;
            }
            return hiddenWord;
        }
        private string ShowLetters(string message, char letter)
        {
            char[] word = message.ToCharArray();
            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == letter)
                {
                    word[i] = letter;
                }
            }
            string output = new string(word);
            return output;
        }
        private bool IsLetterHidden()
        {
            if (!HiddenWord.Contains(Key))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void Quit()
        {
            Application.Exit();
        }
    }
}
