import React, { Component } from 'react'
import KRScheck from './KRScheck';

export default class AddRestaurant extends Component {

    constructor(props){
        super(props);
        this.state={
            cuisines:[],//ma nie byc wszystkie,enumerator
            restaurant:{
                restaurantName:"",
                cuisine:"Włoska",//mam wiedziec co bedzie pierwsze
                streetName:"",
                streetNumber:"",
                localNumber:null,
                district:"Bemowo"
            }, 
            wantToBeOwner:false,
            KRS:null

        }
        this.getCuisines = this.getCuisines.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount(){
       this.getCuisines();
    }


    getCuisines(){
        const address = `api/Restaurants/AllCuisines`;// Zeby zwracał wszystkei jakie mozna wpisac, enumerator na serwerze
        fetch(address).then((response) => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Cuisines not found');
          }
        })
        .then((result) => {
      
            this.setState({
            error:false,
            cuisines:result,
          });
        })
        .catch((error) => {
          console.log(error);
          this.setState({
            error
          }); 
        });
      }


      handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        if(name === "KRS" || name === "wantToBeOwner"){
            this.setState({[name]: value});
        } else{
            this.setState(prevState => ({
                restaurant: {
                    ...prevState.restaurant,
                    [name]: value
                }
              }));
        }
       
      }


      handleSubmit(e){
          e.preventDefault();
          //console.log(RESTAURACJA: ${JSON.stringify(this.state.restaurant)} KRS: ${this.state.KRS});
          const adr =`api/Restaurants`; 
        
           fetch(adr, {
          method: 'PUT',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':'Bearer ' + sessionStorage.getItem("token")

          },
          body: JSON.stringify(
            {
                restaurantName: this.state.restaurant.restaurantName,
                cuisine:this.state.restaurant.cuisine,
                address:{
                  streetName:this.state.restaurant.streetName,
                  streetNumber:this.state.restaurant.streetNumber,
                  localNumber:this.state.restaurant.localNumber,
                  district:this.state.restaurant.district
                },
                userName:sessionStorage.getItem("username")
              
            }
          )
        }).then(res => res.json())
        .then((data) => { 
 
          // Komunikat ze sie udalo
        
         } )
        .catch((err)=>console.log(err))  
      }



    render() {

        const cousinesList = this.state.cuisines.map(cusine=><option key={cusine} value={cusine}>{cusine}</option>);
        const districts = ["Bemowo","Białołęka","Bielany","Mokotów","Ochota","Praga-Południe","Praga-Północ","Rembertów","Śródmieście",
        "Targówek","Ursus","Ursynów","Wawer","Wesoła","Wilanów","Włochy","Wola","Żoliborz"];
        const districtsList = districts.map(district=><option key={district} value={district}>{district}</option>);

        return (
            <div>
                <form>
                    <label>Nazwa restauracji <input name="restaurantName" type="text" onChange={this.handleInputChange} /></label>
                    <label>Typ kuchni
                    <select  name="cuisine" onChange={this.handleInputChange}>
                        {cousinesList}
                    </select>
                    </label>
                    <label>Ulica <input type="text" name="streetName" onChange={this.handleInputChange}/></label>
                    <label>Numer ulicy <input type="text" name="streetNumber" onChange={this.handleInputChange}/></label>
                    <label>Numer lokalu <input type="number" name="localNumber" onChange={this.handleInputChange}/></label>
                    <label>Chcę zostać włascicielem <input type="checkbox" name="wantToBeOwner"onChange={this.handleInputChange} /></label>
                    <KRScheck handleInputChange={this.handleInputChange} />
                    <button onClick ={this.handleSubmit}>Dodaj resturacje</button>
                    <label htmlFor="district">Dzielnica</label>
                    <select name="distric" onChange={this.handleInputChange}>
                      {districtsList}
                    </select>
                </form>
            </div>
        )
    }
}