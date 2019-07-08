import React, { Component } from 'react'
import ListItem from './ListItem';
import Filters from './Filters';
import Sort from './Sort';
import Search from  '../MainPage/Search';
import './RestaurantList.css';
import _, {debounce} from 'lodash';

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
      lng:null,
      currentPage: 1,
      restaurantsPerPage: 10,
      searchFixed:false
      
    };

    this.handlePaginationClick = this.handlePaginationClick.bind(this);
    this.changeClassForSearchInput = this.changeClassForSearchInput.bind(this);
  }

  handlePaginationClick(event) {
    this.setState({currentPage: Number(event.target.id)});
  }

  
componentDidMount(){
  this.getDataByAddress();
  this.getCuisinesAndHighestPrice();
  this.getCoords(this.state.location);
  this.changeClassForSearchInput();
}

  changeClassForSearchInput(){
    document.addEventListener('scroll', _.debounce(()=>{
        if(window.pageYOffset >= 270){
          this.setState({searchFixed:true});
        }else{
          this.setState({searchFixed:false});
        }
      },500));
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
    const { restaurations,cuisines,distance,price,highestPrice,sort,currentPage,restaurantsPerPage} = this.state;

    const indexOfLastRestaurant = currentPage * restaurantsPerPage;
    const indexOfFirstRestaurant = indexOfLastRestaurant - restaurantsPerPage;
    const currentRestaurants = restaurations.slice(indexOfFirstRestaurant, indexOfLastRestaurant);


    
    let list 
    if (restaurations.length === 0) {
      list= <div>
        <h2>Upsss... nic nie znaleziono</h2>
        <p>Nie znaleziono restauracji o podanych parametrach.</p>
        </div>; 
    } else {
       list = currentRestaurants.map((restauration)=><ListItem key={restauration.id} name={restauration.name} address={restauration.address}
        reviewsCount={restauration.reviewsCount} image={restauration.image} id={restauration.id} />);
    }   



    const pageNumbers = [];
    for (let i = 1; i <= Math.ceil(restaurations.length / restaurantsPerPage); i++) {
      pageNumbers.push(i);
    }

    const renderPageNumbers = pageNumbers.map(number => {
      return (
        <div
          className="paginationItem"
          key={number}
          id={number}
          onClick={this.handlePaginationClick}
        >
          {number}
        </div>
      );
    });
    
    if(this.state.mapResonseMessage !== null) alert(this.state.mapResonseMessage);
    const searchClass = this.state.searchFixed ?"searchFixed":"search";

    return (    
      <div className="restaurantListContainer">
        <div className="header">
          <h1>Lista restauracji</h1>
        </div>
        <div className={searchClass} >
          <Search searchFixed={this.state.searchFixed} onAddressUpdate={this.addressUpdate} isMain={false} address={this.state.location}/>
        </div>
        <div className="filters">
          <div className="filtersBox" >
            <Filters  cuisines={cuisines} distance={distance} price={price} highestPrice={highestPrice} onSetFilter={this.handleInputChange} />
            <Sort sort={sort} restaurations={restaurations} onSetSort={this.handleInputChange} />
          </div>
        </div>  
        <div className="list">
          {list}
        </div>
        <div className="paginationContainer">
          {renderPageNumbers}
        </div>
      </div>
    )
  }
}