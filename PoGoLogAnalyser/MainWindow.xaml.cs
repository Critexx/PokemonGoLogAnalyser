using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace PoGoLogAnalyser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Pokemon> CatchedPokemons = new List<Pokemon>();

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            string linepattern = @"\[CatchSuccess.*CP";         // Resultat: "[CatchSuccess - 0] Meowth 9.1% perfect. 288 CP" -> matched alles bis und mit "[CatchSuccess" und "CP"
            string namePattern = @"\s\w+\s";                    // Resultat: "Meowth"   -> Leerzeichen, Buchstaben, Leerzeichen
            string ivPattern = @"\d+\.?\d?%";                   // Resultat: "21.5%"    -> Eine oder mehrere Ziffern, optionaler Punkt, optionale Ziffer und Schluss Prozent
            string cpPattern = @"\d+\sCP";                      // Resultat: "205 CP"   -> Eine oder mehrere Ziffern, ein Leerzeichen, gefolgt von "CP"
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Log File (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    string file = openFileDialog.FileName;
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            while (!sr.EndOfStream)
                            {
                                Match result = Regex.Match(sr.ReadLine(), linepattern);
                                if (result.Success)
                                {
                                    Match resultName = Regex.Match(result.Value, namePattern); // "Name"
                                    Match resultIV = Regex.Match(result.Value, ivPattern); // "21.5%"
                                    Match resultCP = Regex.Match(result.Value, cpPattern); // "205 CP"
                                    CatchedPokemons.Add(new Pokemon()
                                    {
                                        Name = resultName.Value,
                                        Iv = double.Parse(resultIV.Value.Substring(0, resultIV.Value.Length - 1)),
                                        Cp = int.Parse(resultCP.Value.Substring(0, resultCP.Value.Length - 3))
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            ListView.ItemsSource = CatchedPokemons;
            LbPokemonCount.Content = CatchedPokemons.Count + " Pokémons";
        }

        // Clear all
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            CatchedPokemons.Clear();
            LbPokemonCount.Content = "";
        }
    }
}

