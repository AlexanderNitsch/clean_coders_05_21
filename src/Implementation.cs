using System;
using System.Collections.Generic;
using System.Linq;


class Dialog
{
    private AgeComponent _ageComponent;
    private SearchComponent _searchComponent;
    private CountryComponent _countryComponent;

    public Dialog(AgeComponent ageComponent, SearchComponent searchComponent, CountryComponent countryComponent)
    {
        this._ageComponent = ageComponent;
        this._searchComponent = searchComponent;
        this._countryComponent = countryComponent;
    } 

    public bool isMovieAvailable(string movieName)
    {
        return _searchComponent.isMovieAvailable(movieName);
    }

    public void setAge(int new_age){
        _ageComponent.setAge(new_age);
    }

    public void setCountry(Country country)
    {
        _countryComponent.setCountry(country);
    }
}


class AgeComponent
{
    private int Age;

    public void setAge(int new_age){
        this.Age = new_age;
        Console.WriteLine(String.Format("Age was set to {0}", this.Age));
    }

    public int getAge()
    {
        return Age;
    }
}

class SearchComponent
{
    private AgeComponent AgeComponent { get; set; }
    private CountryComponent countryComponent {get; set;}
    private List<string> HorrorMovies = new List<string>() {"Alien", "Cabin in the Woods"};
    private List<string> PixarMovies = new List<string>() {"ToyStory", "Ratatouille"};

    public SearchComponent(AgeComponent ageComponent, CountryComponent countryComponent)
    {
        this.AgeComponent = ageComponent;
        this.countryComponent = countryComponent;
    }
    
    private List<string> getAvailableMovies()
    {
        if(AgeComponent.getAge() >= countryComponent.getMinHorrorMovieAge()){
            return PixarMovies.Concat(HorrorMovies).ToList();
        }
        else{
            return PixarMovies;
        }
    }
    public bool isMovieAvailable(string movieName)
    {   
        return getAvailableMovies().Contains(movieName);
    }
}

class CountryComponent
{
    private Country country{ get; set;} 
    private Dictionary<Country,int> minHorrorMovieAge = new Dictionary<Country, int>
    {
        {Country.Switzerland, 18},
        {Country.Germany , 18},
        {Country.Indonesia, 21}
    };

    public int getMinHorrorMovieAge()
    {
        return minHorrorMovieAge[country];
    }

    public void setCountry(Country new_country)
    {
        this.country = new_country;
    }
}

enum Country
{
    Switzerland,
    Germany,
    Indonesia
}
