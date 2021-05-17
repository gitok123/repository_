using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cosmetics_store2
{
    public partial class EditProductForm : Form
    {
        DBModel1 mdB = new DBModel1();
        Product product = new Product();
        public EditProductForm(Product product)
        {
            InitializeComponent();
            this.product = product;
        }
        private void EditProductForm_Load(object sender, EventArgs e)
        {
            List<Manufacturer> Manufacturers = mdB.Manufacturer.ToList();
            List<string> manufNames = new List<string> { "" };
            foreach (Manufacturer m in Manufacturers)
            {
                comboBox1.AutoCompleteCustomSource.Add(m.Name);
                manufNames.Add(m.Name);
            }
            textBox1.Text = product.Title;
            textBox2.Text = $"{product.Cost:f2}";
            textBox3.Text = product.Description;
            textBox4.Text = product.MainImagePath;
            textBox5.Text = product.IsActive.ToString();
            comboBox1.DataSource = manufNames;
            comboBox1.Text = product.Manufacturer.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product dbProd = mdB.Product.Find(product.ID);
            //dbProd.Title = textBox1.Text;
            //dbProd.Cost = Convert.ToDecimal(textBox2.Text);
            dbProd.Description = textBox3.Text;
            //dbProd.MainImagePath = textBox4.Text;
            //dbProd.IsActive = Convert.ToBoolean(textBox5);
            //dbProd.ID =  comboBox1.Text;/////
            mdB.SaveChanges();
            DestroyHandle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Close(); //?
            DestroyHandle(); //?
            //Dispose(); //?
        }

        private void textBox5_DoubleClick(object sender, EventArgs e)
        {
            textBox5.Text = textBox5.Text.ToLower() == "true" ? "False" : "True";
        }

        private void textBox4_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All files|*|BMP|*.bmp|GIF|*.gif|JPEG|*.jpeg|EXIF|*.exif|PNG|*.png|TIFF|*.tiff|SVG|*.svg";//"txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;
            fileDialog.ShowDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = fileDialog.FileName;
            }
        }
    }
}
