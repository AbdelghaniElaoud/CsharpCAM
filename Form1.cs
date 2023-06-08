using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace yarbi
{
    public partial class Form1 : Form
    {
        private string selectedPort = string.Empty;
        private SerialPort serialPort1;  // Déclaration de la variable SerialPort

        public Form1()
        {
            InitializeComponent();
            serialPort1 = new SerialPort();  // Instanciation de la variable SerialPort
        }


        private void FillDataGridViewFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                bool startParsing = false;

                foreach (string line in lines)
                {
                    if (line.Trim() == "Les outils :")
                    {
                        startParsing = true;
                        continue;
                    }

                    if (startParsing)
                    {
                        string trimmedLine = line.Trim();
                        if (!string.IsNullOrEmpty(trimmedLine))
                        {
                            string[] rowData = trimmedLine.Split(' ');
                            dataGridView1.Rows.Add(rowData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la lecture du fichier : " + ex.Message, "Erreur");
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            // Increase the speed by sending a speed value to the Arduino with the direction command
            int speed = trackBar1.Value;

            if (!serialPort1.IsOpen)  // Vérifiez si le port série est déjà ouvert
                serialPort1.Open();  // Ouvrir le port série

            serialPort1.Write("F" + speed.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Increase the speed by sending a speed value to the Arduino with the direction command
            int speed = trackBar1.Value;

            if (!serialPort1.IsOpen)  // Vérifiez si le port série est déjà ouvert
                serialPort1.Open();  // Ouvrir le port série

            serialPort1.Write("B" + speed.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)  // Vérifiez si le port série est ouvert
                serialPort1.Close();  // Fermer le port série
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Détecter tous les ports série disponibles et les ajouter à la comboBox1
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);

            // Définir les propriétés du TrackBar
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 300;
            // Rendre les étiquettes visibles
            labelMin.Visible = true;
            labelMax.Visible = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPort = comboBox1.SelectedItem.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
{
    if (string.IsNullOrEmpty(selectedPort))
    {
        MessageBox.Show("Veuillez sélectionner un port série.", "Port Non Sélectionné");
        return;
    }

    try
    {
        serialPort1.PortName = selectedPort; // Définir le port série sélectionné
        serialPort1.Open(); // Ouvrir le port série

        MessageBox.Show("Le port " + selectedPort + " est disponible.", "Port Disponible");
    }
    catch (UnauthorizedAccessException)
    {
        MessageBox.Show("Le port " + selectedPort + " est occupé.", "Port Occupé");
    }
    catch (Exception ex)
    {
        MessageBox.Show("Une erreur s'est produite lors de la vérification du port " + selectedPort + ": " + ex.Message, "Erreur");
    }
}


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int speed = trackBar1.Value;
        }

        private void labelMin_Click(object sender, EventArgs e)
        {
            // Code à exécuter lorsque le labelMin est cliqué
        }

        private void labelMax_Click(object sender, EventArgs e)
        {
            // Code à exécuter lorsque le labelMax est cliqué
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void send_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                // Vérifier si le texte du TextBox peut être converti en un entier valide
                if (int.TryParse(stepsTextBox.Text, out int steps))
                {
                    // Envoyer la commande "S" suivie du nombre de pas à Arduino
                    serialPort1.WriteLine("S" + steps);

                    // Afficher un message de confirmation
                    MessageBox.Show("La commande a été envoyée avec succès.", "Commande envoyée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Veuillez entrer un nombre de pas valide.", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers texte (.txt)|.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                FillDataGridViewFromFile(filePath);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
