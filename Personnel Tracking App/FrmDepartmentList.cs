﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;

namespace Personnel_Tracking_App
{
    public partial class FrmDepartmentList : Form
    {
        List<DEPARTMENT> departmentList = new List<DEPARTMENT>();
        public FrmDepartmentList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmDepartment frm = new FrmDepartment();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            departmentList = DepartmentBLL.GetDepartments();
            dataGridView1.DataSource = departmentList;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmDepartment frm = new FrmDepartment();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
        }

        private void FrmDepartmentList_Load(object sender, EventArgs e)
        {
            departmentList = DepartmentBLL.GetDepartments();
            dataGridView1.DataSource = departmentList;
            dataGridView1.Columns[0].HeaderText = "Department ID";
            dataGridView1.Columns[1].HeaderText = "Department Name";


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
