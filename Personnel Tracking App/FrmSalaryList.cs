﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DTO;
using BLL;
using DAL;

namespace Personnel_Tracking_App
{
    public partial class FrmSalaryList : Form
    {
        public FrmSalaryList()
        {
            InitializeComponent();
        }

        SalaryDTO dto = new SalaryDTO();
        private bool combofull = false;
        SalaryDetailDTO detail = new SalaryDetailDTO();


        private void rbMore_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tbEquals_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmSalary frm = new FrmSalary();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.SalaryID == 0)
                MessageBox.Show("Please select a salary from the table.");
            else
            {
                FrmSalary frm = new FrmSalary();
                frm.IsUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();
            }
        }




        void FillAllData()
        {
            dto = SalaryBLL.GetAll();
            if (!UserStatic.isAdmin)
                dto.Salaries = dto.Salaries.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            dataGridView1.DataSource = dto.Salaries;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            if (dto.Departments.Count > 0)
                combofull = true;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            cmbMonth.DataSource = dto.Months;
            cmbMonth.DisplayMember = "MonthName";
            cmbMonth.ValueMember = "ID";
            cmbMonth.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;

        }

        private void FrmSalaryList_Load(object sender, EventArgs e)
        {
            FillAllData();
            //  dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "UserNo";
            //  dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].HeaderText = "Name";
            dataGridView1.Columns[4].HeaderText = "Surname";
            //dataGridView1.Columns[5].Visible = false;
            //dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].HeaderText = "PositionID";
            dataGridView1.Columns[8].HeaderText = "DepartmentID";
            dataGridView1.Columns[9].HeaderText = "Year";
            dataGridView1.Columns[10].HeaderText = "MonthID";
            dataGridView1.Columns[11].HeaderText = "Month";
            dataGridView1.Columns[12].HeaderText = "Salary";
            //  dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].HeaderText = "Old Salary";


        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID ==
                Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

  

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<SalaryDetailDTO> list = dto.Salaries;
            if (txtUserNo.Text.Trim() != "")
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                list = list.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                list = list.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                list = list.Where(x => x.PositionID == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (txtYear.Text.Trim() != "")
                list = list.Where(x => x.SalaryYear == Convert.ToInt32(txtSalary.Text)).ToList();
            if (cmbMonth.SelectedIndex != -1)
                list = list.Where(x => x.MonthID == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            if (txtSalary.Text.Trim() != "")
            {
                if (rbMore.Checked)
                    list = list.Where(x => x.SalaryAmount > Convert.ToInt32(txtSalary.Text)).ToList();
                else if (rbLess.Checked)

                    list = list.Where(x => x.SalaryAmount < Convert.ToInt32(txtSalary.Text)).ToList();
                else

                    list = list.Where(x => x.SalaryAmount == Convert.ToInt32(txtSalary.Text)).ToList();
            }
            dataGridView1.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }
        private void CleanFilters()
        {
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            cmbMonth.SelectedIndex = -1;
            rbMore.Checked = false;
            rbLess.Checked = false;
            rbEquals.Checked = false;
            txtYear.Clear();
            txtSalary.Clear();
            dataGridView1.DataSource = dto.Salaries;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.Name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.Surname = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            detail.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);

            Console.WriteLine(dataGridView1.Rows[e.RowIndex].Cells[13].Value);
            detail.SalaryID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[13].Value);
            detail.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            detail.MonthID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[10].Value);
            detail.SalaryAmount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            detail.OldSalary = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[14].Value);
            detail.SalaryYear = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[9].Value);


        }
    }
}