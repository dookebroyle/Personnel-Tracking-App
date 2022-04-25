using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class SalaryDAO : EmployeeContext
    {
        public static List<MONTH> GetMonths()
        {
            return db.MONTHs.ToList();
        }

        public static void AddSalary(SALARY salary)
        {
            try
            {
                db.SALARies.InsertOnSubmit(salary);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SalaryDetailDTO> GetSalaries()
        {
            List<SalaryDetailDTO> salaryList = new List<SalaryDetailDTO>();
            try
            {
                var list = (from s in db.SALARies
                            join e in db.EMPLOYEEs on s.EmployeeID equals e.ID
                            join m in db.MONTHs on s.MonthID equals m.ID
                            select new
                            {
                                UserNo = e.UserNo,
                                Name = e.Name,
                                Surname = e.Surname,
                                EmployeeID = s.EmployeeID,
                                Amount = s.Amount,
                                Year = s.Year,
                                MonthName = m.MonthName,
                                MonthID = s.MonthID,
                                salaryID = s.ID,
                                PositionID = e.PositionID,
                                DepartmentID = e.DepartmentID
                            }).OrderBy(x => x.Year).ToList();

                foreach (var item in list)
                {
                    SalaryDetailDTO dto = new SalaryDetailDTO
                    {
                        UserNo = item.UserNo,
                        Name = item.Name,
                        Surname = item.Surname,
                        EmployeeID = item.EmployeeID,
                        SalaryAmount = item.Amount,
                        SalaryYear = item.Year,
                        MonthName = item.MonthName,
                        MonthID = item.MonthID,
                        SalaryID = item.salaryID,
                        PositionID = item.PositionID,
                        DepartmentID = item.DepartmentID
                    };
                    salaryList.Add(dto);
                }
                return salaryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void UpdateSalary(SALARY salary)
        {
            try
            {
                Console.WriteLine(salary.ID);
                SALARY s = db.SALARies.First(x => x.ID == salary.ID);
                s.Amount = salary.Amount;
                s.Year = salary.Year;
                s.MonthID = salary.MonthID;
                db.SubmitChanges();
            }catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
