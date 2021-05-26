using System;
using System.Collections.Generic;
using System.Linq;


// The Mediator interface declares a method used by components to notify the
// mediator about various events. The Mediator may react to these events and
// pass the execution to other components.
public interface IMediator
{
    void Notify(object sender, string ev);
}

// Concrete Mediators implement cooperative behavior by coordinating several
// components.
class DialogMediator : IMediator
{
    private AgeComponent _ageComponent;
    private SearchComponent _searchComponent;
    private CountryComponent _countryComponent;

    private FraudDetectionComponent _fraudDetectionComponent;

    public DialogMediator(AgeComponent ageComponent, SearchComponent searchComponent, CountryComponent countryComponent, FraudDetectionComponent fraudDetectionComponent)
    {
        this._ageComponent = ageComponent;
        this._ageComponent.SetMediator(this);
        this._searchComponent = searchComponent;
        this._searchComponent.SetMediator(this);
        this._countryComponent = countryComponent;
        this._countryComponent.SetMediator(this);
        this._fraudDetectionComponent = fraudDetectionComponent;
        this._fraudDetectionComponent.SetMediator(this);
    } 

    public void Notify(object sender, string ev)
    {
        switch (ev)
        {
            case "setAge":
            case "setCountry":
                var age = this._ageComponent.getAge();
                var minHorrorMovieAge = this._countryComponent.getMinHorrorMovieAge();

                this._searchComponent.setHorrorMovieFilter(age, minHorrorMovieAge);

                this._fraudDetectionComponent.checkAgeForFraud(age, minHorrorMovieAge);
                this._fraudDetectionComponent.checkForCyberAttack();
                break;
            case "MovieSearched":
                this._fraudDetectionComponent.checkForCyberAttack();
                break;
        }

        if (this._fraudDetectionComponent.isFraud){
            System.Threading.Thread.Sleep(10000);
            System.Environment.Exit(1);
        }
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

// The Base Component provides the basic functionality of storing a
// mediator's instance inside component objects.
class BaseComponent
{
    protected IMediator _mediator;

    public BaseComponent(IMediator mediator = null)
    {
        this._mediator = mediator;
    }

    public void SetMediator(IMediator mediator)
    {
        this._mediator = mediator;
    }
}

// Concrete Components implement various functionality. They don't depend on
// other components. They also don't depend on any concrete mediator
// classes.
class AgeComponent : BaseComponent
{
    private int Age;
    public void setAge(int new_age){
        this.Age = new_age;
        Console.WriteLine(String.Format("Age was set to {0}", this.Age));

        this._mediator.Notify(this, "setAge");
    }

    public int getAge()
    {
        return Age;
    }

}

class SearchComponent : BaseComponent
{
    private List<string> HorrorMovies = new List<string>() {"Alien", "Cabin in the Woods"};
    private List<string> PixarMovies = new List<string>() {"Toy Story", "Ratatouille"};

    private bool allowHorror {get; set;} = false;

    private List<string> getAvailableMovies()
    {
        if(this.allowHorror){
            return PixarMovies.Concat(HorrorMovies).ToList();
        }
        else{
            return PixarMovies;
        }
    }

    public void setHorrorMovieFilter(int age, int minHorrorMovieAge)
    {
        allowHorror = age >= minHorrorMovieAge;
        
    }


    public bool isMovieAvailable(string movieName)
    {
        var isAvailable = this.getAvailableMovies().Contains(movieName);
        this._mediator.Notify(this, "MovieSearched");
        
        return isAvailable;
    }
}


class CountryComponent : BaseComponent
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
        this._mediator.Notify(this, "setCountry");
    }
}

enum Country
    {
        Switzerland,
        Germany,
        Indonesia
    }

class FraudDetectionComponent : BaseComponent
{
    private int? lastAge;
    private int? lastMinAge;
    private DateTime timeOfLastAccess = DateTime.MinValue;

    public bool isFraud = false;

    public void checkAgeForFraud(int age, int minAge)
    {      
        // If the person lowered their age below the threshold
        if(lastAge < lastMinAge && age >= lastMinAge)
        {
            Console.WriteLine("This is a FRAUD! Sending out Email to the parents.");
            this.isFraud = true;
        }

        // Person changed country to get access
        if(minAge < lastMinAge && lastAge <= lastMinAge && lastAge >= minAge)
        {
            Console.WriteLine("This is a FRAUD! Sending out Email to the parents.");
            this.isFraud = true;
        }


        lastAge = age;
        lastMinAge = minAge;
    }

    public void checkForCyberAttack()
    {
        if ((DateTime.Now - timeOfLastAccess).TotalMilliseconds < 1500)
        {
            Console.WriteLine("Nobody is this fast. This is a Cyberattack! Calling the police.");
            this.isFraud = true;
        }
        
        timeOfLastAccess = DateTime.Now;
    }
}