using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL;

namespace DAL.DAO

{
    public class EmployeeDAO : EmployeeContext
    {
        public static void AddEmployee(EMPLOYEE employee)
        {
            try
            {
                db.EMPLOYEEs.InsertOnSubmit(employee);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

		public static List<EmployeeDetailDTO> GetEmployees()
		{
			List<EmployeeDetailDTO> employeeList = new List<EmployeeDetailDTO>();
			var list = (from e in db.EMPLOYEEs
						join d in db.DEPARTMENTs on e.DepartmentID equals d.ID
						join p in db.POSITIONs on e.PositionID equals p.ID
						select new
						{
							UserNo = e.UserNo,
							Name = e.Name,
							Surname = e.Surname,
							EmployeeID = e.ID,
							Password = e.Password,
							DepartmentName = d.DepartmentName,
							PositionName = p.PositionName,
							DepartmentID = e.DepartmentID,
							PositionId = e.PositionID,
							isAdmin = e.isAdmin,
							Salary = e.Salary,
							ImagePath = e.ImagePath,
							BirthDay = e.BirthDay,
							Address = e.Address

						}).OrderBy(x => x.UserNo).ToList();
			foreach (var item in list)
			{
				EmployeeDetailDTO dto = new EmployeeDetailDTO();
				dto.Name = item.Name;
				dto.UserNo = item.UserNo;
				dto.Surname = item.Surname;
				dto.EmployeeID = item.EmployeeID;
				dto.Password = item.Password;
				dto.DepartmentID = item.DepartmentID;
				dto.DepartmentName = item.DepartmentName;
				dto.PositionID = item.PositionId;
				dto.PositionName = item.PositionName;
				dto.isAdmin = item.isAdmin;
				dto.Salary = item.Salary;
				dto.BirthDay = item.BirthDay;
				dto.Address = item.Address;
				dto.ImagePath = item.ImagePath;
				employeeList.Add(dto);
			}
			return employeeList;
		}

        public static void DeleteEmployee(int employeeID)
        {
            try
            {
				EMPLOYEE e = db.EMPLOYEEs.First(x => x.ID == employeeID);
				db.EMPLOYEEs.DeleteOnSubmit(e);
				db.SubmitChanges();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void UpdateEmployee(EMPLOYEE employee)
        {
			try
			{
				EMPLOYEE e = db.EMPLOYEEs.First(x => x.ID == employee.ID);
				e.UserNo = employee.UserNo;
				e.Name = employee.Name;
				e.Surname = employee.Surname;
				e.Password = employee.Password;
				e.isAdmin = employee.isAdmin;
				e.BirthDay = employee.BirthDay;
				e.Address = employee.Address;
				e.DepartmentID = employee.DepartmentID;
				e.PositionID = employee.PositionID;
				employee.Salary = employee.Salary;
				db.SubmitChanges();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

        public static void UpdateEmployee(int employeeID, int amount)
        {
            try
            {
				EMPLOYEE employee = db.EMPLOYEEs.First(x => x.ID == employeeID);
				employee.Salary = amount;
				db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

		public static void UpdateEmployee(POSITION position)
        {
            try
            {
				List<EMPLOYEE> list = db.EMPLOYEEs.Where(x => x.PositionID == position.ID).ToList();
				foreach(var item in list)
                {
					item.DepartmentID = position.DepartmentID;
                }
				db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<EMPLOYEE> GetEmpoyees(int v, string text)
        {
            try
            {
				List<EMPLOYEE> list = db.EMPLOYEEs.Where(x => x.UserNo == v && x.Password == text).ToList();
				return list;
            }
			catch (Exception ex)
            {
				throw ex;
            }
        }

        public static List<EMPLOYEE> GetUsers(int v)
        {
            return db.EMPLOYEEs.Where(x => x.UserNo == v).ToList();
        }
    }
}

