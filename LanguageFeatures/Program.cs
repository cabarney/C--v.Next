using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanguageFeatures
{
    class Program
    {
        static void Main(string[] args)
        {
            DoStuff();
        }

        static async void DoStuff()
        {
            var r1 = new Rectangle(4, 3);
            var r2 = new Rectangle("2,5");
            var r3 = new Rectangle(0, 4);

            var area = await r1.GetWidthToHeightRatio();
            area = await r3.GetWidthToHeightRatio();  
        }
    }

    public class Rectangle
    {
        private int width;
        private int height;

        public int Width
        {
            get { return width; }
        }   

        public int Height
        {
            get { return height; }
        }
        
        public Rectangle(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public Rectangle(string dimensions)
        {
            string[] parts = dimensions.Split(',');
            int w;
            int h;

            if (!int.TryParse(parts[0], out w))
                return;
            if (!int.TryParse(parts[1], out h))
                return;

            height = h;
            width = w;
        }
        
        Dictionary<string, object> rectangleProperties;
        
        public void InitializeProperties()
        {
            rectangleProperties = new Dictionary<string, object>();

            rectangleProperties["color"] = "Blue";
            rectangleProperties["Rating"] = 4;
        }

        public int GetArea()
        {
            return width * height;
        }

        public async Task<double> GetWidthToHeightRatio()
        {
            try
            {
                return width / height;
            }
            catch (Exception e) if(GetArea() == 0)
            {
                await Logger.Log(e.Message);
            }

            return 0;
        }   
    }

    public static class Logger
    {
        public static Task Log(string message)
        {
            return Task.Run(() => Console.WriteLine(message));
        }
    }
}
