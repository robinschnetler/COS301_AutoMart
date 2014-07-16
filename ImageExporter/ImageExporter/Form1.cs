using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace ImageExporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bytImage = null;
            string constring = @"Data Source=localhost;Initial Catalog=AutomartImages;Integrated Security=True";
//            SqlCommand command = new SqlCommand(@"select count(c.AdvertID) as advert from (select advert.AdvertID 
//,advert.MainImage
//from [AutomartImages].[dbo].[DSAdvertImage] as advert
//inner join
//(select table2.AdvertID
//, table2.CategoryID 
//from [AutomartImages].[dbo].[DSAdvert] as table2
//where CategoryID = 172 or CategoryID = 173) as images
//on advert.AdvertID = images.AdvertID) as c");
//            command.CommandType = CommandType.Text;
            
            //reader.Read();
            //int count = (int)reader.GetInt32(0);
            //button1.Text = Convert.ToString
            SqlCommand command = new SqlCommand(@"select advert.AdvertID 
,advert.MainImage
from [AutomartImages].[dbo].[DSAdvertImage] as advert
inner join
(select table2.AdvertID
, table2.CategoryID 
from [AutomartImages].[dbo].[DSAdvert] as table2
where CategoryID = 172 or CategoryID = 173) as images
on advert.AdvertID = images.AdvertID");
            command.CommandType = CommandType.Text;
            SqlConnection myconn = new SqlConnection(constring);

            command.Connection = myconn;
            myconn.Open();

            SqlDataReader reader = command.ExecuteReader();

            Int32 c = 1;
            while (reader.Read())
            {
                bytImage = (byte[])reader["MainImage"];
                String id = reader["AdvertID"].ToString();
                if (bytImage != null)
                {
                    MemoryStream ms = new MemoryStream(bytImage);
                    Image img = new Bitmap(ms);
                    img.Save("adverts/" + id + ".jpeg");
                    label1.Text = c.ToString();
                }
            }
            System.Windows.Forms.MessageBox.Show("Extraction complete.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }            
    }
}
