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
using DAL;
using BLL;

namespace Personnel_Tracking_App
{
    public partial class FrmPermissionList : Form
    {
        
        private bool combofull = false;
        
        public FrmPermissionList()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmPermission frm = new FrmPermission();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.PermissionID == 0)
                MessageBox.Show("please select a permission from table");

            else
            {

                FrmPermission frm = new FrmPermission();
                frm.IsUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void FillAllData()
        {
            dto = PermissionBLL.GetAll();
            dataGridView1.DataSource = dto.Permissions;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            cmbPermissionState.DataSource = dto.PermissionStates;
            cmbPermissionState.DisplayMember = "StateName";
            cmbPermissionState.ValueMember = "ID";
            cmbPermissionState.SelectedIndex = -1;
            combofull = true;
        }

        PermissionDTO dto = new PermissionDTO();
        private void FrmPermissionList_Load(object sender, EventArgs e)
        {
            FillAllData();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "UserNo";
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].HeaderText = "Name";
            dataGridView1.Columns[4].HeaderText = "Surname";
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].HeaderText = "Permission Start Date";
            dataGridView1.Columns[10].HeaderText = "Permission End Date";
            dataGridView1.Columns[11].HeaderText = "Permission Day Amount";
            dataGridView1.Columns[12].HeaderText = "Permission State";
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].HeaderText = "Explanation";
            dataGridView1.Columns[15].Visible = false;
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
            rbEndDate.Checked = false;
            rbStartDate.Checked = false;
            cmbPermissionState.SelectedIndex = -1;
            txtDayAmount.Clear();
            dataGridView1.DataSource = dto.Permissions;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<PermissionDetailDTO> list = dto.Permissions;
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
            if (rbStartDate.Checked)
                list = list.Where(x => x.StartDate < Convert.ToDateTime(dtEndDate.Value) &&
                  x.StartDate > Convert.ToDateTime(dtStartDate.Value)).ToList();
            else if (rbEndDate.Checked)
                list = list.Where(x => x.EndDate < Convert.ToDateTime(dtEndDate.Value) &&
                  x.EndDate > Convert.ToDateTime(dtStartDate.Value)).ToList();

            if (cmbPermissionState.SelectedIndex != -1)
                list = list.Where(x => x.PermissionState == Convert.ToInt32(cmbPermissionState.SelectedValue)).ToList();

            if (txtDayAmount.Text.Trim() != "")
                list = list.Where(x => x.PermissionDayAmount == Convert.ToInt32(txtDayAmount.Text)).ToList();


            dataGridView1.DataSource = list;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilters();
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

                List<PermissionDetailDTO> list = dto.Permissions;
                dataGridView1.DataSource = list.Where(x => x.PositionID ==
                Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            }
        }

        private void cmbPermissionState_SelectedIndexChanged(object sender, EventArgs e)
        {
   
        }
        PermissionDetailDTO detail = new PermissionDetailDTO();
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.PermissionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[15].Value);
            detail.StartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
            detail.EndDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[10].Value);
            detail.Explanation = dataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString();
            detail.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.PermissionState = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[15].Value);
            detail.PermissionDayAmount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
           

        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionState.Approved);
            MessageBox.Show("Approved");
            FillAllData();
            CleanFilters();
        }

        private void btnDisapprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionState.Disapproved);
            MessageBox.Show("Disapproved");
            FillAllData();
            CleanFilters();
        }
    }
}

