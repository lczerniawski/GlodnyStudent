import React, { Component } from 'react'
import ListItem from './ListItem';
import Filters from './Filters';
import Sort from './Sort';
import Search from  '../MainPage/Search';
import './RestaurantList.css';
import './Search.css'; 
/* import {host} from '../../config' */
import Geocode from "react-geocode";
 
// set Google Maps Geocoding API for purposes of quota management. Its optional but recommended.
Geocode.setApiKey("AIzaSyBxvJXLoj0DtoGczKojLEo_Kc3LsdlPxCQ ");
 
// Enable or disable logs. Its optional.
Geocode.enableDebug();

export class RestaurantList extends Component {

  constructor(props) {
    super(props);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.getDataByFilters = this.getDataByFilters.bind(this);
    this.addressUpdate = this.addressUpdate.bind(this);
    this.state = {
      location:window.location.href.slice(window.location.href.lastIndexOf("/")+1),
      distance:20,// tu wstawić jako poczatkowa wartosc wartosc highestDistance
      price:0,// tu wstawić jako poczatkowa wartosc wartosc highestPrice
      cuisine:'Wszystkie',
      cuisines: [], // tu wstawic liste wszystkich typow kuchni z serwera
      highestPrice:0,// tu wstawić maksymalna wartość dostarczona z serwera
      sort:'priceGrowingly',
      restaurations: [],
      mapResonseMessage:null,
      lat:null,
      lng:null
      
    };
  }

componentDidMount(){
  this.getDataByAddress();
  this.getCuisinesAndHighestPrice();
  this.getCoords(this.state.location);
}

getDataByAddress(){
  const address = `api/Search/${this.state.location}`;
  fetch(address).then((response) => {
    if (response.ok) {
      return response.json();
    } else {
      throw new Error('Restauration not found');
    }
  })
  .then((result) => {
    this.setState({
      restaurations: result
    });
  })
  .catch((error) => {
    console.log(error);
  });
}


getCuisinesAndHighestPrice(firstTime=true){
  const address = `api/Search/Cuisines/${this.state.location}`;
  fetch(address).then((response) => {
    if (response.ok) {
      return response.json();
    } else {
      throw new Error('Cuisines not found');
    }
  })
  .then((result) => {
      const prevPrice = this.state.price>result.highestPrice?result.highestPrice:this.state.price;
      this.setState({
      error:false,
      cuisines:result.cuisinesList,
      highestPrice: result.highestPrice,
      price:firstTime?result.highestPrice:prevPrice
    });
  })
  .catch((error) => {
    console.log(error); 
  });
}




getCoords(streetName){

Geocode.fromAddress(streetName).then(
  response => {
    console.log(response.results[0].geometry.location);
    this.setState({

          lat: response.results[0].geometry.location.lat,
          lng: response.results[0].geometry.location.lng,
          mapResonseMessage:null
     });
  },
  error => {
    console.error(error);
    this.setState({
      mapResonseMessage:"Ulica nie została znaleziona."
    }); 
  }
);
}







    getDataByFilters(){
      const address = `api/Search?address=${this.state.location}&distance=${this.state.distance}&highestPrice=${this.state.price}&cuisine=${this.state.cuisine}&lat=${this.state.lat}&lng=${this.state.lng}`;
      fetch(address).then((response) => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Restauration not found');
        }
      })
      .then((result) => {
        this.setState({
          restaurations: result
        });
      })
      .catch((error) => {
        console.log(error);
      });
      this.getCuisinesAndHighestPrice(false);
    }



  handleInputChange(event) {
    const target = event.target;
    const value = target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  addressUpdate(addr){
    this.setState({
      location:addr
  }, () => {
    this.getDataByFilters();
  });
  }

  

  render() {
    const { restaurations,cuisines,distance,price,highestPrice,sort} = this.state;
    let list 
    if (restaurations.length === 0) {
      list= <div className="notFound wow bounce" data-wow-duration="1s">
        <i className="fas fa-bug fa-4x"></i>
        <h2>Upsss... nic nie znaleziono</h2>
        <p>Nie znaleziono restauracji o podanych parametrach.</p>
        </div>; 
    } else {
       list = restaurations.map((restauration)=><ListItem key={restauration.id} name={restauration.name} address={restauration.address} rate={restauration.score}
        reviewsCount={restauration.reviewsCount} image={restauration.image} id={restauration.id} />);
    }   
    return (    
      <div className="restaurantListContainer">

        <div className="titlePage">
          <h2 className="wow fadeInLeft" data-wow-duration="1s">Lista restauracji</h2>
        </div>

        <div className="searchRestaurant">
          <Search onAddressUpdate={this.addressUpdate} isMain={false} address={this.state.location}/>
        </div>
        {this.state.mapResonseMessage}
        <div className="filersBar">
      <Filters  cuisines={cuisines} distance={distance} price={price} highestPrice={highestPrice} onSetFilter={this.handleInputChange} />
          <Sort sort={sort} restaurations={restaurations} onSetSort={this.handleInputChange} />
        </div>
        
        <div className="restaurantList">
          {list}
        </div>
      </div>
    )
  }
}