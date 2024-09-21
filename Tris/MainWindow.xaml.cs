using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tris
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private bool isPlayerOneTurn = true; // true se è il turno del giocatore 1 (X), false per il giocatore 2 (O)
        private int playerOneScore = 0;
        private int playerTwoScore = 0;
        private int player1Wins = 0;
        private int player2Wins = 0;

        public MainWindow()
        {
            InitializeComponent();
            ResetGame();
        }

        // Evento per il click su un pulsante della griglia del Tris
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Se il pulsante è già stato cliccato, non facciamo nulla
            if (!string.IsNullOrEmpty(clickedButton.Content?.ToString()))
                return;

            // Imposta il testo del pulsante con la X o la O
            if (isPlayerOneTurn)
            {
                clickedButton.Content = "X";
                clickedButton.Foreground = Brushes.Red;
            }
            else
            {
                clickedButton.Content = "O";
                clickedButton.Foreground = Brushes.Blue;
            }

            // Controlla se qualcuno ha vinto
            if (CheckForWinner())
            {
                if (isPlayerOneTurn)
                {
                    playerOneScore++;
                    MessageBox.Show("Player 1 wins!");
                }
                else
                {
                    playerTwoScore++;
                    MessageBox.Show("Player 2 wins!");
                }

                UpdateScore();

                if (playerOneScore == 2 || playerTwoScore == 2)
                {
                    MessageBox.Show($"GAME FINITO {(playerOneScore == 2 ? "Player 1" : "Player 2")} ha vinto il match");
                    ResetScores();
                }

                ResetGame();
            }
            else if (IsBoardFull())
            {
                MessageBox.Show("PAREGGIO!");
                ResetGame();
            }
            else
            {
                // Cambia turno
                isPlayerOneTurn = !isPlayerOneTurn;
                UpdateTurnIndicator();
            }
        }

        private bool IsBoardFull()
        {
            foreach(var control in GameGrid.Children)
            {
                if(control is Button button && string.IsNullOrEmpty(button.Content?.ToString()))
                    return false;
            }
            return true;
        }
        private void UpdateTurnIndicator()
        {
            TurnIndicator.Text = $"Tocca al player: {(isPlayerOneTurn ? "X" : "O")}";
        }

        // Resetta il gioco
        private void ResetGame()
        {

            // Pulisce tutti i pulsanti nella griglia
            foreach (var control in GameGrid.Children)
            {
                if (control is Button button)
                {
                    button.Content = null;
                }
            }

            // Imposta il turno del giocatore 1
            isPlayerOneTurn = true;
        }

        // Aggiorna il testo del punteggio
        private void UpdateScore()
        {
            ScoreText.Text = $"Player 1: {playerOneScore} - Player 2: {playerTwoScore}";
        }


        private void ResetScores()
        {
            playerOneScore = 0;
            playerTwoScore = 0; 
            UpdateScore(); 
            ResetGame();
        }

        // Evento per il click sul pulsante reset
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetScores();
        }

        // Funzione per controllare se c'è un vincitore
        private bool CheckForWinner()
        {
            // Verifica tutte le combinazioni vincenti
            string[][] winningCombinations = new string[][]
            {
                new string[] { Button00.Content?.ToString(), Button01.Content?.ToString(), Button02.Content?.ToString() },
                new string[] { Button10.Content?.ToString(), Button11.Content?.ToString(), Button12.Content?.ToString() },
                new string[] { Button20.Content?.ToString(), Button21.Content?.ToString(), Button22.Content?.ToString() },
                new string[] { Button00.Content?.ToString(), Button10.Content?.ToString(), Button20.Content?.ToString() },
                new string[] { Button01.Content?.ToString(), Button11.Content?.ToString(), Button21.Content?.ToString() },
                new string[] { Button02.Content?.ToString(), Button12.Content?.ToString(), Button22.Content?.ToString() },
                new string[] { Button00.Content?.ToString(), Button11.Content?.ToString(), Button22.Content?.ToString() },
                new string[] { Button02.Content?.ToString(), Button11.Content?.ToString(), Button20.Content?.ToString() }
            };

            foreach (var combination in winningCombinations)
            {
                if (!string.IsNullOrEmpty(combination[0]) &&
                    combination[0] == combination[1] &&
                    combination[1] == combination[2])
                {
                    return true;
                }
            }

            return false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Sei sicuro di voler uscire dall'applicazione?", "Conferma uscita", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                this.Close();
        }
    }
}