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
    public partial class FrmSalary : Form
    {
        public FrmSalary()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        SalaryDTO dto = new SalaryDTO();
        private bool combofull = false;
        public SalaryDetailDTO detail = new SalaryDetailDTO();
        public bool IsUpdate = false;
        int oldSalary = 0;

        private void FrmSalary_Load(object sender, EventArgs e)
        {
            dto = SalaryBLL.GetAll();
            if (!IsUpdate)
            {
                dataGridView1.DataSource = dto.Employees;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].HeaderText = "UserNo";
                dataGridView1.Columns[3].HeaderText = "Name";
                dataGridView1.Columns[4].HeaderText = "Surname";
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].HeaderText = "Salary";
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                combofull = false;
                cmbDepartment.DataSource = dto.Departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "ID";
                cmbPosition.DataSource = dto.Positions;
                cmbPosition.DisplayMember = "PositionName";
                cmbPosition.ValueMember = "ID";
                cmbDepartment.SelectedIndex = -1;
                cmbPosition.SelectedIndex = -1;
                if (dto.Departments.Count > 0)
                {
                    cmbMonth.DataSource = dto.Months;
                    cmbMonth.DisplayMember = "MonthName";
                    cmbMonth.ValueMember = "ID";
                }
                combofull = true;
            }
            else if(IsUpdate)
            {
                //panel1.Hide();
                txtUserNo.Text = detail.UserNo.ToString();
                txtName.Text = detail.Name;
                txtSalary.Text = detail.SalaryAmount.ToString();
                txtSurname.Text = detail.Surname;
                txtYear.Text = detail.SalaryYear.ToString();
                cmbMonth.DataSource = dto.Months;
                cmbMonth.DisplayMember = "MonthName";
                cmbMonth.ValueMember = "ID";
                cmbMonth.SelectedValue = detail.MonthID;
                combofull = false;
                cmbDepartment.DataSource = dto.Departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "ID";
                cmbPosition.DataSource = dto.Positions;
                cmbPosition.DisplayMember = "PositionName";
                cmbPosition.ValueMember = "ID";
                cmbDepartment.SelectedIndex = -1;
                cmbPosition.SelectedIndex = -1;
                if (dto.Departments.Count > 0)
                {

                }
                combofull = true;

            }
         
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserNo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtSurname.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtYear.Text = detail.SalaryYear.ToString();
            cmbMonth.SelectedValue = detail.MonthID;
            txtSalary.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            salary.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
           
            
            
 
            
        }

        SALARY salary = new SALARY();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtYear.Text.Trim() =="")
            {
                MessageBox.Show("Please fill the year");
            }
            else if (cmbMonth.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the month");
            }
            else if (txtSalary.Text.Trim() == "")
            {
                MessageBox.Show("Salary is empty.");
            }
            else
            {
                bool control = false;
                if (!IsUpdate)
                {
                    if (salary.EmployeeID == 0)
                    {
                        MessageBox.Show("Please select an employee from table.");
                    }
                    else
                    {
                        salary.Year = DateTime.Today.Year;
                        salary.MonthID = DateTime.Today.Month;
                        salary.Amount = Convert.ToInt32(txtSalary.Text);
                        
                        if (salary.Amount > oldSalary)
                            control = true;
                        SalaryBLL.AddSalary(salary, control);
                        MessageBox.Show("Salary was added");
                        salary = new SALARY();
                    }
                }
                else 
                {
                    DialogResult result = MessageBox.Show("Are you sure?", "title", MessageBoxButtons.YesNo);
                    if(DialogResult.Yes == result)
                    {
                        SALARY salary = new SALARY();
                        salary.ID = detail.SalaryID;
                        salary.EmployeeID = detail.EmployeeID;
                        salary.Year = Convert.ToInt32(txtYear.Text);
                        salary.MonthID = Convert.ToInt32(cmbMonth.SelectedValue);
                        salary.Amount = Convert.ToInt32(txtSalary.Text);

                        if (salary.Amount > detail.OldSalary)
                        {
                            control = true;
                        }

                        
                            SalaryBLL.UpdateSalary(salary, control);
                            MessageBox.Show("Salary was updated.");
                            this.Close();
       
                    }
                }

            }

        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID ==
                Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {

                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource = list.Where(x => x.PositionID ==
                Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            }
        }
    }
}

