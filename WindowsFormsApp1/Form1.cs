using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Fonction initial lors de loading 
        private void Form1_Load(object sender, EventArgs e)
        {
            get_Ville();
            combo_ville();
            get_Client(); 
        }

        public void get_Ville() {
            using (VilleEntities Context_Ville = new VilleEntities())
            {
                var ville = Context_Ville.Villes.Select(x => new { nom = x.nomV , Code_Postal = x.Code_Postal }).Where(x => x.nom.ToString().StartsWith("M")).OrderBy(x => x.Code_Postal).ToList();
                dataGridView1.DataSource = ville;
            }
        }

        public void get_Client()
        {
            using (VilleEntities Context_Ville = new VilleEntities())
            {
                var Client = Context_Ville.Clients.Select(x => new { nom = x.nom ,Prenom=x.prenom, DateNaissance=x.Date_naissance }).ToList();
                //dataGridView2.DataSource = Client;
                foreach (var c in Client) {
                    dataGridView2.Rows.Add(c.nom , c.Prenom , c.DateNaissance);
                }

            }
        }

        public void combo_ville() {

            using (VilleEntities Context_Ville = new VilleEntities())
            {
                var ville = Context_Ville.Villes.ToList();
                comboBox1.DataSource = ville;
                comboBox1.ValueMember = "id_ville";
                comboBox1.DisplayMember = "nomV"; 
            }
        }


        private void ChercherVille(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Le Champs id est obligatoire dans la phase de recherche");
            }
            else {
                using (VilleEntities Context_Ville = new VilleEntities())
                {
                    int id = Int32.Parse(textBox1.Text);
                    dataGridView2.Rows.Clear();
                    dataGridView2.DataSource = null;
                    var Client2 = Context_Ville.Clients.Select(x => new { nom = x.nom, Prenom = x.prenom, DateNaissance = x.Date_naissance, x.Client_id , x.Idville }).Where(x => x.Client_id == id).ToList().FirstOrDefault();

                    if (Client2 == null) {
                        MessageBox.Show("il n'y a pas aucun Client avec cette id"); 
                    }
                    else
                    {
                        dataGridView2.Rows.Add(Client2.nom, Client2.Prenom, Client2.DateNaissance);
                        textBox2.Text = Client2.nom;
                        textBox3.Text = Client2.Prenom;
                        textBox4.Text = Client2.DateNaissance.ToString();
                        comboBox1.SelectedValue = Client2.Idville; 
                    }
                }
            }
        }
        string Action = ""; 

        private void button2_Click(object sender, EventArgs e)
        {
            Action = "Ajouter"; 


        }

        private void button6_Click(object sender, EventArgs e)
        {
            vider_Client(); 
        }
        private void vider_Client() {

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.SelectedItem = -1; 

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Action == "Ajouter")
            {
                using (VilleEntities context_ville = new VilleEntities())
                {

                    Client c = new Client();
                    c.nom = textBox2.Text;
                    c.prenom = textBox3.Text;
                    c.Date_naissance = Convert.ToDateTime(textBox4.Text.ToString());
                    c.Idville = Int32.Parse(comboBox1.SelectedValue.ToString());

                    context_ville.Clients.Add(c);
                    context_ville.SaveChanges();
                }
            }
            else if (Action == "Modifier")
            {
                using (VilleEntities context_ville = new VilleEntities())
                {
                    int id = Int32.Parse(textBox1.Text); 
                    Client c = context_ville.Clients.Where(x => x.Client_id == id ).FirstOrDefault(); 
                    c.nom = textBox2.Text;
                    c.prenom = textBox3.Text;
                    c.Date_naissance = Convert.ToDateTime(textBox4.Text.ToString());
                    c.Idville = Int32.Parse(comboBox1.SelectedValue.ToString());

                    //context_ville.Clients.Add(c);
                    context_ville.SaveChanges();
                }
            }
            else if (Action == "Supprimer")
            {
                using (VilleEntities context_ville = new VilleEntities())
                {
                    int id = Int32.Parse(textBox1.Text);
                    Client c = context_ville.Clients.Where(x => x.Client_id == id).FirstOrDefault();
                    context_ville.Clients.Remove(c);
                    context_ville.SaveChanges();
                    dataGridView2.Rows.Clear(); 
                    get_Client();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Action = "Modifier"; 

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Action = "Supprimer";

        }
    }
}
