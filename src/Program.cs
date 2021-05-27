using System;

class Program
{
    static void Main(string[] args)
    {
        var ageComponent = new AgeComponent();
        var countryComponent = new CountryComponent();
        var searchComponent = new SearchComponent();
        var fraudDetectionComponent = new FraudDetectionComponent();
        
        var dialog = new DialogMediator(ageComponent, searchComponent, countryComponent, fraudDetectionComponent);

        while (true)
        {
            Console.WriteLine("Set your Age");
            var age_input = Convert.ToInt32(Console.ReadLine());
            dialog.setAge(age_input);

            Console.WriteLine("Set your Country");
            var country_input = Console.ReadLine();
            switch (country_input)
            {
                case "Schweiz":
                case "Switzerland":
                case "CH":
                    dialog.setCountry(Country.Switzerland);
                    break;
                case "Deutschland":
                case "Germany":
                case "DE":
                    dialog.setCountry(Country.Germany);
                    break;
                case "Indonesien":
                case "Indonesia":
                case "ID":
                    dialog.setCountry(Country.Indonesia);
                    break;
                default:
                    Console.WriteLine("Unknown country. Not changing it");
                    break;
            };

            Console.WriteLine("Search for Movie");
            var isAvailable = dialog.isMovieAvailable(Console.ReadLine());

            if(isAvailable){
                Console.WriteLine("This movie is available");
            }
            else{
                Console.WriteLine("This movie is not available");

            Console.WriteLine();
            }
        }
        
    }
}
