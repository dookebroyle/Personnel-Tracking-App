using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL;


namespace DAL.DAO
{
    public class PermissionDAO : EmployeeContext
    {
        public static void AddPermission(PERMISSION permission)
        {
            try
            {
                db.PERMISSIONs.InsertOnSubmit(permission);
                db.SubmitChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static List<PERMISSIONSTATE> GetPermissionStates()
        {
            try
            {
                return db.PERMISSIONSTATEs.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public static List<PermissionDetailDTO> GetPermissions()
        {
            List<PermissionDetailDTO> permissionsList = new List<PermissionDetailDTO>();

            try
            {
                var list = (from p in db.PERMISSIONs
                            join s in db.PERMISSIONSTATEs on p.PermissionState equals s.ID
                            join e in db.EMPLOYEEs on p.EmployeeID equals e.ID
                            select new
                            {
                                UserNo = e.UserNo,
                                Name = e.Name,
                                Surname = e.Surname,
                                PermissionState = p.PermissionState,
                                StateName = s.StateName,
                                PermissionStartDate = p.PermissionStateDate,
                                PermissionEndDate = p.PermissionEndDate,
                                EmployeeID = p.EmployeeID,
                                PermissionID = p.ID,
                                DayAmount = p.PermissionDay,
                                PositionID = e.PositionID,
                                DepartmentID = e.DepartmentID,
                                Explanation = p.PermissionExplanation
                            }).OrderBy(x => x.PermissionStartDate).ToList();
                foreach (var item in list)
                {
                    PermissionDetailDTO dto = new PermissionDetailDTO
                    {
                        UserNo = item.UserNo,
                        Name = item.Name,
                        Surname = item.Surname,
                        PermissionState = item.PermissionState,
                        StartDate = item.PermissionStartDate,
                        EndDate = item.PermissionEndDate,
                        EmployeeID = item.EmployeeID,
                        PermissionID = item.PermissionID,
                        PermissionDayAmount = item.DayAmount,
                        PositionID = item.PositionID,
                        DepartmentID = item.DepartmentID,
                        StateName = item.StateName,
                        Explanation = item.Explanation
                        
                    };
                    permissionsList.Add(dto);
                }

                return permissionsList;

            }
            catch (Exception ex)
            {
                throw ex;
            }


            

        }

        public static void DeletePermission(int permissionID)
        {
            try
            {
                PERMISSION prm = db.PERMISSIONs.First(x => x.ID == permissionID);
                db.PERMISSIONs.DeleteOnSubmit(prm);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdatePermission(int permissionID, int approved)
        {
            try
            {
                PERMISSION pr = db.PERMISSIONs.First(x => x.ID == permissionID);
                pr.PermissionState = approved;
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdatePermission(PERMISSION permission)
        {
            try
            {
                PERMISSION pr = db.PERMISSIONs.First(x => x.ID == permission.ID);
                pr.PermissionStateDate = permission.PermissionStateDate;
                pr.PermissionEndDate = permission.PermissionEndDate;
                pr.PermissionExplanation = permission.PermissionExplanation;
                pr.PermissionDay = permission.PermissionDay;
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}