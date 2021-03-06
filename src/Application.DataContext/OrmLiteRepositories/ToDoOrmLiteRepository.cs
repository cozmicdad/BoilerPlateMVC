﻿using System.Collections.Generic;
using Application.DataInterface;
using Application.Models;
using ServiceStack.OrmLite;

namespace Application.DataContext.OrmLiteRepositories
{
    public class ToDoOrmLiteRepository: OrmLiteRepository<ToDo>, IToDoRepository
    {
        public ToDoOrmLiteRepository(IDbConnectionFactory dbFactory) : base(dbFactory)
        {
        }

        public override void CreateMissingTables()
        {
            base.CreateMissingTables();
            using (var db = DbFactory.OpenDbConnection())
            {
                var count = db.Scalar<int>("SELECT count(*) FROM [dbo].[ToDos]");
                if (count == 0)
                {
                    //Seed data
                    db.Insert(new ToDo { Task = "Pick up laundry" });
                    db.Insert(new ToDo { Task = "Do the dishes" });
                }
            }                          
        }


        public IEnumerable<ToDo> GetRecent()
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                return db.Query<ToDo>("SELECT Top(@pageSize) [Project1].[Id] AS [Id], [Project1].[Task] AS [Task], [Project1].[Completed] AS [Completed] FROM [dbo].[ToDos]  AS [Project1] ORDER BY [Project1].[Id] DESC",
                new
                {
                    pageSize = 10
                });
            }              
        }
    }
}
