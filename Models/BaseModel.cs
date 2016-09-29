using System;

namespace ChatAweria.Models
{
    public class BaseModel
    {
        protected string GenerateName()
        {
            var colors = new string[] { "Green", "Red", "Black", "Yellow", "Blue", "Orange" };
            var animals = new string[] { "Cat", "Dog", "Horse", "Cow", "Snake", "Bird" };

            var r = new Random();
            var preName = r.Next(0, 6);
            var postName = r.Next(0, 6);

            return $"{colors[preName]}-{animals[postName]}";
        }
    }
}
