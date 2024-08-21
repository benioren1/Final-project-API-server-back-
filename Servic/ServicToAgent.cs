using FinalProject_APIServer.DAL;

namespace FinalProject_APIServer.Servic
{
    public class ServicToAgent
    {
        private readonly FinalProjectDbContext _dbcontext;



        public ServicToAgent(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;

        }

        public List<int?> MoveTargetOnePlay(string direction, int? x, int? y)
        {
            int? x_x = x;
            int? y_y = y;


            switch (direction)
            {
                case "n":
                    {
                        x_x += 1;
                        break;
                    }
                case "s":
                    {
                        x_x -= 1;
                        break;
                    }
                case "w":
                    {
                        y_y -= 1;
                        break;
                    }
                case "e":
                    {
                        y_y += 1;
                        break;
                    }
                case "nw":
                    {
                        y_y -= 1;
                        x_x += 1;
                        break;
                    }
                case "ne":
                    {
                        y_y += 1;
                        x_x += 1;
                        break;
                    }
                case "sw":
                    {
                        y_y -= 1;
                        x_x -= 1;
                        break;
                    }
                case "se":
                    {
                        y_y += 1;
                        x_x -= 1;
                        break;
                    }

            }

            List<int?> list = new List<int?>();
            list.Add(x_x);
            list.Add(y_y);
            return list;
        }




    }
}

