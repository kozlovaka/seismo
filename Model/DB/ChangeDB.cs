using SilencePauseApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SilencePauseApp.Model.DB
{
    public class ChangeDB
    {
        private Command _command;
        public ChangeDB()
        {
            _command = new Command();
        }

        #region Get
        public ObservableCollection<Pauses> GetPauses()
        {
            List<Pauses> temp;
            DataTable _dataTemp = new DataTable();
            _command.InitialTable(out _dataTemp, "Select * from Pauses");
            temp = (from DataRow dr in _dataTemp.Rows
                    select new Pauses()
                    {
                        Id = int.Parse(dr["Id"].ToString()),
                        Hour = int.Parse(dr["Hour"].ToString()),
                        Min = int.Parse(dr["Min"].ToString()),
                        Sec = float.Parse(dr["Sec"].ToString()),
                        Latitude = float.Parse(dr["Latitude"].ToString()),
                        Longitude = float.Parse(dr["Longitude"].ToString()),
                        Depth = float.Parse(dr["Depth"].ToString()),
                        DDep = float.Parse(dr["DDep"].ToString()),
                        Ms = float.Parse(dr["Ms"].ToString()),
                        Kl = float.Parse(dr["Kl"].ToString()),
                        DateEvent = dr["DateEvent"].ToString()
                    }).ToList();

            return  new ObservableCollection<Pauses>(temp);
        }       
        #endregion

        public void AddAll(BaseEntity baseEntity)
        {
            _command.Execute(Add(baseEntity));
        }
        public void UpdateAll(BaseEntity baseEntity)
        {
            _command.Execute(Update(baseEntity));
        }
        public void RemoveAll(BaseEntity baseEntity)
        {
            _command.Execute(Remove(baseEntity));
        }

        #region Converter class to sql query

        private string Add(BaseEntity entity)
        {
            List<PropertyInfo> props = entity.GetType().GetProperties().ToList();

            string command = $"SET IDENTITY_INSERT Pauses ON \n Insert into {entity.GetType().Name} (";

            for (int i = 0; i < props.Count(); i++)
            {
                if (!props[i].PropertyType.IsSubclassOf(typeof(BaseEntity)) && props[i].PropertyType != typeof(DateTime))
                    command += props[i].Name + ", ";
            }

            command = command.Remove(command.Length - 2, 2);
            command += ") values (";

            for (int i = 0; i < props.Count(); i++)
            {
                if (!props[i].PropertyType.IsSubclassOf(typeof(BaseEntity)) && props[i].PropertyType != typeof(DateTime))
                    command += "N'" + props[i].GetValue(entity).ToString().Replace(",", ".") + "', ";
            }

            command = command.Remove(command.Length - 2, 2);
            command += ");\n SET IDENTITY_INSERT Pauses OFF";
            return command;
        }

        private string Remove(BaseEntity entity)
        {
            PropertyInfo[] props = entity.GetType().GetProperties();
            return $"Delete from {entity.GetType().Name} where Id = {props[props.Count() - 1].GetValue(entity)}";           
        }

        private string Update(BaseEntity entity)
        {
            List<PropertyInfo> props = entity.GetType().GetProperties().ToList();
            string command = $"Update {entity.GetType().Name} set ";

            foreach (PropertyInfo p in props)
            {
                if (!p.PropertyType.IsSubclassOf(typeof(BaseEntity)) && p.PropertyType != typeof(DateTime))
                    command += $"{p.Name} = N'" + p.GetValue(entity) + "', ";
            }
            command = command.Remove(command.Length - 2, 2);

            command += $" where Id = {props[props.Count() - 1].GetValue(entity)}";

            return command;
        }

        #endregion
    }
}
