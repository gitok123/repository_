using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Math;

namespace Cosmetics_store2
{
    
    public partial class ProductsForm : Form
    {
        public ProductsForm(MainForm M)
        {
            InitializeComponent();
            
        }
        public List<ProdItem> retProdItems()
        {
            return new List<ProdItem>() {
                new ProdItem(panel1,label1,label2,pictureBox7,pictureBox1),
                new ProdItem(panel2,label3,label4,pictureBox8,pictureBox2),
                new ProdItem(panel3,label5,label6,pictureBox9,pictureBox3),
                new ProdItem(panel4,label7,label8,pictureBox10,pictureBox4),
                new ProdItem(panel5,label9,label10,pictureBox11,pictureBox5),
                new ProdItem(panel6,label11,label12,pictureBox12,pictureBox6)
            };
        }
        public ProductsForm()
        {
            InitializeComponent();
        }
        public DBModel1 mdB = new DBModel1();
        public int indx = 0;
        public string rootPth = @"V:\! Учебная практика - Программные решения для бизнеса (2020)\Примеры заданий для ДЭ 2021\11 - Магазин косметики (ДЭ 2020 осень)\Ресурсы для задания\";
        public int top = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            //List<Product> Prdcts = mdB.Product.ToList();
            //foreach (Product P in Prdcts)
            //{
            //    comboBox1.AutoCompleteCustomSource.Add(P.Manufacturer.Name);
            //}
            List<Manufacturer> Manufacturers = mdB.Manufacturer.ToList();
            List<string> manufNames=new List<string>{""};
            foreach (Manufacturer m in Manufacturers)
            {
                comboBox1.AutoCompleteCustomSource.Add(m.Name);
                manufNames.Add(m.Name);
            }
            comboBox1.DataSource = manufNames;
            UPD();
        }
        private void UPD()
        {
            mdB = new DBModel1();
            int itmsonpg = retProdItems().Count;
            List<Product> products = mdB.Product.ToList();
            List<Product> productsfiltered = products;
            for (int i = 0, j = 0; i < products.Count ; i++)
            {
                Product P = products[i];
                if (
                    (!P.Title.ToLower().Contains(textBox1.Text.ToLower()) && !P.Description.ToLower().Contains(textBox1.Text.ToLower()))
                    ||(P.Manufacturer.Name.ToLower() != comboBox1.Text.ToLower() && comboBox1.Text != "")
                    )
                {
                    productsfiltered.RemoveAt(productsfiltered.IndexOf(P));
                    j++;i--;
                }
            }
            top = productsfiltered.Count;
            indx = Max(0, Min(top - itmsonpg, indx));
            int visibleitmsonpg = 0;
            foreach (ProdItem I in retProdItems())
            {
                try
                {
                    I.ttl.Text = productsfiltered[indx].Title;
                    I.dsc.Text = $"{productsfiltered[indx].Cost:f2} ₽";
                    if (!productsfiltered[indx].IsActive) I.pnl.BackColor = Color.DarkGray; else I.pnl.BackColor = Color.White;
                    I.boximg.Image = Image.FromFile(rootPth + productsfiltered[indx].MainImagePath);
                    I.boximg.SizeMode = PictureBoxSizeMode.Zoom;
                    I.pnl.Visible = true;
                }
                catch{ I.pnl.BackColor = Color.FromArgb(255,0,0); I.pnl.Visible = false; }
                indx++;
                if (I.pnl.Visible) visibleitmsonpg++;
            }
            indx -= 6;
            label13.Text = $"{indx + Min(indx+ visibleitmsonpg, 1)}-{indx + visibleitmsonpg} из {productsfiltered.Count}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int itmsonpg = retProdItems().Count;
            indx -= indx < itmsonpg ? indx : itmsonpg;
            UPD();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int itmsonpg = retProdItems().Count;
            indx += indx > top - itmsonpg * 2 + 1  ? top - indx - itmsonpg : itmsonpg;
            UPD();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int itmsonpg = retProdItems().Count;
            for (int i = 0; i < 7; i++) indx -= indx < itmsonpg ? indx : itmsonpg;
            UPD();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int itmsonpg = retProdItems().Count;
            for (int i = 0; i < 7; i++) indx += indx > top - itmsonpg * 2 + 1 ? top - indx - itmsonpg : itmsonpg;
            UPD();
        }
        private void DO_UPD(object sender, EventArgs e)
        {
            UPD();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MainForm f = new MainForm();
            Form f = Application.OpenForms[0];
            f.Show();
        }
        private void edit_item_pictureBox_Click(object sender, EventArgs e)
        {
            List<Product> products = mdB.Product.ToList();
            Product theProduct = new Product();
            List<ProdItem> list = retProdItems();
            for (int i = 0; i < list.Count; i++)
            {
                ProdItem p = list[i];
                if (p.settingsIcn.GetHashCode() == sender.GetHashCode())
                    theProduct = products[indx + i];
            }
            EditProductForm edit = new EditProductForm(theProduct);
            edit.ShowDialog();
            UPD();
        }
    }
    public partial class ProdItem //: Form
    {
        public Label ttl, dsc;
        public PictureBox boximg = null, settingsIcn = null;
        public Panel pnl = null;
        public ProdItem(Panel p, Label n, Label d, PictureBox sttngicn)
        {
            ttl = n; dsc = d; pnl = p; settingsIcn = sttngicn;
        }
        public ProdItem(Panel p, Label n, Label d, PictureBox sttngicn, PictureBox im)
        {
            ttl = n; dsc = d; pnl = p; settingsIcn = sttngicn; boximg = im;
        }
    }
}
