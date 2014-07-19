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
            var r2 = Rectangle.Parse("2,5");
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

        public Rectangle(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public static Rectangle Parse(string dimensions)
        {
            string[] parts = dimensions.Split(',');
            int w;
            int h;

            if (!int.TryParse(parts[0], out w))
                return null;
            if (!int.TryParse(parts[1], out h))
                return null;

            return new Rectangle(w, h);
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


    public class Circle(private int radius)
    {
        public double GetArea()
        {
            var area = Math.PI * Math.Pow(radius, 2);
            Console.WriteLine("Just calculated the Area of a circle #math #geometry");
            return area;
        }
    }
}