using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hnk_bilisim
{
    public partial class listele : Form
    {
        string baglanti = "Server=localhost;Database=barinak;Uid=root;Pwd=;";
        public listele()
        {
            InitializeComponent();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "UPDATE hayvanlar SET hayvan_adi =@adi, yas=@yas, cins=@cins, engel_durumu=@engel_durumu, fotograf_adi=@fotograf_adi WHERE id = @satirid;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@adi", txtYeniİsim.Text);
                cmd.Parameters.AddWithValue("@yas",txtYeniYas.Text);
                cmd.Parameters.AddWithValue("@cins", cmbCinsi.Text);
                cmd.Parameters.AddWithValue("@engel_durumu", cbEngel.Checked);
                cmd.Parameters.AddWithValue("@fotograf_adi", pbResim.Text);

                int id = Convert.ToInt32(dgwHayvanlar.SelectedRows[0].Cells["id"].Value);
                cmd.Parameters.AddWithValue("@satirid", id);


                cmd.ExecuteNonQuery();

                DgwDoldur(); //datagridviewi yenile

            }
        }
        void CmbDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT DISTINCT cins FROM hayvanlar;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();




                da.Fill(dt);
                cmbCinsi.DataSource = dt;

                cmbCinsi.DisplayMember = "tur";   //ekranda kullanıcı görür
                cmbCinsi.ValueMember = "tur";     //veritabanına kayıt edilir
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgwHayvanlar.SelectedRows[0];
            int satirId = Convert.ToInt32(dr.Cells[0].Value);

            DialogResult cevap = MessageBox.Show("hayvanı Silmek İstediğinizden Emin Misiniz?",
                                "Şarkı Sil",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error);

            if (cevap == DialogResult.Yes)
            {

                string sorgu = "DELETE FROM hayvanlar where id = @satirid;";

                using (MySqlConnection baglan = new MySqlConnection(baglanti))
                {
                    baglan.Open();
                    MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                    cmd.Parameters.AddWithValue("@satirid", satirId);
                    cmd.ExecuteNonQuery();

                    DgwDoldur(); //tekrar doldurur
                }


            }
           
         
        }

        private void listele_Load(object sender, EventArgs e)
        {
            DgwDoldur();
        }
        void DgwDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT * FROM hayvanlar;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();


                da.Fill(dt);
                dgwHayvanlar.DataSource = dt;
            }
        }

        private void dgwHayvanlar_SelectionChanged(object sender, EventArgs e)
        {
            if (dgwHayvanlar.SelectedRows.Count > 0)
            {
                txtYeniİsim.Text = dgwHayvanlar.SelectedRows[0].Cells["hayvan_adi"].Value.ToString();
                txtYeniYas.Text = dgwHayvanlar.SelectedRows[0].Cells["yas"].Value.ToString();
               cmbCinsi.Text = dgwHayvanlar.SelectedRows[0].Cells["cins"].Value.ToString();
                cbEngel.Text = dgwHayvanlar.SelectedRows[0].Cells["engel_durumu"].Value.ToString();
               // pbResim.Value = Convert.ToDateTime(dgwHayvanlar.SelectedRows[0].Cells["fotograf_adi"].Value);
                
            }
        }
    }
}
