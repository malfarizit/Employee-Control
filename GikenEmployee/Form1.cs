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
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;

namespace GikenEmployee
{
    public partial class Form1 : Form
    {
        string emp_gender;
        string email;
        string imagelocation = "";
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-6DDLFRC;Initial Catalog=GikenData;Integrated Security=True");
        SqlCommand cmd;
        DataTable dt1 = new DataTable();
        public Form1()
        {
            InitializeComponent();
            display();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtEmpNumber.Text == "" || txtName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || txtDepartement.Text == "" || cmbGrade.Text == "" || emp_gender == "" || txtAddress.Text == "")
            {
                MessageBox.Show("Please Fill All Data");
            }
            else 
            {
                Check();
                if (dt1.Rows.Count == 1)
                {
                    MessageBox.Show("Duplicate Employee Number");
                    txtEmpNumber.Text = "";
                }
                else
                {
                    byte[] images = null;
                    FileStream stream = new FileStream(imagelocation, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(stream);
                    images = br.ReadBytes((int)stream.Length);
                    email = txtEmail.Text;
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into [employees] (employee_number,employee_name,email,phone,joint_date,departement,Grade,gender,employee_address,employee_image) values ('" + txtEmpNumber.Text + "', '" + txtName.Text + "', '" + email + "', '" + txtPhone.Text + "', '" + dtJoint.Text + "', '" + txtDepartement.Text + "', '" + cmbGrade.Text + "', '" + emp_gender + "', '" + txtAddress.Text + "', @images)";
                    cmd.Parameters.Add(new SqlParameter("@images", images));
                    cmd.ExecuteNonQuery();
                    cleardata();
                    conn.Close();
                    display();
                }
                

            }
            
        }

        private void rbL_CheckedChanged(object sender, EventArgs e)
        {
            emp_gender = "L";
        }

        private void rbP_CheckedChanged(object sender, EventArgs e)
        {
            emp_gender = "P";
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|All Files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imagelocation = dialog.FileName.ToString();
                picEmp.ImageLocation = imagelocation;
            }
        }

        public void display()
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from [employees]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter dta = new SqlDataAdapter(cmd);
            dta.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void Check()
        {
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from employees where employee_number='" + txtEmpNumber.Text + "'";
            cmd.ExecuteNonQuery();
            
            SqlDataAdapter dta1 = new SqlDataAdapter(cmd);
            dta1 = new SqlDataAdapter(cmd);
            dta1.Fill(dt1);
            conn.Close();
        }

        public void cleardata()
        {
            txtEmpNumber.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtDepartement.Text = "";
            cmbGrade.Text = "";
            txtAddress.Text = "";
            rbL.Checked = false;
            rbP.Checked = false;
            dtJoint.Value = DateTime.Now;
            picEmp.ImageLocation = null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            display();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from employees where employee_number = '" + txtEmpNumber.Text + "'";
            cmd.ExecuteNonQuery();
            cleardata();
            conn.Close();
            display();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'gikenDataDataSet.employees' table. You can move, or remove it, as needed.
            this.employeesTableAdapter.Fill(this.gikenDataDataSet.employees);

        }
    }
}
