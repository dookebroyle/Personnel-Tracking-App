using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class TaskDAO : EmployeeContext
    {
        public static List<TASKSTATE> GetTaskStates()
        {
            return db.TASKSTATEs.ToList();
        }

        public static void AddTask(TASK task)
        {
            try
            {
                db.TASKs.InsertOnSubmit(task);
                db.SubmitChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static List<TaskDetailDTO> GetTasks()
        {
            List<TaskDetailDTO> taskList = new List<TaskDetailDTO>();

            try
            {
                var list = (from t in db.TASKs
                            join s in db.TASKSTATEs on t.TaskState equals s.ID
                            join e in db.EMPLOYEEs on t.EmployeeID equals e.ID
                            join d in db.DEPARTMENTs on e.DepartmentID equals d.ID
                            join p in db.POSITIONs on e.PositionID equals p.ID
                            select new
                            {
                                TaskID = t.ID,
                                Title = t.TaskTitle,
                                Content = t.TaskContent,
                                TaskStartDate = t.TaskStartDate,
                                TaskDeliveryDate = t.TaskDeliveryDate,
                                StateName = s.StateName,
                                TaskStateID = t.TaskState,
                                UserNo = e.UserNo,
                                Name = e.Name,
                                EmployeeID = t.EmployeeID,
                                Surname = e.Surname,
                                PositionID = e.PositionID,
                                PositionName = p.PositionName,
                                DepartmentName = d.DepartmentName,
                                DepartmentID = p.DepartmentID
                            }).OrderBy(x => x.TaskStartDate).ToList();

                
                foreach (var item in list)
                {
                    TaskDetailDTO dto = new TaskDetailDTO
                    {
                        TaskID = item.TaskID,
                        Title = item.Title,
                        Content = item.Content,
                        TaskStartDate = Convert.ToDateTime(item.TaskStartDate),
                        TaskDeliveryDate = Convert.ToDateTime(item.TaskDeliveryDate),
                        StateName = item.StateName,
                        UserNo = item.UserNo,
                        Name = item.Name,
                        EmployeeID = item.EmployeeID,
                        Surname = item.Surname,
                        PositionID = item.PositionID,
                        PositionName = item.PositionName,
                        DepartmentName = item.DepartmentName,
                        DepartmentID = item.DepartmentID,
                        TaskStateID = item.TaskStateID
                    };
                    taskList.Add(dto);
                }
                return taskList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
        }

        public static void ApproveTask(int taskID, bool isAdmin)
        {
            try
            {
                TASK tsk = db.TASKs.First(x => x.ID == taskID);
                if (isAdmin)
                    tsk.TaskState = TaskStates.Approved;
                else
                    tsk.TaskState = TaskStates.Delivered;
                tsk.TaskDeliveryDate = DateTime.Today;
                db.SubmitChanges();
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void DeleteTask(int taskID)
        {
            try
            {
                TASK tsk = db.TASKs.First(x => x.ID == taskID);
                db.TASKs.DeleteOnSubmit(tsk);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdateTask(TASK task)
        {
            try
            {
                Console.WriteLine(task.ID);
                TASK t = db.TASKs.First(x => x.ID == task.ID);
                t.TaskTitle = task.TaskTitle;
                t.TaskContent = task.TaskContent;
                t.TaskState = task.TaskState;
                t.EmployeeID = task.EmployeeID;
                db.SubmitChanges();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
